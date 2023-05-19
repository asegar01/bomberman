using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class PlayerMovement : MonoBehaviour
{
    GameObject GO;

    //public Transform pointer;

    void Start()
    {
        GO = new GameObject();
        GO.transform.position = transform.position;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Camera.main.enabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                GO.transform.position = hit.point;
                if (GetComponent<NavMeshAgent>().enabled)
                {
                    GetComponent<NavMeshAgent>().SetDestination(GO.transform.position);
                    //pointer.gameObject.SetActive(true);
                    //pointer.position = hit.point + new Vector3(0, (float)0.5, 0);
                }
            }
        }
    }
}