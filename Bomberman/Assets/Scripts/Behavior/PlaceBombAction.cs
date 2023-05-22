using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

// Coloca una bomba en la posicion actual
public class PlaceBombAction : Action
{
    GameObject player;
    PlayerMovement playerMovement;
    BombController bombController;

    public override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        bombController = player.GetComponent<BombController>();
    }

    public override TaskStatus OnUpdate()
    {
        Grid grid = playerMovement.GetGrid();
        Vector3 bombPosition = grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
        bombPosition.y = playerMovement.GetHeight() * 2;
        bombController.StartCoroutine(bombController.CreateBomb(bombPosition));
        return TaskStatus.Success;
    }
}
