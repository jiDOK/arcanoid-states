using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Transform ballParent;
    public Transform BallParent;
    [SerializeField] private Ball ballPrefab;
    public Ball BallPrefab { get => ballPrefab; }
    [SerializeField] private float maxBallAngleOffset= 5f;
    public float MaxBallAngleOffset => maxBallAngleOffset;
    [SerializeField] private float minBallAngleOffset= 15;
    public float MinBallAngleOffset => minBallAngleOffset;
    [SerializeField] private Vector3 defaultShipScale = new Vector3(1.2f, 0.324f, 1);
    public Vector3 DefaultShipScale { get => defaultShipScale; }
    [SerializeField] private Transform shipMesh;
    public Transform ShipMesh { get => shipMesh; }
    //[SerializeField] private float defaultSpeed = 10f;
    //public float DefaultSpeed { get => defaultSpeed; }

    public Ball Ball { get; private set; }
    public ShipState currentState { get; private set; }

    void Start()
    {
        Ball = GetComponentInChildren<Ball>();
        SwitchState(new ShipStateDefault());
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("expandPU"))
    //    {
    //    }
    //}

    void Update()
    {
        currentState.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchState(new ShipStateFastship());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchState(new ShipStateExpand());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchState(new ShipStateFastball());
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchState(new ShipStateInputSwitch());
        }
    }

    public void SwitchState(ShipState state)
    {
        currentState?.OnStateExit();
        currentState = state;
        currentState.Ship = this;
        currentState.OnStateEnter();
    } 
}
