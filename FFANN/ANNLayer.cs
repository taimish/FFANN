using System;
using System.Collections.Generic;
using System.Text;

namespace FFANN
{
    // CLASS OF A LAYER OF THE NETWORK
    class ANNLayer
    {
        ////////// PUBLIC MEMBERS

        ////////// PRIVATE MEMBERS
        
        private float[][] coeffDeltaSums;                   // Total delta sums for coeffs change after passed lessons
        private float[] neuronDeltas;                       // Deltas of the neurons
        private float[] biasDeltaSums;                      // Total delta sums for biases change after passed lessons

        ////////// PROTECTED MEMBERS

        protected float[][] coeffs;                         // Coeffs of neurons of the layer - 2D ARRAY
        protected float[] biases;                           // Biases of neurons of the layer - 1D ARRAY
        protected AFTypeBase AFType;                        // Type of activation function of the layer

        ////////// PUBLIC METHODS

        /// <summary>
        /// Return number of neurons in the layer.
        /// </summary>
        /// <returns>Layer's number of neurons.</returns>
        public int SayNeuronsNum()
        {
            return neuronDeltas.Length;
        }


        /// <summary>
        /// Returns layer's activation function AFTypeBase successor.
        /// </summary>
        /// <returns>Layer's AFTypeBase successor.</returns>
        public AFTypeBase SayAFType()
        {
            return AFType;
        }


        /// <summary>
        /// Returns number of inputs of the layer.
        /// </summary>
        /// <returns>Number of layer's inputs.</returns>
        public int SayInputsNum()
        {
            return coeffs[0].Length;
        }


        /// <summary>
        /// Prints layer parameters (coefficients and biases) using PrintClass
        /// </summary>
        public void ShowParams(bool showDetails)
        {
            PrintClass.PrintLine("  Neurons number: " + neuronDeltas.Length);
            PrintClass.PrintLine("  Inputs number: " + coeffs[0].Length);
            PrintClass.PrintLine("  Activation function type: " + AFType.ToString().Remove(AFType.ToString().IndexOf("AFType")));

            if (showDetails)
            {
                string tmpString;
                for (int i = 0; i < neuronDeltas.Length; i++)
                {
                    PrintClass.PrintLine("\n    - Neuron " + i.ToString() + ": bias " + biases[i] + ", coefficients :");
                    tmpString = "      ";
                    for (int j = 0; j < coeffs[i].Length; j++)
                    {
                        tmpString += coeffs[i][j].ToString() + "  ";
                    }
                    PrintClass.PrintLine(tmpString);
                }
            }
        }


        /// <summary>
        /// Sets new layer's AFTypeBase successor for activation function realization.
        /// </summary>
        /// <param name="newAFType">New AFTypeBase successor.</param>
        public void SetAFType(AFTypeBase newAFType)
        {
            // IF NEW AFTYPE IS NOT NOCHANGE - SAVE IT
            if (newAFType.ToString() != AFTypeNoChange.name)
            {
                AFType = newAFType;
            }
        }


        /// <summary>
        /// Sets layer's neurons number equal to inputed value, or return error if it is non-positive.
        /// </summary>
        /// <param name="newNeuronNum">New layer's neurons number.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SetNeuronsNum(int newNeuronNum)
        {
            if (newNeuronNum <= 0)
            {
                return "\n-> Error chenging layer's neurons number - new number has non-positive value (" + newNeuronNum.ToString() + ").";
            }

            Array.Resize(ref neuronDeltas, newNeuronNum);
            Array.Resize(ref coeffs, newNeuronNum);
            Array.Resize(ref coeffDeltaSums, newNeuronNum);
            Array.Resize(ref biases, newNeuronNum);
            Array.Resize(ref biasDeltaSums, newNeuronNum);

            return "";
        }


        /// <summary>
        /// Sets layer's inputs number equal to inputed value, or return error if it is non-positive.
        /// </summary>
        /// <param name="newInputNum">New layer's inputs number.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SetInputsNum(int newInputNum)
        {
            if (newInputNum <= 0)
            {
                return "\n-> Error changing layer inputs number - new number has non-positive value (" + newInputNum.ToString() + ").";
            }

            // RESIZING SECOND ORDER ARRAYS
            for (int neuronInd = 0; neuronInd < neuronDeltas.Length; neuronInd++)
            {
                Array.Resize(ref coeffs[neuronInd], newInputNum);
                Array.Resize(ref coeffDeltaSums[neuronInd], newInputNum);
            }

            return "";
        }


        /// <summary>
        /// Retes all delta sums of the layer, setting them to 0.
        /// </summary>
        public void ResetLearning()
        {
            for (int i = 0; i < neuronDeltas.Length; i++)
            {
                biasDeltaSums[i] = 0;
                for (int j = 0; j < coeffDeltaSums[i].Length; j++)
                {
                    coeffDeltaSums[i][j] = 0;
                }
            }
        }


        /// <summary>
        /// Initializes network layers' coefficients and biases using inputed lower and upper boundaries, scaling them is needed, setting alpha.
        /// </summary>
        /// <param name="lowerBoundaries">Lower boundaries for randomization of coefficients and biases, default <i>null</i> leads to 0.</param>
        /// <param name="upperBoundaries">Upper boundaries for randomization of coefficients and biases, default <i>null</i> leads to 0.5.</param>
        /// <param name="scale">Set to <i>true</i> for scaling coefficients by inputs number, default is <i>false</i>.</param>
        /// <param name="newAlpha">New alpha valuer for activation function calculations.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string Initialize(float lowerBoundary, float upperBoundary, float scale = 0)
        {
            // CHECKING INPUT PARAMETERS TO BE VALID
            if (upperBoundary < lowerBoundary)
                return "\n-> Error initializing a layer - upper boundary (" + upperBoundary.ToString() + ") is lower, than lower boundary (" + lowerBoundary.ToString() + ").";

            if (scale <= 0)
                return "\n-> Error initializing a layer - scale (" + scale.ToString() + ") is not positive.";

            var randomizer = new Random();

            // GOING THROUGH ALL NEURONS...
            for (int neuronInd = 0; neuronInd < neuronDeltas.Length; neuronInd++)
            {
                // INITIALIZING WITH ZEROS
                biases[neuronInd] = 0;
                //biasDeltas[i] = 0;
                neuronDeltas[neuronInd] = 0;

                // GOING THROUGH ALL INPUTS OF THE NEURON...
                for (int inputInd = 0; inputInd < coeffs[0].Length; inputInd++)
                {
                    // INITIALIZING WITH ZEROS
                    //coeffDeltas[i][j] = 0;
                    coeffDeltaSums[neuronInd][inputInd] = 0;

                    // INITIALIZING COEFFS WITH RANDOM NUMBER BETWEEN BOUNDARIES
                    float randomNumber = (float)randomizer.NextDouble() * (upperBoundary - lowerBoundary) + lowerBoundary;

                    // SCALING COEFFS IF INPUTED SCALE IS GREATER THEN ZERO
                    randomNumber /= scale;

                    coeffs[neuronInd][inputInd] = randomNumber;
                }
            }

            return "";
        }


        /// <summary>
        /// Saves layer parameters to the given file stream (StreamWriter).
        /// </summary>
        /// <param name="fileForWriting">File stream for saving parameters (StreamWriter).</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SaveToFile(System.IO.StreamWriter fileForWriting)
        {
            try
            {
                // SAVING GENERAL LAYER INFO
                fileForWriting.WriteLine(AFType.Index());
                fileForWriting.WriteLine(neuronDeltas.Length.ToString());
                fileForWriting.WriteLine(coeffs[0].Length.ToString());

                // SAVING BIASES
                string tmpString = "";
                for (int i = 0; i < biases.Length; i++)
                {
                    tmpString += biases[i].ToString() + " ";
                }
                fileForWriting.WriteLine(tmpString);

                // SAVING COEFFS
                for (int i = 0; i < neuronDeltas.Length; i++)
                {
                    tmpString = "";
                    for (int j = 0; j < coeffs[i].Length; j++)
                    {
                        tmpString += coeffs[i][j] + " ";
                    }
                    fileForWriting.WriteLine(tmpString);
                }
            }
            catch (Exception e)
            {
                return "\n-> Error saving layer info to a file. Exeption raised with message: " + e.Message;
            }
            return "";
        }


        /// <summary>
        /// Loads layer parameters from the given file stream (StreamReader).
        /// </summary>
        /// <param name="fileForReading">File stream for reading parameters (StreamReader).</param>
        /// <returns>Empty string on success or error message.</returns>
        public string LoadFromFile(System.IO.StreamReader fileForReading)
        {
            try
            {
                // READING GENERAL LAYER INFO
                string tmpString = "";
                tmpString = fileForReading.ReadLine(); // Reading AFType
                switch (int.Parse(tmpString))
                {
                    case 1:
                        AFType = new AFTypeStep();
                        break;
                    case 2:
                        AFType = new AFTypeLinear();
                        break;
                    case 3:
                        AFType = new AFTypeSigmoid();
                        break;
                    case 4:
                        AFType = new AFTypeTanh();
                        break;
                    case 5:
                        AFType = new AFTypeSoftMax();
                        break;
                    case 6:
                        AFType = new AFTypeRectifiedLinear();
                        break;
                    default:
                        return "Error loading a layer info from file. Unknown AFType: " + tmpString + " .";
                }

                tmpString = fileForReading.ReadLine(); // Reading neurons number
                if (int.Parse(tmpString) <= 0)
                    return "Error loading a layer info from file. Not positive neurons number: " + tmpString + " .";
                else
                    SetNeuronsNum(int.Parse(tmpString));

                tmpString = fileForReading.ReadLine(); // Reading inputs number
                if (int.Parse(tmpString) <= 0)
                    return "Error loading a layer info from file. Not positive inputs number: " + tmpString + " .";
                else
                    SetInputsNum(int.Parse(tmpString));

                // INITIALIZING LAYER PARAMETERS FOR DELTAS INIT - NO NEED IN MANUAL INIT AFTER BIASES AND COEFFICIENTS READ
                Initialize(0, 1, 0);

                // READING BIASES
                tmpString = fileForReading.ReadLine(); // Reading all biases in a line, devided by " "
                for (int i = 0; i < neuronDeltas.Length; i++)
                {
                    biases[i] = float.Parse(tmpString.Remove(tmpString.IndexOf(" ")));
                    tmpString = tmpString.Remove(0, tmpString.IndexOf(" ") + 1);
                }

                // READING COEFFICIENTS
                for (int i = 0; i < neuronDeltas.Length; i++)
                {
                    tmpString = fileForReading.ReadLine(); // Reading all i-th neuron coefficients in a line, devided by " "
                    for (int j = 0; j < coeffs[i].Length; j++)
                    {
                        coeffs[i][j] = float.Parse(tmpString.Remove(tmpString.IndexOf(" ")));
                        tmpString = tmpString.Remove(0, tmpString.IndexOf(" ") + 1);
                    }
                }
            }
            catch (Exception e)
            {
                return "Error loading a layer info from file. Exeption raised with message: " + e.Message;
            }
            return "";
        }


        /// <summary>
        /// Calculates outputs of the layer regarding inputs.
        /// </summary>
        /// <param name="inputVector">Vector of inputs.</param>
        /// <param name="resultVector">Vector of resulting outputs of the layer's neurons.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string React(float[] inputVector, ref float[] resultVector)
        {
            // CHECKING INPUT VECTOR LENGTH TO MATCH THE INPUT NUMBER
            if (inputVector.Length != coeffs[0].Length)
            {
                return "\n-> Error calculating layer reaction on input vector with length " + inputVector.Length.ToString() + " instead of " + coeffs[0].Length.ToString() + " .";
            }

            // CHECKING RESULT VECTOR LENGTH TO MATCH THE NEURON NUMBER
            if (resultVector.Length != neuronDeltas.Length)
            {
                return "\n-> Error calculating layer reaction with neurons number " + neuronDeltas.Length.ToString() + " to vector wiht length " + resultVector.Length.ToString() + " .";
            }

            // CALCULATING REACTIONS OF EACH NEURON
            float softMaxSum = 0;
            for (int neuronInd = 0; neuronInd < neuronDeltas.Length; neuronInd++)
            {
                // CALCULATING SUMM OF WEIGHTENED INPUTS FOR CURRENT NEURON
                float sum = biases[neuronInd];
                for (int inputInd = 0; inputInd < coeffs[neuronInd].Length; inputInd++)
                {
                    sum += inputVector[inputInd] * coeffs[neuronInd][inputInd];
                }

                resultVector[neuronInd] = AFType.CalcActivation(sum, ANNetwork.SayNetworkAlpha());
                softMaxSum += resultVector[neuronInd];
            }

            // FOR SOFTMAX FUNCTION ADDITIONAL OPERATION
            if (AFType.ToString() == AFTypeSoftMax.name)
            {
                for (int neuronInd = 0; neuronInd < resultVector.Length; neuronInd++)
                {
                    resultVector[neuronInd] /= softMaxSum;
                }
            }

            return "";
        }


        /// <summary>
        /// Adds values of all layer neuron coefficients squares to linked variable for L2 regularization.
        /// </summary>
        /// <param name="coeffsSum">Link to a variable to accumulate coefficients square sum.</param>
        public void CalcCoeffsSum(ref float coeffsSum)
        {
            for (int neuronInd = 0; neuronInd < neuronDeltas.Length; neuronInd++)
            {
                for (int inputInd = 0; inputInd < coeffs[neuronInd].Length; inputInd++)
                {
                    coeffsSum += coeffs[neuronInd][inputInd] * coeffs[neuronInd][inputInd];
                }
            }
        }


        /// <summary>
        /// Performing a gradient decent back propogation through the layer.
        /// </summary>
        /// <param name="layerInputs">Vector of inputs (outputs of previous layers or inputs of the example).</param>
        /// <param name="layerOutputs">Vector of outputs of the layer, already calculated previously.</param>
        /// <param name="inDerivatives">Vector of incomming from next (in network architecture) layer or from cost function derivatives for their back propogation.</param>
        /// <param name="outDerivatives">Vector of outcomming derivatives for previous (in network architecture) layer.</param>
        /// <param name="lSpeed">Learning speed for deltas calculations.</param>
        /// <param name="examplesNum">Number of examples in a batch for delta sums scaling.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string BPropogate(float[] layerInputs,  float[] layerOutputs, float[] inDerivatives, ref float[] outDerivatives, float lSpeed, int examplesNum)
        {
            // CHECKING INPUTED VECTOR'S LENGTH TO MATCH LAYERS NEURON COEFFICIENTS NUMBER
            if (layerInputs.Length != coeffs[0].Length)
                return "Inputs vector length (" + layerInputs.Length.ToString() + ") don't match layer neurons' coefficients number (" + coeffs[0].Length.ToString() + ").";

            if (layerOutputs.Length != neuronDeltas.Length)
                return "Outputs vector length (" + layerOutputs.Length.ToString() + ") don't match layers neuron number (" + neuronDeltas.Length.ToString() + ").";

            if (inDerivatives.Length != neuronDeltas.Length)
                return "Derivatives vector length (" + inDerivatives.Length.ToString() + ") don't match layers neuron number (" + neuronDeltas.Length.ToString() + ").";

            // PREPARING VECTOR OF DERIVATIVES FOR NEXT (ACTUALY PREVIOUS IN NETWORK ARCHITECTURE) LAYER - NUMBER OF SUMS = NUMBER OF INPUTS (NEURONS IN PREVIOUS LAYER)
            float[] nextDerivatives = new float[coeffs[0].Length];
            for (int inputInd = 0; inputInd < nextDerivatives.Length; inputInd++)
            {
                nextDerivatives[inputInd] = 0;
            }

            // CALCULATING DELTAS OF CURRENT LAYER NEURONS AND DERIVATIVES FOR NEXT LAYER
            for (int neuronInd = 0; neuronInd < neuronDeltas.Length; neuronInd++)
            {
                // CALCULATING ACTIVATION FUNCTION DERIVATIVE USING THIS LAYER OUTPUT AND ALPHA
                try
                {
                    neuronDeltas[neuronInd] = AFType.CalcDerivative(layerOutputs[neuronInd], ANNetwork.SayNetworkAlpha()) * inDerivatives[neuronInd];
                }
                catch (Exception e)
                {
                    return "Error calculating activation function for neuron " + neuronInd.ToString() + " . " + e.Message;
                }

                // CALCULATING EACH COEFFICIENT OF CURRENT NEURON DELTA TO INCREASE DELTA SUM
                for (int inputInd = 0; inputInd < coeffDeltaSums[neuronInd].Length; inputInd++)
                {
                    // SINGLE COEFFICIENT DELTA INCREASE IS CALCULATED WITH NEURON DELTA, INPUTED TO THIS COEFFICIENT VALUE, LEARNING SPEED AND NUMBER OF EXAMPLES TO GET MEAN
                    coeffDeltaSums[neuronInd][inputInd] += neuronDeltas[neuronInd] * layerInputs[inputInd] * lSpeed / examplesNum;

                    // INCRESING DERIVATIVE SUM OF CURRENT INPUT NEURON OF NEXT LAYER
                    nextDerivatives[inputInd] += coeffs[neuronInd][inputInd] * neuronDeltas[neuronInd];
                }

                // BIAS DELTA INCREASE IS CALCULATED WITH NEURON DELTA, LEARNING SPEED AND NUMBER OF EXAMPLES TO GET MEAN
                biasDeltaSums[neuronInd] += neuronDeltas[neuronInd] * lSpeed / examplesNum;
            }

            outDerivatives = nextDerivatives;

            return "";
        }


        /// <summary>
        /// Applies delta sums, modifying coefficients and biases of the layer as a result of back propogation.
        /// </summary>
        /// <param name="showDetails">Set to <i>true</i> - prints delta sums using PrintClass, set to <i>false</i> - no printing.</param>
        /// <param name="batchSize">The number of examples of current teaching for L2 regularization realization.</param>
        public void ApplyDeltaSums(bool showDetails, int batchSize)
        {
            // GOING THROUGH ALL NEURONS...
            for (int neuronInd = 0; neuronInd < neuronDeltas.Length; neuronInd++)
            {
                // CHANGING BIASES
                if (showDetails)
                {
                    PrintClass.Print("\n   - Neuron  " + neuronInd.ToString() + "  bias delta sum: " + biasDeltaSums[neuronInd].ToString() + "\n     coeffs delta sums: ");
                }

                biases[neuronInd] -= biasDeltaSums[neuronInd];
                biasDeltaSums[neuronInd] = 0;
                neuronDeltas[neuronInd] = 0;

                // GOING THROUGH ALL COEFFS OF THE NEURON...
                for (int coeffInd = 0; coeffInd < coeffs[neuronInd].Length; coeffInd++)
                {
                    // CHANGING COEFFS APPLYING L2 REGULARIZATION
                    if (showDetails)
                    {
                        PrintClass.Print(coeffDeltaSums[neuronInd][coeffInd].ToString() + " ");
                    }
                    coeffs[neuronInd][coeffInd] = (1 - ANNetwork.SayNetworkLyambda() * ANNetwork.SayNetworkLearningSpeed() / batchSize) * coeffs[neuronInd][coeffInd] - coeffDeltaSums[neuronInd][coeffInd];
                    coeffDeltaSums[neuronInd][coeffInd] = 0;
                }
            }
            if (showDetails)
            {
                Console.Write("\n");
            }
        }


        ////////// PRIVATE METHODS


        ////////// PROTECTED METHODS


        ////////// CONSTRUCTORS
        /// <summary>
        /// Constractor of a network layer.
        /// </summary>
        /// <param name="neuronNum">Number of neurons in new layer.</param>
        /// <param name="inputNum">Number of input in new layer.</param>
        /// <param name="AFType">Activation function AFTypeBase successor for new layer.</param>
        /// <param name="lowerBoundary">Lower boundary for coefficient initialization, <i>default</i> is 0.</param>
        /// <param name="upperBoundary">Lower boundary for coefficient initialization, <i>default</i> is 0.5.</param>
        /// <param name="scale">A scale for coefficients while initialization, <i>default</i> is 0 for no scaling.</param>
        public ANNLayer(int neuronNum, int inputNum, AFTypeBase AFType, float lowerBoundary = 0, float upperBoundary = 0.5f, float scale = 1)
        {
            SetNeuronsNum(neuronNum);
            SetInputsNum(inputNum);
            SetAFType(AFType);
            string result = Initialize(lowerBoundary, upperBoundary, scale);
            if (result.Length > 0)
                throw new NotImplementedException(result);
        }
    }
}


/*
ANNetwork file format for layer info:

...
<AFType>
<alpha>
<neuron number (n)>
<input number (m)>
<neuron 0 bias> <neuron 1 bias> ... <neuron n bias>
<neuron 0 input 0 coeff> <neuron 0 input 1 coeff> ... <neuron 0 input m coeff>
<neuron 1 input 0 coeff> <neuron 1 input 1 coeff> ... <neuron 1 input m coeff>
...
<neuron n input 0 coeff> <neuron n input 1 coeff> ... <neuron n input m coeff>


*/
