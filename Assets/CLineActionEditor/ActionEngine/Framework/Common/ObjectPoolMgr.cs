/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\ObjectPoolMgr.cs
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
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class ObjectPoolMgr : Singleton<ObjectPoolMgr>
    {
        private Dictionary<string, ObjectPool> mPoolHash = new Dictionary<string, ObjectPool>();

        public override void Init()
        {

        }

        public override void Destroy()
        {
            using (var itr = mPoolHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Value.Dispose();
                }
            }

            mPoolHash.Clear();
        }

        public ObjectPool GetPool(string name, int capacity = 0)
        {
            ObjectPool pool = null;
            if (!mPoolHash.TryGetValue(name, out pool))
            {
                pool = new ObjectPool(name, capacity);
                mPoolHash.Add(name, pool);
            }

            return pool;
        }
        
    }
}
