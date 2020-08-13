using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingSphere {

    // Use this for initialization
    public Vector2 CentrePoint;
    public Vector3 CentrePoint3D; //Centrepoint for the bounding sphere to be based from
    public float Radius;
    
    public BoundingSphere(Vector2 Centre,float radius) //Initialisers for provided values
    {
        CentrePoint = Centre;
        Radius = radius;
    }
    public BoundingSphere(Vector3 Centre, float radius)
    {
        CentrePoint3D = Centre;
        Radius = radius;
    }

    public void UpdateCentrePoint(Vector3 centre) //Update the moving centre
    {
        CentrePoint3D = centre;
    }


    public bool Intersects(BoundingSphere Comparison) //d2 <= (r1 + r2)2 Check if two objects are colliding

    {
        //Create a vector to represent the direction and length in comparison to the other circle
        Vector3 VectorToOther = Comparison.CentrePoint3D - CentrePoint3D;

        //Calculate the combined radio squared;
        float combinedRadiusSqrt = (Comparison.Radius + Radius);
        combinedRadiusSqrt *= combinedRadiusSqrt;

        return VectorMaths.LengthSq(VectorToOther) <= combinedRadiusSqrt;
    }

}
