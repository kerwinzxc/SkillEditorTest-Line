/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\FrameCore.cs
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
    using UnityEngine;
    using System.IO;

    public sealed class FrameCore : Singleton<FrameCore>
    {
        public override void Init()
        {
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        }

        public override void Destroy()
        {
            
        }

        public float TimeScale
        {
            get { return Time.timeScale; }
            set { Time.timeScale = value; }
        }

        public string StreamingAssetsPath
        {
            get { return Application.streamingAssetsPath; }
        }

        public string AssetsPath
        {
            get { return Path.Combine(Application.persistentDataPath, Application.identifier); }
        }

        public string CachePath
        {
            get { return Path.Combine(AssetsPath, "Cache"); }
        }

        public string DownloadPath
        {
            get { return Path.Combine(AssetsPath, "Download"); }
        }

        public string LogPath
        {
            get { return Path.Combine(AssetsPath, "Log"); }
        }

    }
}
