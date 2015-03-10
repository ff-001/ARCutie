using UnityEngine;
using System.Collections;

public class UICreateMenu: MonoBehaviour 
{
    public void OnNormalClicked()
    {
        MainSystem.Instance.LoadStage();
    }
}
