/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Interface\ICameraController.cs
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
    public enum ECameraCtrlType
    {
        ECCT_None,
        ECCT_SmoothFollow,
        ECCT_Shake,
    }

    public interface ICameraController
    {
        ECameraCtrlType GetCtrlType();
        void Init(params object[] param);
        void Reset();
        void LateUpdate(CameraMgr mgr, float fTick);
    }

    public interface ICameraApplyer
    {
        void Apply(CameraMgr mgr, float fTick);
    }

}
