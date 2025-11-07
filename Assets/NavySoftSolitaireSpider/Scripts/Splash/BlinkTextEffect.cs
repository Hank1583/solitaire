using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BlinkTextEffect : MonoBehaviour {
	[SerializeField]
	Text text;
	Color spriteColor;
	


	public  void Start () {
		// find sprite
		if (text == null) {
			text = GetComponent<Text> ();
			spriteColor = text.color;
		}
		// reset animation
		alpha = 1f;
		Update ();
	}


	// BLINK ANIMATION

	float alpha = 0f;
	int fade_direction = 1;
	float speed_magic_number = 1f;

	void Update(){
		if (text == null)
			return;
		
		if (alpha >= 1f) {
			fade_direction = -1;
		} else if (alpha <= 0f) {
			fade_direction = 1;
		}

		alpha += Time.deltaTime * fade_direction * speed_magic_number;
		spriteColor.a = alpha;
		text.color = spriteColor;
	}
}