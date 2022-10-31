using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    [Header("Car Controller")]
    public CarController controller;

    //controls the population for each generation and the mutation rate for weights and biases
    [Header("Pooling Settings")]
    [SerializeField] private int populationSize;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float mutationRate;

    //how many agents to keep for crossbreeding, what agents to cull and how many times to cross breed best agents
    [Header("Crossover Controls")]
    public int bestAgentSelection;
    public int worstAgentSelection;
    public int numberToCrossover;

    //stores indices of ANNs
    private List<int> genePool = new List<int>();
    private int naturallySelected;

    //stores ANNs
    private NeuralNetwork[] population;
    [Header("Testing View")]
    public int currentGeneration;
    public int currentGenome = 0;

    private void Start()
    {
        createPopulation();
    }

    //creates population of random ANNs
    private void createPopulation()
    {
        population = new NeuralNetwork[populationSize];
        fillPopWithRandomANNs(population, 0);
        resetForNextGenome();
    }

    //resets values and moves on to next genome (agent)
    private void resetForNextGenome()
    {
        controller.resetANN(population[currentGenome]);
    }

    //fills population with random ANNS (for randomly selected agents)
    private void fillPopWithRandomANNs(NeuralNetwork[] newPopulation, int startingIndex)
    {
        while (startingIndex < populationSize)
        {
            newPopulation[startingIndex] = new NeuralNetwork();
            newPopulation[startingIndex].initializeANN(controller.layers, controller.nodes);
            startingIndex++;
        }
    }

    //checks what genome we're on, if limit reached, cull, breed then move to next gen
    public void exinction(float fitness, NeuralNetwork network)
    {
        if (currentGenome < population.Length - 1)
        {
            population[currentGenome].fitness = fitness;
            currentGenome++;
            resetForNextGenome();
        }
        else
        {
            repopulatePool();
        }
    }

    //repopulates next gen with some children and some random ANNs
    private void repopulatePool()
    {
        genePool.Clear();
        currentGeneration++;
        naturallySelected = 0;
        sortPopulation();
        NeuralNetwork[] newPopulation = pickBestANNs();
        crossoverParentANNs(newPopulation);
        mutateANN(newPopulation);
        fillPopWithRandomANNs(newPopulation, naturallySelected);
        population = newPopulation;
        currentGenome = 0;
        resetForNextGenome();
    }

    //mutates weights and biases
    private void mutateANN(NeuralNetwork[] newPopulation)
    {
        for (int i = 0; i < naturallySelected; i++)
        {
            for (int c = 0; c < newPopulation[i].weights.Count; c++)
            {
                if (Random.Range(0.0f, 1.0f) < mutationRate)
                {
                    newPopulation[i].weights[c] = mutateHelperFunction(newPopulation[i].weights[c]);
                }
            }
        }
    }

    //mutates weights and biases
    Matrix<float> mutateHelperFunction(Matrix<float> A)
    {
        int randomPoints = Random.Range(1, (A.RowCount * A.ColumnCount) / 7);
        Matrix<float> C = A;
        for (int i = 0; i < randomPoints; i++)
        {
            int randomColumn = Random.Range(0, C.ColumnCount);
            int randomRow = Random.Range(0, C.RowCount);

            C[randomRow, randomColumn] = Mathf.Clamp(C[randomRow, randomColumn] + Random.Range(-1f, 1f), -1f, 1f);
        }
        return C;
    }

    //creates children from parents
    private void crossoverParentANNs(NeuralNetwork[] newPopulation)
    {
        for (int i = 0; i < numberToCrossover; i += 2)
        {
            int parentA = i;
            int parentB = i + 1;

            if (genePool.Count >= 1)
            {
                for (int l = 0; l < 100; l++)
                {
                    parentA = genePool[Random.Range(0, genePool.Count)];
                    parentB = genePool[Random.Range(0, genePool.Count)];

                    if (parentA != parentB)
                        break;
                }
            }
            NeuralNetwork childA = new NeuralNetwork();
            NeuralNetwork childB = new NeuralNetwork();
            childA.initializeANN(controller.layers, controller.nodes);
            childB.initializeANN(controller.layers, controller.nodes);
            childA.fitness = 0;
            childB.fitness = 0;

            for (int w = 0; w < childA.weights.Count; w++)
            {

                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    childA.weights[w] = population[parentA].weights[w];
                    childB.weights[w] = population[parentB].weights[w];
                }
                else
                {
                    childB.weights[w] = population[parentA].weights[w];
                    childA.weights[w] = population[parentB].weights[w];
                }

            }
            for (int w = 0; w < childA.biases.Count; w++)
            {

                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    childA.biases[w] = population[parentA].biases[w];
                    childB.biases[w] = population[parentB].biases[w];
                }
                else
                {
                    childB.biases[w] = population[parentA].biases[w];
                    childA.biases[w] = population[parentB].biases[w];
                }

            }

            newPopulation[naturallySelected] = childA;
            naturallySelected++;

            newPopulation[naturallySelected] = childB;
            naturallySelected++;

        }
    }

    //picks best agents and keeps them for breeding and next gen
    private NeuralNetwork[] pickBestANNs()
    {

        NeuralNetwork[] newPopulation = new NeuralNetwork[populationSize];

        for (int i = 0; i < bestAgentSelection; i++)
        {
            newPopulation[naturallySelected] = population[i].initializeCopyOfANN(controller.layers, controller.nodes);
            newPopulation[naturallySelected].fitness = 0;
            naturallySelected++;

            int f = Mathf.RoundToInt(population[i].fitness * 10);

            for (int c = 0; c < f; c++)
            {
                genePool.Add(i);
            }

        }

        for (int i = 0; i < worstAgentSelection; i++)
        {
            int last = population.Length - 1;
            last -= i;

            int f = Mathf.RoundToInt(population[last].fitness * 10);

            for (int c = 0; c < f; c++)
            {
                genePool.Add(last);
            }

        }

        return newPopulation;

    }

    //sorts between best and worst agents based on fitness
    private void sortPopulation()
    {
        for (int i = 0; i < population.Length; i++)
        {
            for (int j = i; j < population.Length; j++)
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