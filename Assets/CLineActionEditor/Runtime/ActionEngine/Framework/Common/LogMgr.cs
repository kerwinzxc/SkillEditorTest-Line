/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\LogMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2018-9-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public sealed class LogMgr : Singleton<LogMgr>, ILogMgr
    {
        private bool mLogEnable = true;
        private bool mLogUI = false;
        private bool mLogDisk = false;
        private bool mLogTime = false;
        private bool mLogServer = false;

        private List<string> mLogUIContent = new List<string>();
        private Dictionary<string, bool> mLogTag = new Dictionary<string, bool>();

        public override void Init()
        {
            mLogEnable = GameConfig.Instance.GetBool("LogEnable", true);

            mLogTag["Unit"] = true;
            mLogTag["Action"] = true;
            mLogTag["Msg"] = true;
            mLogTag["UI"] = true;
            mLogTag["Property"] = true;
            mLogTag["Core"] = true;
            mLogTag["ActionEditor"] = true;
            mLogTag["Camera"] = true;
            mLogTag["Effect"] = true;
            mLogTag["ResourceMgr"] = true;
            mLogTag["ObjectPool"] = true;
            mLogTag["XObject"] = true;
        }

        public override void Destroy()
        {
            
        }

        public void Log(ELogType type, string tag, string log)
        {
            if (!CheckLog(tag))
                return;

            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendFormat("[{0}] {1}", tag, log);
            log = sbLog.ToString();

            if (mLogTime)
            {
                StringBuilder sbTime = new StringBuilder();
                sbTime.AppendFormat("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log);
                log = sbTime.ToString();
            }

            switch (type)
            {
                case ELogType.ELT_DEBUG:
                    Debug.Log(log);
                    break;
                case ELogType.ELT_WARNING:
                    Debug.LogWarning(log);
                    break;
                case ELogType.ELT_ERROR:
                    Debug.LogError(log);
                    break;
            }

            if (mLogUI)
            {
                if (mLogUIContent.Count >= 30)
                    mLogUIContent.Clear();

                mLogUIContent.Add(log);
            }
        }

        public void Logf(ELogType type, string tag, string format, params object[] objs)
        {
            if (!CheckLog(tag))
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(format, objs);

            Log(type, tag, sb.ToString());
        }

        private bool CheckLog(string tag)
        {
            if (!mLogEnable)
                return false;

            bool show;
            if (!mLogTag.TryGetValue(tag, out show) || !show)
                return false;

            return true;
        }

        public void OnGUI()
        {
            for (int i = 0; i < mLogUIContent.Count; ++i)
            {
                GUI.Label(new Rect(0, i * 22 + 100, 300, 20), mLogUIContent[i]);
            }
        }
    }
}
