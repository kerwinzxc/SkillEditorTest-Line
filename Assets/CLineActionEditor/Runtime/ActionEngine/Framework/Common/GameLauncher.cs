/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\GameLauncher.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2018-9-11        SuperCLine           Created
|
+-----------------------------------------------------------------------------*/


namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public sealed class GameLauncher : MonoBehaviour
    {
        void Awake()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 30;
#endif
        }

        void Start()
        {
            GameObject go = GameObject.FindWithTag("GameManager");
            if (null != go)
            {
                go.AddComponent<GameManager>();
                GameObject.DontDestroyOnLoad(go);
            }
        }
    }
}


