using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CardFaceScreen : MonoBehaviour
{
	[SerializeField]
	private RectTransform parentConteinerRT;
	private PanelControl[] panelControl;
	private void Start ()
	{
		if (GameSettings.Instance.isSoundSet) Sound.Instance.Shift ();
		Sprite[] sourceSprite = ImageSettings.Instance.cardFacesIcon;
		SpawnPanels (sourceSprite);
		panelControl [GameSettings.Instance.visualCardFacesSet].SetActive (true);
	}
	private void SpawnPanels (Sprite[] spriteList)
	{
		int panelCount = spriteList.Length;
		panelControl = new PanelControl[panelCount];
		GameObject panelOriginObj = (GameObject) Resources.Load ("PanelContent");
		for (int index = 0; index < panelCount; index++)
		{
			GameObject obj = Instantiate (panelOriginObj);
			obj.name = spriteList[index].name;
			obj.transform.parent = parentConteinerRT;
			obj.transform.localScale = Vector3.one;
			panelControl [index] = obj.GetComponent<PanelControl> ();
			int spriteIndex = index;
			panelControl [index].InitButton (spriteList [index], () => GeneralClick (spriteIndex));
			panelControl [index].SetActive (false);
		}
	}
	private void GeneralClick (int spriteIndex)
	{
		if (GameSettings.Instance.isSoundSet) Sound.Instance.Up ();
		panelControl [GameSettings.Instance.visualCardFacesSet].SetActive (false);
		panelControl [spriteIndex].SetActive (true);
		GameSettings.Instance.visualCardFacesSet = spriteIndex;
        SolitaireStageViewHelperClass.instance.ChangeCardFace();
	}
	public void OnBack ()
	{
		PopUpManager.Instance.Close ();
	}
}