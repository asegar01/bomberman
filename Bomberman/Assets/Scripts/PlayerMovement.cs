using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Grid grid;
    public float moveSpeed = 20f;          // Velocidad de movimiento del jugador
    public float rotationSpeed = 10f;      // Velocidad de rotacion del jugador
    public int destructibleLayer = 1;
    public int indestructibleLayer = 2;

    private Vector3Int currentCell;       // Celda actual en la que se encuentra el jugador

    private void Start()
    {
        currentCell = grid.WorldToCell(transform.position);
    }

    private void Update()
    {
        // Input del jugador
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Direccion del movimiento
        Vector3 movementDir = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 targetPos = transform.position + movementDir * moveSpeed * Time.deltaTime;

        // Obtener la celda correspondiente al movimiento
        Vector3Int targetCell = grid.WorldToCell(targetPos);

        // Rotacion del jugador
        Vector3 dir = (targetPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Mover el jugador a la celda objetivo
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
            currentCell = targetCell;
    }
}