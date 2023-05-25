using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Indica si se ha encontrado al jugador
public class PlayerFoundCondition : Conditional
{
    EnemyController enemyController;

    public override void OnAwake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public override TaskStatus OnUpdate()
    {
        if(!enemyController.playerFound)
            return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
