using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Indica si se ha colocado una bomba
public class PlacedBombCondition : Conditional
{
    BombController bombController;

    public override void OnAwake()
    {
        bombController = GameObject.FindGameObjectWithTag("Player").GetComponent<BombController>();
    }

    public override TaskStatus OnUpdate()
    {
        if(bombController.isEnemyBombActive)
            return TaskStatus.Success;
        return TaskStatus.Failure;
    }
}
