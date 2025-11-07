using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResolutionTablet : MonoBehaviour
{
    [SerializeField]
    private Vector2 resolution = new Vector2(1500, 1500);
    [SerializeField]
    private Vector2 resolutionTablet = new Vector2(1500, 1500);
    private Vector2 orginResolution;
    [SerializeField]
    private bool onlyLandscape = false;
    private CanvasScaler canvasScaler;
    [SerializeField]
    private float ratioResolution;
    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        orginResolution = canvasScaler.referenceResolution;

#if !UNITY_EDITOR
 
          float    max = Mathf.Max(Screen.height, Screen.width);
          float  min = Mathf.Min(Screen.height, Screen.width);
#else
        float max = Mathf.Max(GetMainGameViewSize().x, GetMainGameViewSize().y);
        float min = Mathf.Min(GetMainGameViewSize().x, GetMainGameViewSize().y);
#endif

        ratioResolution = max / min;
#if UNITY_IOS

#elif UNITY_ANDROID
     if (max >= 1500 && !onlyLandscape)
        {

            canvasScaler.referenceResolution = resolution;
        }
#endif

        if (ratioResolution <= 1.5f)
        {

            canvasScaler.referenceResolution = resolutionTablet;
        }
    }

    private void Update()
    {
        if (!onlyLandscape) return;

        if (!DeviceOrientationHandler.instance.isVertical)
        {
            canvasScaler.referenceResolution = resolution;

            if (ratioResolution <= 1.5f)
            {

                canvasScaler.referenceResolution = resolutionTablet;
            }
        }
        else
        {
            canvasScaler.referenceResolution = orginResolution;
        }

    }


    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }


}
