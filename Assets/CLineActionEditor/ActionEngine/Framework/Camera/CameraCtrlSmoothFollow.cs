/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Camera\CameraCtrlSmoothFollow.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-3      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEngine;

    public sealed class CameraCtrlSmoothFollow : XObject, ICameraController
    {
        private UCameraCtrlSmoothFollow uCtrl = null;

        private Vector3 velocityXOZ = Vector3.zero;
        private Vector3 velocityY = Vector3.zero;
        private Vector3 toPostion = Vector3.zero;
        private Vector3 xozFrom = Vector3.zero;
        private Vector3 yFrom = Vector3.zero;
        private Vector3 xozTo = Vector3.zero;
        private Vector3 yTo = Vector3.zero;
        private Vector3 xozDamp = Vector3.zero;
        private Vector3 yDamp = Vector3.zero;

        public void Init(params object[] param)
        {
            uCtrl = CameraMgr.Instance.Camera.gameObject.GetComponent<UCameraCtrlSmoothFollow>();
            if (uCtrl == null)
                uCtrl = CameraMgr.Instance.Camera.gameObject.AddComponent<UCameraCtrlSmoothFollow>();

            uCtrl.enabled = true;

            if (param.Length >= 1)
                uCtrl.follow = param[0] as Transform;

            if (param.Length >= 2)
                uCtrl.smoothXOZ = (float)param[1];

            if (param.Length >= 3)
                uCtrl.smoothY = (float)param[2];

            if (param.Length >= 4)
                uCtrl.offset = (Vector3)param[3];

            if (param.Length >= 5)
                uCtrl.euler = (Vector3)param[4];

            
            CameraMgr.Instance.Camera.position = uCtrl.follow.position + uCtrl.offset;
            CameraMgr.Instance.Camera.rotation = Quaternion.Euler(uCtrl.euler);
        }

        protected override void OnDispose()
        {

        }

        public ECameraCtrlType GetCtrlType()
        {
            return ECameraCtrlType.ECCT_SmoothFollow;
        }

        public void Reset()
        {
            uCtrl.enabled = false;
        }

        
        public void LateUpdate(CameraMgr mgr, float fTick)
        {
            toPostion = uCtrl.follow.position + uCtrl.offset;

            xozFrom = new Vector3(mgr.Camera.position.x, 0, mgr.Camera.position.z);
            yFrom = new Vector3(0, mgr.Camera.position.y, 0);
            xozTo = new Vector3(toPostion.x, 0, toPostion.z);
            yTo = new Vector3(0, toPostion.y, 0);

            xozDamp = Vector3.SmoothDamp(xozFrom, xozTo, ref velocityXOZ, uCtrl.smoothXOZ);
            yDamp = Vector3.SmoothDamp(yFrom, yTo, ref velocityY, uCtrl.smoothY);

            mgr.Camera.position = new Vector3(xozDamp.x, yDamp.y, xozDamp.z);
        }
    }

}
