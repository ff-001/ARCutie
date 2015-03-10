using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public enum PlayerState{
	Idle,
	Walk,
	GetDamage,
	Dance
}

public class PlayerController : MonoBehaviour {

	public GameObject TimeDialog;
	public Text TimeText;

	NavMeshAgent agent = null;
	Transform sphere;
	PlayerAnimation playerAnim;

	private GameObject PlayMenu;
	private string TimeString = null;
	public AudioClip[] HoursAudioClip;
	public AudioClip DamageAudioClip;
	public AudioClip HelloAudioClip;

	public static PlayerState playerState = PlayerState.Idle;
 
	void Awake(){
		agent = this.GetComponent<NavMeshAgent>();
		playerAnim = this.GetComponent<PlayerAnimation>();
		sphere = GameObject.FindGameObjectWithTag("Target").transform;
		PlayMenu = GameObject.FindGameObjectWithTag("MainMenuBtn");
		TimeDialog = GameObject.Find("Time");
		TimeText = GameObject.Find("TimeText").GetComponent<Text>();
	}

	void Start(){
		HoursAudioClip = Resources.LoadAll<AudioClip>("Voice/04_Hours");
		TimeDialog.SetActive(false);
//		StartCoroutine("WaitSayHello");

	}

	// Update is called once per frame
	void Update () {
//		if(Input.GetMouseButtonDown(0)){
//			playerState = PlayerState.Walk;
//		}
		if(playerState == PlayerState.Walk){
			if(this.transform.position == agent.destination){
				playerState = PlayerState.Idle;
			}
		}

		if(playerState == PlayerState.Idle){
			//enable menu
			if(!PlayMenu.activeInHierarchy){
				PlayMenu.SetActive(true);
			}
			LookAtCamera();
		}

		if(playerState == PlayerState.GetDamage){
			if(audio.isPlaying){
				audio.Stop();
			}
			audio.clip = DamageAudioClip;
			audio.Play();
			StartCoroutine("WaitToIdle");
		}

		if(playerState == PlayerState.Dance){
			//disable menu
			PlayMenu.SetActive(false);
		}
	}

	IEnumerator WaitToIdle(){
		yield return new WaitForSeconds(3);
		playerState = PlayerState.Idle;
	}

	IEnumerator WaitSayHello(){
		yield return new WaitForSeconds(2);
		if(audio.isPlaying){
			audio.Stop();
		}
		audio.clip = HelloAudioClip;
		audio.Play();
	}

	IEnumerator WaitToDance(){
		yield return new WaitForSeconds(4);
		playerState = PlayerState.Dance;
	}

	void LookAtCamera(){
		Quaternion rotation = Quaternion.Euler(sphere.eulerAngles.x, sphere.eulerAngles.y+180, sphere.eulerAngles.z);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);  
	}

	public void SetDestination(Vector3 position){
		if(playerState == PlayerState.Idle){
			agent.destination = position; 
			playerState = PlayerState.Walk;
		}
	}

	public void Damage(){
		if(playerState == PlayerState.Idle)
		playerState = PlayerState.GetDamage;
	}

	public void PlayerDance(string danceName){
		SetDestination(new Vector3(0,0,0));
		StartCoroutine("WaitToDance");
		if(danceName == "BeMyself"){
			playerAnim.playBeself = true;

		}else if(danceName == "WhiteLetter"){
			playerAnim.playWhiteletter = true;

		}
	}

	public void OnAskHour(){
		if(playerState != PlayerState.Dance){
			int hour = DateTime.Now.Hour;
			int minute = DateTime.Now.Minute;
			TimeString = hour + ":" + minute;
			TimeDialog.SetActive(true);
			TimeText.text = TimeString;
			StartCoroutine("DisplayTime");
			if(audio.isPlaying){
				audio.Stop();
			}
			audio.clip = HoursAudioClip[hour];
			audio.Play();
		}
		
	}
	
	IEnumerator DisplayTime(){
		yield return new WaitForSeconds(2);
		TimeDialog.SetActive(false);
	}
}
