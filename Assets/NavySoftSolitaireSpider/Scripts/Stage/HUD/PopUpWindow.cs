using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HUD
{
    public class PopUpWindow : MonoBehaviour
    {
        public int portraitYoffset;
        public int lanscapeYoffset;

        public GameObject hidePanelButton;
        public GameObject hidePanelButton2;


        [SerializeField]
        private GameObject height;

        public List<PopUpButton> popUpButtons = new List<PopUpButton>();

        public bool isOpened
        {
            get
            {
                return popUpButtons[0].gameObject.activeSelf;
            }
        }

        public void Start()
        {
            DeviceOrientationHandler.instance.OnVerticalOrientationChanged += ChangeOffsetY;
            ChangeOffsetY(DeviceOrientationHandler.instance.isVertical);
        }

        public void OnDestroy()
        {
            if (!DeviceOrientationHandler.destroyed)
                DeviceOrientationHandler.instance.OnVerticalOrientationChanged -= ChangeOffsetY;
        }

        public void ChangeOffsetY(bool isPortrait)
        {
            height.GetComponent<LayoutElement>().minHeight = (isPortrait) ? portraitYoffset : lanscapeYoffset;

        }

        public void show(ButtonModel button)
        {
            List<ButtonModel> buts = new List<ButtonModel>();
            buts.Add(button);
            show(buts);
        }
        public void show(List<ButtonModel> buttons)
        {
            hide();
            int i = 0;
            foreach (var b in buttons)
            {

                string title = b.getTitle();
                bool interactable = b.getAction() != null;
                UnityAction action;
                if (interactable)
                {
                    action = b.getAction();
                }
                else
                {
                    action = hide;
                }

                popUpButtons[i].SetUp(title, interactable, action);
                popUpButtons[i].SetActive(true);
                i++;
            }
            gameObject.SetActive(true);

            // "v" button
            //hidePanelButton.SetActive (false);
            //hidePanelButton2.SetActive (false);
        }

        public void hide()
        {
            // "v" button
            //			hidePanelButton.SetActive (true);
            //		hidePanelButton2.SetActive (true);

            gameObject.SetActive(false);
            foreach (var b in popUpButtons)
            {
                b.SetActive(false);
            }
        }
    }
}
