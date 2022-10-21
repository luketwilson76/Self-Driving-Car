using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizeDistances : MonoBehaviour
{
    [SerializeField] private GameObject[] sensors;
    [SerializeField] private float min;
    [SerializeField] private float max;
    [HideInInspector] public float[] normalizedLengths;
    // Start is called before the first frame update
    void Start()
    {
        normalizedLengths = new float[sensors.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < sensors.Length; i++)
        {
            normalizedLengths[i] = normalize(sensors[i].GetComponent<RaycastSensors>().distanceLength);
        }
    }
    
    float normalize(float length)
    {
        float normalizedLength = (length - min) / (max - min);
        return normalizedLength;
    }
}
