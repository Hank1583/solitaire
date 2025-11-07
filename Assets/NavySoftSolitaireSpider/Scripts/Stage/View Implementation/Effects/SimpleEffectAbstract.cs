using UnityEngine;

public abstract class SimpleEffectAbstract : MonoBehaviour {

	public virtual void start () {
		gameObject.SetActive (true);
	}
	
	public virtual void stop () {
		gameObject.SetActive (false);	
	}
}
