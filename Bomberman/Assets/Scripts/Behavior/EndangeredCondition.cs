using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Indica si se encuentra amenzado por la explosion de alguna bomba
public class EndangeredCondition : Conditional
{
    BombController bombController;
    PlayerMovement playerMovement;

    public override void OnAwake()
    {
        bombController = GetComponent<BombController>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        // Obtener la posición actual
        Vector3 currentPosition = transform.position;
        currentPosition.y += playerMovement.GetHeight() / 2;

        if (RangeBomb(currentPosition))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    // Comprobar si hay alguna bomba que amenace al enemigo
    private bool RangeBomb(Vector3 origin)
    {
        return bombController.RangeBomb(origin);
    }
}
