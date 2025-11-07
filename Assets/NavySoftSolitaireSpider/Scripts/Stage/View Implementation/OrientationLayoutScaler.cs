using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OrientationLayoutScaler : MonoBehaviour {

	public RectTransform canvasRect;

	public RectTransform port;
	public RectTransform land;
//
	float width;
	float height;
		
	protected float aspectRatio {
		get {
	 
			return Mathf.Min (width, height) / Mathf.Max(width, height);
		}
	}

    public void VisibleResolution(bool isVertical)
    {
        port.gameObject.SetActive(isVertical);
        land.gameObject.SetActive(!isVertical);
    }
    // Use this for initialization
    void Start () {

		width = Screen.width;//canvasRect.sizeDelta.x;
		height = Screen.height;//canvasRect.sizeDelta.y;
 

	}
 

}
