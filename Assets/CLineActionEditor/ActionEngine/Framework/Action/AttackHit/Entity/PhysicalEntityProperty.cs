/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\PhysicalEntityProperty.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-16      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;
    using System.Collections.Generic;

    public class PhysicalEntityProperty : AttackEntityProperty
    {
        private string mModlePrefab = string.Empty;
        private int mMaxPassNum = 1;
        private string mEntityEndEffect = string.Empty;
        private List<string> mEntityColliderOtherEffectLayerNameList = new List<string>();
        private List<string> mEntityColliderOtherEffectLayerEffectList = new List<string>();

        #region property
        [EditorProperty("攻击体模型", EditorPropertyType.EEPT_GameObjectToString)]
        public string ModlePrefab
        {
            get { return mModlePrefab; }
            set { mModlePrefab = value; }
        }
        [EditorProperty("最大穿透数", EditorPropertyType.EEPT_Int)]
        public int MaxPassNum
        {
            get { return mMaxPassNum; }
            set { mMaxPassNum = value; }
        }
        [EditorProperty("攻击体结束特效", EditorPropertyType.EEPT_GameObjectToString)]
        public string EntityEndEffect
        {
            get { return mEntityEndEffect; }
            set { mEntityEndEffect = value; }
        }
        [EditorProperty("攻击体碰撞非角色层名称列表", EditorPropertyType.EEPT_StringList, Description = "实现碰到不同的材质有不同的反馈")]
        public List<string> EntityColliderOtherEffectLayerNameList
        {
            get { return mEntityColliderOtherEffectLayerNameList; }
            set { mEntityColliderOtherEffectLayerNameList = value; }
        }
        [EditorProperty("攻击体碰撞非角色层特效列表", EditorPropertyType.EEPT_GameObjectToStringList)]
        public List<string> EntityColliderOtherEffectLayerEffectList
        {
            get { return mEntityColliderOtherEffectLayerEffectList; }
            set { mEntityColliderOtherEffectLayerEffectList = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mModlePrefab = JsonHelper.ReadString(jd["ModlePrefab"]);
            mMaxPassNum = JsonHelper.ReadInt(jd["MaxPassNum"]);
            mEntityEndEffect = JsonHelper.ReadString(jd["EntityEndEffect"]);
            mEntityColliderOtherEffectLayerNameList = JsonHelper.ReadListString(jd["EntityColliderOtherEffectLayerNameList"]);
            mEntityColliderOtherEffectLayerEffectList = JsonHelper.ReadListString(jd["EntityColliderOtherEffectLayerEffectList"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "ModlePrefab", mModlePrefab);
            JsonHelper.WriteProperty(ref writer, "MaxPassNum", mMaxPassNum);
            JsonHelper.WriteProperty(ref writer, "EntityEndEffect", mEntityEndEffect);
            JsonHelper.WriteProperty(ref writer, "EntityColliderOtherEffectLayerNameList", mEntityColliderOtherEffectLayerNameList);
            JsonHelper.WriteProperty(ref writer, "EntityColliderOtherEffectLayerEffectList", mEntityColliderOtherEffectLayerEffectList);

            return writer;
        }

    }

}
