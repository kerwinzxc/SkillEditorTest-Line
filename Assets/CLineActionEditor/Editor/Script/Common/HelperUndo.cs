/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Common\HelperUndo.cs
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
    using UnityEngine;
    using System.Diagnostics;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    public static partial class Helper
    {
        public static void PushDestroyUndo(Object thingToDirty, Object objectToDestroy)
        {
#if UNITY_EDITOR
            if (objectToDestroy == null || !DisableUndoGuard.enableUndo)
                return;

            if (thingToDirty != null)
                EditorUtility.SetDirty(thingToDirty);

            Undo.DestroyObjectImmediate(objectToDestroy);
#else
            if (objectToDestroy != null)
                Object.Destroy(objectToDestroy);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void PushUndo(Object[] thingsToDirty, string operation)
        {
#if UNITY_EDITOR
            if (thingsToDirty == null || !DisableUndoGuard.enableUndo)
                return;

            for (var i = 0; i < thingsToDirty.Length; i++)
            {
                EditorUtility.SetDirty(thingsToDirty[i]);
            }
            Undo.RegisterCompleteObjectUndo(thingsToDirty, UndoName(operation));
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void PushUndo(Object thingToDirty, string operation)
        {
#if UNITY_EDITOR
            if (thingToDirty != null && DisableUndoGuard.enableUndo)
            {
                EditorUtility.SetDirty(thingToDirty);
                Undo.RegisterCompleteObjectUndo(thingToDirty, UndoName(operation));
            }
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void RegisterCreatedObjectUndo(Object thingCreated, string operation)
        {
#if UNITY_EDITOR
            if (DisableUndoGuard.enableUndo)
            {
                Undo.RegisterCreatedObjectUndo(thingCreated, UndoName(operation));
            }
#endif
        }

        private static string UndoName(string name) => "CAE " + name;

#if UNITY_EDITOR
        internal struct DisableUndoGuard : System.IDisposable
        {
            internal static bool enableUndo = true;
            static readonly Stack<bool> undoStateStack = new Stack<bool>();
            bool disposed;
            public DisableUndoGuard(bool disable)
            {
                disposed = false;
                undoStateStack.Push(enableUndo);
                enableUndo = !disable;
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    if (undoStateStack.Count == 0)
                    {
                        UnityEngine.Debug.LogError("UnMatched DisableUndoGuard calls");
                        enableUndo = true;
                        return;
                    }
                    enableUndo = undoStateStack.Pop();
                    disposed = true;
                }
            }
        }
#endif
    }
}