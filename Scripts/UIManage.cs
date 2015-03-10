using UnityEngine;
using System.Collections;

public class UIManage : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject ToysMenu;
	public GameObject DanceMenu;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!MainMenu.activeInHierarchy){
			ToysMenu.SetActive(false);
			DanceMenu.SetActive(false);
		}
	}

	public void OnMainMenuClicked(){
		MainMenu.SetActive(!MainMenu.activeInHierarchy);
	}
	public void OnToysMenuClicked(){
		ToysMenu.SetActive(!ToysMenu.activeInHierarchy);
		DanceMenu.SetActive(false);
	}
	public void OnDanceMenuClicked(){
		DanceMenu.SetActive(!DanceMenu.activeInHierarchy);
		ToysMenu.SetActive(false);
	}
	public void OnClicked(){
		MainMenu.SetActive(false);
	}
}
