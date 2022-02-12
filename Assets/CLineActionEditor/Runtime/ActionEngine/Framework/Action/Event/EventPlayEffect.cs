/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventPlayEffect.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-19      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;
    using LitJson;
    using System.Collections.Generic;

    public enum EEffectDummyType
    {
        EEDT_DummyFollow = 0, // move with dummy
        EEDT_DummyPosition,   // only use dummy position and don't move with dummy 
        EEDT_UnitPosition,
        EEDT_Custom,
        EEDT_InputByUser,     // user set 'PropertyName.sInputSkillPosition'
    }

    public sealed class EventPlayEffect : IEventData, IProperty
    {
        [SerializeField] private string mEffectName = string.Empty;
        [SerializeField] private bool mIsLoop = false;
        [SerializeField] private string mLoopName = string.Empty;
        [SerializeField] private EEffectDummyType mDummyType = EEffectDummyType.EEDT_DummyFollow;
        [SerializeField] private string mDummyRoot = string.Empty; // using unit game object when is empty
        [SerializeField] private string mDummyAttach = string.Empty;
        [SerializeField] private Vector3 mPosition = Vector3.zero;
        [SerializeField] private Vector3 mEuler = Vector3.zero;
        [SerializeField] private bool mUseRandom = false;
        [SerializeField] private List<string> mRandomEffectList = new List<string>();

        #region property
        [EditorProperty("特效名称", EditorPropertyType.EEPT_GameObject)]
        public string EffectName
        {
            get { return mEffectName; }
            set { mEffectName = value; }
        }
        [EditorProperty("是否循环特效", EditorPropertyType.EEPT_Bool)]
        public bool IsLoop
        {
            get { return mIsLoop; }
            set { mIsLoop = value; }
        }
        [EditorProperty("循环特效唯一名字", EditorPropertyType.EEPT_String)]
        public string LoopName
        {
            get { return mLoopName; }
            set { mLoopName = value; }
        }
        [EditorProperty("特效类型", EditorPropertyType.EEPT_Enum)]
        public EEffectDummyType DummyType
        {
            get { return mDummyType; }
            set { mDummyType = value; }
        }
        [EditorProperty("Dummy点根结点", EditorPropertyType.EEPT_String)]
        public string DummyRoot
        {
            get { return mDummyRoot; }
            set { mDummyRoot = value; }
        }
        [EditorProperty("Dummy挂载结点", EditorPropertyType.EEPT_String)]
        public string DummyAttach
        {
            get { return mDummyAttach; }
            set { mDummyAttach = value; }
        }
        [EditorProperty("位置", EditorPropertyType.EEPT_Vector3)]
        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
        [EditorProperty("朝向", EditorPropertyType.EEPT_Vector3)]
        public Vector3 Euler
        {
            get { return mEuler; }
            set { mEuler = value; }
        }
        [EditorProperty("使用随机特效", EditorPropertyType.EEPT_Bool)]
        public bool UseRandom
        {
            get { return mUseRandom; }
            set { mUseRandom = value; }
        }
        [EditorProperty("随机特效列表", EditorPropertyType.EEPT_GameObjectList)]
        public List<string> RandomEffectList
        {
            get { return mRandomEffectList; }
            set { mRandomEffectList = value; }
        }
        #endregion


        public EEventDataType EventType
        {
            get { return EEventDataType.EET_PlayEffect; }
        }

        public string DebugName
        {
            get { return GetType().ToString(); }
        }
        public void Enter(Unit unit)
        {

        }
        public void Update(Unit unit, int deltaTime)
        {

        }
        public void Exit(Unit unit)
        {

        }

        public void Execute(Unit unit)
        {
            string effectname = mEffectName;
            if (mUseRandom)
            {
                int idx = UnityEngine.Random.Range(0, mRandomEffectList.Count);
                effectname = mRandomEffectList[idx];
            }

            switch (mDummyType)
            {
                case EEffectDummyType.EEDT_DummyFollow:
                    {
                        Transform dummyRoot = string.IsNullOrEmpty(mDummyRoot) ? unit.UObject : Helper.Find(unit.UObject.transform, mDummyRoot);
                        EffectMgr.Instance.PlayEffect(effectname, Helper.Find(dummyRoot, mDummyAttach).gameObject, mPosition, mEuler, mIsLoop, mLoopName);
                    }
                    break;
                case EEffectDummyType.EEDT_DummyPosition:
                    {
                        Transform dummyRoot = string.IsNullOrEmpty(mDummyRoot) ? unit.UObject : Helper.Find(unit.UObject.transform, mDummyRoot);
                        Transform tr = Helper.Find(dummyRoot, mDummyAttach);
                        EffectMgr.Instance.PlayEffect(effectname, tr.position + mPosition, Quaternion.Euler(tr.rotation.eulerAngles + mEuler), mIsLoop, mLoopName);
                    }
                    break;
                case EEffectDummyType.EEDT_UnitPosition:
                    {
                        EffectMgr.Instance.PlayEffect(effectname, unit.Position + mPosition, Quaternion.Euler(mEuler), mIsLoop, mLoopName);
                    }
                    break;
                case EEffectDummyType.EEDT_InputByUser:
                    {
                        Vector3 pos = Helper.GetAny<Vector3>(unit.PropertyContext.GetProperty(PropertyName.sInputSkillPosition));
                        EffectMgr.Instance.PlayEffect(effectname, pos, Quaternion.Euler(mEuler), mIsLoop, mLoopName);
                    }
                    break;
            }
        }

        public void Deserialize(JsonData jd)
        {
            mEffectName = JsonHelper.ReadString(jd["EffectName"]);
            mIsLoop = JsonHelper.ReadBool(jd["IsLoop"]);
            mLoopName = JsonHelper.ReadString(jd["LoopName"]);
            mDummyType = JsonHelper.ReadEnum<EEffectDummyType>(jd["DummyType"]);
            mDummyRoot = JsonHelper.ReadString(jd["DummyRoot"]);
            mDummyAttach = JsonHelper.ReadString(jd["DummyAttach"]);
            mPosition = JsonHelper.ReadVector3(jd["Position"]);
            mEuler = JsonHelper.ReadVector3(jd["Euler"]);
            mUseRandom = JsonHelper.ReadBool(jd["UseRandom"]);
            mRandomEffectList = JsonHelper.ReadListString(jd["RandomEffectList"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "EffectName", mEffectName);
            JsonHelper.WriteProperty(ref writer, "IsLoop", mIsLoop);
            JsonHelper.WriteProperty(ref writer, "LoopName", mLoopName);
            JsonHelper.WriteProperty(ref writer, "DummyType", mDummyType.ToString());
            JsonHelper.WriteProperty(ref writer, "DummyRoot", mDummyRoot);
            JsonHelper.WriteProperty(ref writer, "DummyAttach", mDummyAttach);
            JsonHelper.WriteProperty(ref writer, "Position", mPosition);
            JsonHelper.WriteProperty(ref writer, "Euler", mEuler);
            JsonHelper.WriteProperty(ref writer, "UseRandom", mUseRandom);
            JsonHelper.WriteProperty(ref writer, "RandomEffectList", mRandomEffectList);

            return writer;
        }

        public IEventData Clone()
        {
            EventPlayEffect evt = new EventPlayEffect();
            evt.mEffectName = this.mEffectName;
            evt.mIsLoop = this.mIsLoop;
            evt.mLoopName = this.mLoopName;
            evt.mDummyType = this.mDummyType;
            evt.mDummyRoot = this.mDummyRoot;
            evt.mDummyAttach = this.mDummyAttach;
            evt.mPosition = this.mPosition;
            evt.mEuler = this.mEuler;
            evt.mUseRandom = this.mUseRandom;
            evt.mRandomEffectList.AddRange(this.mRandomEffectList);

            return evt;
        }
    }

}
