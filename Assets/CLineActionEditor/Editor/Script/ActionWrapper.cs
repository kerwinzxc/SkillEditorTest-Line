/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActionWrapper.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-22      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using System.Collections.Generic;

    public class ActionWrapper
    {
        private Dictionary<EActorType, Actor> mActionActorHash = new Dictionary<EActorType, Actor>();

        public Dictionary<EActorType, Actor> ActionActorHash
        {
            get { return mActionActorHash; }
        }

        public ActionWrapper()
        {
            InitActorHash();
        }

        private void InitActorHash()
        {
            mActionActorHash.Add(EActorType.EAT_GroupAnimation, ActorGroupAnimation.CreateInstance<ActorGroupAnimation>());
            mActionActorHash.Add(EActorType.EAT_GroupEffect, ActorGroupEffect.CreateInstance<ActorGroupEffect>());
            mActionActorHash.Add(EActorType.EAT_GroupSound, ActorGroupSound.CreateInstance<ActorGroupSound>());
            mActionActorHash.Add(EActorType.EAT_GroupCamera, ActorGroupCamera.CreateInstance<ActorGroupCamera>());
            mActionActorHash.Add(EActorType.EAT_GroupOther, ActorGroupOther.CreateInstance<ActorGroupOther>());
            mActionActorHash.Add(EActorType.EAT_GroupAttackDefinition, ActorGroupAttackDefinition.CreateInstance<ActorGroupAttackDefinition>());
            mActionActorHash.Add(EActorType.EAT_GroupInterrupt, ActorGroupInterrupt.CreateInstance<ActorGroupInterrupt>());
        }
    }
}
