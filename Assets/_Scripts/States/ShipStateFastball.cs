using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStateFastball : ShipStateDefault
{
    public override void OnStateEnter()
    {
        Ship.Ball.Speed *= 2f;
    }
    public override void OnStateExit()
    {
        Ship.Ball.Speed = Ship.Ball.DefaultSpeed;
    }
}
