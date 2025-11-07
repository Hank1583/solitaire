using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LayoutResolution : MonoBehaviour
{



    private LayoutElement layoutElement;

    private void Start()
    {
        layoutElement = GetComponent<LayoutElement>();
    }

    private void Update()
    {

        float value = 1200;

        if (!DeviceOrientationHandler.instance.isVertical)
        {
            value = 1700;

        }



        layoutElement.minWidth = (value);
    }
}
