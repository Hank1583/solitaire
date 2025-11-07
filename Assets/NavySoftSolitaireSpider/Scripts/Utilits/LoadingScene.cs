using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private float eplapsedTime = 0;
    private void Update()
    {
        eplapsedTime += Time.deltaTime;
        if (eplapsedTime >= 5)
        {
            ContinueModeGame.instance.SetLoadSuccess(true);
        }
        if (ContinueModeGame.instance.LoadSuccess)
        {
            Destroy(gameObject);
        }
    }
}
