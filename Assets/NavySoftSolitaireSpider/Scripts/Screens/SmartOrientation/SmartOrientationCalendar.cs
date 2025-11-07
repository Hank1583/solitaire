using System.Collections.Generic;

namespace SmartOrientation
{
	using UnityEngine;
	using System.Collections;

	public class SmartOrientationCalendar : MonoBehaviour
	{

		public GameObject portraitLayout;
		public GameObject landscapeLayout;
		public GameObject landscapeLayoutiPad;

 

		void Start(){

			DeviceOrientationHandler.instance.OnVerticalOrientationChanged += OnDeviceOrientationChanged;
			OnDeviceOrientationChanged (DeviceOrientationHandler.instance.isVertical);
           

        }

		void OnDestroy(){
			if(!DeviceOrientationHandler.destroyed)
				DeviceOrientationHandler.instance.OnVerticalOrientationChanged -= OnDeviceOrientationChanged;
		}

		void OnDeviceOrientationChanged (bool o)
		{
			// enable or disable portrait
			portraitLayout.SetActive (o);

			// disable landscapes
			landscapeLayout.SetActive (false);
			landscapeLayoutiPad.SetActive (false);

			// enable regular or iPad landscape layout
			if (!o) {
				float aspect = 1f;

				float width;
				float height;

				#if UNITY_EDITOR
				Canvas c = (Canvas)FindObjectOfType<Canvas> ();
				RectTransform r = c.gameObject.GetComponent<RectTransform> ();
//				aspect = r.rect.width / r.rect.height;
				width = r.rect.width;
				height = r.rect.height;
				#else
//				aspect = Screen.width / Screen.height;
				width = Screen.width;
				height = Screen.height;
				#endif
				aspect = Mathf.Max (width, height) / Mathf.Min(width, height);
				print ("Calendar aspect = "+aspect);
				// well 4:3 is iPad aspect equals 1.33 ratio
				if (aspect < 1.51) {
					landscapeLayoutiPad.SetActive (true);
				} else {
					landscapeLayout.SetActive (true);
				}		
			}
		
//			foreach (var s in smartTransforms) {
//				applyTransformByOrientation (s, o);
//			}
		}

//		void applyTransformByOrientation(SmartTransform st, bool o){
//			st.target.position = o? st.portrait.position : st.landscape.position;
//		}
	}
}
