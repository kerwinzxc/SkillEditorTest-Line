/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\FrameEntity.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-16      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public sealed class FrameEntity : AttackEntity
    {
        public FrameEntity(AttackHit ah, AttackEntityProperty property, Vector3 pos, Vector3 dir)
            : base(ah, property, pos, dir)
        {

        }

        public override bool Update(float fTick)
        {
            if (!base.Update(fTick)) return false;

            Unit[] unitList = null;
            int layer = LayerMask.GetMask("Role");

            switch (AH.Data.EntityType)
            {
                case EEntityType.EET_FrameSphere:
                    {
                        FrameEntitySphereProperty p = Property as FrameEntitySphereProperty;
                        unitList = Helper.Search(StartPosition, p.Radius, layer, new Transform[] { AH.Owner.UObject.transform }, AH.Owner.Camp, false, Vector3.zero);
                    }
                    break;
                case EEntityType.EET_FrameFan:
                    {
                        FrameEntityFanProperty p = Property as FrameEntityFanProperty;
                        unitList = Helper.Search(StartPosition, p.Radius, layer, new Transform[] { AH.Owner.UObject.transform }, AH.Owner.Camp, false, StartDirection, p.Degree, true);
                    }
                    break;
                case EEntityType.EET_FrameCylinder:
                    {
                        FrameEntityCylinderProperty p = Property as FrameEntityCylinderProperty;
                        unitList = Helper.Search(StartPosition, p.Radius, layer, new Transform[] { AH.Owner.UObject.transform }, AH.Owner.Camp, false, Vector3.zero);
                    }
                    break;
                case EEntityType.EET_FrameCub:
                    break;
                case EEntityType.EET_FrameRing:
                    break;
            }

            if (unitList.Length != 0)
            {
                AH.TargetList.AddRange(unitList);
            }

            IsDead = true;

            return true;
        }

    }

}
