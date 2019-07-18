using UnityEngine;

public class Ball : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float shipBias = 0.25f;

    [SerializeField] private float defaultSpeed = 10f;
    public float DefaultSpeed { get => defaultSpeed; }
    [SerializeField] private float minStartAngleOffset = 15;
    public float MinBallAngleOffset => minStartAngleOffset;
    [SerializeField] private float maxStartAngleOffset = 5f;
    public float MaxBallAngleOffset => maxStartAngleOffset;

    private Rigidbody rb;
    private Vector3 initialVel;
    private Vector3 lastFrameVel;
    private bool wasShot;

    private Ship ship;
    public Ship Ship { set => ship = value; }
    private float speed = 10f;
    public float Speed { get => speed; set => speed = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        speed = defaultSpeed;
    }

    private void Update()
    {
        lastFrameVel = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool withShip = collision.collider.CompareTag("ship");
        Bounce(collision.GetContact(0).normal, withShip);
    }

    public void Shoot()
    {
        Shoot(minStartAngleOffset, maxStartAngleOffset);
    }

    public void Shoot(float minStartAngleOffset, float maxStartAngleOffset)
    {
        if (wasShot) { return; }
        wasShot = true;
        transform.parent = null;
        //rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 dir = RotateBallDirRandom(Vector3.up, minStartAngleOffset, maxStartAngleOffset);
        rb.velocity = dir * speed;
    }

    Vector3 RotateBallDirRandom(Vector3 dir, float minStartAngleOffset, float maxStartAngleOffset)
    {
        float randSign = Mathf.Sign(Random.Range(-1f, 1f));
        float angle = randSign * Random.Range(minStartAngleOffset, maxStartAngleOffset);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        return rot * dir;
    }

    private void Bounce(Vector3 normal, bool withShip)
    {
        Vector3 dir = Vector3.Reflect(lastFrameVel.normalized, normal.normalized);
        float bias = withShip ? shipBias : 0f;
        Vector3 newDir = Vector3.Lerp(dir, ship.currentState.Velocity.normalized, bias).normalized;
        rb.velocity = newDir * speed;
        // visualize
        //if (withShip)
        //{
        //    Debug.DrawRay(transform.position, dir, Color.blue, 3f);
        //    Debug.DrawRay(transform.position, newDir, Color.red, 3f);
        //}
    }

    public void Init()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        wasShot = false;
    }
}
