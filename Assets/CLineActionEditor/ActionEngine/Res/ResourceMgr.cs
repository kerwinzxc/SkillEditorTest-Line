/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Res\ResourceMgr.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : resource name format[/gamedata/xx.prefab], and case-insensitive
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2018-9-10        CLine           Created
|
+-----------------------------------------------------------------------------*/


namespace CAE.Core
{
    using System.IO;
    using System.Collections.Generic;

    public sealed class ResourceMgr : Singleton<ResourceMgr>
    {
        public static readonly string ms_ResRoot = "assets/clineactioneditor/demo/resource";
        public Dictionary<string, string> m_FileHash = new Dictionary<string, string>();

        public override void Init()
        {
            BuildResourceMap();
        }

        public override void Destroy()
        {
            
        }

        public UnityEngine.Object LoadObject(string fileName, System.Type type)
        {
            fileName = fileName.ToLower();

            string asset = string.Empty;
            if (FindResource(fileName, out asset))
            {
                UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(asset, type);
                if (null == obj)
                {
                    LogMgr.Instance.Logf(ELogType.ELT_ERROR, "ResourceMgr", "Failed to load asset: {0}", fileName);
                }
                return obj;
            }
            else
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "ResourceMgr", "The resource '{0}' is not exist! ", fileName);
                return null;
            }
        }

        public T LoadObject<T>(string fileName) where T : UnityEngine.Object
        {
            fileName = fileName.ToLower();

            string asset = string.Empty;
            if (FindResource(fileName, out asset))
            {
                T obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(asset);
                if (null == obj)
                {
                    LogMgr.Instance.Logf(ELogType.ELT_ERROR, "ResourceMgr", "Failed to load asset: {0}", fileName);
                }
                return obj;
            }
            else
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "ResourceMgr", "The resource '{0}' is not exist! ", fileName);
                return null;
            }
        }

        public string FormatResourceName(string name)
        {
            string formatName = name.ToLower();
            return formatName.Replace(ms_ResRoot, "");
        }

        private void BuildResourceMap()
        {
            string abRoot = Path.Combine(Directory.GetCurrentDirectory(), ms_ResRoot);
            if (!Directory.Exists(abRoot))
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "ResourceMgr", "The directory of resource is not exist!");

            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Texture t:TextAsset t:GameObject t:Shader t:Font", new string[] { ms_ResRoot });
            for (int i=0; i<guids.Length; ++i)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]).ToLower();
                if (File.Exists(assetPath))
                {
                    string fileName = assetPath.Replace(ms_ResRoot, "");
                    m_FileHash[fileName] = assetPath;
                }
            }
        }

        public bool FindResource(string name, out string asset)
        {
            if (m_FileHash.TryGetValue(name.ToLower(), out asset))
                return true;

            return false;
        }


        public string[] GetFiles(string path)
        {
            List<string> fileList = new List<string>();

            using (var itr = m_FileHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Key.Contains(path.ToLower()))
                    {
                        fileList.Add(itr.Current.Key);
                    }
                }
            }

            return fileList.ToArray();
        }

    }

}
