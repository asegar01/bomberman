using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Update()
    {
        // Verificar si se pulsa la tecla Espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Cargar la escena del nivel uno
            SceneManager.LoadScene("Level1");
        }
    }
}
