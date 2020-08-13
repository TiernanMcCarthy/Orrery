using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingCapsule {

    public Vector3 A; //Start
    public Vector3 B;
    public float Radius;
   // public List<EnemyHolder>
    public BoundingCapsule(Vector3 A, Vector3 B, float Radius)
    {
        this.A = A;
        this.B = B;
        this.Radius = Radius;
    }

    public bool Intersects(BoundingSphere otherCircle)
    {
        //Square the sum of both radii
        float CombinedRadiusSq = (Radius + otherCircle.Radius) * (Radius + otherCircle.Radius);

        //Check if square distance is less than square radius, return result.
        //True means both objects have intersected
       // return false;
        return VectorMaths.SqDistanceFromPointToSegment(A, B, otherCircle.CentrePoint3D) <= CombinedRadiusSq;
    }
}
