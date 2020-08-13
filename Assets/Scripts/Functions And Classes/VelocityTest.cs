using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTest : MonoBehaviour {
    float accumulator = 0.0f;
    const float FixedTimeStep = 0.02f; //1.0f/50


    //Linear Motion
    Vector3 acceleration;
    public Vector3 velocity;
    public float mass = 1;
    public Vector3 force;
    public float friction;
    public float gravity = 1;
    public float minimumspeedbeforestop;



    //Angular Motion
    public Vector3 torque;
    Vector3 AngularAcceleration;
    Vector3 AngularVelocity;
    public float inertia = 1;



    bool forcedupon = false;

    public Trans Transformation;

    List<bool> activeAxis = new List<bool> { false, false, false };
    // Use this for initialization
    void Start() {
        Transformation = GetComponentInParent<Trans>();
    }

    //Return a list of active axis for processing
    void CheckAxis()
    {
        if (force.x == 0)
        {
            activeAxis[0] = false;
        }
        else
            activeAxis[0] = true;
        if (force.y == 0)
        {
            activeAxis[1] = false;
        }
        else
            activeAxis[1] = true;
        if (force.z == 0)
        {
            activeAxis[2] = false;
        }
        else
            activeAxis[2] = true;


    }
    struct frictionAndGravity
    {
        public short x;
        public short y;
        public short z;


    }


    bool Moving()
    {
        if (velocity.x != 0)
            return true;
        else if (velocity.y != 0)
            return true;
        else if (velocity.z != 0)
            return true;
        return false;
    }


    void BringBackTo0()
    {

        if(velocity.x>0)
        {
            if (velocity.x < minimumspeedbeforestop)
            {
                velocity = new Vector3(0, velocity.y, velocity.z);
            }
        }
        if (velocity.x < 0)
        {
            if (velocity.x > -minimumspeedbeforestop)
            {
                velocity = new Vector3(0, velocity.y, velocity.z);
            }
        }

        if (velocity.z > 0)
        {
            if (velocity.z < minimumspeedbeforestop)
            {
                velocity = new Vector3(velocity.x, velocity.y, 0);
            }
        }
        if (velocity.z < 0)
        {
            if (velocity.z > -minimumspeedbeforestop)
            {
                velocity = new Vector3(velocity.x, velocity.y, 0);
            }
        }
    }

    void PhysicsUpdate()
    {

        //Angular Velocity


        AngularAcceleration = torque / inertia;
        AngularVelocity += AngularVelocity * Time.deltaTime;

        Quat q = new Quat();

        //convert angular velocity in quaternion
        float avMag = VectorMaths.VectorMagnitude(AngularVelocity * Time.fixedDeltaTime);

        q.w = Mathf.Cos(avMag / 2);
        q.x = Mathf.Sin(avMag / 2) * (AngularVelocity * Time.fixedDeltaTime).x / avMag;
        q.y = Mathf.Sin(avMag / 2) * (AngularVelocity * Time.fixedDeltaTime).y / avMag;
        q.z = Mathf.Sin(avMag / 2) * (AngularVelocity * Time.fixedDeltaTime).z / avMag;

        Quat TargetOrientation = q * Transformation.RotationQuat;


        //UpdateAxis();
        if (forcedupon == true)
        {
            acceleration = force / mass;
            velocity += acceleration * Time.deltaTime;

        }
        forcedupon = false;

        if (Moving()==true)
        {
            BringBackTo0();
            velocity += -velocity * friction * Time.deltaTime;
            Transformation.Position += velocity * Time.deltaTime;
        }
    }
	void setAcceleration()
    {
        forcedupon = true;
        force = new Vector3(60,0,0); //Force
    }
    void CalculateTorque(Vector3 Forcey,Vector3 ImpactPoint,Vector3 CentreOfMass)
    {
        torque = VectorMaths.VectorCrossProduct(Forcey  ,(ImpactPoint - CentreOfMass));
    }
	// Update is called once per frame
	void Update () {

        accumulator += Time.deltaTime;

        while(accumulator>=FixedTimeStep)
        {
            //Update Physics
            PhysicsUpdate();
            accumulator -= FixedTimeStep;


        }

        if (Input.GetKeyDown("w"))
            setAcceleration();

        

	}
}
