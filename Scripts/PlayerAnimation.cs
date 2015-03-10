using UnityEngine;
using System.Collections;



public class PlayerAnimation: MonoBehaviour {

	private Animator anim;
	private NavMeshAgent navMeshAgent;
	private bool DamagePlayed = false;
	private AnimatorStateInfo DanceLayerStateInfo;

	public bool playBeself = false;
	public bool playWhiteletter = false;

	public float speedDampTime = 0.3f;
	public float angleSpeedDampTime = 0.3f;
	public Vector3 destination;
	public AudioClip byMyself;
	public AudioClip whiteLetter;
	
	public PlayerState playerState;

	public AnimationClip[] faceAnimClip;
	public string[] faceAnimClipName; 


	void Awake(){
		anim = this.GetComponent<Animator>();
		navMeshAgent = this.GetComponent<NavMeshAgent>();

	}

	void Start(){
		faceAnimClip = Resources.LoadAll<AnimationClip>("FaceAnim");
		faceAnimClipName = new string[faceAnimClip.Length];
		for(int i=0; i<faceAnimClip.Length; i++){
			faceAnimClipName[i] = faceAnimClip[i].name;
		}
	}
	// Update is called once per frame
	void Update () {

		playerState = PlayerController.playerState;
		switch(PlayerController.playerState){
		case PlayerState.Idle: 
//			StartCoroutine("StartChangeFace");
			anim.SetBool("Damage",false);
			anim.SetFloat("Speed", 0 , speedDampTime, Time.deltaTime);
			anim.SetFloat("AngleSpeed", 0 , angleSpeedDampTime, Time.deltaTime);
			break;
		case PlayerState.Walk: 
			PlayerMoveToPoint();
			if(transform.position == navMeshAgent.destination){
				anim.SetFloat("Speed", 0 , speedDampTime, Time.deltaTime);
				anim.SetFloat("AngleSpeed", 0 , angleSpeedDampTime, Time.deltaTime);
			}
			break;
		case PlayerState.GetDamage: 
			if(anim.GetBool("Damage") == false){
				anim.SetBool("Damage",true);
			}
			break;
		case PlayerState.Dance: 
			if(playBeself)
			{
				StartCoroutine("StartChangeFace");
				anim.SetBool("Be Myself",true);
				if(audio.isPlaying){
					audio.Stop();
				}
				audio.clip = byMyself;
				audio.Play();
				playBeself = false;
			}else if(playWhiteletter){
				StartCoroutine("StartChangeFace");
				anim.SetTrigger("WhiteLetter");
				if(audio.isPlaying){
					audio.Stop();
				}
				audio.clip = whiteLetter;
				audio.Play(); 
				playWhiteletter = false;
			}
			break;
		}
		DanceLayerStateInfo = anim.GetCurrentAnimatorStateInfo(1);
//		print(DanceLayerStateInfo.normalizedTime);
	
	}

	IEnumerator StartChangeFace(){

		while(true){
			ChangeFace();
			yield return new WaitForSeconds(2);
		}
	}

	void ChangeFace(){
		anim.SetLayerWeight(2,1);
		int index = Random.Range(0, faceAnimClipName.Length);
		anim.CrossFade(faceAnimClipName[index], 0);
	}

	void PlayerMoveToPoint()  
	{  
		destination = navMeshAgent.destination;
			float angleRad;
			if(navMeshAgent.desiredVelocity == Vector3.zero){
				anim.SetFloat("Speed", 0 , speedDampTime, Time.deltaTime);
				anim.SetFloat("AngleSpeed", 0 , angleSpeedDampTime, Time.deltaTime);
			}else{
				float angle = Vector3.Angle(transform.forward, navMeshAgent.desiredVelocity);
				Vector3 projection = Vector3.Project(navMeshAgent.desiredVelocity, transform.forward);
				anim.SetFloat("Speed", projection.magnitude, speedDampTime, Time.deltaTime);
				angleRad = angle * Mathf.Deg2Rad;
				Vector3 crossRes = Vector3.Cross(transform.forward, navMeshAgent.desiredVelocity);
				if(crossRes.y < 0){
					angleRad = -angleRad;
				}
				anim.SetFloat("AngleSpeed", angleRad , angleSpeedDampTime, Time.deltaTime);
			}
	} 
	
}
