using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class cans : MonoBehaviour
{
    public Text hw;
    void Start()
    {
        hw.text = Screen.width + ":" + Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
