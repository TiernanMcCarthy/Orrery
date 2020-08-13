using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
    public Ship shippy;
    //public Component gravityPosition;
    //Enable and Disable depending on what is needed
    public Trans gravityPosition;
    public PlanetTransform gravityPos;
    public Moon gravityPosM;

    public float GravityForce = 0;
    public float Distance;
    public float RealGravityForce;
    char TransType;
    Vector3 ShipPos;
	// Use this for initialization
	void Start () {
        
        shippy = FindObjectOfType<Ship>(); //Find the ship
        if(gameObject.GetComponent<PlanetTransform>()!=null)
        {
            TransType = 'P'; //PLanet Transform

        }
        else if(gameObject.GetComponent<Moon>() != null)
        {
            TransType = 'M';
        }

	}
	
	// Update is called once per frame
	void Update () {
        switch(TransType)
        {
            case 'P':
                gravityPosition.Position = gravityPos.actualposition;
                break;
            case 'M':
                gravityPosition.Position = gravityPosM.actualposition;
                break;
        }
       
        ShipPos = shippy.Transform.Position;

        Vector3 Direction = ShipPos - gravityPosition.Position;

        float Magnitude = VectorMaths.VectorMagnitude(ShipPos);
         if(Magnitude!=0) //Prevent dividing by 0  
            RealGravityForce = GravityForce*(Distance/ Magnitude);
        shippy.Rig.force += Direction * RealGravityForce * Time.deltaTime; //Realistically this should only occur if the ship was within this range and if outside it would go the opposite direction
         //This doesn't matter in the content of this however.

	}
}
