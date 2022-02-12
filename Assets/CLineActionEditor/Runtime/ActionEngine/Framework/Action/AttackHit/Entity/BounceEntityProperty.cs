/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\BounceEntityProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-17      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections;

    public class BounceEntityProperty : AttackEntityProperty
    {
        [SerializeField] private string mModelPrefab = string.Empty;
        [SerializeField] private string mEntityEndEffect = string.Empty;
        [SerializeField] private int mBounceTims = 1;
        [SerializeField] private float mBounceRandomRadius = 0f;

        #region property
        [EditorProperty("攻击体模型 ", EditorPropertyType.EEPT_GameObject)]
        public string ModelPrefab
        {
            get { return mModelPrefab; }
            set { mModelPrefab = value; }
        }
        [EditorProperty("攻击体结束特效 ", EditorPropertyType.EEPT_GameObject)]
        public string EntityEndEffect
        {
            get { return mEntityEndEffect; }
            set { mEntityEndEffect = value; }
        }
        [EditorProperty("弹射次数 ", EditorPropertyType.EEPT_Int)]
        public int BounceTimes
        {
            get { return mBounceTims; }
            set { mBounceTims = value; }
        }
        [EditorProperty("弹射随机范围 ", EditorPropertyType.EEPT_Float)]
        public float BounceRandomRadius
        {
            get { return mBounceRandomRadius; }
            set { mBounceRandomRadius = value; }
        }
        #endregion

        public override void Deserialize(LitJson.JsonData jd)
        {
            base.Deserialize(jd);

            mModelPrefab = JsonHelper.ReadString(jd["ModelPrefab"]);
            mEntityEndEffect = JsonHelper.ReadString(jd["EntityEndEffect"]);
            mBounceTims = JsonHelper.ReadInt(jd["BounceTims"]);
            mBounceRandomRadius = JsonHelper.ReadFloat(jd["BounceRandomRadius"]);
        }

        public override LitJson.JsonWriter Serialize(LitJson.JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "ModelPrefab", mModelPrefab);
            JsonHelper.WriteProperty(ref writer, "EntityEndEffect", mEntityEndEffect);
            JsonHelper.WriteProperty(ref writer, "BounceTims", mBounceTims);
            JsonHelper.WriteProperty(ref writer, "BounceRandomRadius", mBounceRandomRadius);

            return writer;
        }
    }

}
