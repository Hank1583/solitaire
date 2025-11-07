using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
 
    private bool useSingleton = false;
    [SerializeField]
    private bool useUpdate = false;
    [SerializeField]
    private GameObject titleInBackGround  ;
    private bool isVertical;
    private void Start()
    {
        isVertical = DeviceOrientationHandler.instance.isVertical;
        SetBackGround(GameSettings.Instance.visualPlayBackgroundSet);
    }

    public void SetBackGround(int position)
    {
        if(titleInBackGround!=null)
        {
            titleInBackGround.SetActive(position == 0);
        }
        if (DeviceOrientationHandler.instance.isVertical)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = ImageSettings.Instance.backgroundPortrait[position];
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().sprite = ImageSettings.Instance.background[position];
        }
     
    }

    private void Update()
    {
        if (isVertical != DeviceOrientationHandler.instance.isVertical)
        {
            SetBackGround(GameSettings.Instance.visualPlayBackgroundSet);
        }
        if (!useUpdate) return;
        SetBackGround(GameSettings.Instance.visualPlayBackgroundSet);

    }
}
