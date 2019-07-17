using UnityEngine;

public class Ball : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float playerBias = 0.25f;
    [SerializeField] private float defaultSpeed = 10f;
    public float DefaultSpeed { get => defaultSpeed; }
    private Rigidbody rb;
    private float startYpos;
    private Vector3 initialVel;
    private Vector3 lastFrameVel;
    private bool wasShot;

    private Ship player;
    public Ship Player { set => player = value; }
    private Transform playerTransform;
    public Transform PlayerTransform { set => playerTransform = value; }
    private float speed;
    public float Speed { get => speed; set => speed = value; }

    public void Shoot(float minStartAngleOffset,float maxStartAngleOffset)
    {
        if (wasShot) { return; }
        wasShot = true;
        transform.parent = player.BallParent;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        float randSign = Mathf.Sign(Random.Range(-1, 1));
        float angle = randSign * Random.Range(minStartAngleOffset, maxStartAngleOffset);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.velocity = (rot * Vector3.up) * speed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        speed = defaultSpeed;
        startYpos = transform.position.y;
    }

    private void Update()
    {
        lastFrameVel = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool withPlayer = collision.collider.CompareTag("player");
        Bounce(collision.GetContact(0).normal, withPlayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("void"))
        {
            Reset();
        }
    }

    private void Bounce(Vector3 normal, bool withPlayer)
    {
        Vector3 dir = Vector3.Reflect(lastFrameVel.normalized, normal.normalized);
        float bias = withPlayer ? playerBias : 0f;
        Vector3 newDir = Vector3.Lerp(dir, player.currentState.Velocity.normalized, bias).normalized;
        if (withPlayer)
        {
            Debug.DrawRay(transform.position, dir, Color.blue, 3f);
            Debug.DrawRay(transform.position, newDir, Color.red, 3f);
        }
        rb.velocity = newDir * speed;
    }

    public void Reset()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(playerTransform.position.x, startYpos, playerTransform.position.z);
        transform.parent = playerTransform;
        wasShot = false;
    }
}
