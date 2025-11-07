using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTrigger : MonoBehaviour
{
   

    // Update is called once per frame
    void FixedUpdate()
    {
       if(DeviceOrientationHandler.instance.isVertical)
        {
            transform.localScale = new Vector3(1.7f, 0.8f, 1);
        }
        else
        {
            transform.localScale = new Vector3(0.8f, 1.7f, 1);
        }
    }
}
