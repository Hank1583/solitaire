using UnityEngine;
using UnityEngine.UI;
public class PanelControl : MonoBehaviour
{
	[SerializeField]
	private GameObject checkMarkPanelObj;
	[SerializeField]
	private GameObject checkMarkObj;
	[SerializeField]
	private Image buttonImage;
	[SerializeField]
	private AspectRatioFitter buttonAspectRatio;
	[SerializeField]
	private Button button;
	public void InitButton (Sprite sprite, UnityEngine.Events.UnityAction action)
	{
		buttonImage.sprite = sprite;
		Rect spriteRect = sprite.rect;
//		buttonAspectRatio.aspectRatio = spriteRect.width / spriteRect.height;
		button.onClick.AddListener (action);
       // buttonImage.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

    }
	public void SetActive (bool isActive)
	{
		checkMarkPanelObj.SetActive (isActive);
		checkMarkObj.SetActive (isActive);
	}
}