using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float strength = 10f;
    private float xInput;
    private Transform ball;
    private Rigidbody rb;
    private State currentState;

    void Start()
    {
        ball = transform.GetChild(0);
        rb = ball.GetComponent<Rigidbody>();
    }

    void Update()
    {
        currentState.OnUpdate();
        Movement();
        ShootBall();
    }

    private void Movement()
    {
        xInput = Input.GetAxis("Horizontal");
        Vector3 newPos = transform.position;

        newPos += xInput * Time.deltaTime * speed * Vector3.right;
        if (newPos.x < -4f)
        {
            newPos = new Vector3(-4f, transform.position.y, transform.position.z);
        }
        else if (newPos.x > 4f)
        {
            newPos = new Vector3(4f, transform.position.y, transform.position.z);
        }
        transform.position = newPos;
    }

    private void ShootBall()
    {
        if (Input.GetButtonDown("Jump"))
        {
            float randomXOffset = Random.Range(-0.1f, 0.1f);
            Vector3 pos = transform.position;
            Vector3 randomDir = new Vector3(pos.x + randomXOffset, pos.y + 1, pos.z);
            ball.parent = null;
            rb.isKinematic = false;
            rb.AddForce(randomDir.normalized * strength, ForceMode.Impulse);
        }
    }
}
