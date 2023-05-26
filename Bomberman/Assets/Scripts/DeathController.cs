using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    private float restartTime = 2f; // Tiempo de espera antes de reiniciar la escena

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si colisiona con la explosion
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            if(gameObject.layer == LayerMask.NameToLayer("Player"))
                StartCoroutine(RestartScene());
            else if(gameObject.layer == LayerMask.NameToLayer("Enemy"))
                StartCoroutine(NextScene());

        }
    }

    // Reinicia la escena
    private IEnumerator RestartScene()
    {
        // Pausar completamente la escena
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(restartTime);
        Time.timeScale = 1f;

        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator NextScene()
    {
        // Pausar completamente la escena
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(restartTime);
        Time.timeScale = 1f;

        // Cargar la siguiente escena
    }
}
