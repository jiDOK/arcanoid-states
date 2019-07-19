using UnityEngine;
using System;

public abstract class ShipState
{
    public virtual Ship Ship { get; set; }
    public virtual Vector3 Velocity { get; protected set; }
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void OnUpdate();
}


public class ShipStateDefault : ShipState
{
    protected float xInput;
    protected float speed = 10f;
    protected float inputMultiplier = 1f;

    public override Vector3 Velocity { get; protected set; }

    public override void OnStateEnter()
    {
        Debug.Log("Ship switched to DefaultState!");
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
        xInput = inputMultiplier * Input.GetAxis("Horizontal");
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

public class ShipStateExpand : ShipStateDefault
{
    public override void OnStateEnter()
    {
        Ship.ShipMesh.transform.localScale += new Vector3(1f, 0f, 0f);
    }
    public override void OnStateExit()
    {
        Ship.ShipMesh.transform.localScale = Ship.DefaultShipScale;
    }
}

public class ShipStateFastball : ShipStateDefault
{
    public override void OnStateEnter()
    {
        Ship.Ball.Speed *= 2f;
    }
    public override void OnStateExit()
    {
        Ship.Ball.Speed = Ship.Ball.DefaultSpeed;
    }
}

public class ShipStateFastship : ShipStateDefault
{
    public override void OnStateEnter()
    {
        speed = 20f;
    }
}

public class ShipStateInputSwitch : ShipStateDefault
{
    public override void OnStateEnter()
    {
        inputMultiplier = -1f;
    }
}

public class ShipStateMultiball : ShipStateDefault
{
    public static event Action MultiballTriggered;
    public override void OnStateEnter()
    {
        //TODO: (bug) letzter Ball muss ball werden... sonst kann es keine 2 Multiballs hintereinader geben
        MultiballTriggered?.Invoke();
    }
}
