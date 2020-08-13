using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTransform : MonoBehaviour
{

    float yawangle = 0;
    Vector3[] ModelSpaceVertices;
    MeshFilter MF;

    public float OrbitRotationX; //Orbit Rotation values, these are an Euler representation of the angle they rotate on
    public float OrbitRotationY;
    public float OrbitRotationZ;



    public BoundingSphere Boundy; //A bounding sphere made use of by the planet for collision with the player ship
     
    public float RotationOnOwnAxisSpeed; //Rotation speed of the planet on its own axis

    public Vector3 AxisRotation; //An euler representation of a planet's rotation

    public Vector3 WorldPos; //The position that the planet orbits around. This stays as 0,0 but can be moved

    public float scale; //The scale of the planet object 

    public float speed; //How fast the planet orbits
    public Vector3 orbitdistance; //The distance offset the planet has from its world position

    public Vector3 actualposition; //The position of the planet post rotation matrix and scaling is applied stored as a vector 3
    Vector3 realoffset; //The actual offset used to move the planet depeding on rotation
    // Use this for initialization
    void Start()
    {
        //Collect the Mesh 
        MeshFilter MF = GetComponent<MeshFilter>(); 
        ModelSpaceVertices = MF.mesh.vertices;
        Boundy = new BoundingSphere(actualposition, scale/2); //Get their true radius which is half of their diameter which starts as 1 in the editor
                                                               //Before they are scaled by me
    }
    

    public Vector3 ReturnOffset()
    {
        return realoffset; //For use with child objects like moons
    }
    // Update is called once per frame
    void Update()
    {
        if(OrbitRotationX>0) //Dictating which direction the planet can orbit
        {
            OrbitRotationX += Time.deltaTime * speed;
        }
        else if (OrbitRotationY > 0) //Only one orbit direction is allowed, but technically this could work in any direction
        {
            OrbitRotationY += Time.deltaTime * speed;
        }
       else if (OrbitRotationZ > 0)
        {
            OrbitRotationZ += Time.deltaTime * speed;
        }

        if(AxisRotation.x!=0) //If the player has set a rotation value then the planet will rotate on this axis
        {
            AxisRotation.x += RotationOnOwnAxisSpeed;
        }
        if (AxisRotation.y != 0)
        {
            AxisRotation.y += RotationOnOwnAxisSpeed;
        }
        if (AxisRotation.z != 0)
        {
            AxisRotation.z += RotationOnOwnAxisSpeed;
        }


        Vector3[] TransformedVertices = new Vector3[ModelSpaceVertices.Length]; //Create a list containing all the model space vertices
        yawangle += Time.deltaTime;
        Matrix4by4 rollMatrix = new Matrix4by4
            (new Vector3(Mathf.Cos(AxisRotation.z), Mathf.Sin(AxisRotation.z), 0), //Create the rotation Matrices for Roll, yaw and pitch to combine into one rotation matrix
            new Vector3(-Mathf.Sin(AxisRotation.z), Mathf.Cos(AxisRotation.z), 0),
            new Vector3(0, 0, 1),
            Vector3.zero);
        Matrix4by4 pitchmatrix = new Matrix4by4(
            new Vector3(1, 0, 0),
            new Vector3(0, Mathf.Cos(AxisRotation.x), Mathf.Sin(AxisRotation.x)),
            new Vector3(0, -Mathf.Sin(AxisRotation.x), Mathf.Cos(AxisRotation.x)), //Rotation values designated by the editor are stored here
            Vector3.zero);
        Matrix4by4 yawmatrix = new Matrix4by4(
            new Vector3(Mathf.Cos(AxisRotation.y), 0, -Mathf.Sin(AxisRotation.y)),
            new Vector3(0, 1, 0),
            new Vector3(Mathf.Sin(AxisRotation.y), 0, Mathf.Cos(AxisRotation.y)),
            Vector3.zero);


        Matrix4by4 OrbitRoll = new Matrix4by4
            (new Vector3(Mathf.Cos(OrbitRotationZ), Mathf.Sin(OrbitRotationZ), 0), //The orbit rotation matrices are created and combined, just like the above example
            new Vector3(-Mathf.Sin(OrbitRotationZ), Mathf.Cos(OrbitRotationZ), 0),
            new Vector3(0, 0, 1),
            Vector3.zero);
        Matrix4by4 OrbitPitch = new Matrix4by4(
            new Vector3(1, 0, 0),
            new Vector3(0, Mathf.Cos(OrbitRotationX), Mathf.Sin(OrbitRotationX)),
            new Vector3(0, -Mathf.Sin(OrbitRotationX), Mathf.Cos(OrbitRotationX)),
            Vector3.zero);
        Matrix4by4 OrbitYaw = new Matrix4by4(
            new Vector3(Mathf.Cos(OrbitRotationY), 0, -Mathf.Sin(OrbitRotationY)),
            new Vector3(0, 1, 0),
            new Vector3(Mathf.Sin(OrbitRotationY), 0, Mathf.Cos(OrbitRotationY)),
            Vector3.zero);



        Matrix4by4 R = yawmatrix * (pitchmatrix * rollMatrix); //A rotation matrix is constructed


        Matrix4by4 RotationOfOrbit = OrbitYaw * (OrbitPitch * OrbitRoll); //The orbit matrix is constructed



        realoffset = RotationOfOrbit * orbitdistance; //The orbit rotation is offset by the desired distance and stored

        actualposition = WorldPos + realoffset*2; //The position is set to the location the planet orbits around, and the desired offset *2

        Matrix4by4 T = new Matrix4by4(new Vector3(1, 0, 0), //A translation matrix is constructed out of this new postion
            new Vector3(0, 1, 0)
            , new Vector3(0, 0, 1),
            new Vector3(actualposition.x, actualposition.y, actualposition.z));

        Matrix4by4 S = new Matrix4by4(new Vector3(1, 0, 0) * scale, new Vector3(0, 1, 0) * scale, new Vector3(0, 0, 1) * scale, Vector3.zero); //The scale matrix is made from the desired scale size
        Matrix4by4 M = T * R * S; //The matrices are combined in TRS order (It completes in this order as it translates correctly, brackets are not needed)

        for (int i = 0; i < TransformedVertices.Length; i++)
        {
            TransformedVertices[i] = M * ModelSpaceVertices[i]; //The vertices list is ran through and multiplied by the M matrix to offset correctly
        }

        Boundy.UpdateCentrePoint(actualposition); //The bounding sphere is offset to be the real position of the planet, allowing for collision


        MeshFilter MF = GetComponent<MeshFilter>();
        MF.mesh.vertices = TransformedVertices; //These vertices are applied and rendered by Unity
        MF.mesh.RecalculateNormals();
        MF.mesh.RecalculateBounds();
    }
}

