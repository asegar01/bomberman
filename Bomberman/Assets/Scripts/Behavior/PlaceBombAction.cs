using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Coloca una bomba en la posicion actual
public class PlaceBombAction : Action
{
    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
