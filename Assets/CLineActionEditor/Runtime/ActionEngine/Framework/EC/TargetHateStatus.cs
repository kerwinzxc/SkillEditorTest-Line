/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Runtime\ActionEngine\Framework\EC\TargetHateStatus.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-4      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class TargetHateStatus
    {
        private Dictionary<Unit, float> hateDict = new Dictionary<Unit, float>();
        private List<Unit> handleList = new List<Unit>();
        private List<KeyValuePair<Unit, float>> hateList = new List<KeyValuePair<Unit, float>>();

        public Unit owner
        {
            get;
            set;
        }

        public void Destroy()
        {
            owner = null;
            hateDict.Clear();
            hateList.Clear();
            handleList.Clear();
        }

        public void AddHate(Unit unit, float hateValue)
        {
            float val;
            if (!hateDict.TryGetValue(unit, out val))
            {
                hateDict.Add(unit, 0);
            }

            hateDict[unit] += hateValue;
        }

        public void Update(float fTick)
        {
            handleList.Clear();
            using (var itr = hateDict.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Key.IsDead)
                        handleList.Add(itr.Current.Key);
                }
            }
            using (var itr = handleList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    hateDict.Remove(itr.Current);
                }
            }

            if (owner.Target == null)
            {
                float radius = Helper.GetAny<float>(owner.PropertyContext.GetProperty(PropertyName.sSearchDist));
                Unit[] units = Helper.Search(owner.Position, radius, LayerMask.GetMask("Role"), new Transform[] { owner.UObject }, owner.Camp, true, Helper.Vec3Zero);
                if (units.Length > 0)
                {
                    owner.Target = units[0];
                    AddHate(owner.Target, 10);
                }
                else
                {
                    owner.Target = null;
                }
            }

            if (hateDict.Count > 2)
            {
                hateList.Clear();
                using (var itr = hateDict.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        hateList.Add(new KeyValuePair<Unit, float>(itr.Current.Key, itr.Current.Value));
                    }
                }

                hateList.Sort((lhs, rhs) =>
                {
                    return rhs.Value.CompareTo(lhs.Value);
                });

                float curHateVal = hateDict[owner.Target];
                float highestHateVal = hateList[0].Value;

                if (highestHateVal >= curHateVal * 1.5)
                {
                    owner.Target = hateList[0].Key;
                }
            }

        }

    }
}