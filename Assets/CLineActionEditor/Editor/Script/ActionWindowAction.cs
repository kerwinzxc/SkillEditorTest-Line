/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Script\ActionWindowAction.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-20      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEditor;
    using UnityEngine;
    using System;
    using System.Text;
    using System.IO;
    using LitJson;
    using System.Collections.Generic;

    public partial class ActionWindow : EditorWindow
    {
        private void DrawInspectorActionList()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("New"))
            {
                CreateAction();
            }
            if (GUILayout.Button("Delete"))
            {
                DeleteAction();
            }
            if (GUILayout.Button("Save"))
            {
                SaveAction();
                SaveActionInterrupt();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            mScrollViewPosition = GUILayout.BeginScrollView(mScrollViewPosition, false, true);

            DrawPropertyList(mActionList);

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void CreateAction()
        {
            if (!UnitWrapper.Instance.IsReady) return;

            Action action = new Action();
            action.TotalTime = 1333;
            mActionList.Add(action);

            ActionInterrupt interrupt = new ActionInterrupt();
            mActionInterruptList.Add(action, interrupt);

            ActionWrapper wrapper = new ActionWrapper();
            mActionWrapperHash.Add(action, wrapper);
        }

        private void DeleteAction()
        {
            if (mCurProperty is Action)
            {
                mActionList.Remove(mCurProperty as Action);
                mActionInterruptList.Remove(mCurProperty);
                mActionWrapperHash.Remove(mCurProperty);
                mCurProperty = null;
                mCurActionWrapper = null;
            }
        }

        private string GetActionPath()
        {
            string groupName = UnitWrapper.Instance.ActionGroupName;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}Action/{1}.json", FilePath, groupName);

            return sb.ToString();
        }

        private void SaveAction()
        {
            string path = GetActionPath();
            if (File.Exists(path))
            {
                //File.Move(path, path + System.DateTime.Now.ToString("yyyyMMddhhmmss"));
                File.Delete(path);
            }
            FileInfo fi = new FileInfo(path);
            StreamWriter sw = fi.CreateText();

            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.WriteObjectStart();
            writer.WritePropertyName("Property");
            writer.WriteArrayStart();
            for (int i = 0; i < mActionList.Count; ++i)
            {
                Action ac = mActionList[i] as Action;
                GetActionWrapper(ref ac, mActionWrapperHash[ac]);
                writer = ac.Serialize(writer);
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();

            sw.WriteLine(sb.ToString());
            sw.Close();
            sw.Dispose();
            AssetDatabase.Refresh();
        }

        private string GetActionInterruptPath()
        {
            string groupName = UnitWrapper.Instance.ActionGroupName;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}ActionInterrupt/{1}.json", FilePath, groupName);

            return sb.ToString();
        }

        private void SaveActionInterrupt()
        {
            string path = GetActionInterruptPath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            FileInfo fi = new FileInfo(path);
            StreamWriter sw = fi.CreateText();

            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.WriteObjectStart();
            writer.WritePropertyName("ActionInterrupts");
            writer.WriteArrayStart();
            foreach (ActionInterrupt aci in mActionInterruptList.Values)
            {
                writer = aci.Serialize(writer);
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();

            sw.WriteLine(sb.ToString());
            sw.Close();
            sw.Dispose();
            AssetDatabase.Refresh();
        }

        public void DeserializeAction()
        {
            mActionList.Clear();
            mActionInterruptList.Clear();
            
            string groupname = UnitWrapper.Instance.ActionGroupName;
            if (File.Exists(GetActionPath()))
            {
                TextAsset t = ResourceMgr.Instance.LoadObject("/GameData/Action/" + groupname + ".json", typeof(TextAsset)) as TextAsset;
                JsonData jd = JsonMapper.ToObject(t.ToString().Trim());
                JsonData actions = jd["Property"];
                for (int i = 0; i < actions.Count; ++i)
                {
                    Action ac = new Action();
                    ac.Deserialize(actions[i]);
                    mActionList.Add(ac);

                    CreateWrapperByAction(ac);
                }
            }

            if (File.Exists(GetActionInterruptPath()))
            {
                TextAsset t = ResourceMgr.Instance.LoadObject("/GameData/ActionInterrupt/" + groupname + ".json", typeof(TextAsset)) as TextAsset;
                JsonData jd = JsonMapper.ToObject(t.ToString().Trim());
                JsonData interrupts = jd["ActionInterrupts"];
                for (int i = 0; i < interrupts.Count; ++i)
                {
                    ActionInterrupt aci = new ActionInterrupt();
                    aci.Deserialize(interrupts[i]);
                    for (int j = 0; j < mActionList.Count; ++j)
                    {
                        Action ac = mActionList[j] as Action;
                        if (ac.ID.Equals(aci.ActionID))
                        {
                            mActionInterruptList.Add(ac, aci);
                            break;
                        }
                    }
                }
            }

            // TO CLine:
            using (List<IProperty>.Enumerator itr1 = mActionList.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    ActionWrapper aw = mActionWrapperHash[itr1.Current];

                    ActorGroupInterrupt groupInterrupt = aw.ActionActorHash[EActorType.EAT_GroupInterrupt] as ActorGroupInterrupt;
                    using (List<Actor>.Enumerator itr2 = groupInterrupt.ActorList.GetEnumerator())
                    {
                        while (itr2.MoveNext())
                        {
                            using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                            {
                                while (itr3.MoveNext())
                                {
                                    ActorEventInterrupt interrupt = itr3.Current as ActorEventInterrupt;
                                    interrupt.DeserializeShortcutIndex();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetActionWrapper(ref Action action, ActionWrapper wrpper)
        {
            action.EventList.Clear();
            action.InterruptList.Clear();
            action.AttackList.Clear();
            using (Dictionary<EActorType, Actor>.Enumerator itr1 = wrpper.ActionActorHash.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    if (itr1.Current.Value.GetActorType != EActorType.EAT_GroupInterrupt &&
                        itr1.Current.Value.GetActorType != EActorType.EAT_GroupAttackDefinition)
                    {
                        using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                        {
                            while (itr2.MoveNext())
                            {
                                using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                                {
                                    while (itr3.MoveNext())
                                    {
                                        ActorEvent ae = itr3.Current as ActorEvent;
                                        ae.Event.ActorID = ae.Parent.ID.ToString();
                                        action.EventList.Add(ae.Event);
                                    }
                                }
                            }
                        }
                    }
                    else if (itr1.Current.Value.GetActorType == EActorType.EAT_GroupInterrupt)
                    {
                        using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                        {
                            while (itr2.MoveNext())
                            {
                                using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                                {
                                    while (itr3.MoveNext())
                                    {
                                        ActorEventInterrupt ai = itr3.Current as ActorEventInterrupt;
                                        ai.Interrupt.ActorID = ai.Parent.ID.ToString();
                                        action.InterruptList.Add(ai.Interrupt);
                                    }
                                }
                            }
                        }
                    }
                    else if (itr1.Current.Value.GetActorType == EActorType.EAT_GroupAttackDefinition)
                    {
                        using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                        {
                            while (itr2.MoveNext())
                            {
                                using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                                {
                                    while (itr3.MoveNext())
                                    {
                                        ActorEventAttackDef aa = itr3.Current as ActorEventAttackDef;
                                        aa.Attack.ActorID = aa.Parent.ID.ToString();
                                        action.AttackList.Add(aa.Attack);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void BuildActorTree<AG, AA>(ActionWrapper aw, Event evt, EActorType type)
            where AG : Actor
            where AA : Actor
        {
            AG animGroup = aw.ActionActorHash[type] as AG;
            AA anim = null;
            using (List<Actor>.Enumerator animItr = animGroup.ActorList.GetEnumerator())
            {
                while (animItr.MoveNext())
                {
                    if (animItr.Current.Name == evt.ActorID)
                    {
                        anim = animItr.Current as AA;
                        break;
                    }
                }
            }

            if (anim == null)
            {
                anim = ScriptableObject.CreateInstance<AA>();
                anim.Name = evt.ActorID;
                anim.Parent = animGroup;
                animGroup.ActorList.Add(anim);
            }

            ActorEvent ae = ActorEvent.CreateInstance<ActorEvent>();
            ae.Init(anim, 0);
            ae.Event = evt;
            anim.ActorList.Add(ae);

            int actorID = Convert.ToInt32(evt.ActorID);
            anim.ID = actorID;
            animGroup.ID = (actorID > animGroup.ID ? actorID : animGroup.ID);
        }

        private void CreateWrapperByAction(Action ac)
        {
            ActionWrapper aw = new ActionWrapper();
            mActionWrapperHash.Add(ac, aw);
            using (List<Event>.Enumerator itr = ac.EventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    switch (itr.Current.EventType)
                    {
                        case EEventType.EET_AttackDef:
                            {
                                BuildActorTree<ActorGroupAttackDefinition, ActorAttackDefinition>(aw, itr.Current, EActorType.EAT_GroupAttackDefinition);
                            }
                            break;
                        case EEventType.EET_CameraShake:
                        case EEventType.EET_CameraEffect:
                            {
                                BuildActorTree<ActorGroupCamera, ActorCamera>(aw, itr.Current, EActorType.EAT_GroupCamera);
                            }
                            break;
                        case EEventType.EET_PlayAnim:
                            {
                                BuildActorTree<ActorGroupAnimation, ActorAnimation>(aw, itr.Current, EActorType.EAT_GroupAnimation);
                            }
                            break;
                        case EEventType.EET_PlayEffect:
                            {
                                BuildActorTree<ActorGroupEffect, ActorEffect>(aw, itr.Current, EActorType.EAT_GroupEffect);
                            }
                            break;
                        case EEventType.EET_PlaySound:
                            {
                                BuildActorTree<ActorGroupSound, ActorSound>(aw, itr.Current, EActorType.EAT_GroupSound);
                            }
                            break;
                        default:
                            {
                                BuildActorTree<ActorGroupOther, ActorOther>(aw, itr.Current, EActorType.EAT_GroupOther);
                            }
                            break;
                    }
                }
            }

            ActorGroupInterrupt groupInterrupt = aw.ActionActorHash[EActorType.EAT_GroupInterrupt] as ActorGroupInterrupt;
            using (List<ActionInterrupt>.Enumerator itr1 = ac.InterruptList.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    ActorInterrupt actorInterrupt = null;
                    using (List<Actor>.Enumerator actorItr = groupInterrupt.ActorList.GetEnumerator())
                    {
                        while (actorItr.MoveNext())
                        {
                            if (actorItr.Current.Name == itr1.Current.ActorID)
                            {
                                actorInterrupt = actorItr.Current as ActorInterrupt;
                                break;
                            }
                        }
                    }

                    if (actorInterrupt == null)
                    {
                        actorInterrupt = ScriptableObject.CreateInstance<ActorInterrupt>();
                        actorInterrupt.Name = itr1.Current.ActorID;
                        actorInterrupt.Parent = groupInterrupt;
                        groupInterrupt.ActorList.Add(actorInterrupt);
                    }

                    ActorEventInterrupt aei = ActorEventInterrupt.CreateInstance<ActorEventInterrupt>();
                    aei.Init(actorInterrupt, 0);
                    aei.Interrupt = itr1.Current;
                    actorInterrupt.ActorList.Add(aei);

                    int actorID = Convert.ToInt32(itr1.Current.ActorID);
                    actorInterrupt.ID = actorID;
                    groupInterrupt.ID = (actorID > groupInterrupt.ID ? actorID : groupInterrupt.ID);
                }
            }

            ActorGroupAttackDefinition groupAttack = aw.ActionActorHash[EActorType.EAT_GroupAttackDefinition] as ActorGroupAttackDefinition;
            using (List<ActionAttackDef>.Enumerator itr1 = ac.AttackList.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    ActorAttackDefinition actorAttack = null;
                    using (List<Actor>.Enumerator actorItr = groupAttack.ActorList.GetEnumerator())
                    {
                        while (actorItr.MoveNext())
                        {
                            if (actorItr.Current.Name == itr1.Current.ActorID)
                            {
                                actorAttack = actorItr.Current as ActorAttackDefinition;
                                break;
                            }
                        }
                    }

                    if (actorAttack == null)
                    {
                        actorAttack = ScriptableObject.CreateInstance<ActorAttackDefinition>();
                        actorAttack.Name = itr1.Current.ActorID;
                        actorAttack.Parent = groupAttack;
                        groupAttack.ActorList.Add(actorAttack);
                    }

                    ActorEventAttackDef aea = ActorEventAttackDef.CreateInstance<ActorEventAttackDef>();
                    aea.Init(actorAttack, 0);
                    aea.Attack = itr1.Current;
                    actorAttack.ActorList.Add(aea);

                    int actorID = Convert.ToInt32(itr1.Current.ActorID);
                    actorAttack.ID = actorID;
                    groupAttack.ID = (actorID > groupAttack.ID ? actorID : groupAttack.ID);
                }
            }
        }

    }

}