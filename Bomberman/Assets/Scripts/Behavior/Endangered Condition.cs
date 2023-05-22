using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Indica si se encuentra amenzado por la explosion de alguna bomba
public class EndangeredCondition : Conditional
{
    BombController bombController;
    PlayerMovement playerMovement;
    private int radius;

    public override void OnAwake()
    {
        bombController = GetComponent<BombController>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        radius = bombController.explosionRadius;
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

    // Comprobar si hay una bomba dentro del rango de la explosion en una direccion específica
    private bool RangeBomb(Vector3 origin, Vector3 direction)
    {
        Debug.DrawRay(origin, direction * radius, Color.red);

        int layerMask = ~(LayerMask.GetMask("Player")); // Excluir la capa del jugador del raycast

        // Comprueba si hay una bomba en la posicion actual
        Collider[] colliders = Physics.OverlapSphere(origin, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
                return true;
        }

        // Comprueba si hay una bomba que amenace al enemigo
        RaycastHit[] hits = Physics.RaycastAll(origin, direction, radius, layerMask);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
                return true;
        }

        return false;
    }
}
