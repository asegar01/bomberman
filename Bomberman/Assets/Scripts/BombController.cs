using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    private KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombsRemaining = 3;
    PlayerMovement playerMovement;
    private float offset = 0.3f;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public float explosionDuration = 1f;
    public int explosionRadius = 2;

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

    // Instancia una bomba en la posicion del jugador
    private IEnumerator CreateBomb()
    {
        Vector3 currentCell = playerMovement.GetCurrentCell();
        Vector3 spawnPoint = currentCell;
        spawnPoint.x -= offset;
        spawnPoint.z += offset;
        GameObject bomb = Instantiate(bombPrefab, spawnPoint, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        GameObject explosion = Instantiate(explosionPrefab, currentCell, Quaternion.Euler(-90f, 0f, 180f));
        Destroy(explosion, explosionDuration);

        Explode(currentCell, Vector3.forward, explosionRadius);
        Explode(currentCell, Vector3.right, explosionRadius);
        Explode(currentCell, Vector3.back, explosionRadius);
        Explode(currentCell, Vector3.left, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    // Explosion de la bomba
    private void Explode(Vector3 position, Vector3 direction, int length)
    {
        if (length <= 0) return;

        position += direction;
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);
        foreach (Collider collider in colliders)
        {
            // Verificar si el objeto se puede atravesar
            if (collider.gameObject.layer == LayerMask.NameToLayer("Unbreakable"))
                return;
        }

        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.Euler(-90f, 0f, 180f));
        Destroy(explosion, explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }
}
