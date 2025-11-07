using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DeviceOrientationHandler : MonoBehaviour
{

    /*	This class will call an action every time orientation was changed.
	 * 	The main trigger is compare device orientation with saved previously.

			+----------+
			|          | ---------------+
			|          |				|
			| PORTRAIT |				|
			|   MODE   |				V
			|          |		+----------------+
			|          |		|                |
			+----------+		|    LANDSCAPE   |
								|      MODE      |
								|                |
								+----------------+
	*/

    public static bool destroyed = false;

    private static DeviceOrientationHandler _instance = null;
    public static DeviceOrientationHandler instance
    {
        get
        {
            if (_instance == null)
            {
                // create instance
                GameObject go = new GameObject("DeviceOrientationHandler");
                _instance = go.AddComponent<DeviceOrientationHandler>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }


    private DeviceOrientation prevOrientation;
    void Start()
    {
        DeviceOrientationHandler.destroyed = false;
        FirstTimeSetUp();
        prevOrientation = currentOrientation;
    }
    void OnDestroy()
    {
        DeviceOrientationHandler.destroyed = true;
    }

    public event Action<DeviceOrientation> OnDeviceOrientationChanged;
    public event Action<ScreenOrientation> OnScreenOrientationChanged;
    public event Action<bool> OnVerticalOrientationChanged;
    public event Action<bool> OnVerticalOrientationWin;

    const float DELAY_IN_SECONDS = 1f;

    DeviceOrientation currentOrientation = DeviceOrientation.Portrait;

    private DeviceOrientation[] allovedOrientations = new DeviceOrientation[] {
        DeviceOrientation.Portrait,
        DeviceOrientation.PortraitUpsideDown,
        DeviceOrientation.LandscapeLeft,
        DeviceOrientation.LandscapeRight
    };

    // TODO: Add Device Strategy and Editor Canvas Strategy

#if UNITY_EDITOR
    DeviceOrientation getMockOrientaionByCanvas()
    {
        Canvas c = (Canvas)FindObjectOfType<Canvas>();
        if (c == null)
            return DeviceOrientation.Portrait;

        if (c.GetComponent<RectTransform>() == null)
            return DeviceOrientation.Portrait;

        return c.GetComponent<RectTransform>().sizeDelta.y > c.GetComponent<RectTransform>().sizeDelta.x ? DeviceOrientation.Portrait : DeviceOrientation.LandscapeLeft;
    }
#endif


    DeviceOrientation deviceOrientation
    {
        get
        {
#if UNITY_EDITOR
            switch (GameSettings.Instance.orientationType)
            {

                case GameSettings.OrientationType.LandSpace: return DeviceOrientation.LandscapeLeft;
                case GameSettings.OrientationType.Portrait: return DeviceOrientation.PortraitUpsideDown;

            }

            return getMockOrientaionByCanvas();
#else

              switch (GameSettings.Instance.orientationType)
            {
                
                case GameSettings.OrientationType.LandSpace: return DeviceOrientation.LandscapeLeft;
                case GameSettings.OrientationType.Portrait: return DeviceOrientation.PortraitUpsideDown;
                   
            }
		return Input.deviceOrientation;
#endif
        }
    }


    void FirstTimeSetUp()
    {
        // first time set up
        if (isOrientationAllowed(deviceOrientation))
            ApplyOrientation(deviceOrientation);
        else
            ApplyOrientation(DeviceOrientation.Portrait);
    }



    float localTimer = 0;
    void Update()
    {
        if (localTimer < DELAY_IN_SECONDS)
        {
            localTimer += Time.deltaTime;
            return;
        }

        CheckOrientation();
    }




    /// <summary>
    /// Main check logic
    /// </summary>
    void CheckOrientation()
    {
        // pass only allowed orientation
        if (!isOrientationAllowed(deviceOrientation))
            return;

        // pass only new orientation
        if (currentOrientation.Equals(deviceOrientation))
            return;

        ApplyOrientation(deviceOrientation);
    }

    public bool isVertical;
    void ApplyOrientation(DeviceOrientation orientation)
    {



        // allow change orientation
        localTimer = 0;



        switch (GameSettings.Instance.orientationType)
        {
            case GameSettings.OrientationType.Auto:
                // save as current orientation
                currentOrientation = orientation;

                // apply orientation
                Screen.orientation = (ScreenOrientation)currentOrientation;
                break;
            case GameSettings.OrientationType.LandSpace:
                currentOrientation = DeviceOrientation.LandscapeLeft;

                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
            case GameSettings.OrientationType.Portrait:
                currentOrientation = DeviceOrientation.PortraitUpsideDown;

                Screen.orientation = ScreenOrientation.Portrait;
                break;
        }
        if (prevOrientation != currentOrientation)
        {

           
            prevOrientation = currentOrientation;
           

        }



        if (OnDeviceOrientationChanged != null)
        {
            OnDeviceOrientationChanged(currentOrientation);
        }
        if (OnScreenOrientationChanged != null)
        {
            OnScreenOrientationChanged((ScreenOrientation)currentOrientation);
        }
        isVertical = DeviceOrientation.Portrait.Equals(currentOrientation) || DeviceOrientation.PortraitUpsideDown.Equals(currentOrientation);
        isVertical = true;
        if (isVertical)
        {
           // SolitaireStageViewHelperClass.instance.ResetDistance();
        }
        else
        {
             SolitaireStageViewHelperClass.instance.SetAllDistanceBetweenCard(true);
        }
        if (OnVerticalOrientationChanged != null)
        {

            OnVerticalOrientationChanged(isVertical);
        }
        if (OnVerticalOrientationWin != null)
        {

            OnVerticalOrientationWin(isVertical);
        }

    }




    /// <summary>
    /// Return entering into allowed list
    /// </summary>
    bool isOrientationAllowed(DeviceOrientation orientation)
    {
        for (int i = 0; i < allovedOrientations.Length; i++)
        {
            if (allovedOrientations[i].Equals(orientation))
                return true;

        } 
          
        return false;
    }
}
