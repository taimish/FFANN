# FFANN C#
Feed-forward artificial neural nerwork

C# classes for work with feed-forward artificial neural networks, trained using the gradient descent.
Next network parameters can be customized:
- number of inputs of the network;
- number of layers;
- number of neurons in each layer;
- type of activation function for every layer neurons;
- type of cost function of the network;
- learning speed;
- activation function softness coefficient;
- L2 regularization coefficient.

There are 6 àctivation function types currently supported:
- step;
- linear;
- sigmoid;
- hyperbolic tangent;
- softmax;
- rectified linear.

And 3 cost function types currently supported:
- quadratic;
- cross-entropy;
- log-likelihood.

Network is represented by next classes:
- activation function abstract class and subclasses for each type;
- cost function abstract class and subclasses for each type;
- single layer class;
- network class, combining layers classes and different function classes.
Network is a singleton class, initialized or loaded from file by a static method.
The class interface supports next basic functions: initialization, getting and setting main parameters, loading from and saving to files, training, cost and error estimation.

Also network class uses a class, representing a bank of examples, where each example exists as an object of simple single example class.
The class of examples supports functions for loading from files, shuffling, dividing into training and testing banks, dividing into batche for training.

Project also contains support class for console user interaction, for printing network and examples info and for parsing strings.

Project operating capability was check on a simple task of recognition of 5x3 bool matrix with digits (examples included) and on a logistic statistics with 435 inputs, 53 outputs and 10k examples (not included).