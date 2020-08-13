using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unfinished
public class MyRigidBody : MonoBehaviour {

    public float BallRadius;
    public BoundingSphere boundingCircle;
    public bool test;

    public Trans MotherObject;
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

    public Vector3 Momentum;


    bool forcedupon = true;

    // Use this for initialization
    void Start()
    {

        MotherObject = GetComponentInParent<Trans>(); //Find the transform component
        boundingCircle = new BoundingSphere(MotherObject.Position, BallRadius);
    }


    public void  ApplyMomentum(Vector3 summed)
    {
        velocity = summed / mass;

        //Expand with angular velocity
    }


    public Vector3 GetMomentumAtPoint(Vector3 point)
    {
        Vector3 MomentumLocal;

        //If there is any angular velocity //Use your owN?
        if (AngularVelocity.magnitude>0)
        {                                                                                       //Assume Position is centre of mass
            Vector3 pointVelocity = velocity + VectorMaths.VectorCrossProduct(AngularVelocity,MotherObject.Position - point);

            MomentumLocal = mass * pointVelocity;
        }
        else //Just calculate momentum using normal velocity
        {
            MomentumLocal = mass * velocity;
        }

        return MomentumLocal;
    }
    public void ImpartMomentum(MyRigidBody otherbody, Vector3 ContactPoint)
    {
        //Calculate momentums of both rigid bodies
        Vector3 otherMomentum = otherbody.GetMomentumAtPoint(ContactPoint);
        Vector3 thisMomentum = GetMomentumAtPoint(ContactPoint);

        //Sum the momentums
        Vector3 SummedMomentum = thisMomentum + otherMomentum;

        //Apply the new momentum onto both objects
        ApplyMomentum(SummedMomentum);
        otherbody.ApplyMomentum(SummedMomentum);
    }



    void BringBackTo0()
    {

        if (velocity.x > 0)
        {
            if (velocity.x < minimumspeedbeforestop)
            {
                velocity = new Vector3(0, velocity.y, velocity.z);
            }
        }
        else if (velocity.x < 0)
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
        else if (velocity.z < 0)
        {
            if (velocity.z > -minimumspeedbeforestop)
            {
                velocity = new Vector3(velocity.x, velocity.y, 0);
            }
        }



        if (velocity.y > 0)
        {
            if (velocity.z < minimumspeedbeforestop)
            {
                velocity = new Vector3(velocity.x, 0, velocity.z);
            }
        }
        else if (velocity.y < 0)
        {
            if (velocity.y > -minimumspeedbeforestop)
            {
                velocity = new Vector3(velocity.x, 0, velocity.z);
            }
        }
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



    public BoundingSphere GiveBoundingSphere()
    {
        return boundingCircle;
    }

    //Force                 //Where Collided        //Centre of Object
    public void CalculateTorque(Vector3 Forcey, Vector3 ImpactPoint, Vector3 CentreOfMass)
    {
        torque += VectorMaths.VectorCrossProduct(Forcey, (ImpactPoint - CentreOfMass));
    }



    void PhysicsUpdate()
    {

        //Angular Velocity


        //UpdateAxis();
        if (force.x != 0 || force.y != 0 || force.z != 0)
        {
            acceleration = force / mass;
            velocity += acceleration * Time.fixedDeltaTime;
            if (torque.x != 0 || torque.y != 0 || torque.z != 0)
            {
                AngularAcceleration = torque / inertia;
                AngularVelocity += AngularVelocity * Time.fixedDeltaTime;

                Quat q = new Quat();

                //convert angular velocity in quaternion
                float avMag = VectorMaths.VectorMagnitude(AngularVelocity * Time.fixedDeltaTime);

                q.w = Mathf.Cos(avMag / 2);
                q.x = Mathf.Sin(avMag / 2) * (AngularVelocity * Time.fixedDeltaTime).x / avMag;
                q.y = Mathf.Sin(avMag / 2) * (AngularVelocity * Time.fixedDeltaTime).y / avMag;
                q.z = Mathf.Sin(avMag / 2) * (AngularVelocity * Time.fixedDeltaTime).z / avMag;

                Quat TargetOrientation = q * MotherObject.RotationQuat;
            }
            force = new Vector3(0, 0, 0);

        }
        forcedupon = false;

        if (Moving() == true)
        {
            BringBackTo0();
            velocity += -velocity * friction * Time.fixedDeltaTime;
            MotherObject.Position += velocity;
        }
        boundingCircle.UpdateCentrePoint(MotherObject.Position);
        Momentum = mass * velocity;
    }





    void FixedUpdate()
    {
        PhysicsUpdate();


    }


    // Update is called once per frame
    void Update()
    {

    }

}