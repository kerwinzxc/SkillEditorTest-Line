/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\HitAddBuff.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-16      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class HitAddBuff : IProperty, IHitFeedback
    {
        [SerializeField] private List<string> mAddBuffList = new List<string>();

        #region property
        [EditorProperty("BUFF列表", EditorPropertyType.EEPT_List)]
        public List<string> AddBuffList
        {
            get { return mAddBuffList; }
            set { mAddBuffList = value; }
        }
        #endregion property

        public string DebugName
        {
            get { return "HitAddBuff"; }
        }

        public void Deserialize(JsonData jd)
        {
            mAddBuffList = JsonHelper.ReadListString(jd["AddBuffList"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "AddBuffList", mAddBuffList);

            return writer;
        }

        public EHitFeedbackType FeedbackType
        {
            get { return EHitFeedbackType.EHT_HitAddBuff; }
        }

        public void OnHitFeedback(Unit attacker, Unit attackee, params object[] param)
        {
            attackee.AddBuff(mAddBuffList);
        }
    }
}