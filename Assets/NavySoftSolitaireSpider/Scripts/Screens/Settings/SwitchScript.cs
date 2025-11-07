using UnityEngine;
using UnityEngine.UI;
public class SwitchScript : MonoBehaviour
{
	private const float SPEED = 5;
	[SerializeField]
	private Image buttonBackImage;
	[SerializeField]
	private RectTransform switcherRT;
	[SerializeField]
	private Sprite backOnSprite;
	[SerializeField]
	private Sprite backOffSprite;

	private float switchOnPosition;
	private float currentNormalizeAnimationPosition;
	private bool isAnimate;
	private bool isSwitchOn;

	private void Awake ()
	{
		switchOnPosition = Mathf.Abs (switcherRT.localPosition.x);
		currentNormalizeAnimationPosition = 0f;
		isAnimate = false;
		isSwitchOn = false;
	}
	private void Update ()
	{
		if (isAnimate)
		{
			float deltaPosition = Time.deltaTime * SPEED;
			currentNormalizeAnimationPosition = (isSwitchOn) ? currentNormalizeAnimationPosition + deltaPosition : currentNormalizeAnimationPosition - deltaPosition;
			if (currentNormalizeAnimationPosition < 0f)
			{
				buttonBackImage.sprite = backOffSprite;
				currentNormalizeAnimationPosition = 0f;
				isAnimate = false;
			}
			if (currentNormalizeAnimationPosition > 1f)
			{
				buttonBackImage.sprite = backOnSprite;
				currentNormalizeAnimationPosition = 1f;
				isAnimate = false;
			}
			switcherRT.localPosition = LocalPosition (currentNormalizeAnimationPosition);
		}
	}
	#region Public
	public void Init(bool isOn)
	{
		isSwitchOn = isOn;
		buttonBackImage.sprite = (isOn) ? backOnSprite : backOffSprite;
		currentNormalizeAnimationPosition = (isOn) ? 1f : 0f;
		switcherRT.localPosition = (isOn) ? LocalPosition (1f) : LocalPosition (0f);
	}
	public void SwitchTo(bool isOn)
	{
		if (!isSwitchOn.Equals (isOn))
		{
			if (isAnimate) buttonBackImage.sprite = (isOn) ? backOffSprite : backOnSprite;
			isSwitchOn = isOn;
			isAnimate = true;
		}
	}
	private Vector3 LocalPosition(float normalizeX)
	{
		float value = Mathf.Clamp01 (normalizeX);
		float length = switchOnPosition * 2;
		float position = (length * value) - switchOnPosition;
		return new Vector3 (position, 0, 0);
	}
	#endregion
}