using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerResolution : MonoBehaviour
{
    [SerializeField]
    private Vector2 portrait;
    [SerializeField]
    private Vector2 landSpace;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!DeviceOrientationHandler.instance.isVertical)
        {
            rectTransform.sizeDelta = portrait;
        }
        else
        {
            rectTransform.sizeDelta = landSpace;

        }
    }
}
