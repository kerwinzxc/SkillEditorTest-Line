/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Message\Message.cs
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
    using System.Collections.Generic;

    public sealed class Message
    {
        private Dictionary<string, object> mArgHash = new Dictionary<string, object>();
        
        public string Name { get; set; }

        public Message(string name)
        {
            Name = name;
        }

        public void AddArg(string name, object arg)
        {
            mArgHash.Add(name, arg);
        }

        public object GetArg(string name)
        {
            object arg = null;
            if (mArgHash.TryGetValue(name, out arg))
            {
                return arg;
            }

            LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Msg", "[Message: {0}, Arg: {1}] is not exist!", Name, name);

            return null;
        }
    }
}
