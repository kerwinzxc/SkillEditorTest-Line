/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Runtime\ActionEngine\Framework\EC\TargetStatus.cs
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

    public sealed class TargetStatus
    {
        public Unit owner
        {
            get;
            set;
        }

        public void Destroy()
        {
            owner = null;
        }

        public void Update(float fTick)
        {
            if (owner.HasInputStatus())
            {
                owner.Target = null;
            }
            else
            {
                float radius = Helper.GetAny<float>(owner.PropertyContext.GetProperty(PropertyName.sSearchDist));
                Unit[] units = Helper.Search(owner.Position, radius, LayerMask.GetMask("Role"), new Transform[] { owner.UObject }, owner.Camp, true, Helper.Vec3Zero);
                if (units.Length > 0)
                {
                    owner.Target = units[0];
                }
                else
                {
                    owner.Target = null;
                }
            }
        }
    }
}