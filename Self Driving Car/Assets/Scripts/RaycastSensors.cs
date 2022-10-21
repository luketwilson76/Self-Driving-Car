using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaycastSensors : MonoBehaviour
{
    private GameObject lastHit;
    private Vector3 collision = Vector3.zero;
    [HideInInspector] public float distanceLength;

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
        distance(ray.origin, collision);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(collision, new Vector3(1, 1, 1));
    }
    private void distance(Vector3 self, Vector3 collision)
    {
        float deltaX = collision.x - self.x;
        float deltaY = collision.y - self.y;
        float deltaZ = collision.z - self.z;
        double distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        distanceLength = ((float)distance);
    }
}