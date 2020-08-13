using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EulerMaths {
    const float pi = 3.14159265359f;
    public static float DegtoRag(float v)
    {
        float y = v * pi / 180;
        return y;
    }
    public static Vector3 EulerToDirection(float x, float y, float z)
    {
        Vector3 temp;
        temp.x = Mathf.Cos(x) * Mathf.Sin(y);
        temp.y = Mathf.Sin(x);
        temp.z = Mathf.Cos(y) * Mathf.Cos(x);
        return temp;
    }
    public static Vector3 EulerToDirection(Vector3 v)
    {
        Vector3 temp;
        temp.x = Mathf.Cos(v[0]) * Mathf.Sin(v[1]);
        temp.y = Mathf.Sin(v[0]);
        temp.z = Mathf.Cos(v[1]) * Mathf.Cos(v[0]);
        return temp;
    }
    public static Vector3 ForwardDirection(Vector3 euler)
    {
        Vector3 Eulerrotation;
        Eulerrotation.x = DegtoRag(euler.x);
        Eulerrotation.y = DegtoRag(euler.y);
        Eulerrotation.z = DegtoRag(euler.z);
        return EulerToDirection(Eulerrotation.x, Eulerrotation.y, Eulerrotation.z);
    }
}
