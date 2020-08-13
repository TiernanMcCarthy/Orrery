using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Custom Transform class making use of Matrix transformations and Quaternions
public class Trans : MonoBehaviour {

    Vector3[] ModelSpaceVertices; //Model Vertices that can be scaled
    MeshFilter MF;
    public bool DoNotTransform = false;
    //Vector3 containing the position of the object
    public Vector3 Position;
    //Rotation of the object stored as Euler
    public Vector3 RotationEuler;
    ////Rotation of the object stored as Quaternion
    public Quat RotationQuat = new Quat(0, new Vector3(0, 0, 0));
    //Scale of each axis stored in Vector3
    public Vector3 Scale = new Vector3(1, 1, 1); //Default to 1,1,1 size so it renders

    public float  BallRadius;
    //Rotation Matrix
    Matrix4by4 yaw;
    Matrix4by4 pitch;
    Matrix4by4 roll;
    Matrix4by4 M;
    // Use this for initialization
    void Start () {
        MeshFilter MF = GetComponent<MeshFilter>(); 
        ModelSpaceVertices = MF.mesh.vertices; //Collect the model vertices

        yaw = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 0)); //Setup Identity Matrices
        roll = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 0));
        pitch =new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 0));

        M = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 0));

    }
	

    public Matrix4by4 ReturnFinishedMatrix() //Return the Finished matrix for the child object
    {
        return M;
    }
	// Update is called once per frame
	void Update () {
        if (DoNotTransform == false) //The Sun requires this for its functions, but actually rotates with quaternions, this is a simple check
        {
            Vector3[] TransformedVertices = new Vector3[ModelSpaceVertices.Length]; //Get Model Vertices
            roll = new Matrix4by4
               (new Vector3(Mathf.Cos(RotationEuler.x), Mathf.Sin(RotationEuler.x), 0),  //Plug in the rotation values set in the editor
               new Vector3(-Mathf.Sin(RotationEuler.x), Mathf.Cos(RotationEuler.x), 0),
               new Vector3(0, 0, 1),
               Vector3.zero);
            pitch = new Matrix4by4(
                new Vector3(1, 0, 0),
                new Vector3(0, Mathf.Cos(RotationEuler.z), Mathf.Sin(RotationEuler.z)),
                new Vector3(0, -Mathf.Sin(RotationEuler.z), Mathf.Cos(RotationEuler.z)),
                Vector3.zero);
            yaw = new Matrix4by4(
               new Vector3(Mathf.Cos(RotationEuler.y), 0, -Mathf.Sin(RotationEuler.y)),
               new Vector3(0, 1, 0),
               new Vector3(Mathf.Sin(RotationEuler.y), 0, Mathf.Cos(RotationEuler.y)),
               Vector3.zero);




            Matrix4by4 R = yaw * (pitch * roll); //Create one rotation matrix
            Matrix4by4 S = new Matrix4by4(new Vector3(1, 0, 0) * Scale.x, new Vector3(0, 1, 0) * Scale.y, new Vector3(0, 0, 1) * Scale.z, Vector3.zero); //Create a scale matrix with the editor scale

            Matrix4by4 T = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), //Create a translation Matrix with the custom translation values
                new Vector3(0, 0, 1),
                new Vector3(Position.x, Position.y, Position.z));
            M = T * R * S; //Multiply the matrixes in TRS order (No brackets are needed as it multiplys with TRS correctly like this)


            for (int i = 0; i < TransformedVertices.Length; i++) //Run through the list and multiply vertices by M
            {
                TransformedVertices[i] = M * ModelSpaceVertices[i];
            }
            MeshFilter MF = GetComponent<MeshFilter>();
            MF.mesh.vertices = TransformedVertices; //Send these vertices for rendering in Unity
            MF.mesh.RecalculateNormals();
            MF.mesh.RecalculateBounds();
        }
    }
}
