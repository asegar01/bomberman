using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Busca al jugador
public class ChasePlayerAction : Action
{
    private PlayerMovement playerMovement;
    private Transform playerTransform;
    private Vector3 playerCellPosition;
    private Vector3Int targetCell;
    private Vector3 targetCellCenter;
    private Vector3Int currentCell;       // Celda actual en la que se encuentra el enemigo
    private float moveSpeed = 1.0f;
    private float currentTime = 0.0f;
    private float thinkTime = 2.0f;
    private float rotationSpeed = 10f;
    public float timeWait = 10f;
    private AStarPathfinder pathfinder;
    private Grid grid;

    public override void OnStart()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerCellPosition = playerMovement.GetCurrentCell();
        currentCell = playerMovement.GetGrid().WorldToCell(transform.position);
        grid = playerMovement.GetGrid();
    }

    public override TaskStatus OnUpdate()
    {
        currentTime += Time.deltaTime;

        if (FinishChase())
            return TaskStatus.Success;

        MoveCell();

        return TaskStatus.Running;
    }

    private void MoveCell()
    {
        if (currentTime < thinkTime) return;
        currentTime = 0.0f;

        // Rotacion del enemigo
        //Vector3 dir = (targetCellCenter - transform.position).normalized;
        //Quaternion targetRotation = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        transform.position = Vector3.Lerp(transform.position, targetCellCenter, moveSpeed);
        if (Vector3.Distance(transform.position, targetCellCenter) <= 0.1f)
            currentCell = targetCell;
    }

    private bool FinishChase()
    {
        // Obtener la posición actual
        Vector3 currentPosition = transform.position;
        Vector3 distance = playerCellPosition - currentPosition;

        // Comprobar si hay obstaculos en las direcciones de movimiento
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z))
        {
            if((distance.x < 0 && CheckObstacles(currentPosition, Vector3.left)) ||
                (distance.x > 0 && CheckObstacles(currentPosition, Vector3.right)))
                return true;
        }
        else
        {
            if ((distance.z < 0 && CheckObstacles(currentPosition, Vector3.back)) ||
                (distance.z > 0 && CheckObstacles(currentPosition, Vector3.forward)))
                return true;
        }

        return false;
    }

    private bool CheckObstacles(Vector3 currentCell, Vector3 direction)
    {
        Vector3 dir = new Vector3(Mathf.Round(direction.x), 0, Mathf.Round(direction.z));
        Vector3 nextCell = currentCell + dir;
        nextCell.y = playerMovement.GetHeight();

        // Comprobar si hay un obstáculo en la siguiente casilla
        Collider[] colliders = Physics.OverlapSphere(nextCell, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Breakable") ||
                collider.gameObject.layer == LayerMask.NameToLayer("Unbreakable"))
                return true;
        }

        // Comprobar si se puede encontrar un camino
        //pathfinder = new AStarPathfinder(grid);

        //Vector3Int playerCellInt = grid.WorldToCell(playerCellPosition);
        //Vector3Int currentCellInt = grid.WorldToCell(currentCell);
        //List<Vector3Int> path = pathfinder.FindPath(currentCellInt, playerCellInt);

        //if (path == null || path.Count == 0)
        //{

        //    // No se encuentra un camino directo, quedarse en la posicion mas cercana
        //    targetCellCenter = GetClosestCell(currentCellInt, playerCellPosition);
        //    return true;
        //}

        targetCellCenter = nextCell;

        return false;
    }

    private Vector3 GetClosestCell(Vector3Int startCell, Vector3 targetPosition)
    {
        return new Vector3();
    }
}
