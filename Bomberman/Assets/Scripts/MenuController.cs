using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BehaviorDesigner.Runtime;

public class MenuController : MonoBehaviour
{
    //BehaviorTree behaviorTree;
    //ChasePlayerAction chasePlayerAction;

    private void Start()
    {
        //behaviorTree = GetComponent<BehaviorTree>();
        //chasePlayerAction = behaviorTree.FindTask<ChasePlayerAction>();
    }

    private void Update()
    {
        // Verificar si se pulsa la tecla Espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //chasePlayerAction.thinkTime = 1.2f;

            // Cargar la escena del nivel uno
            SceneManager.LoadScene("Level1");
        }
    }
}
