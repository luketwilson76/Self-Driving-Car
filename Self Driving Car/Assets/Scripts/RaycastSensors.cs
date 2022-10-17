using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSensors : MonoBehaviour
{
    private GameObject lastHit;
    private Vector3 collision = Vector3.zero;
    [HideInInspector] float 

    // Update is called once per frame
    void Update()
    {
        var ray = new Ray(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;
        }
        distance()
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(collision, new Vector3(1, 1, 1));
    }

}