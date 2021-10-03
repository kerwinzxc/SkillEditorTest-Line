/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorGroupCamera.cs
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

    public class ActorGroupCamera : Actor
    {
        public ActorGroupCamera()
        {
            mColor = new Color(0f, 0.8f, 0.8f, 1f);
            mName = "相机组";
            mNameOffsetX = 30f;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_GroupCamera; }
        }

        public override Actor Clone()
        {
            ActorGroupCamera aa = ActorGroupCamera.CreateInstance<ActorGroupCamera>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/Camera/New Camera")]
        public static void NewCamera(MenuCommand cmd)
        {
            ActorGroupCamera groupCamera = (ActorGroupCamera)cmd.context;

            ActorCamera camera = ActorCamera.CreateInstance<ActorCamera>();
            camera.Parent = groupCamera;
            camera.ID = groupCamera.GetActorID();
            groupCamera.mActorList.Add(camera);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Camera/", new MenuCommand(this));
        }
    }
}
