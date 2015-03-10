using UnityEngine;
using System.Collections;

public class StageTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    public MainSystem mainSystem;   
    private TrackableBehaviour mTrackableBehaviour;

	private void Awake()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
	}

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			mainSystem.OnTrackableStateChanged(true);
		}
		else
		{
			mainSystem.OnTrackableStateChanged(false);
		}

    }
}
