using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour {

    public Ship ShipLocal; //Get the ship position
    public float CameraDistance;
    public float RotateAmount;
    // Use this for initialization
    void Start()
    {
        RotateAmount = ShipLocal.RotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = new Vector3(ShipLocal.Transform.Position.x, ShipLocal.Transform.Position.y + CameraDistance, ShipLocal.Transform.Position.z);

    }
}