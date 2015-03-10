using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Stage : MonoBehaviour
{
    public enum State
    {
        Ready, 
        Loading, 
    }

    public Transform Trans { get; private set; }
    [SerializeField]
    private GameObject prefabPawn;
    private State state = State.Ready;
    private bool isVisible = true; 

    private void Awake()
    {
        Trans = transform;
        GameObject objPawn = Instantiate(prefabPawn, Vector3.zero, Quaternion.Euler(0.0f, 180.0f, 0.0f)) as GameObject;
        objPawn.transform.parent = Trans;
        SetState(State.Ready);
    }

    private void SetState(State paramState)
    {

        state = paramState;

    }
	
    public void SetVisible(bool visible)
    {
        if (isVisible == visible) { return; }
        isVisible = visible;

        Renderer[] rendererComponents = gameObject.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = visible;
        }
    }


    public GameObject InstantiatePrefab(string source)
    {
        UnityEngine.Object prefab = Resources.Load(source);
        if (prefab == null)
        {
            Debug.LogError("[Stage] InstantiatePrefab() Invalid prefab source");
            return null;
        }

        GameObject obj = GameObject.Instantiate(prefab) as GameObject;
        obj.transform.parent = Trans;
        
        if (!isVisible)
        {
            Renderer[] rendererComponents = obj.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = isVisible;
            }
        }

        obj.SetActive(false);
        return obj;
    }
	
    public Coroutine StartStageCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
	
    public void StopStageCoroutine(IEnumerator routine)
    {
        StopCoroutine(routine);
    }
}