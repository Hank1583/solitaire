namespace SmartOrientation
{
	using UnityEngine;
	using System.Collections;

	public class SmartOrientationSimple : CanvasSmartOrientation
	{

		#region implemented abstract members of CanvasSmartOrientation

	
		public override void OnDeviceOrientationChanged (bool orientation)
		{
            if (!DeviceOrientationHandler.instance.isVertical)
            {
                scaler.matchWidthOrHeight = 1;
            }
            else
            {
                scaler.matchWidthOrHeight = 0;
            }
          
        }

	
		#endregion
	}
}
