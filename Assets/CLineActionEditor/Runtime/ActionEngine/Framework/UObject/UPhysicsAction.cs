/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\UObject\UPhysicsAction.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-13      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;

    public sealed class UPhysicsAction : MonoBehaviour
    {
        public Action<Collision, Transform> OnCollisionEnterAction;
        public Action<Collision, Transform> OnCollisionExitAction;
        public Action<Collision, Transform> OnCollisionStayAction;
        public Action<Collider, Transform> OnTriggerEnterAction;
        public Action<Collider, Transform> OnTriggerExitAction;
        public Action<Collider, Transform> OnTriggerStayAction;

        void OnCollisionEnter(Collision collision)
        {
            if (OnCollisionEnterAction != null)
            {
                OnCollisionEnterAction(collision, transform);
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (OnCollisionExitAction != null)
            {
                OnCollisionExitAction(collision, transform);
            }
        }

        void OnCollisionStay(Collision collision)
        {
            if (OnCollisionStayAction != null)
            {
                OnCollisionStayAction(collision, transform);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (OnTriggerEnterAction != null)
            {
                OnTriggerEnterAction(collider, transform);
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (OnTriggerExitAction != null)
            {
                OnTriggerExitAction(collider, transform);
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (OnTriggerStayAction != null)
            {
                OnTriggerStayAction(collider, transform);
            }
        }
    }

}
