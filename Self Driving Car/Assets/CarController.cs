using UnityEngine;

[RequireComponent(typeof(NeuralNetwork))]
public class CarController : MonoBehaviour
{
    [Header("Normalization Values")]
    [SerializeField] private float maxDistance;

    [Header("Magnitudes of Acceleration")]
    [SerializeField] public float Vertical = 11.4f;
    [SerializeField] public float Horizontal = 0.02f;

    //sets inital starting point
    private Vector3 startPosition, startRotation;


    private NeuralNetwork network;


    [Range(-1f, 1f)]
    public float acceleration, turningValue;

    [HideInInspector] public float timeSinceStart = 0f;

    [HideInInspector] public float overallFitness;

    //these two variables tell the fitness function what counts more (the distance a car goes or the speed at which a car goes?)
    [Header("Fitness Function Factors")]
    [SerializeField] public float distanceMultiplier = 1.4f;
    [SerializeField] public float avgSpeedMultiplier = 0.2f;
    [SerializeField] public float sensorMultiplier = 0.1f;
    [SerializeField] private float timeReset = 20;
    [SerializeField] private float fitnessLowReset = 40f;
    [SerializeField] private float fitnessHighReset = 1000f;

    [Header("Network Options")]
    public int layers = 1;
    public int neurons = 10;
    
    private Vector3 lastPosition;
    private float totalDistanceTraveled;
    private float avgSpeed;

    //sensors that gives us the inputs for the ANN
    private float aSensor, bSensor, cSensor;

    private void FixedUpdate()
    {
        InputSensor();
        lastPosition = transform.position;
        //ANN code here

        (acceleration, turningValue) = network.RunNetwork(aSensor, bSensor, cSensor);

        MoveCar(acceleration, turningValue);
        timeSinceStart += Time.deltaTime;
        CalculateFitness();

        //acceleration = 0;
        //time = 0;

    }

    private void Awake()
    {
        //sets inital starting point
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        network = GetComponent<NeuralNetwork>();
    }

    public void ResetWithNetwork(NeuralNetwork neuralNetwork)
    {
        network = neuralNetwork;
    }

    public void Reset()
    {
        timeSinceStart = 0f;
        totalDistanceTraveled = 0f;
        avgSpeed = 0f;
        overallFitness = 0f;
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }

     private void OnCollisionEnter(Collision collision)
    {

        Death();
    }

    private void Death()
    {
        GameObject.FindObjectOfType<GeneicManager>().Death(overallFitness, network);
    }

    private void CalculateFitness()
    {
        //find total distance traveled
        totalDistanceTraveled += Vector3.Distance(transform.position, lastPosition);
        avgSpeed = totalDistanceTraveled / timeSinceStart;
        //calculates the fitness for the car using all factors
        overallFitness = (totalDistanceTraveled * distanceMultiplier) + (avgSpeed * avgSpeedMultiplier) + (((aSensor+bSensor+cSensor)/3)*sensorMultiplier);
        //if car is too slow or not moving, reset simulation
        if (timeSinceStart > timeReset && overallFitness < fitnessLowReset)
        {
            Death();
        }

        if (overallFitness >= fitnessHighReset)
        {
            //Saves network to a JSON!!!!!!!!!!!!!!!!!!!!!!
            Death();
        }
    }

    private void InputSensor()
    {
        //sensor looking diagonally right
        Vector3 a = (transform.forward + transform.right);
        //sensor looking forward
        Vector3 b = (transform.forward);
        //sensor looking diagonally left
        Vector3 c = (transform.forward - transform.right);

        Ray r = new Ray(transform.position, a);
        RaycastHit hit;

        if (Physics.Raycast(r,out hit))
        {
            aSensor = hit.distance / maxDistance;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        r.direction = b;
        if (Physics.Raycast(r, out hit))
        {
            bSensor = hit.distance / maxDistance;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        r.direction = c;
        if (Physics.Raycast(r, out hit))
        {
            cSensor = hit.distance / maxDistance;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }
    }

    private Vector3 input;
    //verticalMovement = acceleration, horizontalMovement = rotation
    public void MoveCar(float verticalMovement, float horizontalMovement)
    {
        // Lerp or Linear Interpolation, is a mathematical function in Unity that returns a value between two others points
        input = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, verticalMovement * Vertical), Horizontal);
        input = transform.TransformDirection(input);
        //tells us where to turn our car
        transform.position += input;
        // horizontalMovement * 90 = % of when to turn 90 degrees
        transform.eulerAngles += new Vector3(0, (horizontalMovement*90)*Horizontal,0);
    }
}
