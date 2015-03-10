using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {
	
	private PlayerController playerController;
//	private PlayerAnimation playerAnim;
	void Start(){
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
//		playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector3 cursorScreenPosition = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
			RaycastHit hit;  

			if(!EventSystem.current.IsPointerOverGameObject()){
				if(Physics.Raycast(ray,out hit)){  
					if(hit.collider.gameObject.tag=="Terrain"){ 
						playerController.SetDestination(hit.point); 
					}else if(hit.collider.gameObject.tag=="Player"){
						playerController.Damage();
					} 
				}  
			}
		}

	}

	public void PlayerDanceBeMyself(){
		playerController.PlayerDance("BeMyself");
	}
	public void PlayerDanceWhiteLetter(){
		playerController.PlayerDance("WhiteLetter");
	}
	public void AskTime(){
		playerController.OnAskHour();
	}
	
}
