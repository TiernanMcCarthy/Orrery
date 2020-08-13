using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quat {
    public float w, x, y, z;
    public Quat(float angle, Vector3 axis)
    {
        float halfAngle = angle / 2;
        w = Mathf.Cos(halfAngle);
        x = axis.x * Mathf.Sin(halfAngle);
        y = axis.y * Mathf.Sin(halfAngle);
        z = axis.z * Mathf.Sin(halfAngle);
    }
    public Quat(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = 0; 
    }
    public Quat()
    {

    }
    public Vector3 GetVector()
    {
        return (new Vector3(x, y, z));
    }
    public void SetVector(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public void SetVector(Vector4 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = v.w;
    }
    public Quat Inverse()
    {
        Quat rv = new Quat();
        rv.w = w;
        rv.SetVector(-GetVector());
        return rv;
    }


    public void ToEuler(ref float roll,ref float yaw, ref float pitch) //https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
    {
        //roll rotation
        float sinroll = +2.0f * (w * x + y * z);
        float cosroll = +1.0f - 2.0f * (x * x + y * y);
        roll = Mathf.Atan2(sinroll, cosroll);

        //pitch rotation
        float sinp = +2.0f * (w * y - z * x);
        pitch = Mathf.Asin(sinp); //Out of range section?

        //Yaw rotation
        float siny = +2.0f * (w * z * x * y);
        float cosy = +1.0f - 2.0f * (y * y + z * z);
        yaw = Mathf.Atan2(siny, cosy);
    }





    public Vector3 GetEuler()
    {
        float X, Y, Z;
        Vector4 axis = GetVector();
        float SquaredW = axis.w * axis.w;
        float SquaredZ = axis.z * axis.z;
        float SquaredY = axis.y * axis.y;
        Y = (float)Mathf.Atan2(2f * axis.x * axis.w + 2f * axis.y * axis.z, 1 - 2f * (SquaredZ + SquaredW));     // Yaw 
        X = (float)Mathf.Asin(2f * (axis.x * axis.z - axis.w * axis.y));                             // Pitch 
        Z = (float)Mathf.Atan2(2f * axis.x * axis.y + 2f * axis.z *axis.w, 1 - 2f * (SquaredY + SquaredZ));      // Roll
        return (new Vector3(X, Y, Z));
    }

    public static Quat operator*(Quat R,Quat S)
    {
        Quat RS = new Quat(0, Vector3.zero);
        RS.w = S.w * R.w - VectorMaths.DotProduct(S.GetVector(), R.GetVector());
        RS.SetVector(S.w * R.GetVector() + R.w * S.GetVector() + VectorMaths.VectorCrossProduct(R.GetVector(), S.GetVector()));
        return RS;
    }

    public Vector4 ReturnVector()
    {
        return new Vector4(x, y, z, w);
    }
    public Vector4 GetAxisAngle()
    {
        Vector4 rv = new Vector4();

        //Inverse cosine to get our half angle back
        float halfAngle = Mathf.Acos(w);
        rv.w = halfAngle * 2; //Create the full angle

        //Define the normal axis
        rv.x = x / Mathf.Sin(halfAngle);
        rv.y = y / Mathf.Sin(halfAngle);
        rv.z = z / Mathf.Sin(halfAngle);

        return rv;
    }
    public static Quat SLERP(Quat q, Quat r, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        Quat d = r * q.Inverse();
        Vector4 AxisAngle = d.GetAxisAngle();
        Quat dT = new Quat(AxisAngle.w * t, new Vector3(AxisAngle.x, AxisAngle.y, AxisAngle.z));

        return dT * q;
    }
}
