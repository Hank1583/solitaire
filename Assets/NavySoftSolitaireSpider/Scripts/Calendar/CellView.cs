namespace Calendar
{
	using UnityEngine;
	using UnityEngine.UI;
    [RequireComponent(typeof(CanvasGroup))]
	public class CellView : MonoBehaviour
	{
		private const float ROTATE_KOEF = 80f;
		private const float ROTATE_SPEED = 4f;
		private const float STOP_ROTATE_TIME = 5f;
		private const float HALO_SPEED = 1f;
		[SerializeField]
		private Text dateText;
 
		 
		[SerializeField]
		private GameObject haloObj;
        [SerializeField]
        private Color colorDayShow = Color.white;
        [SerializeField]
        private Color colorDayHide = Color.black;
        [SerializeField]
        private Sprite boardComplete;
        [SerializeField]
        private Sprite boardCurrent;
 
		private Image haloImage;

        
 

		 


		private int dateNumb;

		bool isActiveCell;
		public bool IsActiveCell {get{ return isActiveCell;}}
		public void Awake1 ()
		{
		 
		 
			haloImage = haloObj.GetComponent<Image> ();
			HideComponents ();
		}
		public void HideComponents()
		{
		 
 
			 

            transform.Find("bkg").GetComponent<Image>().sprite = boardCurrent;
          
            dateText.gameObject.SetActive(true);

            haloObj.SetActive (false);
		 
			SetTextColor (Color.grey);
			isActiveCell = false;
			dateNumb = -1;
		}
	 
		#region Public
		public void Print()
		{
			dateText.text = "";
		}
		public void Print(int value)
		{
			dateText.text = value.ToString ();
		}
	 
		public void ShowGoldMedal()
		{
		 
            dateText.gameObject.SetActive(false);
            transform.Find("bkg").GetComponent<Image>().sprite = boardComplete;
        }
	 
		public void AnimateGoldMedal()
		{
			ShowGoldMedal ();
		 
		 
		}
		public void ShowHalo(bool visible)
		{
			haloObj.SetActive (visible);
         
		 
		}
	 
		public void SetTextColor (Color color)
		{
			dateText.color = color;
		}
		#endregion


		#region Active
		public void SetDate(int date)
		{
			dateNumb = date;
			Print (date);
		}
		public int GetDate()
		{
			return dateNumb;
		}
		private void ClearDate()
		{
			this.dateNumb = -1;
			Print ();
		}
		public void SetActiveCell (bool isActive)
		{
			Color textColor = (isActive) ? colorDayShow : colorDayHide;
            GetComponent<Button>().interactable = (isActive);
            SetTextColor (textColor);
			isActiveCell = isActive; 
		}
		#endregion
	}
}