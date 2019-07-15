using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public virtual Ship ship { get; set; }
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void OnUpdate();
}
