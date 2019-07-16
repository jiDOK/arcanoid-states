using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipState
{
    public virtual Ship Ship { get; set; }
    public virtual Vector3 Velocity { get; protected set; }
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void OnUpdate();
}

