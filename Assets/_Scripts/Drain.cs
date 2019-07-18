using System;
using UnityEngine;

public class Drain : MonoBehaviour
{
    public event Action<Ball> BallInDrain;

    private void OnTriggerEnter(Collider other)
    {
        // trigger event
        var ball = other.GetComponent<Ball>();
        BallInDrain?.Invoke(ball);
    }
}
