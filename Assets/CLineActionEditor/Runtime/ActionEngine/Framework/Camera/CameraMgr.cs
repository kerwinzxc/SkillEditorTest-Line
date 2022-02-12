/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Camera\CameraMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-3      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
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

        private Dictionary<ECameraCtrlType, ICameraController> mCtrlHash = new Dictionary<ECameraCtrlType, ICameraController>();
        private Dictionary<ECameraCtrlType, ICameraApplyer> mApplyerHash = new Dictionary<ECameraCtrlType, ICameraApplyer>();

        #region property
        public Camera Camera
        {
            get;
            set;
        }
        public ICameraController Ctrl
        {
            get;
            set;
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

            using (var itr = mCtrlHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    ICameraApplyer applyer = itr.Current.Value as ICameraApplyer;
                    if (applyer != null)
                    {
                        mApplyerHash.Add(itr.Current.Key, applyer);
                    }
                }
            }
        }

        public override void Destroy()
        {
            Camera = null;
            Ctrl = null;

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

        public void BindCamera()
        {
            BindCamera(Camera.main);
        }

        public void BindCamera(Camera camera)
        {
            this.Camera = camera;
        }

        public void BindCtrl(ECameraCtrlType type, params object[] param)
        {
            if (null != Ctrl)
            {
                Ctrl.Reset();
                Ctrl = null;
            }

            if (type == ECameraCtrlType.ECCT_None) return;

            ICameraController ctrl;
            if (mCtrlHash.TryGetValue(type, out ctrl))
            {
                Ctrl = ctrl;
                Ctrl.Init(param);
                return;
            }

            LogMgr.Instance.Log(ELogType.ELT_ERROR, "Camera", "Error camera type!!!");
        }

        public void Reset()
        {
            Camera = null;

            if (null != Ctrl)
            {
                Ctrl.Reset();
                Ctrl = null;
            }

        }

        public void LateUpdate(float fTick)
        {
            if (null == Camera)
                return;

            if (null == Ctrl)
                return;


            Ctrl.LateUpdate(this, fTick);

            using (var itr = mApplyerHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Value.Apply(this, fTick);
                }
            }

        }

        public void AddShakeEffect(float duration, float intensity, int vibrato, bool disableX, bool disableY, bool attenuation)
        {
            CameraCtrlShake cs = mCtrlHash[ECameraCtrlType.ECCT_Shake] as CameraCtrlShake;
            if (cs != null)
            {
                cs.Init(duration, intensity, vibrato, disableX, disableY, attenuation);
            }
        }

    }

}
