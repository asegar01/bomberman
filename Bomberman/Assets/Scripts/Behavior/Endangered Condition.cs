using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Indica si se encuentra amenzado por la explosion de alguna bomba
public class EndangeredCondition : Conditional
{
    BombController bombController;
    private int radius;

    public override void OnAwake()
    {
        bombController = GetComponent<BombController>();
        radius = bombController.explosionRadius;
    }

    public override TaskStatus OnUpdate()
    {
        // Obtener la posición actual
        Vector3 currentPosition = transform.position;

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

    // Comprobar si hay una bomba dentro del rango de la explosion en una direccion específica
    private bool RangeBomb(Vector3 origin, Vector3 direction)
    {
        RaycastHit[] hits = Physics.RaycastAll(origin, direction, radius, LayerMask.GetMask("Bomb"));
        if (hits.Length > 0) return true;
        return false;
    }
}
