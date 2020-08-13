using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour {
    public Ship Parent;
    Vector3[] ModelSpaceVertices;
    MeshFilter MF;
    public float Distance;
    Matrix4by4 M2;
    public float scale;
    // Use this for initialization
    void Start () {
        MeshFilter MF = GetComponent<MeshFilter>();
        ModelSpaceVertices = MF.mesh.vertices;
        Matrix4by4 M2 = new Matrix4by4(new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 1), new Vector3(0, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
        Vector3[] TransformedVertices = new Vector3[ModelSpaceVertices.Length];
        Vector3 ParentForward = EulerMaths.ForwardDirection(Parent.Transform.RotationEuler)*Distance;
        
         Matrix4by4 T = new Matrix4by4(new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 1),
            new Vector3(ParentForward.x, ParentForward.y, ParentForward.z));
        Matrix4by4 S = new Matrix4by4(new Vector3(1, 0, 0) * scale,
            new Vector3(0, 1, 0) * scale,
            new Vector3(0, 0, 1) * scale, new Vector3(0, 0, 0));
         M2 = Parent.Transform.ReturnFinishedMatrix()*T*S;
        for (int i = 0; i < TransformedVertices.Length; i++)
        {
            TransformedVertices[i]= M2*ModelSpaceVertices[i];
        }

        MeshFilter MF = GetComponent<MeshFilter>();
        MF.mesh.vertices = TransformedVertices;
        MF.mesh.RecalculateNormals();
        MF.mesh.RecalculateBounds();
    }


}
