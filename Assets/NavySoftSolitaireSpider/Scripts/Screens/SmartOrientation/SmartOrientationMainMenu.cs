using UnityEngine;
using System.Collections;

namespace SmartOrientation
{
	public class SmartOrientationMainMenu : MonoBehaviour
	{

		public GameObject portraitLayout;
		public GameObject landscapeLayout;

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
			portraitLayout.SetActive (o);
			landscapeLayout.SetActive (!o);
		}
	}
}
