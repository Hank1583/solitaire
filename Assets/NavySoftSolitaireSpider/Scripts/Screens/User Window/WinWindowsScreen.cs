namespace UserWindow
{
	using UnityEngine;
    using System.Collections;
    public class WinWindowsScreen : MonoBehaviour
	{
        [SerializeField]
        private ParticleSystem winEffect;
		private bool isAnimating;
		private float timer;
		private void Start ()
		{
            

            ContinueModeGame.instance.ClearAllDataCard();
			isAnimating = true;
			timer = 0f;
          
          
            winEffect.Play();
            StartCoroutine(Close());
        }

        private IEnumerator Close()
        {
            yield return new WaitForSeconds(4);
            PopUpManager.Instance.Close();
        }
		 
	}
}