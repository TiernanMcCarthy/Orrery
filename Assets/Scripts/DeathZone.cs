using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
    public AABB Bound;
    public Vector3 Min, Max;
    public Trans Transform;
    public TestCube Cube;
    public Vector3 TestCubeSpawn;
	// Use this for initialization
	void Start () {
        Transform = GetComponentInParent<Trans>();
        Bound = new AABB(Min, Max, Transform.Position);
	}
	
	// Update is called once per frame
	void Update () {

        if (AABB.Intersects(Cube.Bound, Bound)==true)
        {
            Cube.Transform.Position = TestCubeSpawn;
        }
        Bound.position = Transform.Position;

	}
}
