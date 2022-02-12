/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventPlayAnimatorAnim.cs
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
    using UnityEngine;
    using System;
    using LitJson;

    public class EventPlayAnimatorAnim : IEventData, IProperty
    {
        [SerializeField] private string mModeName = string.Empty;
        [SerializeField] private string mAnimName = string.Empty;

        #region property
        [EditorProperty("带动画的模型", EditorPropertyType.EEPT_String)]
        public string ModeName
        {
            get { return mModeName; }
            set { mModeName = value; }
        }
        [EditorProperty("动画名", EditorPropertyType.EEPT_String)]
        public string AnimName
        {
            get { return mAnimName; }
            set { mAnimName = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_PlayAnimatorAnim; }
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
            GameObject mode = null;
            if (mModeName.Contains("|"))
            {
                string[] strName = mModeName.Split(new char[] { '|' });
                Transform modeRoot = Helper.Find(unit.UObject, strName[0]);
                mode = Helper.Find(modeRoot, strName[strName.Length - 1]).gameObject;
            }
            else
            {
                mode = Helper.Find(unit.UObject, mModeName).gameObject;
            }

            Animator anim = mode.GetComponent<Animator>();
            if (anim == null || string.IsNullOrEmpty(mAnimName)) return;

            anim.Play(mAnimName, 0, 0);
        }

        public void Deserialize(JsonData jd)
        {
            mModeName = JsonHelper.ReadString(jd["ModeName"]);
            mAnimName = JsonHelper.ReadString(jd["AnimName"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "ModeName", mModeName);
            JsonHelper.WriteProperty(ref writer, "AnimName", mAnimName);

            return writer;
        }

        public IEventData Clone()
        {
            EventPlayAnimatorAnim evt = new EventPlayAnimatorAnim();
            evt.mModeName = this.mModeName;
            evt.mAnimName = this.mAnimName;

            return evt;
        }
    }
}
