/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\HitAttackerSpeed.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-19      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class HitAttackerSpeed : IProperty, IHitFeedback
    {
        public string DebugName
        {
            get { return "HitAttackerSpeed"; }
        }

        public void Deserialize(JsonData jd)
        {

        }

        public JsonWriter Serialize(JsonWriter writer)
        {

            return writer;
        }

        public EHitFeedbackType FeedbackType
        {
            get { return EHitFeedbackType.EHT_HitAttackerSpeed; }
        }

        public void OnHitFeedback(Unit attacker, Unit attackee, params object[] param)
        {
            if (attacker.IsDead)
            {
                return;
            }

            Helper.SetAny<bool>(attacker.PropertyContext.GetProperty(PropertyName.sHitAttackerSpeed),  true);
            //if (attacker.IsDead || string.IsNullOrEmpty(mActionID))
            //{
            //    return;
            //}

            //attacker.ActionStatus.ChangeAction(mActionID);

            //Vector3 dir = attacker.Position - attackee.Position;
            //dir.y = 0f;
            //dir = dir.normalized * mXOZSpeed;
            //dir.y = mYSpeed;
            //attackee.ActionStatus.SetVelocity(dir);
        }

    }
}