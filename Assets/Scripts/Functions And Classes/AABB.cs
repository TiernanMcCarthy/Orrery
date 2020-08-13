using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB {
    //Initialise with two Vector3s that define the shape of the bouding box
    Vector3 MinExtent;
    Vector3 MaxExtent;
    public Vector3 position;
    public AABB(Vector3 Min,Vector3 Max,Vector3 Position) 
    {
        MinExtent = Min;
        MaxExtent = Max;
        position = Position;
    }
    public float Top
    {
        get { return MaxExtent.y+position.y; }
    }
    public float Bottom
    {
        get { return MinExtent.y + position.y; }
    }
    public float Left
    {
        get { return MinExtent.x + position.x; }
    }
    public float Right
    {
        get { return MaxExtent.x + position.x; }
    }
    public float Front
    {
        get { return MaxExtent.z + position.z; }
    }
    public float Back
    {
        get { return MinExtent.z+position.z; }
    }


    public static bool Intersects(AABB Box1, AABB Box2)
    {
        //If outside any of these bounds return false
        return !(Box2.Left > Box1.Right
            || Box2.Right  < Box1.Left 
            || Box2.Top  < Box1.Bottom 
            || Box2.Bottom > Box1.Top 
            || Box2.Back > Box1.Front 
            || Box2.Front   <Box1.Back );

    }
    public static bool IntersectingAxis(Vector3 Axis, AABB Box,Vector3 StartPoint, Vector3 EndPoint, ref float Lowest, ref float Highest)
    {
        //Calculate Minimum and Maximum based on the current axis
        float minimum = 0.0f, maximum = 1.0f;
        if(Axis == Vector3.right)
        {
            minimum = (Box.Left - StartPoint.x) / (EndPoint.x - StartPoint.x);
            maximum = (Box.Right - StartPoint.x) / (EndPoint.x - StartPoint.x);
        }
        else if (Axis == Vector3.up)
        {
            minimum = (Box.Bottom - StartPoint.y) / (EndPoint.y - StartPoint.y);
            maximum = (Box.Top - StartPoint.y) / (EndPoint.y - StartPoint.y);
        }
        else if (Axis==Vector3.forward)
        {
            minimum = (Box.Back - StartPoint.z) / (EndPoint.z - StartPoint.z);
            maximum = (Box.Front - StartPoint.z) / (EndPoint.z - StartPoint.z);
        }

        if(maximum<minimum)
        {
            //Swap values
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
        }

        //Eliminate non intersections to optimise

        if (maximum < Lowest)
            return false;
        if (minimum > Highest)
            return false;

        Lowest = Mathf.Max(minimum, Lowest);
        Highest = Mathf.Min(maximum, Highest);

        if (Lowest > Highest)
            return false;

        return true;
    }
    public static bool LineIntersection(AABB Box,Vector3 StartPoint, Vector3 EndPoint, out Vector3 IntersectionPoint)
    {
        //Define initial lowest and highest values
        float Lowest = 0.0f;
        float Highest = 1.0f;

        //Default Value for intersection point
        IntersectionPoint = Vector3.zero;

        //Intersection Checks on every axis 
        if (!IntersectingAxis(Vector3.right, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
            return false;
        if (!IntersectingAxis(Vector3.up, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
            return false;
        if (!IntersectingAxis(Vector3.forward, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
            return false;

        //Calculate Intersection point using interpolation
        //Not the only method
        IntersectionPoint = VectorMaths.VectorLerp(StartPoint, EndPoint, Lowest);
        return true;
    }
    //https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection
    public static bool Intersects(BoundingSphere sphere, AABB box)
    {
        // get box closest point to sphere center by clamping
        var x = Mathf.Max(box.MinExtent.x+box.position.x, Mathf.Min(sphere.CentrePoint3D.x, box.MaxExtent.x + box.position.x));
        var y = Mathf.Max(box.MinExtent.y + box.position.y, Mathf.Min(sphere.CentrePoint3D.y, box.MaxExtent.y + box.position.y));
        var z = Mathf.Max(box.MinExtent.z + box.position.z, Mathf.Min(sphere.CentrePoint3D.z, box.MaxExtent.z + box.position.z));

        // this is the same as isPointInsideSphere
        var distance = Mathf.Sqrt((x - sphere.CentrePoint3D.x) * (x - sphere.CentrePoint3D.x) +
                                 (y - sphere.CentrePoint3D.y) * (y - sphere.CentrePoint3D.y) +
                                 (z - sphere.CentrePoint3D.z) * (z - sphere.CentrePoint3D.z));

        return distance < sphere.Radius;
    }







    //https://stackoverflow.com/questions/5883169/intersection-between-a-line-and-a-sphere
    public static Vector3 LineIntersection(Vector3 StartPoint,Vector3 EndPoint, Vector3 CircleCentre,float CircleRadius,out Vector3 IntersectionPoint)
    {
        float cx = CircleCentre.x;
        float cy = CircleCentre.y;
        float cz = CircleCentre.z;

        float px = StartPoint.x;
        float py = StartPoint.y;
        float pz = StartPoint.z;

        float vx = EndPoint.x - px;
        float vy = EndPoint.y - py;
        float vz = EndPoint.z - pz;

        float A = vx * vx + vy * vy + vz * vz;
        float B = 2.0f * (px * vx + py * vy + pz * vz - vx * cx - vy * cy - vz * cz);
        float C = px * px - 2 * px * cx + cx * cx + py * py - 2 * py * cy + cy * cy +
                   pz * pz - 2 * pz * cz + cz * cz - CircleRadius * CircleRadius;

        float D = B * B - 4 * A * C;

        if (D < 0)
        {
            IntersectionPoint = new Vector3(0, 0, 0);
        }

        float t1 = (-B - Mathf.Sqrt(D)) / (2.0f * A);

        Vector3 solution1 = new Vector3(StartPoint.x * (1.0f - t1) + t1 * EndPoint.x,
                                         StartPoint.y * (1.0f - t1) + t1 * EndPoint.y,
                                         StartPoint.z * (1.0f - t1) + t1 * EndPoint.z);
        if (D == 0)
        {
            IntersectionPoint = solution1;
        }

        float t2 = (-B + Mathf.Sqrt(D)) / (2.0f * A);
        Vector3 solution2 = new Vector3(StartPoint.x * (1 - t2) + t2 * EndPoint.x,
                                         StartPoint.y * (1 - t2) + t2 * EndPoint.y,
                                         StartPoint.z * (1 - t2) + t2 * EndPoint.z);

        // prefer a solution that's on the line segment itself

        if (Mathf.Abs(t1 - 0.5f) < Mathf.Abs(t2 - 0.5f))
        {
            IntersectionPoint = solution2;
        }
        IntersectionPoint = solution1;
        return solution2;



    }



}
