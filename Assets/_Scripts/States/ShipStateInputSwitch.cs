using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStateInputSwitch : ShipStateDefault
{
    public override void OnStateEnter()
    {
        inputMultiplier = -1f;
    }
}
