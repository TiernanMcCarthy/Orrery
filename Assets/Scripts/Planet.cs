using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    Trans Transforms;
    float yawangle = 0;
    Vector3[] ModelSpaceVertices;
    MeshFilter MF;

    public float OrbitRotationX;
    public float OrbitRotationY;
    public float OrbitRotationZ;


    public float scale;

    public float speed;
    public Vector3 orbitdistance;

    Vector3 actualposition;
    public Vector3 orbitOffset;
    // Use this for initialization
    void Start () {
        Transforms = GetComponentInParent<Trans>();
        MeshFilter MF = GetComponent<MeshFilter>();
        ModelSpaceVertices = MF.mesh.vertices;
    }
	
	// Update is called once per frame
	void Update () {

        OrbitRotationY += Time.deltaTime*speed;
        Vector3[] TransformedVertices = new Vector3[ModelSpaceVertices.Length];
        yawangle += Time.deltaTime;
        Matrix4by4 rollMatrix = new Matrix4by4
            (new Vector3(Mathf.Cos(yawangle), Mathf.Sin(yawangle), 0),
            new Vector3(-Mathf.Sin(yawangle), Mathf.Cos(yawangle), 0),
            new Vector3(0, 0, 1),
            Vector3.zero);
        Matrix4by4 pitchmatrix = new Matrix4by4(
            new Vector3(1, 0, 0),
            new Vector3(0, Mathf.Cos(yawangle), Mathf.Sin(yawangle)),
            new Vector3(0, -Mathf.Sin(yawangle), Mathf.Cos(yawangle)),
            Vector3.zero);
        Matrix4by4 yawmatrix = new Matrix4by4(
            new Vector3(Mathf.Cos(yawangle), 0, -Mathf.Sin(yawangle)),
            new Vector3(0, 1, 0),
            new Vector3(Mathf.Sin(yawangle), 0, Mathf.Cos(yawangle)),
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


        Vector3 offset = RotationOfOrbit * orbitdistance;

        actualposition = Transforms.Position+offset;

        Matrix4by4 T = new Matrix4by4(new Vector3(1, 0, 0),
            new Vector3(0, 1, 0)
            , new Vector3(0, 0, 1),
            new Vector3(Transforms.Position.x+offset.x, Transforms.Position.y+  offset.y, Transforms.Position.z+  offset.z));

        Matrix4by4 S = new Matrix4by4(new Vector3(1, 0, 0)*scale , new Vector3(0, 1, 0) * scale, new Vector3(0, 0, 1) * scale, Vector3.zero);
        Matrix4by4 M = T * (R *S);

        //Transforms.Position = actualposition;
        for (int i = 0; i < TransformedVertices.Length; i++)
        {
            TransformedVertices[i] = M * ModelSpaceVertices[i];
        }


        MeshFilter MF = GetComponent<MeshFilter>();
        MF.mesh.vertices = TransformedVertices;
        MF.mesh.RecalculateNormals();
        MF.mesh.RecalculateBounds();
    }
}
