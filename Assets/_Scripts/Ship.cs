using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 10f;
    public float DefaultSpeed { get => defaultSpeed; }

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchState(new ShipStateFastship());
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
