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
    private float rotationSpeed = 50f;
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
        // Tiempo de reaccion
        if (currentTime < thinkTime) return;
        currentTime = 0.0f;

        // Rotacion del enemigo
        Vector3 dir = (targetCellCenter - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Movimiento del enemigo
        transform.position = Vector3.Lerp(transform.position, targetCellCenter, moveSpeed);
        if (Vector3.Distance(transform.position, targetCellCenter) <= 0.1f)
            currentCell = targetCell;
    }

    private bool FinishChase()
    {
        // Obtener la posición actual
        Vector3 currentPosition = transform.position;
        Vector3 distance = playerCellPosition - currentPosition;

        // Obtiene la direccion de movimiento
        Vector3 primaryDirection, secondaryDirection;

        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z))
        {
            primaryDirection = distance.x < 0 ? Vector3.left : Vector3.right;
            secondaryDirection = distance.z < 0 ? Vector3.back : Vector3.forward;
        }
        else
        {
            primaryDirection = distance.z < 0 ? Vector3.back : Vector3.forward;
            secondaryDirection = distance.x < 0 ? Vector3.left : Vector3.right;
        }

        // Comprueba si hay obstaculos en esas posiciones
        if (!CheckObstacles(currentPosition, primaryDirection))
            return false;

        else if (!CheckObstacles(currentPosition, secondaryDirection))
            return false;

        return true;
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
