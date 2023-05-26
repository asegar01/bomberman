using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    PlayerMovement playerMovement;
    Grid grid;
    public bool playerFound = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        grid = playerMovement.GetGrid();
    }

    void Update()
    {
        Vector3 distance = player.transform.position - transform.position;
        if(distance.magnitude <= grid.cellSize.x) playerFound = true;
        else playerFound = false;
    }
}
