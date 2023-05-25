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
    Vector3 nextCell;
    private int length = 2;
    private float currentTime = 0.0f;
    private float thinkTime = 0.5f;
    private float moveSpeed = 1.0f;
    private float rotationSpeed = 50.0f;

    public override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bombController = player.GetComponent<BombController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        grid = playerMovement.GetGrid();
    }

    public override TaskStatus OnUpdate()
    {
        currentTime += Time.deltaTime;

        // Obtener la posición actual
        Vector3 currentPosition = transform.position;
        //currentPosition.y += playerMovement.GetHeight() / 2;

        // Lista de celdas seguras
        List<Vector3Int> safeCells = GetSafeCells(currentPosition, length);
        if(safeCells.Count > 0)
        {
            // Encontrar la celda mas cercana al enemigo
            Vector3Int closestCell = FindClosestCell(currentPosition, safeCells);
            Vector3 closestCellCenter = grid.GetCellCenterWorld(grid.WorldToCell(closestCell));
            closestCellCenter.y = 0.725f;

            //StartCoroutine(MoveToTarget(closestCellCenter));

            Vector3 distance = closestCellCenter - transform.position;
            if (distance.x < 0 && !IsCellObstacle(transform.position + Vector3.left)) nextCell = transform.position + Vector3.left;
            else if (distance.x > 0 && !IsCellObstacle(transform.position + Vector3.right)) nextCell = transform.position + Vector3.right;
            else if (distance.z < 0 && !IsCellObstacle(transform.position + Vector3.back)) nextCell = transform.position + Vector3.back;
            else if(distance.z > 0 && !IsCellObstacle(transform.position + Vector3.forward)) nextCell = transform.position + Vector3.forward;

            if (currentTime > thinkTime)
            {
                Vector3 dir = (nextCell - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

                transform.position = Vector3.Lerp(currentPosition, nextCell, moveSpeed);
                currentTime = 0.0f;
            }
            if (Vector3.Distance(transform.position, closestCellCenter) <= 0.1f)
                currentPosition = closestCell;
            else return TaskStatus.Running;
        }

        return TaskStatus.Success;
    }

    // Encontrar la celda más cercana al personaje
    private Vector3Int FindClosestCell(Vector3 currentCell, List<Vector3Int> cells)
    {
        Vector3Int closestCell = cells[0];
        float closestDistance = Vector3.Distance(currentCell, closestCell);

        for (int i = 1; i < cells.Count; i++)
        {
            float distance = Vector3.Distance(currentCell, cells[i]);
            if (distance < closestDistance)
            {
                closestCell = cells[i];
                closestDistance = distance;
            }
        }

        return closestCell;
    }

    private List<Vector3Int> GetSafeCells(Vector3 position, int length)
    {
        // Lista de celdas seguras
        List<Vector3Int> safeCells = new List<Vector3Int>();

        // Comprobar las cuatro direcciones para encontrar celdas seguras
        Vector3Int upCell = new Vector3Int((int)position.x, (int)position.y, (int)position.z + 1);
        Vector3Int rightCell = new Vector3Int((int)position.x + 1, (int)position.y, (int)position.z);
        Vector3Int downCell = new Vector3Int((int)position.x, (int)position.y, (int)position.z - 1);
        Vector3Int leftCell = new Vector3Int((int)position.x - 1, (int)position.y, (int)position.z);

        GetSafeCellRecursive(position, upCell, safeCells, length);
        GetSafeCellRecursive(position, downCell, safeCells, length);
        GetSafeCellRecursive(position, rightCell, safeCells, length);
        GetSafeCellRecursive(position, leftCell, safeCells, length);

        return safeCells;
    }

    private void GetSafeCellRecursive(Vector3 position, Vector3Int cell, List<Vector3Int> safeCells, int length)
    {
        if (IsCellObstacle(cell) || length <= 0)
            return;
        else if (!IsCellInDanger(cell))
        {
            safeCells.Add(cell);
            return;
        }

        // Obtener las celdas vecinas
        Vector3Int upCell = new Vector3Int(cell.x, cell.y, cell.z + 1);
        Vector3Int rightCell = new Vector3Int(cell.x + 1, cell.y, cell.z);
        Vector3Int downCell = new Vector3Int(cell.x, cell.y, cell.z - 1);
        Vector3Int leftCell = new Vector3Int(cell.x - 1, cell.y, cell.z);

        // Encontrar la celda segura mas cercana en cada direccion recursivamente
        GetSafeCellRecursive(position, upCell, safeCells, length - 1);
        GetSafeCellRecursive(position, downCell, safeCells, length - 1);
        GetSafeCellRecursive(position, rightCell, safeCells, length - 1);
        GetSafeCellRecursive(position, leftCell, safeCells, length - 1);
    }

    private bool IsCellObstacle(Vector3 cell)
    {
        Vector3 cellPositionCx = grid.GetCellCenterWorld(grid.WorldToCell(cell));
        cellPositionCx.y = 1;

        // Comprobar si hay un obstáculo en la casilla
        Collider[] colliders = Physics.OverlapSphere(cellPositionCx, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Breakable") ||
                collider.gameObject.layer == LayerMask.NameToLayer("Unbreakable"))
                return true;
        }

        return false;
    }

    private bool IsCellInDanger(Vector3Int cell)
    {
        Vector3 cellPositionCx = grid.GetCellCenterWorld(grid.WorldToCell(cell));
        cellPositionCx.y = 1;

        // Comprobar si hay algun obstaculo en la casilla
        //Collider[] colliders = Physics.OverlapSphere(cellPositionCx, 0.1f);
        //foreach (Collider collider in colliders)
        //{
        //    if (collider.gameObject.layer == LayerMask.NameToLayer("Breakable") ||
        //        collider.gameObject.layer == LayerMask.NameToLayer("Unbreakable"))
        //        return true;
        //}

        return bombController.RangeBomb(cellPositionCx);
    }
}
