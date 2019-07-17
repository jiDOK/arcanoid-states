using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStateMultiball : ShipStateDefault
{
    public override void OnStateEnter()
    {
        for (int i = 0; i < 3; i++)
        {
            Ball ball = GameObject.Instantiate<Ball>(
                Ship.BallPrefab, 
                Ship.Ball.transform.position, 
                Quaternion.identity, 
                Ship.BallParent);
            ball.Player = Ship;
            ball.PlayerTransform = Ship.transform;
            ball.Speed = 10f;
            ball.Shoot(0f, 360f);
        }
    }
}
