using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMaths  {

    // Use this for initialization
    const float pi = 3.14159265359f;
	public static Vector3 VectorAddition(Vector3 v1,Vector3 v2)
    {
        Vector3 v3 = new Vector3(v1.x + v2.x, v1.y + v2.y,v1.z+v2.z);
        return (v3);
    }
    public static Vector2 VectorAddition(Vector2 v1,Vector2 v2)
    {
        Vector2 v3 = new Vector2(v1.x + v2.x, v1.y + v2.y);
        return (v3);
    }
    // static Vector2 VectorAddition operator+()
    public static Vector3 VectorSubtraction(Vector3 v1, Vector3 v2)
    {
        Vector3 v3 = v1 - v2;
        return (v3);
    }
    public static Vector2 VectorSubtraction(Vector2 v1, Vector2 v2)
    {
        Vector2 v3 = new Vector2(v1.x - v2.x, v1.y - v2.y);
        return (v3);
    }
    public static float VectorMagnitude(Vector3 v1)
    {
        float temp = Mathf.Sqrt((v1.x * v1.x) + (v1.y * v1.y) + (v1.z * v1.z));
        return temp;
    }
    public static float VectorMagnitude(Vector2 v1)
    {
        float temp = Mathf.Sqrt(v1.x * v1.x) + Mathf.Sqrt(v1.y * v1.y);
        return temp;
        
    }
    public static float LengthSq(Vector3 v1)
    {
        float temp = VectorMagnitude(v1);
        return (temp * temp);
    }
    public static Vector3 MultiplyVector(Vector3 v1, float scalar)
    {
        Vector3 temp = v1 * scalar;
        return (temp);
    }
    public static Vector3 DivideVector(Vector3 v1,float divisor)
    {
        Vector3 temp = v1 / divisor;
        return (temp);
    }
    public static Vector3 VectorNormalised(Vector3 v1)
    {
        Vector3 temp = DivideVector(v1, VectorMagnitude(v1));
        return (temp);
    }
    public static float DotProduct(Vector3 v1, Vector3 v2)
    {
        float temp = (v1.x * v2.x + v1.y * v2.y + v1.z*v2.z);
        return (temp);
    }
    public static float DotProduct(Vector3 v1, Vector3 v2, bool Normalize=true)
    {
        if(Normalize==true)
        {
            v1.Normalize();
            v2.Normalize();
        }
        float temp = (v1.x * v2.x + v1.y * v2.y + v1.z * v2.z);
        return (temp);
    }
    //EULER
    public static float VecAngle(Vector2 v1) //Radians
    {
        float rot = Mathf.Atan(v1[1] / v1[0]); //Opposite over adjacent
        return rot;
    }
    public static Vector2 AngleToVec(float angle)
    {
        Vector2 rv = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return rv;
    }
    public static Vector3 VectorCrossProduct(Vector3 A,Vector3 B)
    {
        Vector3 C = new Vector3();
        C.x = A.y * B.z - A.z * B.y;
        C.y = A.z * B.x - A.x * B.z;
        C.z = A.x * B.y - A.y * B.x;
        return C;
    }
    //Return the desired position specified in float T between vectors A and B
    public static Vector3 VectorLerp(Vector3 A,Vector3 B, float t)
    {
        return A * (1.0f - t) + B * t;
    }
    public static Vector3 RotateVertexAroundAxis(float Angle,Vector3 Axis, Vector3 Vertex)
    {
        Vector3 rv = (Vertex * Mathf.Cos(Angle)) +
            DotProduct(Vertex, Axis) * Axis * (1 - Mathf.Cos(Angle)) +
            VectorCrossProduct(Axis, Vertex) * Mathf.Sin(Angle);
        return rv;
    }
    //A Segement start, B Segement end, C Other Point
    public static float SqDistanceFromPointToSegment(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 AC = (C - A);
        Vector3 AB = (B - A);
        float squaredDistance = VectorMaths.LengthSq(AC) - (VectorMaths.DotProduct(AC, AB, false) * (VectorMaths.DotProduct(AC, AB, false)) / VectorMaths.LengthSq(AB));

        return squaredDistance;
    }

}
