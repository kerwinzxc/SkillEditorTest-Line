/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\UObject\UInputMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-11      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine.UI;

    public sealed class UInputMgr : MonoSingleton<UInputMgr>
    {
        public UJoyStick InputJoyStick;
        public Button SkillAttack;
        public Button[] SkillSlot;
        private bool mLongPressed = false;

        public System.Action<Button> OnClicked = null;
        public System.Action<Button> OnLongPressed = null;
        public System.Action<Button> OnLongPressedEnd = null;

        public void Init()
        {
            Button.ButtonClickedEvent attackClick = new Button.ButtonClickedEvent();
            attackClick.AddListener(() => {

                if (mLongPressed)
                {
                    mLongPressed = false;
                    if (OnLongPressedEnd != null)
                        OnLongPressedEnd(SkillAttack);
                }
                else
                {
                    if (OnClicked != null)
                        OnClicked(SkillAttack);
                }
            });
            SkillAttack.onClick = attackClick;

            UButtonPressed btnPressed = SkillAttack.GetComponent<UButtonPressed>();
            btnPressed.OnLongPressed = onLongPressed;

            for (int i=0; i < SkillSlot.Length; ++i)
            {
                Button.ButtonClickedEvent click = new Button.ButtonClickedEvent();
                Button bt = SkillSlot[i];
                click.AddListener(() => { if (OnClicked != null) OnClicked(bt); });

                SkillSlot[i].onClick = click;
            }
        }

        public bool EnableJoyStick
        {
            set { InputJoyStick.gameObject.SetActive(value); }
        }

        private void onLongPressed(Button btn)
        {
            mLongPressed = true;

            if (OnLongPressed != null)
                OnLongPressed(btn);
        }

    }
}
