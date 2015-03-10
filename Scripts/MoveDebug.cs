using UnityEngine;
using System.Collections;
using System.IO;

public class MoveDebug : MonoBehaviour {
	
	Vector3 targetPosition;
	
	//the speed, in units per second, we want to move towards the target
	public float speed = 5f;
	
	public bool holdToFollow = true;
	
	public bool targetSet = false;
	
	public float stoppingAccuracy = 2f;
	
//	public MouseUtils.Button respondToMouseButton = MouseUtils.Button.Left;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(holdToFollow) {
			if(Input.GetMouseButton(0)) {
				//mouse has been clicked! Update position
				//Use the object's current depth, so we can move around on a 2D plane
				targetPosition = GetMouseAtObjectDepth();
				//make sure we "turn on" the movement
				targetSet = true;
			}
		} else {
			if(Input.GetMouseButtonDown(0)) {
				//mouse has been clicked! Update position
				//Use the object's current depth, so we can move around on a 2D plane
				targetPosition = GetMouseAtObjectDepth();
				//make sure we "turn on" the movement
				targetSet = true;
			}
		}
		
		MoveTowardsTarget();
	}
	
	//Get position of the mouse at the depth of an object
	Vector3 GetMouseAtObjectDepth() {
		
		//Find the depth into the scene, the distance won't work here because objects 
		// directly in front of the camera are closer than objects around the edges of the screen.
		//So we'll get the vector that points to the object, then use the dot product to see 
		// how much of that overlaps with the forward heading of the camera.
		//Get the vector from the camera to the object
		Vector3 headingToObject = this.transform.position - Camera.main.transform.position;
		//Find the projection on the forward vector of the camera
		float depth = Vector3.Dot(headingToObject, Camera.main.transform.forward);
		return GetMouseAtSpecifiedDepth(depth);
	}
	
	Vector3 GetMouseAtSpecifiedDepth(float depth) {
		Vector3 mouseScreenPosition = Input.mousePosition;
		mouseScreenPosition.z = depth;
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
		return new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, mouseWorldPosition.z);
	}
	
	public void OnDrawGizmos() {
		if(targetSet) {
			//Show a line from our current position to the target position
			Gizmos.DrawLine(this.transform.position, targetPosition);
			
			//show us where the minimum stopping distance is
			Gizmos.DrawWireSphere(targetPosition, stoppingAccuracy);
			
		}
	}
	
	//move towards a target at a set speed.
	private void MoveTowardsTarget() {
		//only do work if we have a valid target
		if(targetSet) {
			Vector3 currentPosition = this.transform.position;
			//first, check to see if we're close enough to the target
			//this check prevents us from oscillating back and forth over the target
			//if we're farther than 1 unit away, do the movement, otherwise do nothing
			if(Vector3.Distance(currentPosition, targetPosition) > stoppingAccuracy) { 
				
				//get the direction we need to go by subtracting the current position from the target position
				Vector3 directionOfTravel = targetPosition - currentPosition;
				//now normalize the direction, since we only want the direction information
				directionOfTravel.Normalize();
				
				//now move at the specified speed in the direction of travel
				//scale the movement on each axis by the directionOfTravel vector components
				
				this.transform.Translate(
					(directionOfTravel.x * speed * Time.deltaTime),
					(directionOfTravel.y * speed * Time.deltaTime),
					(directionOfTravel.z * speed * Time.deltaTime),
					Space.World);
				
			} else {
				//We've completed the move, indicate we're done by turning off targetSet
				targetSet = false;
			}
		}
	}
	
	public override string ToString ()
	{
		return string.Format ("[MoveTowardsClick]: " + targetPosition + " @ "
		                      + speed + "m/s, target set: " + targetSet);
	}
}