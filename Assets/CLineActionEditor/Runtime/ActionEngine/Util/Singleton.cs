/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Util\Singleton.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2018-9-10        SuperCLine           Created
|
+-----------------------------------------------------------------------------*/


namespace SuperCLine.ActionEngine
{
    using System;

    public abstract class Singleton<T>
    {
        protected static readonly T ms_instance = Activator.CreateInstance<T>();

        public static T Instance
        {
            get
            {
                return Singleton<T>.ms_instance;
            }
        }

        static  Singleton()
        { }

        protected Singleton()
        { }

        public abstract void Init();
        public abstract void Destroy();
    }
}

