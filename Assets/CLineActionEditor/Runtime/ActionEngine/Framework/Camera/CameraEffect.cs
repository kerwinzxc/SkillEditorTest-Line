/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Camera\CameraEffect.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-15      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public enum ECameraEffectType
    {
        ECET_None,
        ECET_GrayScale,
    }

    public static class CameraEffect
    {
        public static void PlayCameraEffect(ECameraEffectType type)
        {
            GameObject camera = GameObject.FindWithTag("MainCamera") as GameObject;

            switch (type)
            {
                case ECameraEffectType.ECET_GrayScale:
                    {
                        UCameraEffectGrayscale eff = camera.GetComponent<UCameraEffectGrayscale>();
                        if (eff == null)
                        {
                            eff = camera.AddComponent<UCameraEffectGrayscale>();
                            eff.shader = ResourceMgr.Instance.LoadObject("/Shader/GrayscaleEffect.shader", typeof(Shader)) as Shader;
                        }

                        eff.enabled = true;
                    }
                    break;
            }
        }

        public static void StopCameraEffect(ECameraEffectType type)
        {
            GameObject camera = GameObject.FindWithTag("MainCamera") as GameObject;

            switch (type)
            {
                case ECameraEffectType.ECET_GrayScale:
                    {
                        camera.GetComponent<UCameraEffectGrayscale>().enabled = false;
                    }
                    break;
            }
        }
    }

}
