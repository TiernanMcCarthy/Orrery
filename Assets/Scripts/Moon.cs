using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {
    float yawangle = 0;
    Vector3[] ModelSpaceVertices;
    MeshFilter MF;

    public float OrbitRotationX; //Exact Same Principle as the Parent planet
    public float OrbitRotationY;
    public float OrbitRotationZ;

    public BoundingSphere Boundy;

    public float RotationOnOwnAxisSpeed;

    public Vector3 AxisRotation;

    public Vector3 WorldPos;


    public PlanetTransform parent; //Mother planet position

    public float scale;
    public float speed;
    public Vector3 orbitdistance;

    public Vector3 actualposition;
    public Vector3 orbitOffset;
    // Use this for initialization
    void Start()
    {
        MeshFilter MF = GetComponent<MeshFilter>();
        ModelSpaceVertices = MF.mesh.vertices;
        Boundy = new BoundingSphere(actualposition, scale / 2);
    }

    // Update is called once per frame
    void Update()
    {
        WorldPos = parent.actualposition; //Collect parent final position
        if (OrbitRotationX > 0)
        {
            OrbitRotationX += Time.deltaTime * speed;
        }
        else if (OrbitRotationY > 0)
        {
            OrbitRotationY += Time.deltaTime * speed;
        }
        else if (OrbitRotationZ > 0)
        {
            OrbitRotationZ += Time.deltaTime * speed;
        }
        Vector3[] TransformedVertices = new Vector3[ModelSpaceVertices.Length];
        yawangle += Time.deltaTime;
        Matrix4by4 rollMatrix = new Matrix4by4
            (new Vector3(Mathf.Cos(AxisRotation.z), Mathf.Sin(AxisRotation.z), 0),
            new Vector3(-Mathf.Sin(AxisRotation.z), Mathf.Cos(AxisRotation.z), 0),
            new Vector3(0, 0, 1),
            Vector3.zero);
        Matrix4by4 pitchmatrix = new Matrix4by4(
            new Vector3(1, 0, 0),
            new Vector3(0, Mathf.Cos(AxisRotation.x), Mathf.Sin(AxisRotation.x)),
            new Vector3(0, -Mathf.Sin(AxisRotation.x), Mathf.Cos(AxisRotation.x)),
            Vector3.zero);
        Matrix4by4 yawmatrix = new Matrix4by4(
            new Vector3(Mathf.Cos(AxisRotation.y), 0, -Mathf.Sin(AxisRotation.y)),
            new Vector3(0, 1, 0),
            new Vector3(Mathf.Sin(AxisRotation.y), 0, Mathf.Cos(AxisRotation.y)),
            Vector3.zero);


        Matrix4by4 OrbitRoll = new Matrix4by4
            (new Vector3(Mathf.Cos(OrbitRotationZ), Mathf.Sin(OrbitRotationZ), 0),
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


        Matrix4by4 RotationOfOrbit = OrbitYaw * (OrbitPitch * OrbitRoll);


        Matrix4by4 R = yawmatrix * (pitchmatrix * rollMatrix);


        Vector3 offset = RotationOfOrbit * orbitdistance; //Add your offset to the parent planet

        actualposition = WorldPos + offset;

        Matrix4by4 T = new Matrix4by4(new Vector3(1, 0, 0),
            new Vector3(0, 1, 0)
            , new Vector3(0, 0, 1),
            new Vector3(actualposition.x + offset.x, actualposition.y + offset.y, actualposition.z + offset.z)); //Add this offset to the planet and rotate around them

        Matrix4by4 S = new Matrix4by4(new Vector3(1, 0, 0) * scale, new Vector3(0, 1, 0) * scale, new Vector3(0, 0, 1) * scale, Vector3.zero); //Scale to the moons size
        Matrix4by4 M = T * R * S; 

        //Transforms.Position = actualposition;
        for (int i = 0; i < TransformedVertices.Length; i++) //Do the same rendering as every other transform
        {
            TransformedVertices[i] = M * ModelSpaceVertices[i];
        }

        Boundy.UpdateCentrePoint(actualposition + offset);


        MeshFilter MF = GetComponent<MeshFilter>();
        MF.mesh.vertices = TransformedVertices;
        MF.mesh.RecalculateNormals();
        MF.mesh.RecalculateBounds();
    }
}