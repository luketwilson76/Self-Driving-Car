using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class NeuralNetwork : MonoBehaviour
{
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 3);
}
