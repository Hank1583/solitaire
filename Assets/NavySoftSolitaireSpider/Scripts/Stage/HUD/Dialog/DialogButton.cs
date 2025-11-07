using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HUD
{
	public class DialogButtonModel
	{
		private string title;
		private UnityAction action;

		public DialogButtonModel setTitle (string title)
		{
			this.title = title;
			return this;
		}

		public DialogButtonModel setAction (UnityAction action)
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

	public class DialogButton : MonoBehaviour, IPointerClickHandler
	{

		private Text title;
		private UnityAction action;
		private UnityAction second_action;

		public void SetUp (string title_name, bool interactable, UnityAction click_action, UnityAction reserve_action = null)
		{
			title = GetComponentInChildren<Text> ();
			title.text = title_name;
	
			if (interactable) {
				title.color = Color.white;

			} else {
				title.color = Color.gray;
			}

			action = click_action;
			second_action = reserve_action;
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
			if (second_action != null) {
				second_action ();
			}
		}

		#endregion
	}
}
