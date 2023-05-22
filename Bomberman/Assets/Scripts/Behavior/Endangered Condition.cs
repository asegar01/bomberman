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

        // Realizar un raycast en cada direccion y comprobar si hay una bomba dentro del rango de la explosion
        if (RangeBomb(currentPosition, Vector3.forward) ||
            RangeBomb(currentPosition, Vector3.back) ||
            RangeBomb(currentPosition, Vector3.left) ||
            RangeBomb(currentPosition, Vector3.right))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    // Comprueba si hay alguna bomba dentro del rango de la explosion en una direccion específica
    private bool RangeBomb(Vector3 origin, Vector3 direction)
    {
        return bombController.RangeBomb(origin, direction);
    }
}
