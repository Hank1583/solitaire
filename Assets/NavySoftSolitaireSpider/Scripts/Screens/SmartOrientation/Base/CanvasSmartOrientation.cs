namespace SmartOrientation
{
	using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;
	using System.Collections.Generic;

	[RequireComponent (typeof(CanvasScaler))]
	public abstract class CanvasSmartOrientation : MonoBehaviour
	{
	
	
	
		public List<GameObject> portraitOnlyObjects = new List<GameObject> ();
		public List<GameObject> landscapeOnlyObjects = new List<GameObject> ();
       
	
		public enum LandscapeMatchingMode
		{
			WIDTH = 0,
			HEIGHT = 1,
			AS_PORTRAIT = 3
		}

		public LandscapeMatchingMode landscapeMatching = LandscapeMatchingMode.AS_PORTRAIT;



		RectTransform _rectTransform;
		RectTransform rect{
			get
			{ 
				if(_rectTransform == null){
					_rectTransform = GetComponent<RectTransform> ();
				}
				return _rectTransform;
			}
		}

	protected	CanvasScaler _scaler;
    protected CanvasScaler scaler{
			get
			{ 
				if(_scaler == null){
					_scaler = GetComponent<CanvasScaler> ();
				}
				return _scaler;
			}
		}

	

		void Start(){
			DeviceOrientationHandler.instance.OnVerticalOrientationChanged += ChangeCanvasMatching;
			ChangeCanvasMatching (DeviceOrientationHandler.instance.isVertical);
            OnDeviceOrientationChanged(DeviceOrientationHandler.instance.isVertical);
        }

		void OnDestroy(){
			if(!DeviceOrientationHandler.destroyed)
				DeviceOrientationHandler.instance.OnVerticalOrientationChanged -= ChangeCanvasMatching;
		}

	
		// PUBLIC METHODS

//		bool isVert = true;
	

		public virtual void ChangeCanvasMatching (bool vert)
		{
 
			float matching = 0;

			if (vert) {
                matching = 0;
 
            
		
			} else {
              
                 
                // landscape mode
                if (LandscapeMatchingMode.WIDTH.Equals (landscapeMatching)) {
					matching = 0f;
				} else if (LandscapeMatchingMode.HEIGHT.Equals (landscapeMatching)) {
					matching = 1f;
				} else if (LandscapeMatchingMode.AS_PORTRAIT.Equals (landscapeMatching)) {
					matching = DeviceOrientationHandler.instance.isVertical ? 0 : 1f;// 1f - aspectRatio;
				}


               // scaler.referenceResolution = new Vector2(1080, 1080);
            }

			// apply matching
			scaler.matchWidthOrHeight = matching;
			ChangeLayout (vert);
		}

	


		// third party call FORCE UPDATE LAYOUT
//		public void UpdateLayout(){
//			ChangeLayout (true);
//		}



		// TODO: prevent ~5 times calling that function
//		public void OnRectTransformDimensionsChange(){
//			ChangeLayout(isVert);
//		}




		// CALL ACTION
		void ChangeLayout (bool vert)
		{
 

			UpdateObjects (vert);
       
			OnDeviceOrientationChanged (vert);
		}


		public void Refresh(){
			UpdateObjects (DeviceOrientationHandler.instance.isVertical);
       
            OnDeviceOrientationChanged (DeviceOrientationHandler.instance.isVertical);
		}


		public virtual void UpdateObjects (bool isPort)
		{
			foreach (var go in portraitOnlyObjects) {
				go.SetActive (isPort);
			}
			foreach (var go in landscapeOnlyObjects) {
				go.SetActive (!isPort);
			}
		}

		public abstract void OnDeviceOrientationChanged (bool vert);
	



		// PRIVATE GETTERS

		protected float aspectRatio {
			get {
//				if (height > width) {
//					return width / height;
//				} else {
//					return height / width;
//				}
				return Mathf.Min (width, height) / Mathf.Max(width, height);
			}
		}

		float width {
			get { 
				#if UNITY_EDITOR
				return rect.sizeDelta.x;
				#else
				return Screen.width;
				#endif
			}
		}

		float height {
			get { 
				#if UNITY_EDITOR
				return rect.sizeDelta.y;
				#else
				return Screen.height;
				#endif
			}
		}
	}
}
