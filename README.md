# Self-Driving-Car

![Screenshot 2022-11-01 134201](https://user-images.githubusercontent.com/78549037/199303430-7607bf18-4969-400e-b7cf-1b816185a4da.jpg)
*Figure 1: An ANN is being trained in a population*

This is a self driving car that uses a gentic algorithm to progressivley train a population of ANNs towards a better fitness. 
This project was made in Unity 2021.3.11f1. This self driving car features three main C# scripts that run the program.

## Car Controller
 The car controller allows the user to input certain parameters for methods that contribute to the structure of the ANN and fitnesses of these ANNs.
 
### Normalization
**Max Distance:** input max distance used in the normalization function for sensors that gather information for car.

### Fitness Function
**Max Fitness:** when to stop the program when a contingency in fitness is met </br>
**Min Fitness:** if agent's fitness is tool low so the program can move onto the next agent </br>
**Cut-off time:** if agent's fitness is not equal to min fitness by cut off time, then move to next agent </br>
**Speed Multiplier:** how important is speed to fitness function? </br>
**Distance Multiplier:** how important is distance to fitness function? </br>
**Sensor Multiplier:** how important is driving on center of road important? </br>

### Network Structure
**Layers:** Number of hidden layers </br>
**Nodes:** Number of nodes per layer </br>
note: be careful not to overfit too much

## Neural Network
The neural network script creates the structure of the ANN including the layers (input, hidden, output), weights, and biases. The neural network script makes
sure to randomize the values of the biases and weights when creating the inital population. The neural network script also has activation functions for each node. The neural network has 4 input nodes (3 sensors that collect distances from car to walls, and the speed of the car).
 
## Genetic Manager
The genetic manager allows the user to customize the size of a population for each generation, the mutuation rate, and how crossover will function in each generation's population.

### Pooling Settings
**Population Size:** How big do you want the population to be for each gen? </br>
**Mutation Rate:** How much change do you want for each child in their weights and biases? </br>

## Crossover Controls
**Best Agent Selection:** What agents are suitable for crossover? </br> 
**Worst Agent Selection:**: Which agents should we cull from the population? </br>
**Number to crossover:** How many agents should with crossover with the best performing agents (parents)


## Plug Ins
- Math.Net </br>
- ProBuilder
