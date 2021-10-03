/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\IHitFeedback.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-16      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    public interface IHitFeedback
    {
        EHitFeedbackType FeedbackType { get; }
        void OnHitFeedback(Unit attacker, Unit attackee, params object[] param);
    }
}