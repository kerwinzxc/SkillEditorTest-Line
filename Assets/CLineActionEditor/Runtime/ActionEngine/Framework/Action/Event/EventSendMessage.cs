/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventSendMessage.cs
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
    using LitJson;
    using UnityEngine;

    public class EventSendMessage : IEventData, IProperty
    {
        [SerializeField] private string mMessageName = string.Empty;
        [SerializeField] private string mMessageContent = string.Empty;
        [SerializeField] private bool mImmediatelySend = false;
        
        #region property
        [EditorProperty("发送信息名字", EditorPropertyType.EEPT_String)]
        public string MessageName
        {
            get { return mMessageName; }
            set { mMessageName = value; }
        }
        [EditorProperty("发送信息内容", EditorPropertyType.EEPT_String)]
        public string MessageContent
        {
            get { return mMessageContent; }
            set { mMessageContent = value; }
        }
        [EditorProperty("是否立即发送", EditorPropertyType.EEPT_Bool)]
        public bool ImmediatelySend
        {
            get { return mImmediatelySend; }
            set { mImmediatelySend = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_SendMessage; }
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
            Message ms = new Message(mMessageName);
            ms.AddArg("Object", unit);
            ms.AddArg("MessageContent", mMessageContent);

            if (mImmediatelySend)
                MessageMgr.Instance.SendMessage(ms);
            else
                MessageMgr.Instance.PostMessage(ms);
        }

        public void Deserialize(JsonData jd)
        {
            mMessageName = JsonHelper.ReadString(jd["MessageName"]);
            mMessageContent = JsonHelper.ReadString(jd["MessageContent"]);
            mImmediatelySend = JsonHelper.ReadBool(jd["ImmediatelySend"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "MessageName", mMessageName);
            JsonHelper.WriteProperty(ref writer, "MessageContent", mMessageContent);
            JsonHelper.WriteProperty(ref writer, "ImmediatelySend", mImmediatelySend);

            return writer;
        }

        public IEventData Clone()
        {
            EventSendMessage evt = new EventSendMessage();
            evt.mMessageName = this.mMessageName;
            evt.mMessageContent = this.mMessageContent;
            evt.mImmediatelySend = this.mImmediatelySend;

            return evt;
        }
    }

}
