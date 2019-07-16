using UnityEngine;

public class ShipStateFastship : ShipStateDefault
{
    public override void OnStateEnter()
    {
        speed = 20f;
    }
}
