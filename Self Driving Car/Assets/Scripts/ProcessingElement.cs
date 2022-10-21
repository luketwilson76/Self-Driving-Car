using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingElement : MonoBehaviour
{
    [SerializeField] private TextAsset weights;
    //[SerializeField] private float[] patterns;
    private int numWeights;

    private void Start()
    {
        Debug.Log(weights);
        //patterns = gameObject.GetComponent<NormalizeDistances>().normalizedLengths;
    }

    /*float sigmoid(float input)
    {
        return 1/(1+Mathf.Exp(-input));
    }

    float generate_output(float[] pattern)
    {
        float output = 0;
        for (int i = 0; i < pattern.Length; i++)
        {
            
        }
        output = sigmoid(output);
        return output;
    }*/
}


