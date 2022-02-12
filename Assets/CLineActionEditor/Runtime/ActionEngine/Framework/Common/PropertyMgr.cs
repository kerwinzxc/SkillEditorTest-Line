/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\PropertyMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-2      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/


namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;
    using LitJson;
    using System.IO;

    public sealed class PropertyMgr : Singleton<PropertyMgr>
    {
        private Dictionary<string, PlayerProperty> mPlayerPropertyHash = new Dictionary<string, PlayerProperty>();
        private Dictionary<string, MonsterProperty> mMonsterPropertyHash = new Dictionary<string, MonsterProperty>();
        private Dictionary<string, WeaponProperty> mWeaponPropertyHash = new Dictionary<string, WeaponProperty>();
        private Dictionary<string, Dictionary<string, Action>> mActionHash = new Dictionary<string, Dictionary<string, Action>>();
        private Dictionary<string, BuffFactoryProperty> mBuffPropertyHash = new Dictionary<string, BuffFactoryProperty>();

        public override void Init()
        {
            Load("/GameData/Unit/Player.json", (PlayerProperty p) => { mPlayerPropertyHash.Add(p.ID, p); });
            Load("/GameData/Unit/Monster.json", (MonsterProperty m) => { mMonsterPropertyHash.Add(m.ID, m); });
            Load("/GameData/Unit/Weapon.json", (WeaponProperty w) => { mWeaponPropertyHash.Add(w.ID, w); });
            Load("/GameData/Buff/Buff.json", (BuffFactoryProperty b) => { mBuffPropertyHash.Add(b.ID, b); });

            string[] fileList = ResourceMgr.Instance.GetFiles("/GameData/Action/");
            for (int i=0; i<fileList.Length; ++i)
            {
                Dictionary<string, Action> actionHash = new Dictionary<string, Action>();
                Load(fileList[i], (Action ac) => { actionHash.Add(ac.ID, ac); });
                mActionHash.Add(Path.GetFileNameWithoutExtension(fileList[i]), actionHash);
            }
        }

        public override void Destroy()
        {
            mPlayerPropertyHash.Clear();
            mMonsterPropertyHash.Clear();
            mWeaponPropertyHash.Clear();
            mActionHash.Clear();
            mBuffPropertyHash.Clear();
        }

        public void Load<T>(string name, System.Action<T> handler) where T : IProperty, new()
        {
            TextAsset text = ResourceMgr.Instance.LoadObject(name, typeof(TextAsset)) as TextAsset;
            if (text == null)
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Property", "the {0} of resource is not exist!!!", name);
                return;
            }

            JsonData jd = JsonMapper.ToObject(text.ToString().Trim());
            JsonData data = jd["Property"];
            for (int i = 0; i < data.Count; ++i)
            {
                T t = new T();
                t.Deserialize(data[i]);
                handler(t);
            }
        }

        public PlayerProperty GetPlayerProperty(string id)
        {
            PlayerProperty player = null;
            if (!mPlayerPropertyHash.TryGetValue(id, out player))
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Property", "the \"{0}\" of player's property is not exist!!!", id);
            }

            return player;
        }

        public MonsterProperty GetMonsterProperty(string id)
        {
            MonsterProperty monster = null;
            if (!mMonsterPropertyHash.TryGetValue(id, out monster))
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Property", "the \"{0}\" of monster's property is not exist!!!", id);
            }

            return monster;
        }

        public WeaponProperty GetWeaponProperty(string id)
        {
            WeaponProperty weapon = null;
            if (!mWeaponPropertyHash.TryGetValue(id, out weapon))
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Property", "the \"{0}\" of weapon's property is not exist!!!", id);
            }
            
            return weapon;
        }

        public BuffFactoryProperty GetBuffProperty(string id)
        {
            BuffFactoryProperty buff = null;
            if (!mBuffPropertyHash.TryGetValue(id, out buff))
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Property", "the \"{0}\" of BuffProperty is not exists!", id);
            }

            return buff;
        }

        public Action GetAction(string group, string id)
        {
            Action action = null;
            Dictionary<string, Action> actionHash = null;

            if (mActionHash.TryGetValue(group, out actionHash))
            {
                if (!actionHash.TryGetValue(id, out action))
                {
                    LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Property", "the \"{0}\" of action's property is not exist!!!", id);
                }

                return action;
            }
            else
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Property", "the \"{0}\" of ActionGroup is not exist!!!", group);
            }

            return null;
        }

        public bool HasAction(string group, string id)
        {
            bool has = false;
            Dictionary<string, Action> actionHash = null;

            if (mActionHash.TryGetValue(group, out actionHash))
            {
                if (actionHash.ContainsKey(id))
                    has = true;
            }

            return has;
        }
    }
}
