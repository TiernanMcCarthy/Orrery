using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    // Use this for initialization


    public Trans Transform;
    public MyRigidBody Rig;
    public float BallRadius;
    public float RotationSpeed;
    public float speed;


    bool MoveLeft = false;
    bool MoveRight = false;
    bool MoveForward = false;
    bool MoveBackward = false;
    void Start () {
        Transform = GetComponentInParent<Trans>(); //Find the transform component
        Rig = GetComponentInParent<MyRigidBody>();
        Rig.BallRadius = BallRadius;
    }


    void UpdateMovement()
    {
        if(MoveForward==true) //Forward state is defined
        {
            Rig.force = EulerMaths.EulerToDirection(Transform.RotationEuler) * speed*Time.deltaTime; //Work out the direction and apply thrust in that direction
        }
        else if(MoveBackward==true)
        {
              Rig.force = EulerMaths.EulerToDirection(Transform.RotationEuler) * -speed*Time.deltaTime; //the same but in reverse
        }
        if (MoveLeft == true)
        {
              Transform.RotationEuler = new Vector3(Transform.RotationEuler.x, Transform.RotationEuler.y - RotationSpeed * Time.deltaTime, Transform.RotationEuler.z); //Rotate in the left direction by the rotation speed
        }
        else if (MoveRight == true)
        {
            Transform.RotationEuler = new Vector3(Transform.RotationEuler.x , Transform.RotationEuler.y + RotationSpeed * Time.deltaTime, Transform.RotationEuler.z);
           
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Get forward direction of ship
        Rig.boundingCircle.UpdateCentrePoint(Transform.Position); //Set the centre point of the ship to be its current postion
        if (Input.GetKeyDown("w"))
        {
            MoveForward = true;   //Set the forward state to be true and backward to be false as both can't happen at the same time
            MoveBackward = false;
            
        }
        else if(Input.GetKeyDown("s"))
        {
            MoveBackward = true; //The same
            MoveForward = false;
        }
        if (Input.GetKeyDown("a"))
        {
            MoveLeft = true; //You can only turn left and not right at the same time, make this so
            MoveRight = false;
        }
        else if (Input.GetKeyDown("d"))
        {
            MoveLeft = false;
            MoveRight = true;
        }

        if (Input.GetKeyUp("w"))
        {
            MoveForward = false; //If any of the keys are up, cancel their action
        }
        if (Input.GetKeyUp("s"))
        {
            MoveBackward = false;
        }
        if (Input.GetKeyUp("a"))
        {
            MoveLeft = false;
        }
        if (Input.GetKeyUp("d"))
        {
            MoveRight = false;
        } 
        UpdateMovement(); //Update the player input state
    }
}
