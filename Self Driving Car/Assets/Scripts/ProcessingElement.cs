using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingElement : MonoBehaviour
{
    [SerializeField]
    void Start()
    {
        Debug.Log("test");
        WeightFile weightFile = new WeightFile();
        weightFile.test = 1;

        string json = JsonUtility.ToJson(weightFile);
        File.WriteAllText(Application.dataPath + "/Json/weights.json", json);
    }
    private class WeightFile
    {
        public int test;
    }
    
}
