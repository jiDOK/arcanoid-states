using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float ballYoffset = 0.338f;
    [SerializeField] private int numMultiballs = 3;
    public int NumMultiballs { get => numMultiballs; }
    [SerializeField] private Vector3 defaultShipScale = new Vector3(1.2f, 0.324f, 1);
    public Vector3 DefaultShipScale { get => defaultShipScale; }
    [SerializeField] private Transform shipMesh;
    public Transform ShipMesh { get => shipMesh; }

    public Ball Ball { get; set; }
    public Stack<Ball> BallPool { get; set; } = new Stack<Ball>(4);
    public ShipState currentState { get; private set; }


    public void Init()
    {
        transform.position = startPos;
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
        // Test buttons
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
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchState(new ShipStateMultiball());
        }
    }

    public void SwitchState(ShipState state)
    {
        currentState?.OnStateExit();
        currentState = state;
        currentState.Ship = this;
        currentState.OnStateEnter();
    }

    public void SpawnOnShip(Ball ball)
    {
        Ball = ball;
        ball.gameObject.SetActive(true);
        ball.transform.position = new Vector3(transform.position.x, transform.position.y + ballYoffset, transform.position.z);
        ball.transform.parent = transform;
    }

    public void SpawnMultiballs()
    {
        for (int i = 0; i < numMultiballs; i++)
        {
            Ball mb = BallPool.Pop();
            mb.gameObject.SetActive(true);
            mb.transform.position = Ball.transform.position;
            mb.Shoot(0f, 360f);
        }
    }
}
