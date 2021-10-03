/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\EditorUtil.cs
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
    using System.IO;
    using System.Reflection;
    using UnityEngine;
    using System.Collections.Generic;

    public static class EditorUtil
    {
        public static float TimeToMillisecond = 1000f;
        public static float TimeToSecond = 0.001f;

        public static object GetProperty(object obj, string propertyName)
        {
            return obj.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, obj, null);
        }

        public static void SetProperty(object obj, string propertyName, object newValue)
        {
            obj.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, obj, new object[] { newValue });
        }

        public static string ResPath
        {
            get { return Application.dataPath + @"/CLineActionEditor/Demo/Resource/"; }
        }

        public static string GetAssetPath(string searchPattern, SearchOption searchOption)
        {
            DirectoryInfo dif = new DirectoryInfo(ResPath);
            FileInfo[] fifs = dif.GetFiles(searchPattern, SearchOption.AllDirectories);
            if (fifs.Length == 0)
                return string.Empty;

            string szPath = string.Empty;
            for (int i = 0; i < fifs.Length; ++i)
            {
                if (fifs[i].FullName.Contains(".meta"))
                    continue;

                szPath = fifs[0].FullName.Substring(Application.dataPath.Length + 1);
                szPath = szPath.Replace('\\', '/');
                szPath = "Assets/" + szPath;
                break;
            }

            return szPath;
        }

        public static bool IsPointInRect(ref Vector2 pt, ref Rect rc)
        {
            return (pt.x <= rc.x + rc.width) && (pt.x >= rc.x) &&
                   (pt.y <= rc.y + rc.height) && (pt.y >= rc.y);
        }

        public static int GetStringIndex(List<string> list, string val)
        {
            int idx = -1;

            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i] == val)
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }
    }
}
