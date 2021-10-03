/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\XObject.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-17      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using System;

    public abstract class XObject : IDisposable
    {
        private bool mDisposed = false;

        public string DebugName { get; set; }

        ~XObject()
        {
            if (!mDisposed)
                Dispose(false);
        }

        public void Dispose()
        {
            if (!mDisposed)
                Dispose(true);
        }

        protected abstract void OnDispose();

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                mDisposed = true;
                OnDispose();
                GC.SuppressFinalize(this);
            }
            else
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "XObject", "Memory leak!!!(Type: {0}), (Instance: {1})",
                    this.GetType().ToString(), DebugName);
            }
        }
    }
}
