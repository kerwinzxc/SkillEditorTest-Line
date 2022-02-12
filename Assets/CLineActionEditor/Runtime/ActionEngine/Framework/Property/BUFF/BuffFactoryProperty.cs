/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\BuffFactoryProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-15      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class BuffFactoryProperty : IProperty
    {
        [SerializeField] private EBuffType mBuffType = EBuffType.EBT_None;
        [SerializeReference] private BuffProperty mBuff = null;

        #region property
        [EditorProperty("BUFF类型", EditorPropertyType.EEPT_Enum)]
        public EBuffType BuffType
        {
            get { return mBuffType; }
            set
            {
                if (mBuffType != value)
                {
                    mBuffType = value;
                    switch (mBuffType)
                    {
                        case EBuffType.EBT_NumericalBuff:
                            mBuff = new NBuffProperty();
                            break;
                        case EBuffType.EBT_DeltaBuff:
                            mBuff = new DBuffProperty();
                            break;
                        case EBuffType.EBT_ConditionBuff:
                            mBuff = new CBuffProperty();
                            break;
                        case EBuffType.EBT_SpecialBuffHP:
                            mBuff = new SBuffHPProperty();
                            break;
                        case EBuffType.EBT_SpecialBuffDizzy:
                            mBuff = new SBuffDizzyProperty();
                            break;
                        case EBuffType.EBT_SpecialBuffGodMode:
                            mBuff = new SBuffGodModeProperty();
                            break;
                        case EBuffType.EBT_SpecialBuffResetSkillCD:
                            mBuff = new BuffProperty();
                            break;
                        case EBuffType.EBT_SpecialBuffSummon:
                            mBuff = new SBuffSummonProperty();
                            break;
                    }
                }
            }
        }
        #endregion property

        public string DebugName
        {
            get { return mBuff != null ? mBuff.Name : "noname" + System.DateTime.Now.ToString("yyyyMMddhhmmss"); }
        }

        public string ID
        {
            get { return mBuff != null ? mBuff.ID : string.Empty; }
        }

        public BuffProperty BuffProperty
        {
            get { return mBuff; }
        }

        public void Deserialize(JsonData jd)
        {
            BuffType = JsonHelper.ReadEnum<EBuffType>(jd["BuffType"]);
            mBuff.Deserialize(jd["BuffData"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WriteObjectStart();
            JsonHelper.WriteProperty(ref writer, "BuffType", mBuffType.ToString());

            writer.WritePropertyName("BuffData");
            writer.WriteObjectStart();
            writer = mBuff.Serialize(writer);
            writer.WriteObjectEnd();

            writer.WriteObjectEnd();

            return writer;
        }
    }
}