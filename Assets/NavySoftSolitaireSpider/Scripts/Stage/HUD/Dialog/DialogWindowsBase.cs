using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HUD
{
	public class DialogWindowsBase : MonoBehaviour
	{
		public GameObject container;
		public Text message;
		public List<DialogButton> dialogButtons;
	

		public void show (string message_text, List<ButtonModel> buttons)
		{
			hide ();
			// set up title
			message.text = message_text;

			if (buttons.Count > dialogButtons.Count) {
				throw new UnityException("Dialog window can't hold more then " + dialogButtons.Count + " buttons!");
			}

			int i = 0;
			foreach (var b in buttons) {

				string title = b.getTitle ();
				bool interactable = b.getAction () != null;
				UnityAction action; 
				if (interactable) {
					action = b.getAction ();
				} else {
					action = hide;
				}

				dialogButtons [i].SetUp (title, interactable, action, hide);
				dialogButtons [i].SetActive (true);
				i++;
			}
			container.SetActive (true);
		}

		public void hide ()
		{
			container.SetActive (false);
			foreach (var b in dialogButtons) {
				b.SetActive (false);
			}
		}
	}
}
