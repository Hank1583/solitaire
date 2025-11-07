using UnityEngine;
using System.Collections;

public class HideBottomPanelAnim : MonoBehaviour {

	[SerializeField]
	private Transform targetPanel;
	[SerializeField]
	private Transform openedAnchor;
	[SerializeField]
	private Transform closedAnchor;
 
 
 


	bool anim = false;
	bool is_open = true;

	public void ShowPanel(bool open){

		if (open) {
 
			if(SolitaireStageViewHelperClass.instance != null)
				SolitaireStageViewHelperClass.instance.UpdateViewSettings ();
		}
       
       
		is_open = open;
		if (gameObject.activeSelf) {
			anim = true;
		}else{
			
		}
	}

	 

	void OnEnable(){

        StartCoroutine(SetBeginPosition());
        
	}
	
   private IEnumerator SetBeginPosition()
    {
        yield return new WaitForSeconds(.02f);
        targetPanel.position = is_open ? openedAnchor.position : closedAnchor.position;
    }
	void Update () {

       
		if (!anim)
			return;

		float speed = (Screen.height / 1) *2;
		float delta =  Time.deltaTime * (is_open? 1:-1) * speed;
		Vector3 newPos = new Vector3 (targetPanel.position.x, targetPanel.position.y + delta);
       
        targetPanel.position = newPos;

		 CheckPosition ();
	}

	void CheckPosition(){
		if (targetPanel.position.y < closedAnchor.position.y) {
			targetPanel.position = closedAnchor.position;
			anim = false;
		} else if (targetPanel.position.y > openedAnchor.position.y) {
			targetPanel.position = openedAnchor.position;
			anim = false;
		}

 
    }
}
