/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\GameConfig.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-17      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

using System;

namespace SuperCLine.ActionEngine
{
    using System.IO;
    using System.Text;
    using System.Collections.Generic;

    public sealed class GameConfig : Singleton<GameConfig>
    {
        private static readonly string mName = "gameconfig";
        private Dictionary<string, string> mConfig = new Dictionary<string, string>();

        public static string CodingKey { get; set; }

        public override void Init()
        {
            CodingKey = "cline";

            string cfg = Path.Combine(FrameCore.Instance.AssetsPath, mName);
            if (Helper.FileExist(cfg))
            {
                byte[] data = Helper.ReadBytes(cfg);
                data = Helper.Coding(data);
                mConfig = Helper.ParseConfig(Helper.BytesToString(data));
            }
        }

        public override void Destroy()
        {
            
        }

        public void Flush()
        {
            StringBuilder sb = new StringBuilder();
            using (var itr = mConfig.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    sb.AppendFormat("{0}={1}\n", itr.Current.Key, itr.Current.Value);
                }
            }

            Helper.WriteBytes(Path.Combine(FrameCore.Instance.AssetsPath, mName),
                Helper.Coding(Helper.StringToBytes(sb.ToString())));
        }

        public string GetString(string key, string defVal = "")
        {
            string val;
            if (!mConfig.TryGetValue(key, out val))
            {
                val = defVal;
            }

            return val;
        }

        public void SetString(string key, string val, bool flush = true)
        {
            mConfig[key] = val;
            if (flush)
                Flush();
        }

        public int GetInt(string key, int defVal = 0)
        {
            int val;
            if (!int.TryParse(GetString(key), out val))
            {
                val = defVal;
            }

            return val;
        }

        public void SetInt(string key, int val, bool flush = true)
        {
            SetString(key, val.ToString(), flush);
        }

        public float GetFloat(string key, float defVal = 0f)
        {
            float val;
            if (!float.TryParse(GetString(key), out val))
            {
                val = defVal;
            }

            return val;
        }

        public void SetFloat(string key, float val, bool flush = true)
        {
            SetString(key, val.ToString(), flush);
        }

        public bool GetBool(string key, bool defVal = false)
        {
            bool val;
            if (!bool.TryParse(GetString(key), out val))
            {
                val = defVal;
            }

            return val;
        }

        public void SetBool(string key, bool val, bool flush = true)
        {
            SetString(key, val.ToString(), flush);
        }

        public bool HasKey(string key)
        {
            return mConfig.ContainsKey(key);
        }

        public void DeleteKey(string key, bool flush = true)
        {
            if (mConfig.ContainsKey(key))
            {
                mConfig.Remove(key);
                if (flush)
                    Flush();
            }
        }

        public void DeleteAll()
        {
            if (mConfig.Count > 0)
            {
                mConfig.Clear();
                Flush();
            }
        }
    }

}
