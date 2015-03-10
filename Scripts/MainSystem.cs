using UnityEngine;
using System.Collections;

public class MainSystem : MonoBehaviour
{
    private static MainSystem instance;
    public static MainSystem Instance { get { return instance; } }
	
    public enum State
    {
        NoTarget,  
        Stage,      
    }

    private State state = State.NoTarget;
	
    [SerializeField]
    private GameObject prefabStage; 
    private Stage stage;
    public Stage Stage { get { return stage; } }
	
    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private GameObject backgroundCamera;
    [SerializeField]
    private StageTargetEventHandler stageTargetEventHandler;

	
    public Transform MainCameraTansform { get; private set; } 
	

    private void Awake()
    {
        instance = this;

        MainCameraTansform = arCamera.transform;
        
        SetState(State.NoTarget);
    }

	private void Update(){
		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit();
		}
	}

    private void SetState(State paramState)
    {

        state = paramState;
    }
	
    public void LoadStage()
    {
        if (stage != null) { return; }
        SetStageTargetType();

        GameObject obj = Instantiate(
            prefabStage
            , Vector3.zero
            , Quaternion.identity) as GameObject;
        stage = obj.GetComponent<Stage>();
        
            stage.SetVisible(false);

            if (stageTargetEventHandler != null)
            {
                stageTargetEventHandler.BuildNewTarget(false);
            }
        

        SetState(State.Stage);
    }


    private void SetStageTargetType()
    {        
        
        arCamera.gameObject.SetActive(true);
		backgroundCamera.SetActive(true);
        MainCameraTansform = arCamera.transform;
    }
	
    public void OnTrackableStateChanged(bool visible)
    {
        if (stage != null)
        {
            stage.SetVisible(visible);
        }
    }
}
