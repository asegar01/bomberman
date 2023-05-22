using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Busca un lugar seguro
public class ChaseSafePlaceAction : Action
{
    GameObject player;
    PlayerMovement playerMovement;
    BombController bombController;
    Grid grid;

    public override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bombController = player.GetComponent<BombController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        grid = playerMovement.GetGrid();
    }

    public override TaskStatus OnUpdate()
    {
        // Obtener la posición actual
        Vector3 currentPosition = transform.position;
        currentPosition.y += playerMovement.GetHeight() / 2;

        // Lista de celdas seguras
        List<Vector3Int> safeCells = GetSafeCells(currentPosition);
        if(safeCells.Count > 0)
        {
            // Encontrar la celda mas cercana al enemigo
            //Vector3Int closestCell = FindClosestCell(currentPosition, safeCells);
        }

        return TaskStatus.Success;
    }

    private List<Vector3Int> GetSafeCells(Vector3 position)
    {
        // Lista de celdas seguras
        List<Vector3Int> safeCells = new List<Vector3Int>();

        // Comprobar las cuatro direcciones para encontrar celdas seguras
        Vector3Int upCell = new Vector3Int((int)position.x, (int)position.y, (int)position.z + 1);
        Vector3Int rightCell = new Vector3Int((int)position.x + 1, (int)position.y, (int)position.z);
        Vector3Int downCell = new Vector3Int((int)position.x, (int)position.y, (int)position.z - 1);
        Vector3Int leftCell = new Vector3Int((int)position.x - 1, (int)position.y, (int)position.z);

        if(!IsCellInDanger(upCell))
            safeCells.Add(upCell);

        if(!IsCellInDanger(rightCell))
            safeCells.Add(rightCell);

        if(!IsCellInDanger(downCell))
            safeCells.Add(downCell);

        if(!IsCellInDanger(leftCell))
            safeCells.Add(leftCell);

        return safeCells;
    }

    private bool IsCellInDanger(Vector3Int cell)
    {
        Vector3 cellPositionCx = grid.GetCellCenterWorld(cell);

        // Comprobar si hay un obstáculo en la casilla
        Collider[] colliders = Physics.OverlapBox(cellPositionCx, grid.cellSize / 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Breakable") ||
                collider.gameObject.layer == LayerMask.NameToLayer("Unbreakable"))
                return true;
        }

        return bombController.RangeBomb(cellPositionCx);
    }
}
