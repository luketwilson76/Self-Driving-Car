using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    private Vector3 oldPos;
    private float speedPerSec;
    [HideInInspector] public double normalizedSpeed;
    [SerializeField] private float min;
    [SerializeField] private float max;
    private bool isSec;
    // Update is called once per frame
    void Start()
    {
        oldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isSec = true;
    }
    void Update()
    {
        if (isSec == true)
        {
            StartCoroutine(speed());
        }
    }
    IEnumerator speed()
    {
        isSec = false;
        yield return new WaitForSeconds(1f);
        speedPerSec = Vector3.Distance(oldPos, transform.position) / Time.deltaTime;
        normalize();
        isSec = true;
    }
    double normalize()
    {
        normalizedSpeed = (speedPerSec - min) / (max-min);
        return normalizedSpeed;
    }
}