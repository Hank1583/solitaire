using System.Collections.Generic;

namespace SmartOrientation
{
	using UnityEngine;
	using System.Collections;

    public class SmartOrientationStage : CanvasSmartOrientation
    {


   
 
        [System.Serializable]
        public class SmartTransformHand
        {
            public Transform target;
            public Transform portraitLeft;


            public Transform portraitRight;

            public Transform landscapeLeft;

            public Transform landscapeRight;

        }

        [System.Serializable]
        public class SmartTransform
        {
            public Transform target;
            public Transform portrait;
            public Transform landscape;
        }

        public List<SmartTransformHand> stackTransforms = new List<SmartTransformHand>();
        public List<SmartTransformHand> stackTransforms_1 = new List<SmartTransformHand>();
        public List<SmartTransformHand> stackTransforms_2 = new List<SmartTransformHand>();
        public List<SmartTransformHand> stackTransforms_3 = new List<SmartTransformHand>();
        public List<SmartTransformHand> stackTransforms_4 = new List<SmartTransformHand>();
        public List<SmartTransformHand> stackTransforms_5 = new List<SmartTransformHand>();
        public List<SmartTransformHand> foundationTransforms = new List<SmartTransformHand>();
        public List<SmartTransformHand> tableuSmartHandTransforms = new List<SmartTransformHand>();
        public List<SmartTransform> tableuSmartTransforms = new List<SmartTransform>();



        public override void OnDeviceOrientationChanged(bool orientation)
        {

 


            StopAllCoroutines();

            ChangeLayout(orientation);

         
   
        }



        private void  ChangeLayout(bool orientation)
        {
 
         
            //AdvertisementsManager.Instance.VisibleBanner(orientation);


            foreach (var s in tableuSmartTransforms)
            {
                //applyTransformByOrientation(s, orientation);
            }


            foreach (var s in stackTransforms)
            {

                StartCoroutine(applyTransformByOrientation(s, orientation));
            }
            foreach (var s in stackTransforms_1)
            {
                StartCoroutine(applyTransformByOrientation(s, orientation));
              
            }
            foreach (var s in stackTransforms_2)
            {
                StartCoroutine(applyTransformByOrientation(s, orientation));
              
            }
            foreach (var s in stackTransforms_3)
            {
                StartCoroutine(applyTransformByOrientation(s, orientation));
                
            }
            foreach (var s in stackTransforms_4)
            {
                StartCoroutine(applyTransformByOrientation(s, orientation));
           
            }
            foreach (var s in stackTransforms_5)
            {
                StartCoroutine(applyTransformByOrientation(s, orientation));
              
            }

            foreach (var s in tableuSmartHandTransforms)
            {
                StartCoroutine(applyTransformByOrientation(s, orientation));
              
            }
            foreach (var s in foundationTransforms)
            {
              StartCoroutine(  applyTransformByOrientation(s, orientation)) ;
              
            }


            SolitaireStageViewHelperClass.instance.SetAllDistanceBetweenCard(true);
        }

		void applyTransformByOrientation(SmartTransform st, bool isPortrait){
			st.target.position = isPortrait? st.portrait.position : st.landscape.position;
		}

      

        IEnumerator applyTransformByOrientation(SmartTransformHand st, bool isPortrait)
        {
            Transform newTransform;
  

            bool isLeftHand = GameSettings.Instance.isHandSet;

           

            yield return new WaitForSeconds(1);
          

            if (isPortrait)
            {

                newTransform = isLeftHand ? st.portraitLeft : st.portraitRight;
            }
            else
            {

                newTransform = isLeftHand ? st.landscapeLeft : st.landscapeRight;
            }

          
            st.target.position = newTransform.position;
            ConvertSizeCard(st.target, isPortrait);
        }

 
        private void ConvertSizeCard(Transform target, bool isPortrait)
        {
            SolitaireStageViewHelperClass.instance.ScaleCard(isPortrait);

        }

        public void ToggleBottomPanel()
        {
            if (HUDController.instance == null) return;
            HUDController.instance.ToggleBottomPanel();
        }

    }

   
}
