using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Grid grid;
    public float moveSpeed = 20f;          // Velocidad de movimiento del jugador
    public float rotationSpeed = 10f;      // Velocidad de rotacion del jugador
    public int destructibleLayer = 6;
    public int indestructibleLayer = 7;
    private float playerHeight;

    private Vector3Int currentCell;       // Celda actual en la que se encuentra el jugador
    //private Animation anim;

    private void Start()
    {
        currentCell = grid.WorldToCell(transform.position);
        playerHeight = transform.position.y;
        //anim = GetComponent<Animation>();
    }

    private void FixedUpdate()
    {
        // Input del jugador
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Direccion del movimiento
        Vector3 movementDir = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 targetPos = transform.position + movementDir * moveSpeed * Time.fixedDeltaTime;

        // Obtener la celda correspondiente al movimiento
        Vector3Int targetCell = grid.WorldToCell(targetPos);

        // Calcular el centro de la celda objetivo
        Vector3 targetCellCenter = grid.GetCellCenterWorld(targetCell);
        targetCellCenter.y = playerHeight;

        // Rotacion del jugador
        Vector3 dir = (targetCellCenter - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Animacion del jugador
        //if (Vector3.Distance(transform.position, targetCellCenter) > 0.1f)
        //{
        //    // Reproducir la animacion de movimiento
        //    if (!anim.isPlaying || anim.clip.name != "Run")
        //    {
        //        anim.Play("Run");
        //    }
        //}
        //else
        //{
        //    // Reproducir la animacion de inactividad
        //    if (!anim.isPlaying || anim.clip.name != "Idle")
        //    {
        //        anim.Play("Idle");
        //    }
        //}

        bool canMove = true;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit) && hit.collider != null)
        {
            if (dir.x > 0f && hit.distance < 0.5f || dir.x < 0f && hit.distance < 0.65f)
                canMove = false;
            if (dir.z > 0f && hit.distance < 0.6f || dir.z < 0f && hit.distance < 0.5f)
                canMove = false;
        }

        // Mover el jugador a la celda objetivo
        if (canMove)
        {
            transform.position = Vector3.Lerp(transform.position, targetCellCenter, moveSpeed * Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, targetCellCenter) <= 0.1f)
                currentCell = targetCell;
        }
        else
        {
            Vector3 targetCellCx = grid.GetCellCenterWorld(currentCell);
            targetCellCx.y = playerHeight;
            transform.position = Vector3.Lerp(transform.position, targetCellCx, moveSpeed * Time.fixedDeltaTime);
        }
    }
}