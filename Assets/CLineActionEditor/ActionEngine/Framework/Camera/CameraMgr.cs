/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Camera\CameraMgr.cs
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
    using System.Text;
    using System.Collections.Generic;

    public sealed class CameraMgr : Singleton<CameraMgr>
    {
        private delegate ICameraController CreateController();
        private static CreateController[] mCreateCtrls =
        {
            () => {return new CameraCtrlSmoothFollow();},
            () => {return new CameraCtrlShake();},
        };

        private Transform mCurCamera = null;
        private ICameraController mCurCtrl = null;

        private Dictionary<ECameraCtrlType, ICameraController> mCtrlHash = new Dictionary<ECameraCtrlType, ICameraController>();
        private Dictionary<ECameraCtrlType, ICameraApplyer> mApplyerHash = new Dictionary<ECameraCtrlType, ICameraApplyer>();

        #region property
        public Transform Camera
        {
            get { return mCurCamera; }
        }
        public ICameraController Ctrl
        {
            get { return mCurCtrl; }
        }
        public Dictionary<ECameraCtrlType, ICameraController> CtrlHash
        {
            get { return mCtrlHash; }
        }
        #endregion


        public override void Init()
        {
            for (int i = 0; i < mCreateCtrls.Length; ++i)
            {
                ICameraController ctrl = mCreateCtrls[i]();
                mCtrlHash[ctrl.GetCtrlType()] = ctrl;
            }

            mApplyerHash[ECameraCtrlType.ECCT_Shake] = mCtrlHash[ECameraCtrlType.ECCT_Shake] as ICameraApplyer;
        }

        public override void Destroy()
        {
            mCurCamera = null;
            mCurCtrl = null;

            mApplyerHash.Clear();
            using (var itr = mCtrlHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    XObject ob = itr.Current.Value as XObject;
                    ob.Dispose();
                }
            }
            mCtrlHash.Clear();
        }

        public void BindCamera(Transform camera)
        {
            mCurCamera = camera;
        }

        public void BindCtrl(ECameraCtrlType type, params object[] param)
        {
            if (null != mCurCtrl)
            {
                mCurCtrl.Reset();
                mCurCtrl = null;
            }

            if (type == ECameraCtrlType.ECCT_None) return;

            if (mCtrlHash.TryGetValue(type, out mCurCtrl))
            {
                mCurCtrl.Init(param);
                return;
            }

            LogMgr.Instance.Log(ELogType.ELT_ERROR, "Camera", "Error camera type!!!");
        }

        public void Reset()
        {
            mCurCamera = null;

            if (null != mCurCtrl)
            {
                mCurCtrl.Reset();
                mCurCtrl = null;
            }

        }

        public void LateUpdate(float fTick)
        {
            if (null == mCurCamera)
                return;

            if (null == mCurCtrl)
                return;


            mCurCtrl.LateUpdate(this, fTick);

            using (var itr = mApplyerHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Value.Apply(this, fTick);
                }
            }

        }

        public void AddShakeEffect(float duration = 0.2f, float shakeIntensity = 0.7f, bool disableX = true, bool disableY = false)
        {
            CameraCtrlShake cs = mCtrlHash[ECameraCtrlType.ECCT_Shake] as CameraCtrlShake;
            if (cs != null)
            {
                cs.Init(duration, disableX, disableY);
                cs.StartShake(shakeIntensity);
            }
        }

    }

}
