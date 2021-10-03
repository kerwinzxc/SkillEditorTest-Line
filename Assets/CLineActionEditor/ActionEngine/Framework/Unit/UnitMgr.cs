/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\UnitMgr.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-4      CLine           Created
|
+-----------------------------------------------------------------------------*/

using System;

namespace CAE.Core
{
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class UnitMgr : Singleton<UnitMgr>
    {
        private LinkedList<Unit> mAddList = new LinkedList<Unit>();
        private LinkedList<Unit> mUpdateList = new LinkedList<Unit>();
        private LinkedList<Unit> mHandleList = new LinkedList<Unit>();

        public LocalPlayer LocalPlayer;

        public override void Init()
        {
            
        }

        public override void Destroy()
        {
            using (var itr = mAddList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }
            mAddList.Clear();

            using (var itr = mUpdateList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }
            mUpdateList.Clear();
        }

        public Unit CreateUnit(Unit.EUnitType type, string resID, Vector3 pos, float yaw, ECampType campType)
        {
            Unit unit = null;
            switch (type)
            {
                case Unit.EUnitType.EUT_LocalPlayer:
                    {
                        LocalPlayer = new LocalPlayer();
                        unit = LocalPlayer as Unit;
                    }
                    break;
                case Unit.EUnitType.EUT_Player:
                    {
                        unit = new Player();
                    }
                    break;
                case Unit.EUnitType.EUT_Monster:
                    {
                        unit = new Monster();
                    }
                    break;
            }

            unit.Init(resID, pos, yaw, campType);
            unit.UpdateAttributes(null);

            mAddList.AddLast(unit);

            return unit;
        }

        public void DestroyUnit(Unit unit)
        {
            if (unit == LocalPlayer)
                LocalPlayer = null;

            unit.Dispose();

            mUpdateList.Remove(unit);
        }

        public void Update(float fTick)
        {
            using (var itr = mAddList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mUpdateList.AddLast(itr.Current);
                }
            }
            mAddList.Clear();

            using (var itr = mUpdateList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (!itr.Current.IsDeleted)
                        itr.Current.Update(fTick);
                    else
                        mHandleList.AddLast(itr.Current);
                }
            }

            using (var itr = mHandleList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    DestroyUnit(itr.Current);
                }
            }
            mHandleList.Clear();
        }

        public void FixedUpdate(float fTick)
        {

        }

        public void LateUpdate(float fTick)
        {

        }

    }
}
