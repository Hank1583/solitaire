using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HUD
{
	public class ButtonModel
	{
		private string title;
		private UnityAction action;

		public ButtonModel setTitle (string title)
		{
			this.title = title;
			return this;
		}

		public ButtonModel setAction (UnityAction action)
		{
			this.action = action;
			return this;
		}

		public string getTitle ()
		{
			return this.title;
		}

		public UnityAction getAction ()
		{
			return this.action;
		}
	}

	public class PopUpButton : MonoBehaviour, IPointerClickHandler
	{
        public Color colorSelected = Color.white;
		private Text title
		{
			get{
				return GetComponentInChildren<Text> ();
			}
		}
		private UnityAction action;

		public void SetUp (string title_name, bool interactable, UnityAction click_action)
		{
			title.text = title_name;
	
			if (interactable) {
				title.color = colorSelected;

			} else {
				title.color = Color.gray;
			}

			action = click_action;
		}
		
		// Update is called once per frame
		public void SetActive (bool active)
		{
			gameObject.SetActive (active);
		}

		#region IPointerClickHandler implementation

		void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
		{
			if (action != null) {
				action ();
			}
		}

		#endregion
	}
}
