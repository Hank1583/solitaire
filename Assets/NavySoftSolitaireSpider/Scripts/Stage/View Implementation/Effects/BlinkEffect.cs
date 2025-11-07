using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BlinkEffect : SimpleEffectAbstract {

	Image sprite;
	Color spriteColor;
	


	public override void start () {
		// find sprite
		if (sprite == null) {
			sprite = GetComponent<Image> ();
			spriteColor = sprite.color;
		}
		// reset animation
		alpha = 0.2f;
		Update ();

		base.start ();
	}


	// BLINK ANIMATION

	float alpha = 0f;
	int fade_direction = 1;
	float speed_magic_number = 2f;

	void Update(){
		if (alpha >= 1f) {
			fade_direction = -1;
		} else if (alpha <= 0f) {
			fade_direction = 1;
		}

		alpha += Time.deltaTime * fade_direction * speed_magic_number;
		spriteColor.a = alpha;
		sprite.color = spriteColor;
	}
}