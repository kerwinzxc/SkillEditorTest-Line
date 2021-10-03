/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorCamera.cs
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
    using UnityEngine;
    using UnityEditor;

    public class ActorCamera : Actor
    {
        public ActorCamera()
        {

        }

        public override Actor Clone()
        {
            ActorCamera aa = ActorCamera.CreateInstance<ActorCamera>();
            aa.Clone(this);
            return aa;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_Camera; }
        }

        [MenuItem("CONTEXT/CameraEvt/Add Event")]
        public static void AddEvent(MenuCommand cmd)
        {
            ActorCamera camera = (ActorCamera)cmd.context;

            ActorEvent evt = ActorEvent.CreateInstance<ActorEvent>();
            evt.Init(camera, ActionWindow.Instance.WorkTime);
            camera.ActorList.Add(evt);
        }

        [MenuItem("CONTEXT/CameraEvt/Del Camera")]
        public static void DelCamera(MenuCommand cmd)
        {
            ActorCamera camera = (ActorCamera)cmd.context;

            ActorGroupCamera groupCamera = camera.Parent as ActorGroupCamera;
            groupCamera.ActorList.Remove(camera);
            if (camera.ID == groupCamera.ID && groupCamera.ActorList.Count == 0)
                groupCamera.ID--;

            EditorUtility.SetDirty(groupCamera);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/CameraEvt/", new MenuCommand(this));
        }

        public override void OnDraw(ref Rect rect, bool active)
        {
            base.OnDraw(ref rect, active);

            GUI.color = new Color(0.4f, 0.45f, 0.6f, 1f);
            if (!active)
                GUI.color *= new Color(0.75f, 0.75f, 0.75f, 1f);

            mName = ID.ToString();
            if (mActorList.Count != 0)
            {
                Rect rc = rect;
                rc.x = ActionWindow.Instance.WorkTimeToPos(mActorList[0].StartTime);
                rc.width = ActionWindow.Instance.WorkTimeToPos(0.5f);

                GUI.Box(rc, "", ActionWindow.Instance.EditorSkin.GetStyle("Actor"));
            }
        }

    }
}
