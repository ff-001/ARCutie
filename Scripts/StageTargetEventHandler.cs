using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageTargetEventHandler : MonoBehaviour, IUserDefinedTargetEventHandler
{
    [SerializeField]
    private ImageTargetBehaviour targetTemplate_Normal; 

    private UserDefinedTargetBuildingBehaviour mTargetBuildingBehaviour;
    private ImageTracker mImageTracker;
    private DataSet mBuiltDataSet;

    private Trackable trackable; 
    private GameObject objTarget;  

	public void Awake ()
    {
        mTargetBuildingBehaviour = GetComponent<UserDefinedTargetBuildingBehaviour>();
        if (mTargetBuildingBehaviour)
        {
            mTargetBuildingBehaviour.RegisterEventHandler(this);
            Debug.Log("Registering to the events of IUserDefinedTargetEventHandler");
        }
        trackable = null;
	}

    public void OnInitialized()
    {
        mImageTracker = TrackerManager.Instance.GetTracker<ImageTracker>();
        if (mImageTracker != null)
        {
            // create a new dataset
            mBuiltDataSet = mImageTracker.CreateDataSet();
            mImageTracker.ActivateDataSet(mBuiltDataSet);
        }
    }

    public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality)
    {
        //mFrameQuality = frameQuality;
    }

    public void OnNewTrackableSource(TrackableSource trackableSource)
    {
        if (trackable != null) { return; } 

        // deactivates the dataset first
        mImageTracker.DeactivateDataSet(mBuiltDataSet);

		ImageTargetBehaviour targetTemplate = targetTemplate_Normal;
        if (targetTemplate == null) { return; }
        GameObject objCopy = Instantiate(targetTemplate.gameObject) as GameObject;
        ImageTargetBehaviour imageTargetCopy = objCopy.GetComponent<ImageTargetBehaviour>();
        imageTargetCopy.gameObject.name = targetTemplate.TrackableName + "_Created";

        DataSetTrackableBehaviour created = mBuiltDataSet.CreateTrackable(trackableSource, imageTargetCopy.gameObject);
        trackable = created.Trackable;
        objTarget = imageTargetCopy.gameObject;

        // activate the dataset again
        mImageTracker.ActivateDataSet(mBuiltDataSet);
    }
	
    public void BuildNewTarget(bool extended)
    {
        if (trackable != null)
        {
            Debug.Log("Target already created");
            return;
        }

		ImageTargetBehaviour targetTemplate = targetTemplate_Normal;
        if (targetTemplate == null) { return; }

        string trackableName = targetTemplate.TrackableName;
        // Debug.Log("BuildNewTarget " + trackableName + " size:" + targetTemplate.GetSize());
        mTargetBuildingBehaviour.BuildNewTarget(trackableName, targetTemplate.GetSize().x);
    }
	
    public void ClearTarget()
    {
        mImageTracker.DeactivateDataSet(mBuiltDataSet);
        
        mBuiltDataSet.Destroy(trackable, true);
        trackable = null;
        objTarget = null;
        
        mImageTracker.ActivateDataSet(mBuiltDataSet);
    }
	
    public bool IsTargetCreated()
    {
        return (trackable != null);
    }
	
    public GameObject GetTargetObject()
    {
        return objTarget;
    }

    
}
