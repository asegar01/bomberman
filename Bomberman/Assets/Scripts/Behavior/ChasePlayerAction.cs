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
    private float moveSpeed = 27f;
    private float rotationSpeed = 10f;
    public float timeWait = 1f; 

    public override void OnStart()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerCellPosition = playerMovement.GetCurrentCell();
    }

    public override TaskStatus OnUpdate()
    {
        if(FinishChase())
            return TaskStatus.Success;

        StartCoroutine(MoveCell());

        return TaskStatus.Running;
    }

    private IEnumerator MoveCell()
    {
        Debug.Log(currentCell);
        Debug.Log(targetCellCenter);

        transform.position = Vector3.Lerp(transform.position, targetCellCenter, moveSpeed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, targetCellCenter) <= 0.1f)
            currentCell = targetCell;

        yield return new WaitForSeconds(timeWait);
    }

    private bool FinishChase()
    {
        Vector3 currentCell = playerMovement.GetGrid().WorldToCell(transform.position);
        Vector3 distance = playerCellPosition - currentCell;

        // Comprobar si hay obstaculos en las direcciones de movimiento
        if (distance.x > distance.z)
        {
            if (CheckObstacles(currentCell, Vector3.right))
                return true;
        }
        else
        {
            if (CheckObstacles(currentCell, Vector3.forward))
                return true;
        }

        return false;
    }

    private bool CheckObstacles(Vector3 currentCell, Vector3 direction)
    {
        Grid grid = playerMovement.GetGrid();
        Vector3 nextCell = currentCell + direction;
        targetCell = grid.WorldToCell(nextCell);

        // Comprobar si hay un obstáculo en la siguiente casilla
        Collider[] colliders = Physics.OverlapBox(targetCell, grid.cellSize / 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Breakable") ||
                collider.gameObject.layer == LayerMask.NameToLayer("Unbreakable"))
                return true;
        }
        Debug.Log("AAAAAAAAAAA");
        targetCellCenter = grid.GetCellCenterWorld(targetCell);
        targetCellCenter.y = 0.725f;

        return false;
    }
}
