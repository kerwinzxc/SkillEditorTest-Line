/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Message\MessageMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2018-9-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/


namespace SuperCLine.ActionEngine
{
    using System;
    using System.Collections.Generic;

    public sealed class MessageMgr : Singleton<MessageMgr>
    {
        private Dictionary<string, LinkedList<Action<Message>>> mMsgHandlerHash = new Dictionary<string, LinkedList<Action<Message>>>();
        private LinkedList<Message> mMsgList = new LinkedList<Message>();
        private LinkedList<Message> mHandleList = new LinkedList<Message>();

        public override void Init()
        {
            
        }

        public override void Destroy()
        {
            mMsgHandlerHash.Clear();
            mMsgList.Clear();
        }

        public void Register(string name, Action<Message> handler)
        {
            LinkedList<Action<Message>> handlers = null;
            if (!mMsgHandlerHash.TryGetValue(name, out handlers))
            {
                handlers = new LinkedList<Action<Message>>();
                mMsgHandlerHash[name] = handlers;
            }

            handlers.AddLast(handler);
        }

        public void Unregister(string name, Action<Message> handler)
        {
            if (null == handler)
            {
                mMsgHandlerHash.Remove(name);
                return;
            }

            LinkedList<Action<Message>> handlers = null;
            if (mMsgHandlerHash.TryGetValue(name, out handlers))
            {
                handlers.Remove(handler);
            }
        }

        public void SendMessage(Message msg)
        {
            LinkedList<Action<Message>> handlers = null;
            if (mMsgHandlerHash.TryGetValue(msg.Name, out handlers))
            {
                using (var itr = handlers.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        itr.Current(msg);
                    }
                }
            }
        }

        public void PostMessage(Message msg)
        {
            lock (mMsgList)
            {
                mMsgList.AddLast(msg);
            }
        }

        public void Update(float fTick)
        {
            lock (mMsgList)
            {
                using (var itr = mMsgList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        mHandleList.AddLast(itr.Current);
                    }
                }

                mMsgList.Clear();
            }

            using (var itr = mHandleList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    SendMessage(itr.Current);
                }
            }

            mHandleList.Clear();
        }

    }
}
