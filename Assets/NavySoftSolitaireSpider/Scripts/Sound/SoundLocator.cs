using UnityEngine;
public class SoundLocator : MonoBehaviour
{
	private void Start ()
	{
//		GameObject canvas = GameObject.Find ("Canvas");
//		RectTransform rt = canvas.GetComponent<RectTransform> ();
//		float width = rt.sizeDelta.x;
//		float height = rt.sizeDelta.y;
//		float scale = rt.localScale.x;
//		gameObject.transform.position = new Vector3 (width * scale / 2, height * scale / 2, 0);
		gameObject.transform.position = Vector2.zero;
		DontDestroyOnLoad (this);
	}
}