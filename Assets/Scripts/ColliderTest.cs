using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour {
    List<PlanetTransform> PlanetList = new List<PlanetTransform>();
    List<Moon> MoonList = new List<Moon>();
    public Ship ShipLocal;
    public DeathZone d;
    public Vector3 ShipDeathLocation;
    public MyRigidBody Sun;
    // Use this for initialization
    void Start () {
        PlanetList.AddRange(GameObject.FindObjectsOfType<PlanetTransform>());
        MoonList.AddRange(GameObject.FindObjectsOfType<Moon>());
    }
     void ResetShip()
    {
        ShipLocal.Transform.Position = ShipDeathLocation;
        ShipLocal.Rig.velocity = new Vector3(0, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        foreach (PlanetTransform b in PlanetList)
        {
            if (b.Boundy.Intersects(ShipLocal.Rig.boundingCircle) == true)
            {
                ResetShip();
            }
        }

        foreach (Moon b in MoonList)
        {
            if (b.Boundy.Intersects(ShipLocal.Rig.boundingCircle) == true)
            {
                ResetShip();
            }
        }
        if (AABB.Intersects(ShipLocal.Rig.boundingCircle,d.Bound)==true)
        {
            ResetShip();
        }
        if(ShipLocal.Rig.boundingCircle.Intersects(Sun.boundingCircle)==true)
        {
            ResetShip();
        }
    }
}
