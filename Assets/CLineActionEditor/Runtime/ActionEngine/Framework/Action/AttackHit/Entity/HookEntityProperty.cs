/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\HookEntityProperty.cs
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
    using LitJson;
    using UnityEngine;

    public class HookEntityProperty : AttackEntityProperty
    {
        [SerializeField] private string mModelPrefab;
        [SerializeField] private string mFollowEffect;
        [SerializeField] private bool mIsBackOnCollided;
        [SerializeField] private bool mIsUnitFollow = true;
        [SerializeField] private float mUnitStopDistance = 0f;

        #region property
        [EditorProperty("模型预制件 ", EditorPropertyType.EEPT_GameObject)]
        public string ModelPrefab
        {
            get { return mModelPrefab; }
            set { mModelPrefab = value; }
        }
        [EditorProperty("跟随特效 ", EditorPropertyType.EEPT_GameObject)]
        public string FollowEffect
        {
            get { return mFollowEffect; }
            set { mFollowEffect = value; }
        }
        [EditorProperty("碰到是否立即返回 ", EditorPropertyType.EEPT_Bool)]
        public bool IsBackOnCollided
        {
            get { return mIsBackOnCollided; }
            set { mIsBackOnCollided = value; }
        }
        [EditorProperty("被钩后Unit是否跟随 ", EditorPropertyType.EEPT_Bool)]
        public bool IsUnitFollow
        {
            get { return mIsUnitFollow; }
            set { mIsUnitFollow = value; }
        }
        [EditorProperty("被钩Unit停止跟随距离 ", EditorPropertyType.EEPT_Float)]
        public float UnitStopDistance
        {
            get { return mUnitStopDistance; }
            set { mUnitStopDistance = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mModelPrefab = JsonHelper.ReadString(jd["ModelPrefab"]);
            mFollowEffect = JsonHelper.ReadString(jd["FollowEffect"]);
            mIsBackOnCollided = JsonHelper.ReadBool(jd["IsBackOnCollided"]);
            mIsUnitFollow = JsonHelper.ReadBool(jd["IsUnitFollow"]);
            mUnitStopDistance = JsonHelper.ReadFloat(jd["UnitStopDistance"]);
        }

        public override LitJson.JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "ModelPrefab", mModelPrefab);
            JsonHelper.WriteProperty(ref writer, "FollowEffect", mFollowEffect);
            JsonHelper.WriteProperty(ref writer, "IsBackOnCollided", mIsBackOnCollided);
            JsonHelper.WriteProperty(ref writer, "IsUnitFollow", mIsUnitFollow);
            JsonHelper.WriteProperty(ref writer, "UnitStopDistance", mUnitStopDistance);

            return writer;
        }
    }

}
