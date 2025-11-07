using System.Collections.Generic;

namespace SmartOrientation
{
    using UnityEngine;
    using System.Collections;

    public class SmartOrientationHUD : CanvasSmartOrientation
    {



        #region implemented abstract members of CanvasSmartOrientation	
        public override void OnDeviceOrientationChanged(bool vert)
        {
            if (HUDController.instance.WinGame)
            {
                HUDController.instance.VisibleLayout(false);
            }
        }
        #endregion
    }
}
