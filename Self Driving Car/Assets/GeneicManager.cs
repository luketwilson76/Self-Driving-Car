using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class GeneicManager : MonoBehaviour
{
    [Header("references")]
    public CarController controller;

    [Header("Controls")]
    public int initialPopulation = 85;
    [Range(0.0f, 1.0f)]
    public float mutationRate = 0.055f;

    [Header("Crossover Controls")]
    public int bestAgenetSelection = 8;
    public int worstAgentSelection = 3;
    public int numberToCrossover;

    private List<int> genePool = new List<int>();
    private int naturallySelected;

    private NeuralNetwork[] population;

    [Header("Public View")]
    public int currentGeneration;
    public int currentGenome = 0;

    private void Start()
    {
        CreatePopulation();
    }

    private void CreatePopulation()
    {
        population = new NeuralNetwork[initialPopulation];
        FillPopulationWithRandomValues(population, 0);
        ResetToCurrentGenome();
    }

    private void ResetToCurrentGenome()
    {
        controller.ResetWithNetwork(population[currentGenome]);
    }

    private void FillPopulationWithRandomValues (NeuralNetwork[] newPopulation, int startingIndex)
    {
        while (startingIndex < initialPopulation)
        {
            newPopulation[startingIndex] = new NeuralNetwork();
            newPopulation[startingIndex].Initialise(controller.layers, controller.neurons);
            startingIndex++;
        }
    }
    public void Death (float fitness, NeuralNetwork network)
    {
        if (currentGeneration < population.Length - 1)
        {
            population[currentGenome].fitness = fitness;
            currentGenome++;
            ResetToCurrentGenome();
        }
        else
        {
            Repopulate();
        }
    }

    private void Repopulate()
    {
        genePool.Clear();
        currentGeneration++;
        naturallySelected = 0;
        SortPopulation();
        NeuralNetwork[] newPopulation = PickBestPopulation();
    }

    private NeuralNetwork[] PickBestPopulation()
    {
        NeuralNetwork[] newPopulation = new NeuralNetwork[initialPopulation];
        for (int i = 0;i< bestAgenetSelection; i++)
        {
            newPopulation[naturallySelected] = population[i];


            naturallySelected++;
        }
    }

    private void SortPopulation()
    {
        for (int i = 0; i < population.Length; i++)
        {
            for (int j = 0; i < population.Length; i++)
            {
                if (population[i].fitness < population[j].fitness)
                {
                    NeuralNetwork temp = population[i];
                    population[i] = population[j];
                    population[j] = temp;
                }
            }
        }
    }
}
