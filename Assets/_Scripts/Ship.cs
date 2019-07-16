using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    private float xInput;
    private Ball ball;
    private Rigidbody rb;
    private State currentState;

    public Vector3 Velocity { get; private set; }

    void Start()
    {
        ball = GetComponentInChildren<Ball>();
        rb = ball.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //currentState.OnUpdate();
        Movement();
        ShootBall();
    }

    private void Movement()
    {
        xInput = Input.GetAxis("Horizontal");
        Vector3 newPos = transform.position;

        Velocity = xInput * Time.deltaTime * speed * Vector3.right;
        newPos += Velocity;
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
            ball.Shoot();
        }
    }
}
