/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Common\Helper.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using UnityEngine;

    public static partial class Helper
    {
        public static string GetRootDirectory()
        {
            string[] directories = Directory.GetDirectories("Assets", "CLineActionEditor", SearchOption.AllDirectories);
            return directories.Length > 0 ? directories[0] : string.Empty;
        }

        public static EventModifiers actionModifier
        {
            get
            {
                if (Application.platform == RuntimePlatform.OSXEditor ||
                    Application.platform == RuntimePlatform.OSXPlayer)
                    return EventModifiers.Command;

                return EventModifiers.Control;
            }
        }

        public static bool CanClearSelection(Event evt)
        {
            return !evt.control && !evt.command && !evt.shift;
        }

        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private static readonly int maxDecimals = 15;
        public static int GetNumberOfDecimalsForMinimumDifference(float minDifference)
        {
            return Mathf.Clamp(-Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(minDifference))), 0, maxDecimals);
        }

        public static void CreateDirectory(string path, bool del = false)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                if (del)
                {
                    Directory.Delete(path, true);
                    Directory.CreateDirectory(path);
                }
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static void MoveFile(string srcPath, string destPath)
        {
            if (srcPath == destPath)
                return;

            if (!File.Exists(srcPath))
                return;

            DeleteFile(destPath);

            CreateDirectory(Path.GetDirectoryName(destPath));
            File.Move(srcPath, destPath);
        }

        public static object GetProperty(object obj, string propertyName)
        {
            return obj.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, obj, null);
        }

        public static void SetProperty(object obj, string propertyName, object newValue)
        {
            obj.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, obj, new object[] { newValue });
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