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

    public override void OnStart()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerCellPosition = playerMovement.GetCurrentCell();
        currentCell = playerMovement.GetGrid().WorldToCell(transform.position);
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

        transform.position = Vector3.Lerp(transform.position, targetCellCenter, moveSpeed);
        if (Vector3.Distance(transform.position, targetCellCenter) <= 0.1f)
            currentCell = targetCell;
    }

    private bool FinishChase()
    {
        Vector3 currentCell = playerMovement.GetGrid().WorldToCell(transform.position);
        Vector3 distance = playerCellPosition - currentCell;

        // Comprobar si hay obstaculos en las direcciones de movimiento
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z))
        {
            if((distance.x < 0 && CheckObstacles(currentCell, Vector3.left)) ||
                (distance.x > 0 && CheckObstacles(currentCell, Vector3.right)))
                return true;
        }
        else
        {
            if ((distance.z < 0 && CheckObstacles(currentCell, Vector3.forward)) ||
                (distance.z > 0 && CheckObstacles(currentCell, Vector3.back)))
                return true;
        }

        return false;
    }

    private bool CheckObstacles(Vector3 currentCell, Vector3 direction)
    {
        Debug.DrawRay(transform.position, direction, Color.red);

        Grid grid = playerMovement.GetGrid();
        Vector3 dir = new Vector3(Mathf.Round(direction.x), 0, Mathf.Round(direction.z));
        Vector3 nextCell = currentCell + dir;
        Vector3Int nextCellInt = new Vector3Int((int)nextCell.x, (int)nextCell.y, (int)nextCell.z);

        targetCell = grid.WorldToCell(nextCell);

        // Comprobar si hay un obstáculo en la siguiente casilla
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, grid.cellSize / 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Breakable") ||
                collider.gameObject.layer == LayerMask.NameToLayer("Unbreakable"))
                return true;
        }

        targetCellCenter = grid.GetCellCenterWorld(nextCellInt);
        targetCellCenter.y = 0.725f;

        return false;
    }
}
