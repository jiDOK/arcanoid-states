using System;
using UnityEngine;

public class Drain : MonoBehaviour
{
    public event Action<Ball> BallInDrain;

    private void OnTriggerEnter(Collider other)
    {
        var ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            BallInDrain?.Invoke(ball);
        }
    }
}
