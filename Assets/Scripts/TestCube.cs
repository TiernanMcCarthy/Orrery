using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour {
    public Trans Transform;
    public AABB Bound;
    public Vector3 Min, Max;
    public float speed;
	// Use this for initialization
	void Start () {
        Transform = GetComponentInParent<Trans>();
        Bound = new AABB(Min, Max, Transform.Position);
	}
	
	// Update is called once per frame
	void Update () {
        Transform.Position += new Vector3(speed * Time.deltaTime, 0, 0);
        Bound.position = Transform.Position;
	}
}
