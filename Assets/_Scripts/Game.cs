using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Ship ship;
    private Ball ball;

    void Awake()
    {
        ship = GameObject.FindObjectOfType<Ship>();
        ball = GameObject.FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
