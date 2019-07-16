using UnityEngine;

[System.Serializable]
public class ShipStateDefault : ShipState
{
    protected float speed;
    protected float xInput;

    public override Vector3 Velocity { get; protected set; }

    public override void OnStateEnter()
    {
        speed = Ship.DefaultSpeed;
    }

    public override void OnStateExit()
    {
    }

    public override void OnUpdate()
    {
        Movement();
        ShootBall();
    }

    private void Movement()
    {
        xInput = Input.GetAxis("Horizontal");
        Vector3 newPos = Ship.transform.position;

        Velocity = xInput * Time.deltaTime * speed * Vector3.right;
        newPos += Velocity;
        if (newPos.x < -4f)
        {
            newPos = new Vector3(-4f, Ship.transform.position.y, Ship.transform.position.z);
        }
        else if (newPos.x > 4f)
        {
            newPos = new Vector3(4f, Ship.transform.position.y, Ship.transform.position.z);
        }
        Ship.transform.position = newPos;
    }

    private void ShootBall()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Ship.Ball.Shoot();
        }
    }

}
