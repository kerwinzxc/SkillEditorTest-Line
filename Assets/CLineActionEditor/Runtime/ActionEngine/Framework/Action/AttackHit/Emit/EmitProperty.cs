/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Emit\EmitProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-12      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using System;
    using UnityEngine;

    public enum EEmitType
    {
        EET_None,
        EET_Normal,
        EET_ARC
    }

    public enum EEmitRuleTpye
    {
        EET_Interval,   // 间隔发射
        EET_SameTime,   // 同时发射
        EET_Random,     // 随机发射
        EET_RandomOnNavmesh//在navmesh上随机
    }

    public class EmitProperty : IProperty
    {
        [SerializeField] protected int mNum = 1;
        [SerializeField] protected float mInterval = 0f;
        [SerializeField] protected EEmitRuleTpye mType = EEmitRuleTpye.EET_Interval;

        #region property
        [EditorProperty("发射个数", EditorPropertyType.EEPT_Int)]
        public int Num
        {
            get { return mNum; }
            set { mNum = value; }
        }
        [EditorProperty("发射间隔(s)", EditorPropertyType.EEPT_Float)]
        public float Interval
        {
            get { return mInterval; }
            set { mInterval = value; }
        }
        [EditorProperty("发射规则", EditorPropertyType.EEPT_Enum)]
        public EEmitRuleTpye Type
        {
            get { return mType; }
            set { mType = value; }
        }
        #endregion

        public string DebugName
        {
            get { return string.Empty; }
        }

        public virtual void Deserialize(JsonData jd)
        {
            mNum = JsonHelper.ReadInt(jd["Num"]);
            mInterval = JsonHelper.ReadFloat(jd["Interval"]);
            mType = JsonHelper.ReadEnum<EEmitRuleTpye>(jd["Type"]);
        }

        public virtual JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Num", mNum);
            JsonHelper.WriteProperty(ref writer, "Interval", mInterval);
            JsonHelper.WriteProperty(ref writer, "Type", mType.ToString());

            return writer;
        }
    }

}
