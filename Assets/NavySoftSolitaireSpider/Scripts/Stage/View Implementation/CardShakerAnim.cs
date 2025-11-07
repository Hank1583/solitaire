using UnityEngine;

public class CardShakerAnim : MonoBehaviour {

	Vector2 originOffset;
	CardItem card;
	// Use this for initialization
	void Start () {
		card = GetComponent<CardItem> ();
	}
	bool shake = false;
	public void Shake ()
	{
		if (!shake) {
			originOffset = transform.localPosition;
		} else {
			// reset position
			transform.localPosition = originOffset;
		}
		shake = true;
		// TODO remove it
	 
	}

	float time = .15f; //how long it shakes
	float speed = 50.0f; //how fast it shakes
	float amount = 20.0f; //how much it shakes

	float timer;
	void Update()
	{
		if (!shake)
			return;

		float shake_effect = Mathf.Sin (Time.time * speed) * amount;
		transform.localPosition = new Vector3(originOffset.x + shake_effect, originOffset.y);
		timer += Time.deltaTime;

		if (timer > time) {
			shake = false;
			timer = 0;
			transform.localPosition = originOffset;
 
		}
	}
}
