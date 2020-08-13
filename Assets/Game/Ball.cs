using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public Trans Transform;
    public float BallRadius;
    public GameObject enemy;

    public bool test;

    public MyRigidBody Rig;


    // Use this for initialization
    void Start()
    {
        Transform = GetComponentInParent<Trans>(); //Find the transform component
        Rig = GetComponentInParent<MyRigidBody>();
        Rig.BallRadius = BallRadius;

    }


    public BoundingSphere GiveBoundingSphere()
    {
        return Rig.boundingCircle;
    }


    // Update is called once per frame
    void Update()
    {
        Rig.boundingCircle.UpdateCentrePoint(Transform.Position); //Update bounding location
        if (test == true) //One time collision ensured by test
        {
            if (Rig.boundingCircle.Intersects(enemy.GetComponent<Ship>().Rig.boundingCircle) == true) //If the player object intersects with the ball
            {
                Rig.ImpartMomentum(enemy.GetComponent<Ship>().Rig, (enemy.GetComponent<Ship>().Transform.Position - Transform.Position) * BallRadius) ; //Add momentum to this ball and the ship
                test = false; //Do not repeat this, as it breaks 
            }
        }
    }

}
