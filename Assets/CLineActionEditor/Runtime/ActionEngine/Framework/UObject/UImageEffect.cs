/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\UObject\UImageEffect.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-9-8      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("")]
    public class UImageEffect : MonoBehaviour
    {
        public Shader shader;
        private Material mMaterial;

        protected virtual void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                enabled = false;
                return;
            }

            if (!shader || !shader.isSupported)
                enabled = false;
        }

        protected Material material
        {
            get
            {
                if (mMaterial == null)
                {
                    mMaterial = new Material(shader);
                    mMaterial.hideFlags = HideFlags.HideAndDontSave;
                }
                return mMaterial;
            }
        }

        protected virtual void OnDisable()
        {
            if (mMaterial)
            {
                DestroyImmediate(mMaterial);
            }
        }
    }

}