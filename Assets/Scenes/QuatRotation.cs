using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuatRotation : MonoBehaviour
{
    public float RotationSpeed;
    Vector3[] ModelSpaceVertices;
    MeshFilter MF;
    float angle;
    float t;
    Quat Rotation = new Quat(EulerMaths.DegtoRag(10), VectorMaths.VectorNormalised(new Vector3(0, 1, 0)));
    // Use this for initialization
    void Start()
    {
        MeshFilter MF = GetComponent<MeshFilter>();
        ModelSpaceVertices = MF.mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] TransformedVertices = new Vector3[ModelSpaceVertices.Length];
        angle += Time.deltaTime * 1;
        Rotation *= new Quat(EulerMaths.DegtoRag(RotationSpeed), VectorMaths.VectorNormalised(new Vector3(0, 1, 0)));
        for (int i = 0; i < TransformedVertices.Length; i++)
        {
            Quat p = new Quat(ModelSpaceVertices[i]);
            Quat newp = ((Rotation * p) * Rotation.Inverse());
            Vector3 newpos = newp.GetAxisAngle();
            TransformedVertices[i] = newpos;
        }
        MeshFilter MF = GetComponent<MeshFilter>();
        MF.mesh.vertices = TransformedVertices;
        MF.mesh.RecalculateNormals();
        MF.mesh.RecalculateBounds();
    }

}
