/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActionWindow.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-22      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEditor.SceneManagement;

    public enum EToolState
    {
        ETS_None,
        ETS_Play,
        ETS_Pause,
        ETS_Stop,
        ETS_Step,
        ETS_Save,
        ETS_Help,

        ETS_Max,
    }

    public partial class ActionWindow : EditorWindow
    {
        public Texture2D TexturePlay;
        public Texture2D TexturePause;
        public Texture2D TextureStop;
        public Texture2D TextureStep;
        public Texture2D TextureSave;
        public Texture2D TextureHelp;
        public Texture2D TextureTimeArrow;
        public GUISkin EditorSkin;

        private static ActionWindow mInstance;
        private static string[] mInspectorString = new string[] { "Role", "Weapon", "Action", "AI", "Buff" };
        private static readonly float mToolBarHeight = 18f;
        private static readonly float mInspectorWidth = 800f;
        private static readonly float mSkillNameWidth = 100f;
        private static readonly float mActorHeight = 20f;

        private int mInspectorID = 0;
        private EToolState mToolState = EToolState.ETS_None;
        private float mZoom = 500f;
        private float mTime = 0f;
        private float mLastTime = 0f;

        private Vector2 mLastMouseDownPos;
        private double mLastMouseClickTime;

        private Texture2D mCachedTex;
        private Vector2 mScrollPosition;
        private Vector2 mWorkAreaMousePos;
        private Vector2 mWorkAreaAbsMousePos;
        private Rect mWorkArea;

        // actor
        private Actor mCurActor;
        private ActionWrapper mCurActionWrapper;
        private float mTotalTime = 1.333f;

        public static ActionWindow Instance
        {
            get { return mInstance; }
        }

        public float TotalTime
        {
            get { return mTotalTime; }
            set { mTotalTime = value; }
        }

        //[MenuItem("Tools/CLine Action Editor Initialize Setting")]
        //public static void InitSetting()
        //{
        //    EditorUtility.DisplayDialog("Editor Setting Initialize", "Please Copy `/Editor/Layout/ActionEditor.wlt` To `/Users/Your_User_name/AppData/Roaming/Unity/Editor-5.x/Preferences/Layouts`", "OK");
        //}

        [MenuItem("Tools/CLine Action Editor")]
        public static void Init()
        {
            EditorSceneManager.OpenScene("Assets/CLineActionEditor/Editor/Scene/ActionEditor.unity");
            //EditorApplication.ExecuteMenuItem("Window/Layouts/ActionEditor");

            mInstance = EditorWindow.GetWindow(typeof(ActionWindow), false, "Action Editor") as ActionWindow;

            mInstance.OnInitialize();
        }

        private void Update()
        {
            if (!Application.isPlaying && mToolState == EToolState.ETS_Play)
            {
                Preview(Time.realtimeSinceStartup - mLastTime);
                Repaint();
            }

            mLastTime = Time.realtimeSinceStartup;
        }

        private void OnGUI()
        {
            // TO CLine: [fix bug] Getting control 0's position in a group with only 0 controls when doing
            // When Unity is drawing GUI, function OnGUI is called twice on same frame. 
            // First call is for layout and second is for painting or mouse event or whatever.
            /*
             *  This is wrong
                void OnGUI()
                {
                  if (Event.current.type == EventType.MouseDown)
                      GUILayout.Label("had mouse down"); // <-- will only happen in MouseDown and not Layout event!
                }
                
             *  This is right
                bool down = false;
                void OnGUI()
                {
                  if (down)
                      GUILayout.Label("had mouse down");
 
                  if (Event.current.type == EventType.MouseDown)
                      down = true;
                }
             */
            DrawInspector();

            UpdateInput();

            DrawToolBar();
            DrawSkillTree();
            DrawTime();
        }       

        private void UpdateInput()
        {
            UnityEngine.Event evt = UnityEngine.Event.current;
            Vector2 mousePos = evt.mousePosition;

            mWorkAreaMousePos = mousePos;

            if (!mWorkArea.Contains(mousePos)) return;

            Vector2 point = mWorkAreaAbsMousePos;
            point.x *= mZoom;

            switch (evt.type)
            {
                case EventType.MouseDown:
                    mToolState = EToolState.ETS_None;
                    mInspectorID = 5;
                    switch (evt.button)
                    {
                        case 0:
                            if (EditorApplication.timeSinceStartup - mLastMouseClickTime < 0.3f &&
                                Vector2.Distance(mousePos, mLastMouseDownPos) < 3f)
                            {
                                OnLeftMouseDoubleClick(point);
                            }
                            else
                            {
                                OnLeftMouseDown(point);

                                mLastMouseClickTime = EditorApplication.timeSinceStartup;
                                mLastMouseDownPos = mousePos;
                            }
                            break;
                        case 1:
                            OnRightMouseDown(mousePos);
                            break;
                    }
                    break;
                case EventType.MouseUp:
                    switch (evt.button)
                    {
                        case 0:
                            OnLeftMouseUp(point);
                            break;
                        case 1:
                            OnRightMouseUp(point);
                            break;
                    }
                    break;
                case EventType.MouseDrag:
                    switch (evt.button)
                    {
                        case 0:
                            OnLeftMouseDrag(point);
                            break;
                        case 2:
                            OnMidMouseDrag(evt.delta);
                            break;
                    }
                    break;
                case EventType.KeyDown:
                    switch (evt.keyCode)
                    {
                        case KeyCode.Alpha1://play
                            mToolState = EToolState.ETS_Play;
                            break;
                        case KeyCode.Alpha2://step
                            mToolState = EToolState.ETS_Step;
                            Preview(0.016667f);
                            break;
                        case KeyCode.Alpha3://pause
                            mToolState = EToolState.ETS_Pause;
                            break;
                        case KeyCode.Alpha4://stop
                            mToolState = EToolState.ETS_Stop;
                            mTime = 0f;
                            ResetPreview();
                            break;
                        case KeyCode.Delete:
                            break;
                    }
                    break;
            }

            Repaint();
        }

        private void DrawToolBar()
        {
            GUI.Box(new Rect(0, 0, position.width, mToolBarHeight), "", EditorStyles.toolbar);

            // zoom
            mZoom = GUI.HorizontalSlider(new Rect(5, 0, 150, mToolBarHeight), mZoom, 1500f, 1f);


            // play
            GUI.color = (mToolState == EToolState.ETS_Play ? Color.red : Color.white);
            if (GUI.Button(new Rect(160, 0, 30, mToolBarHeight), TexturePlay, EditorStyles.toolbarButton))
            {
                mToolState = (mToolState == EToolState.ETS_Play ? EToolState.ETS_None : EToolState.ETS_Play);
            }

            // pause
            GUI.color = (mToolState == EToolState.ETS_Pause ? Color.red : Color.white);
            if (GUI.Button(new Rect(190, 0, 30, mToolBarHeight), TexturePause, EditorStyles.toolbarButton))
            {
                mToolState = (mToolState == EToolState.ETS_Pause ? EToolState.ETS_None : EToolState.ETS_Pause);
            }

            // stop
            GUI.color = (mToolState == EToolState.ETS_Stop ? Color.red : Color.white);
            if (GUI.Button(new Rect(220, 0, 30, mToolBarHeight), TextureStop, EditorStyles.toolbarButton))
            {
                mToolState = (mToolState == EToolState.ETS_Stop ? EToolState.ETS_None : EToolState.ETS_Stop);
                mTime = 0f;
                ResetPreview();
            }

            // step
            GUI.color = (mToolState == EToolState.ETS_Step ? Color.red : Color.white);
            if (GUI.Button(new Rect(250, 0, 30, mToolBarHeight), TextureStep, EditorStyles.toolbarButton))
            {
                mToolState = (mToolState == EToolState.ETS_Step ? EToolState.ETS_None : EToolState.ETS_Step);
                Preview(0.016667f);
                Repaint();
            }

            // save
            //GUI.color = (mToolState == EToolState.ETS_Save ? Color.red : Color.white);
            GUI.color = Color.white;
            if (GUI.Button(new Rect(280, 0, 30, mToolBarHeight), TextureSave, EditorStyles.toolbarButton))
            {
                mToolState = (mToolState == EToolState.ETS_Save ? EToolState.ETS_None : EToolState.ETS_Save);
                Save();
            }

            // help
            //GUI.color = (mToolState == EToolState.ETS_Help ? Color.red : Color.white);
            GUI.color = Color.white;
            if (GUI.Button(new Rect(310, 0, 30, mToolBarHeight), TextureHelp, EditorStyles.toolbarButton))
            {
                mToolState = (mToolState == EToolState.ETS_Help ? EToolState.ETS_None : EToolState.ETS_Help);
                Help();
            }
        }

        private void DrawTime()
        {
            // background
            GUI.color = new Color(0, 0, 0, 0.25f);
            GUI.DrawTexture(new Rect(0, mToolBarHeight, mSkillNameWidth, position.height), GetCachedTex(Color.white));

            GUI.color = new Color(1, 1, 1, 0.025f);
            GUI.DrawTexture(new Rect(mSkillNameWidth, mToolBarHeight, position.width - mSkillNameWidth - mInspectorWidth, position.height), GetCachedTex(Color.white));

            // time
            GUI.color = Color.white;
            GUI.Box(new Rect(0, mToolBarHeight, mSkillNameWidth, mActorHeight), "");
            GUI.Label(new Rect(2, mToolBarHeight, 98, mActorHeight), Helper.FormatTime(mTotalTime));

            // time horizontal
            GUI.color = new Color(0, 0, 0, 0.4f);
            GUI.DrawTexture(new Rect(mSkillNameWidth, mToolBarHeight, position.width - mInspectorWidth - mSkillNameWidth - 14, mActorHeight), GetCachedTex(Color.white));
            GUI.color = Color.white;
            GUI.Box(new Rect(mSkillNameWidth, mToolBarHeight, position.width - mInspectorWidth - mSkillNameWidth - 14, mActorHeight), "");

            // time vertical
            Rect rcTimeLine = new Rect(mSkillNameWidth, mToolBarHeight, position.width - mSkillNameWidth - mInspectorWidth - 14, position.height - mToolBarHeight);
            GUILayout.BeginArea(rcTimeLine);
            GUI.color = new Color(0.9f, 0.25f, 0.25f, 1f);
            GUI.DrawTexture(new Rect(mTime * mZoom - mScrollPosition.x - 1, 0, 2, rcTimeLine.height - 15), GetCachedTex(Color.white));
            GUI.color = Color.white;
            GUI.Label(new Rect(mTime * mZoom - mScrollPosition.x, 0, 100, 24), Helper.FormatTime(mTime));
            GUI.color = new Color(0.8f, 0.25f, 0.2f, 1);
            GUI.DrawTexture(new Rect(mTime * mZoom - mScrollPosition.x - 6, 0, 12, 6), TextureTimeArrow);
            GUILayout.EndArea();

            // right scroll view frame
            GUI.color = new Color(0, 0, 0, 0.4f);
            GUI.DrawTexture(new Rect(position.width - mInspectorWidth - 14, mToolBarHeight, 14, position.height), GetCachedTex(Color.white));
        }

        private void DrawSkillTree()
        {
            mWorkAreaMousePos.x -= mSkillNameWidth;
            mWorkAreaMousePos.y -= (mToolBarHeight + mActorHeight);
            mWorkAreaAbsMousePos = mWorkAreaMousePos;
            mWorkAreaAbsMousePos.x = (mWorkAreaAbsMousePos.x + mScrollPosition.x) / mZoom;
            mWorkAreaAbsMousePos.y = (mWorkAreaAbsMousePos.y + mScrollPosition.y);

            float totalWidth = mTotalTime * mZoom;
            float totalHeight = GetActorCount() * mActorHeight;
            float forceScrollWidth = position.width - mSkillNameWidth - mInspectorWidth - 14 + 1;
            float forceScrollHeight = position.height - mToolBarHeight - mActorHeight - 14;

            mWorkArea = new Rect(mSkillNameWidth, mToolBarHeight + mActorHeight, position.width - mSkillNameWidth - mInspectorWidth, position.height - mToolBarHeight - mActorHeight);
            GUI.color = Color.white;

            if (mCurActionWrapper == null) return;

            mScrollPosition = GUI.BeginScrollView(mWorkArea, mScrollPosition, new Rect(0, 0, Mathf.Max(forceScrollWidth, totalWidth), Mathf.Max(forceScrollHeight, totalHeight)), true, true);
            mScrollPosition.y = (totalHeight < forceScrollHeight ? 0 : mScrollPosition.y);

            Rect rect = new Rect(0, 1, totalWidth, mActorHeight);
            using (Dictionary<EActorType, Actor>.Enumerator itr1 = mCurActionWrapper.ActionActorHash.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    itr1.Current.Value.OnDraw(ref rect, mCurActor == itr1.Current.Value);

                    rect.y += rect.height;
                    using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                    {
                        while (itr2.MoveNext())
                        {
                            itr2.Current.OnDraw(ref rect, mCurActor == itr2.Current);

                            using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                            {
                                while (itr3.MoveNext())
                                {
                                    itr3.Current.OnDraw(ref rect, mCurActor == itr3.Current);
                                }
                            }

                            rect.y += rect.height;
                        }
                    }
                }
            }

            GUI.EndScrollView();

            // draw name
            rect.y = 1 - mScrollPosition.y;
            rect.width = mSkillNameWidth;
            GUILayout.BeginArea(new Rect(0, mToolBarHeight + mActorHeight, mSkillNameWidth, position.height - mToolBarHeight - mActorHeight));
            using (Dictionary<EActorType, Actor>.Enumerator itr1 = mCurActionWrapper.ActionActorHash.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    itr1.Current.Value.OnDrawName(ref rect);

                    rect.y += rect.height;
                    using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                    {
                        while (itr2.MoveNext())
                        {
                            itr2.Current.OnDrawName(ref rect);
                            rect.y += rect.height;
                        }
                    }
                }
            }
            GUILayout.EndArea();
        }

        #region mouse event
        private void OnLeftMouseDown(Vector2 point)
        {
            if (mCurActionWrapper == null) return;

            mCurActor = null;

            using (Dictionary<EActorType, Actor>.Enumerator itr1 = mCurActionWrapper.ActionActorHash.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    if (itr1.Current.Value.IsInActor(point))
                    {
                        itr1.Current.Value.OnLeftMouseDown(point);
                        //mCurActor = itr1.Current.Value;
                        return;
                    }

                    using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                    {
                        while (itr2.MoveNext())
                        {
                            using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                            {
                                while (itr3.MoveNext())
                                {
                                    if (itr3.Current.IsInActor(point))
                                    {
                                        itr3.Current.OnLeftMouseDown(point);
                                        mCurActor = itr3.Current;
                                        return;
                                    }
                                }
                            }

                            if (itr2.Current.IsInActor(point))
                            {
                                //itr2.Current.OnLeftMouseDown(point);
                                mCurActor = itr2.Current;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void OnLeftMouseUp(Vector2 point)
        {

        }

        private void OnLeftMouseDrag(Vector2 point)
        {
            if (mCurActor != null)
            {
                mCurActor.OnLeftMouseDrag(point);
            }
        }

        private void OnLeftMouseDoubleClick(Vector2 point)
        {

        }

        private void OnRightMouseDown(Vector2 point)
        {
            if (mCurActionWrapper == null) return;

            Vector2 pt = mWorkAreaAbsMousePos;
            pt.x *= mZoom;

            using (Dictionary<EActorType, Actor>.Enumerator itr1 = mCurActionWrapper.ActionActorHash.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    if (itr1.Current.Value.IsInActor(pt))
                    {
                        itr1.Current.Value.OnRightMouseDown(point);
                        return;
                    }

                    using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                    {
                        while (itr2.MoveNext())
                        {
                            using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                            {
                                while (itr3.MoveNext())
                                {
                                    if (itr3.Current.IsInActor(pt))
                                    {
                                        itr3.Current.OnRightMouseDown(point);
                                        return;
                                    }
                                }
                            }

                            if (itr2.Current.IsInActor(pt))
                            {
                                itr2.Current.OnRightMouseDown(point);
                                return;
                            }
                        }
                    }
                }
            }

        }

        private void OnRightMouseUp(Vector2 point)
        {

        }

        private void OnMidMouseDrag(Vector2 delta)
        {
            mScrollPosition -= delta * 0.5f;
        }

        #endregion

        private void OnDraw()
        {

        }

        private void Save()
        {

        }

        private void Help()
        {
            Application.OpenURL("https://www.supercline.com/");
        }

        public Texture2D GetCachedTex(Color color, int size = 1)
        {
            if (mCachedTex) DestroyImmediate(mCachedTex);
            if (!mCachedTex)
            {
                mCachedTex = new Texture2D(size, size);
                for (int i = 0; i < size; ++i)
                    for (int j = 0; j < size; ++j)
                        mCachedTex.SetPixel(i, j, color);
                mCachedTex.Apply();
                mCachedTex.hideFlags = HideFlags.HideAndDontSave;
            }

            return mCachedTex;
        }

        private int GetActorCount()
        {
            if (mCurActionWrapper == null) return 0;

            int count = 0;

            using (Dictionary<EActorType, Actor>.Enumerator itr = mCurActionWrapper.ActionActorHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    count += 1;
                    count += itr.Current.Value.ActorList.Count;
                }
            }

            return count;
        }

        private void Preview(float fTick)
        {
            mTime += fTick;

            Tick(fTick);

            if (mTime > mTotalTime)
            {
                mTime = 0f;
                mToolState = EToolState.ETS_None;
                ResetPreview();
            }
        }

        public float WorkTime
        {
            get { return mWorkAreaAbsMousePos.x; }
        }

        public float WorkTimeToPos(float time)
        {
            return time * mZoom;
        }

        public float PosToWorkTime(float pos)
        {
            return pos / mZoom;
        }

    }
}
