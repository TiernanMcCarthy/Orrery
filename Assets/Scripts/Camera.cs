using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to get the Camera to follow the ship
//As the camera has no vertices and is a position for the projection matrix to calculate 
//Transform has to be used instead of my own, as to set the values for the view matrix to use
public class Camera : MonoBehaviour {
    public Ship ShipLocal; //Get the ship position
    public float CameraDistance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = ShipLocal.Transform.Position + EulerMaths.EulerToDirection(ShipLocal.Rig.velocity) * -CameraDistance;
	}
}
