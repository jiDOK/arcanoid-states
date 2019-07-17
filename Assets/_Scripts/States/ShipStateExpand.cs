using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStateExpand : ShipStateDefault
{
    public override void OnStateEnter()
    {
        Ship.ShipMesh.transform.localScale += new Vector3(1f, 0f, 0f);
    }
    public override void OnStateExit()
    {
        Ship.ShipMesh.transform.localScale = Ship.DefaultShipScale;
    }
}
