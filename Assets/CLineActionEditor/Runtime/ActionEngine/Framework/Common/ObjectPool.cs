/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\ObjectPool.cs
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
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class ObjectPool : XObject
    {
        private LinkedList<GameObject> mPool = new LinkedList<GameObject>();
        private string mName = string.Empty;
        private GameObject mPrefab = null;

        public ObjectPool(string name, int capacity)
        {
            mName = name;
            mPrefab = ResourceMgr.Instance.LoadObject<GameObject>(name);

            if (null == mPrefab)
            {
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "ObjectPool", "Prefab is not exist!!! " + mName);
                return;
            }

            for (int i = 0; i < capacity; ++i)
            {
                GameObject obj = GameObject.Instantiate(mPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                obj.SetActive(false);
                mPool.AddLast(obj);
            }
        }

        public GameObject Get(bool active = true)
        {
            return Get(Vector3.zero, Quaternion.identity, active);
        }

        public GameObject Get(Vector3 pos, Quaternion rot, bool active = true)
        {
            if (mPool.Count > 0)
            {
                GameObject obj = mPool.First.Value;
                mPool.RemoveFirst();

                obj.transform.position = pos;
                obj.transform.rotation = rot;
                obj.SetActive(active);

                return obj;
            }
            else
            {
                return GameObject.Instantiate(mPrefab, pos, rot) as GameObject;
            }
        }

        public void Cycle(GameObject obj)
        {
            if (null != obj)
            {
                //obj.transform.parent = null;
                obj.transform.SetParent(null);
                obj.SetActive(false);

                mPool.AddLast(obj);
            }
        }

        protected override void OnDispose()
        {
            mPrefab = null;
            using (var itr = mPool.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (null != itr.Current)
                        GameObject.DestroyImmediate(itr.Current);
                }
            }
            mPool.Clear();
        }
    }
}
