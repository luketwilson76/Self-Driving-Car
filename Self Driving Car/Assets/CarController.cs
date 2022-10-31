using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NeuralNetwork))]
public class CarController : MonoBehaviour
{
    [Header("Normalize Data Values")]
    [SerializeField] private float maxSensorDistance;

    [Header("Genetic Manager")]
    [SerializeField] private GameObject geneticManager;

    private Vector3 startPos, startRotation;
    private NeuralNetwork network;

    //acceleration and turning values
    [Range(-1f, 1f)]
    [SerializeField] private float acceleration, turning;

    //records time of car while alive
    [HideInInspector] public float timer = 0f;

    //variables used to calculate fitness
    [Header("Fitness Function Variables")]
    [SerializeField] private float maxFitness;
    [SerializeField] private float minFitness;
    [SerializeField] private float cutOffTime;
    [SerializeField] private float distanceMultiplier = 1.4f;
    [SerializeField] private float speedMultiplier = 0.2f;
    [SerializeField] private float sensorMultiplier = 0.1f;

    private float fitness;

    //structure of ANN
    [Header("Network Structure")]
    public int layers = 1;
    public int nodes = 10;

    //records last position to calculate distanceMultiplier travelled
    private Vector3 lastPos;
    private float totalDis;
    private float avgSpeed;

    //gather raw data for distanceMultiplier between car and wall
    private float sensor1, sensor2, sensor3;
    private Vector3 input;

    //when program starts, record start pos of car and grab network script off car
    private void Awake()
    {
        startPos = transform.position;
        startRotation = transform.eulerAngles;
        network = GetComponent<NeuralNetwork>();
    }

    //resets the network
    public void resetANN(NeuralNetwork net)
    {
        network = net;
        Reset();
    }

    //resets all stats of car
    public void Reset()
    {
        timer = 0f;
        totalDis = 0f;
        avgSpeed = 0f;
        lastPos = startPos;
        fitness = 0f;
        transform.position = startPos;
        transform.eulerAngles = startRotation;
    }

    //when car runs into wall, reset
    private void OnCollisionEnter(Collision collision)
    {
        die();
    }

    //main function
    private void FixedUpdate()
    {
        normalizeSensorInputs();
        lastPos = transform.position;
        (acceleration, turning) = network.runNetwork(sensor1, sensor2, sensor3);
        moveCar(acceleration, turning);
        timer += Time.deltaTime;
        fitnessCalc();
    }

    //kills car and moves on to next genome
    private void die()
    {
        geneticManager.GetComponent<GeneticManager>().exinction(fitness, network);
    }

    //calculates fitness
    private void fitnessCalc()
    {
        totalDis += Vector3.Distance(transform.position, lastPos);
        avgSpeed = totalDis / timer;
        fitness = (totalDis * distanceMultiplier) + (avgSpeed * speedMultiplier) + (((sensor1 + sensor2 + sensor3) / 3) * sensorMultiplier);
        if (timer > cutOffTime && fitness < minFitness)
        {
            die();
        }

        if (fitness >= maxFitness)
        {
            die();
        }

    }

    //gathers raw data from sensors and normalizes it, also draws rays
    private void normalizeSensorInputs()
    {
        Vector3 a = (transform.forward + transform.right);
        Vector3 b = (transform.forward);
        Vector3 c = (transform.forward - transform.right);

        Ray r = new Ray(transform.position, a);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit))
        {
            sensor1 = hit.distance / maxSensorDistance;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        r.direction = b;

        if (Physics.Raycast(r, out hit))
        {
            sensor2 = hit.distance / maxSensorDistance;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        r.direction = c;

        if (Physics.Raycast(r, out hit))
        {
            sensor3 = hit.distance / maxSensorDistance;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

    }

    //calculates turning of car
    public void moveCar(float v, float h)
    {
        input = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, v * 11.4f), 0.02f);
        input = transform.TransformDirection(input);
        transform.position += input;

        transform.eulerAngles += new Vector3(0, (h * 90) * 0.02f, 0);
    }

}