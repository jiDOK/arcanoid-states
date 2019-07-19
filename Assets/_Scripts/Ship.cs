using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Vector3 defaultShipScale = new Vector3(1.2f, 0.324f, 1);
    public Vector3 DefaultShipScale { get => defaultShipScale; }
    [SerializeField] private Transform shipMesh;
    public Transform ShipMesh { get => shipMesh; }

    public Ball Ball { get; set; }
    public ShipState currentState { get; private set; }


    public void Init(Vector3 startPos)
    {
        transform.position = startPos;
        //SwitchState(new ShipStateDefault());
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
        if (Input.GetKeyDown(KeyCode.LeftAlt))
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

}
