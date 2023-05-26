using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    private KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3.5f;
    public int bombsRemaining = 2;
    PlayerMovement playerMovement;
    private float offset = 0.3f;
    public bool isEnemyBombActive = false;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public float explosionDuration = 0.5f;
    public int explosionRadius = 2;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey) && bombsRemaining > 0)
        {
            StartCoroutine(CreateBomb(playerMovement.GetCurrentCell()));
        }
    }

    // Comprueba si hay alguna bomba dentro del rango de la explosion en una direccion específica
    public bool RangeBomb(Vector3 origin)
    {
        int layerMask = ~(LayerMask.GetMask("Player")); // Excluir la capa del jugador del raycast

        // Comprueba si hay alguna bomba en la posicion actual
        Collider[] colliders = Physics.OverlapSphere(origin, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
                return true;
        }

        // Comprueba si hay alguna bomba que amenace al enemigo en alguna direccion
        RaycastHit[] forwardHits = Physics.RaycastAll(origin, Vector3.forward, explosionRadius, layerMask);
        foreach (RaycastHit hit in forwardHits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
                return true;
        }

        RaycastHit[] backHits = Physics.RaycastAll(origin, Vector3.back, explosionRadius, layerMask);
        foreach (RaycastHit hit in backHits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
                return true;
        }

        RaycastHit[] leftHits = Physics.RaycastAll(origin, Vector3.left, explosionRadius, layerMask);
        foreach (RaycastHit hit in leftHits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
                return true;
        }

        RaycastHit[] rightHits = Physics.RaycastAll(origin, Vector3.right, explosionRadius, layerMask);
        foreach (RaycastHit hit in rightHits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
                return true;
        }

        //RaycastHit[] hits = Physics.RaycastAll(origin, direction, explosionRadius, layerMask);
        //foreach (RaycastHit hit in hits)
        //{
        //    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        //        return true;
        //}

        return false;
    }

    // Instancia una bomba en la posicion del jugador
    public IEnumerator CreateBomb(Vector3 position)
    {
        //Vector3 currentCell = playerMovement.GetCurrentCell();
        Vector3 spawnPoint = position;
        spawnPoint.x -= offset;
        spawnPoint.z += offset;
        GameObject bomb = Instantiate(bombPrefab, spawnPoint, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        Vector3Int newCurrentCell = playerMovement.GetGrid().WorldToCell(bomb.transform.position);
        Vector3 targetCellCx = playerMovement.GetGrid().GetCellCenterWorld(newCurrentCell);
        targetCellCx.y = 1.5f;
        bomb.transform.position = playerMovement.GetGrid().WorldToCell(targetCellCx);

        SpawnBomb(targetCellCx);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void SpawnBomb(Vector3 targetPos)
    {
        CreateExplosion(targetPos);

        Explode(targetPos, Vector3.forward, explosionRadius);
        Explode(targetPos, Vector3.right, explosionRadius);
        Explode(targetPos, Vector3.back, explosionRadius);
        Explode(targetPos, Vector3.left, explosionRadius);
    }

    public IEnumerator CreateEnemyBomb(Vector3 position)
    {
        Vector3 spawnPoint = position;
        spawnPoint.x -= offset;
        spawnPoint.z += offset;
        GameObject bomb = Instantiate(bombPrefab, spawnPoint, Quaternion.identity);
        isEnemyBombActive = true;

        yield return new WaitForSeconds(bombFuseTime);

        Vector3Int newCurrentCell = playerMovement.GetGrid().WorldToCell(bomb.transform.position);
        Vector3 targetCellCx = playerMovement.GetGrid().GetCellCenterWorld(newCurrentCell);
        targetCellCx.y = 1.5f;
        bomb.transform.position = playerMovement.GetGrid().WorldToCell(targetCellCx);

        SpawnBomb(targetCellCx);

        Destroy(bomb);
        isEnemyBombActive = false;
    }

    private void CreateExplosion(Vector3 position)
    {
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.Euler(-90f, 0f, 180f));
        Destroy(explosion, explosionDuration);
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

            if (collider.gameObject.layer == LayerMask.NameToLayer("Breakable"))
            {
                Destroy(collider.gameObject);
                CreateExplosion(position);
                return;
            }
        }

        CreateExplosion(position);

        Explode(position, direction, length - 1);
    }
}
