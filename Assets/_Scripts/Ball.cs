using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float strength = 10f;
    private Rigidbody rb;
    private bool didCollide;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        didCollide = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * strength;
        didCollide = false;
    }
}
