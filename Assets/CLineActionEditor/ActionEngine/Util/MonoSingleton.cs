/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Util\MonoSingleton.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
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
    using UnityEngine;

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T ms_instance = null;

        public static T Instance
        {
            get
            {
                if (ms_instance == null)
                {
                    ms_instance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (ms_instance == null)
                    {
                        ms_instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                        ms_instance.Init();
                    }
                }

                return ms_instance;
            }
        }

        private void Awake()
        {
            if (ms_instance == null)
            {
                ms_instance = this as T;
                ms_instance.Init();
            }
        }

        public virtual void Init() { }

        private void OnApplicationQuit()
        {
            ms_instance = null;
        }
    }

}
