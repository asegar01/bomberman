using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject bombPrefab;
    private KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombsRemaining = 3;
    PlayerMovement playerMovement;
    private float offset = 0.25f;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey) && bombsRemaining > 0)
        {
            StartCoroutine(CreateBomb());
        }
    }

    private IEnumerator CreateBomb()
    {
        Vector3 currentCell = playerMovement.GetCurrentCell();
        currentCell.x -= offset;
        currentCell.z += offset;
        GameObject bomb = Instantiate(bombPrefab, currentCell, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger");

        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }
}
