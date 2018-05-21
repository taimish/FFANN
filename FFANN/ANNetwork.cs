using System;
using System.Collections.Generic;
using System.Text;

namespace FFANN
{
    // CLASS OF THE NETWORK ITSELF
    class ANNetwork
    {
        /// PUBLIC MEMBERS
        public const int classesVersion = 1;

        /// PRIVATE MEMBERS

        /// PROTECTED MEMBERS
        protected List<ANNLayer> layers;                    // Layers of the network
        protected CFTypeBase CFType;                        // Type of cost function of the network
        protected float alpha;                              // Alpha coefficient for activation function calculation
        protected float lyambda;                            // Lyabda coefficient for L2 regularization
        protected float learningSpeed;                      // Learing speed for back propogation
        protected float[][] layersOutputs;                  // Outputs of layers of the network
        protected int lessonsProceeded;                     // Number of examples already used to teach this network and stored in delta sums
        protected static ANNetwork singleton = null;        // Singleton object
        protected string fileExtension;                     // Extension of files for saving and loading network parameters
        protected string fileDirectory;                     // Directory for files for saving and loading network parameters
        public Examples examples;                           // Examples network work with

        ////////// PUBLIC METHODS

        /// <summary>
        /// Initializes a singleton with a new ANNetwork class unit, constructed with inputed values.
        /// </summary>
        /// <param name="neuronNums">Array with numbers of neurons of the new network.</param>
        /// <param name="newInputNum">Number of network's inputs (for first layer).</param>
        /// <param name="newLearningSpeed">Learning speed of new network, <i>default</i> is 0.01.</param>
        /// <param name="newAlpha">Alpha of new network, <i>default</i> is 1.</param>
        /// <param name="newLyambda">Lyambda of new network, <i>default</i> is 0.</param>
        /// <param name="newCFType">Cost function type of new network, <i>default</i> will be set depending on the activation function of last layer.</param>
        /// <param name="newAFTypes">Activation function types for layers of the network, <i>default</i> is AFTypeSigmoid class.</param>
        /// <param name="lowerBoundaries">Lower boundaries for the network's coefficients initialization, <i>default</i> is 0.</param>
        /// <param name="upperBoundaries">Upper boundaries for the network's coefficients initialization, <i>default</i> is 0.5.</param>
        /// <param name="initScale">Set to <i>true</i> - for new network's coefficients initialization scale by quare root of inputs number, set to <i>false</i> - for no scaling.</param>
        /// <returns>Initialized ANNetwork class unit.</returns>
        public static ANNetwork NewNetwork(int[] neuronNums, int newInputNum, float newLearningSpeed = 0.01f, float newAlpha = 1, float newLyambda = 0, CFTypeBase newCFType = null, AFTypeBase[] newAFTypes = null, bool initScale = false, float[] lowerBoundaries = null, float[] upperBoundaries = null)
        {
            // CREATING NEW NETWORK
            try
            {
                singleton = new ANNetwork(neuronNums, newInputNum, newLearningSpeed, newAlpha, newLyambda, newCFType, newAFTypes, lowerBoundaries, upperBoundaries, initScale);
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }

            return singleton;
        }


        /// <summary>
        /// Loads network's parameters from a file. Can be used independently from network being previously initialized.
        /// </summary>
        /// <param name="fileNameArgument"></param>
        /// <param name="useDirAndExtension"></param>
        /// <returns></returns>
        public static ANNetwork LoadNetwork(string fileNameArgument, bool useDirAndExtension = true)
        {
            if (singleton == null)
                singleton = NewNetwork(new int[] { 1 }, 1);

            singleton.LoadFromFile(fileNameArgument, useDirAndExtension);

            return singleton;
        }
        

        /// <summary>
        /// Returns alpha of the network if it exists.
        /// </summary>
        /// <returns>Alpha of nerwork, if it exists, or 0 for error.</returns>
        public static float SayNetworkAlpha()
        {
            if (singleton == null)
                return 0;
            else
                return singleton.SayAlpha();
        }


        /// <summary>
        /// Returns lyambda of the network if it exists.
        /// </summary>
        /// <returns>Lyambda of nerwork, if it exists, or -1 for error.</returns>
        public static float SayNetworkLyambda()
        {
            if (singleton == null)
                return -1;
            else
                return singleton.SayLyambda();
        }


        /// <summary>
        /// Returns learning speed of the network if it exists.
        /// </summary>
        /// <returns>Learning speed of nerwork, if it exists, or 0 for error.</returns>
        public static float SayNetworkLearningSpeed()
        {
            if (singleton == null)
                return 0;
            else
                return singleton.SayLearningSpeed();
        }


        /// <summary>
        /// Returns current number of layers in the network.
        /// </summary>
        /// <returns>Number of layers.</returns>
        public int SayLayerNum()
        {
            return layers.Count;
        }


        /// <summary>
        /// Returns current number of network's inputs (it's number of inputs of nerwork's first layer).
        /// </summary>
        /// <returns>Number of inputs.</returns>
        public int SayInputNum()
        {
            return layers[0].SayInputsNum();
        }

                
        /// <summary>
        /// Returns number of neurons in layer with inputed index or -1, if index exceeds layers indexes.
        /// </summary>
        /// <param name="layerInd">Index of layer which neurons number should be returned.</param>
        /// <returns>Number of neurons of layer with index <i>layerInd</i> or -1.</returns>
        public int SayNeuronNumInLayer(int layerInd)
        {
            // CHECKING TARGET LAYER INDEX TO BE IN BOUNDARIES
            if ((layerInd >= 0) && (layerInd < SayLayerNum()))
            {
                return layers[layerInd].SayNeuronsNum();
            }
            else
                throw new NotImplementedException("Illegal attempt to get neuron number of layer with index " + layerInd.ToString() + " .");
        }


        /// <summary>
        /// Returns current number of network's outputs (it's number of neurons in the last layer of the network).
        /// </summary>
        /// <returns>Number of network's outputs.</returns>
        public int SayOutputNum()
        {
            return layers.Count == 0 ? 0 : layers[layers.Count - 1].SayNeuronsNum();
        }


        /// <summary>
        /// Returns current alpha of the network.
        /// </summary>
        /// <returns>Network's alpha.</returns>
        public float SayAlpha()
        {
            return alpha;
        }


        /// <summary>
        /// Returns L2 regularization lyabda of the network.
        /// </summary>
        /// <returns>Lyambda for coefficients decay.</returns>
        public float SayLyambda()
        {
            return lyambda;
        }


        /// <summary>
        /// Returns current learning speed of the network.
        /// </summary>
        /// <returns>Network's learning speed.</returns>
        public float SayLearningSpeed()
        {
            return learningSpeed;
        }


        /// <summary>
        /// Returns current file extension for saving or loading nerworks.
        /// </summary>
        /// <returns>Networks files extension.</returns>
        public string SayFileextension()
        {
            return fileExtension;
        }


        /// <summary>
        /// Returns network's current directory name for networks files
        /// </summary>
        /// <returns>Network's directory for files.</returns>
        public string SayFileDirectory()
        {
            return fileDirectory;
        }


        /// <summary>
        /// Prints parameters of network using PrintClass, including general parameters, coefficients and biases.
        /// </summary>
        public void ShowParams(bool showDetails)
        {
            PrintClass.PrintLine("\nANNetwork class with classes version: " + classesVersion.ToString());
            PrintClass.PrintLine("Cost function type: " + CFType.ToString().Remove(CFType.ToString().IndexOf("CFType")));
            PrintClass.PrintLine("Alpha: " + alpha.ToString());
            PrintClass.PrintLine("Lyambda: " + lyambda.ToString());
            PrintClass.PrintLine("Learning speed: " + learningSpeed.ToString());
            PrintClass.PrintLine("Layers number: " + layers.Count.ToString());

            for (int i = 0; i < layers.Count; i++)
            {
                PrintClass.PrintLine("\n  - Layer with index " + i.ToString() + ":");
                layers[i].ShowParams(showDetails);
            }

        }


        /// <summary>
        /// Sets number of network's inputs (inputs of the network's first layer).
        /// </summary>
        /// <param name="newInputsNum">New number of network's inputs.</param>
        /// <returns>Empty string or error message.</returns>
        public string SetInputsNum(int newInputsNum)
        {
            if (layers.Count == 0)
                return "\n-> Error setting unputs number - no layers initialized";

            layers[0].SetInputsNum(newInputsNum);
            return "";
        }


        /// <summary>
        /// Sets neurons number or certain layer equal to inputed value
        /// </summary>
        /// <param name="layerInd"></param>
        /// <param name="newNeuronNum"></param>
        /// <returns>Empty string on success or error message.</returns>
        public string SetNeuronNumInLayer(int layerInd, int newNeuronNum)
        {
            // CHECKING TARGET LAYER INDEX TO BE IN BOUNDARIES AND NEW NEURONS NUMBER TO BE POSITIVE
            if ((layerInd < 0) || (layerInd >= SayLayerNum()))
                return "\n-> Error changing number of neurons in a layer - layer index (" + layerInd.ToString() + ") is out of boundaries (0..." + (SayLayerNum() - 1).ToString() + ").";

            if (newNeuronNum <= 0)
                return "\n-> Error changing nuber of neurons in layer " + layerInd.ToString() + " - new neurons number isn't positive (" + newNeuronNum.ToString() + ").";

            // CHANGING LAYER'S INPUT NUM
            layers[layerInd].SetNeuronsNum(newNeuronNum);

            // CHANGING NEXT LAYER NUMBER OF INPUTS, IF CURRENT LAYER IS NOT LAST
            if (layerInd < layers.Count - 1)
            {
                layers[layerInd + 1].SetInputsNum(newNeuronNum);
            }

            return "";
        }


        /// <summary>
        /// Sets the network layers' activation functions type by specifining certain classes.
        /// </summary>
        /// <param name="newAFTypes">Array with new AFTypeBase class successors for layers.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SetAFTypes(AFTypeBase[] newAFTypes)
        {
            // CHECKING LENGTH OF NEW AFTYPES ARRAY TO MATCH THE NUMBER OF LAYERS
            if (newAFTypes.Length != layers.Count)
            {
                return "-> Error setting new AFTypes of layers with array length " + newAFTypes.Length.ToString() + " while actual layer number is " + layers.Count.ToString() + " .";
            }

            for (int i = 0; i < newAFTypes.Length; i++)
            {
                layers[i].SetAFType(newAFTypes[i]);
            }
            return "";
        }


        /// <summary>
        /// Sets the network's cost function class.
        /// </summary>
        /// <param name="newCFType">New network's cost function CFTypeBase successor.</param>
        public void SetCFType(CFTypeBase newCFType)
        {
            CFType = newCFType;
        }


        /// <summary>
        /// Sets network's alpha equal to inputed value, if it is positive, or returns an error.
        /// </summary>
        /// <param name="newAlpha">New network's alpha value.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SetAlpha(float newAlpha)
        {
            // CHECKING IF NEW ALPHA IS NOT A ZERO
            if (newAlpha <= 0)
                return "\n-> Error changing alpha of the network - non-positive number (" + newAlpha.ToString() + ").";

                alpha = newAlpha;
            return "";
        }


        /// <summary>
        /// Sets new value of network lyambda for L2 regularization, if it is not negative, or return an error.
        /// </summary>
        /// <param name="newLyambda">New valuer for lyamda, default - <i>0</i>.</param>
        /// <returns>Empty string or error message.</returns>
        public string SetLyambda(float newLyambda = 0)
        {
            // CHECKING IF NEW ALPHA IS NOT A ZERO
            if (newLyambda < 0)
                return "\n-> Error changing lyambda of the network - negative number (" + newLyambda.ToString() + ").";

            lyambda = newLyambda;
            return "";
        }
        

        /// <summary>
        /// Sets network's learning speed equal to inputed value, if it is positive, or returns an error.
        /// </summary>
        /// <param name="newLearningSpeed">New value for learning speed.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SetLearningSpeed(float newLearningSpeed)
        {
            // CHECKING IF NEW ALPHA IS NOT A ZERO
            if (newLearningSpeed <= 0)
                return "\n-> Error changing learning speed of network - nonpositive number (" + newLearningSpeed.ToString() + ").";

            learningSpeed = newLearningSpeed;
            return "";
        }


        /// <summary>
        /// Sets new network's file extension for loading and saving.
        /// </summary>
        /// <param name="newextension">New network's files extension.</param>
        public void SetFileextension(string newextension)
        {
            if (newextension.Length > 0)
                fileExtension = newextension;
        }


        /// <summary>
        /// Sets new directory name for network files load and save (in current program directory).
        /// </summary>
        /// <param name="newDirectory">New name for saving and loading directory.</param>
        public void SetFileDirectory(string newDirectory)
        {
            fileDirectory = newDirectory;
        }


        /// <summary>
        /// Not recomended.
        /// </summary>
        /// <param name="newExamples"></param>
        /// <returns></returns>
        public string SetExamples(ref Examples newExamples)
        {
            if (newExamples == null)
                return "\n-> Error setting new examples bank of network - new bank link is empty (null).";

            examples = newExamples;

            return "";
        }


        /// <summary>
        /// Resets number of proceeded lessons in network's layers.
        /// </summary>
        public void ResetLessonsNum()
        {
            lessonsProceeded = 0;
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].ResetLearning();
            }
        }


        /// <summary>
        /// Initializes existing network's coefficients and biases using inputed lower and upper boundaries, scaling them is needed, setting alpha.
        /// </summary>
        /// <param name="lowerBoundaries">Lower boundaries for randomization of coefficients and biases, default <i>null</i> leads to 0.</param>
        /// <param name="upperBoundaries">Upper boundaries for randomization of coefficients and biases, default <i>null</i> leads to 0.5.</param>
        /// <param name="scale">Set to <i>true</i> for scaling coefficients by inputs number, default is <i>false</i>.</param>
        /// <param name="newAlpha">New alpha valuer for activation function calculations.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string Initialize(float[] lowerBoundaries = null, float[] upperBoundaries = null, bool scale = false, float newAlpha = 1)
        {
            // CHECKING IF BOUNDARY ARRAYS SIZES ARE EQUAL AND MATCH THE NUMBER OF LAYERS
            if ((lowerBoundaries != null) && (lowerBoundaries.Length != layers.Count))
                return "\n-> Error initializing the network: lower boundaries are specified, but their number don't match number of layers: " + lowerBoundaries.Length.ToString() + " .";

            if ((upperBoundaries != null) && (upperBoundaries.Length != layers.Count))
                return "\n-> Error initializing the network: upper boundaries are specified, but their number don't match number of layers: " + upperBoundaries.Length.ToString() + " .";

            if (((upperBoundaries != null) && (lowerBoundaries == null)) || ((upperBoundaries == null) && (lowerBoundaries != null)))
                return "\n-> Error initializing the network: only one side boundaries are specified.";

            // CHECKING NEW ALPHA TO BE POSITIVE
            if (newAlpha <=0)
                return "\n-> Error initializing the network: alpha is not positive: " + alpha.ToString() + " .";

            // MACKING DEFAULT LOWERBOUNDARIES - 0
            if (lowerBoundaries == null)
            {
                lowerBoundaries = new float[layers.Count];
                for (int i = 0; i < lowerBoundaries.Length; i++)
                {
                    lowerBoundaries[i] = 0;
                }
            }

            // MACKING DEFAULT UPPERBOUNDARIES - 0.5
            if (upperBoundaries == null)
            {
                upperBoundaries = new float[layers.Count];
                for (int i = 0; i < upperBoundaries.Length; i++)
                {
                    upperBoundaries[i] = 0.5f;
                }
            }

            // INITIALIZING LAYERS
            for (int i = 0; i < layers.Count; i++)
            {
                float currScale = 0;

                // CHECKING IF SCALE SHOUND BE USED
                if (scale)
                {
                    // CHECKING IF LAYER IS FIRST OF NOT TO AQUIRE SCALING NUMBER
                    if (i == 0)
                        currScale = (float)Math.Sqrt(layers[0].SayInputsNum());
                    else
                        currScale = (float)Math.Sqrt(layers[i - 1].SayNeuronsNum());
                }

                layers[i].Initialize(lowerBoundaries[i], lowerBoundaries[i], currScale);
            }

            lessonsProceeded = 0;

            return "";
        }


        /// <summary>
        /// Shows list of files with <i>.fileExtension</i> extension in <i>fileDirectory</i> subdirectory of current program placement.
        /// </summary>
        /// <returns>Returns empty string.</returns>
        public string ShowFileList()
        {
            PrintClass.PrintLine("\n-> All networks files in " + fileDirectory + " directory with ." + fileExtension + " extension:");
            string[] fileNames = System.IO.Directory.GetFiles(fileDirectory + "\\", "*." + fileExtension);
            for (int fileInd = 0; fileInd < fileNames.Length; fileInd++)
            {
                fileNames[fileInd] = fileNames[fileInd].Remove(0, fileNames[fileInd].IndexOf('\\') + 1);
                PrintClass.PrintLine("- " + fileNames[fileInd]);
            }

            PrintClass.PrintLine("\n   End of networks file list.");

            return "";
        }


        /// <summary>
        /// Saves current network's parameters to a file with inputed filename, using network default directory name and file extension or not.
        /// </summary>
        /// <param name="fileNameArgument">Argument with file name (without path and extension) or with full file name, including path and extension.</param>
        /// <param name="useDirAndExtension">If set to <i>true</i> - using network's defauls directory and file extension, if set to <i>false</i> - using only inputed file name argument as full file name.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SaveToFile(string fileNameArgument, bool useDirAndExtension = true)
        {
            if (layers.Count == 0)
                return "\n-> Error saving network to a file - the network doesn't contain any layer.";

            // PARSING ARGUMENT
            fileNameArgument = fileNameArgument.Trim();

            if (fileNameArgument.Length == 0)
                return "-> Error saving network to a file - no argument with file name entered.";

            string fileName = "";

            int tmpInt = fileNameArgument.IndexOf(' ');

            if (tmpInt > 0)
                fileName = fileNameArgument.Remove(tmpInt);
            else
                fileName = fileNameArgument;

            // CONSTRUCTING FULL FILE NAME USING INPUTED NAME, NETWORK'S DIRECTORY AND FILE EXTENSION, OR JUST USING INPUTED NAME
            string fullFileName;

            if (useDirAndExtension)
                fullFileName = fileDirectory + "\\" + fileName + "." + fileExtension;
            else
                fullFileName = fileName;
            
            try
            {
                // CREATING DIRECTORY FOR SAVING
                if (useDirAndExtension)
                {
                    if (fileDirectory.Length > 0)
                        System.IO.Directory.CreateDirectory(fileDirectory);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(fileName.Remove(fileName.LastIndexOf('/')));
                }
                
                using (System.IO.StreamWriter fileForWriting = new System.IO.StreamWriter(fullFileName, false))
                {
                    /*
                    ANNetwork file format:

                    ANNetwork
                    <version>
                    <CFType>
                    <alpha>
                    <lyambda>
                    <learningSpeed>
                    <number of layers>
                    layer 1 info...
                    layer 2 info...
                    */

                    // SAVING GENERAL NETWORK PARAMETERS
                    fileForWriting.WriteLine("ANNetwork");
                    fileForWriting.WriteLine(classesVersion.ToString());
                    fileForWriting.WriteLine(CFType.Index().ToString());
                    fileForWriting.WriteLine(alpha.ToString());
                    fileForWriting.WriteLine(lyambda.ToString());
                    fileForWriting.WriteLine(learningSpeed.ToString());
                    fileForWriting.WriteLine(layers.Count.ToString());

                    // SAVING LAYER PARAMETERS ONE BY ONE
                    string tmpString;
                    for (int i = 0; i < layers.Count; i++)
                    {
                        tmpString = layers[i].SaveToFile(fileForWriting);
                        if (tmpString.Length > 0)
                        {

                            return "\n-> Error saving layer " + i.ToString() + ": " + tmpString;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return "\n-> Error saving network parameters to a file. Exception message: " + e.Message;
            }

            return "";
        }


        /// <summary>
        /// Loads parameters to the network from a file with inputed filename, using network default directory name and file extension or not.
        /// </summary>
        /// <param name="fileNameArgument">Argument with file name (without path and extension) or with full file name, including path and extension.</param>
        /// <param name="useDirAndExtension">If set to <i>true</i> - using network's defauls directory and file extension, if set to <i>false</i> - using only inputed file name argument as full file name.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string LoadFromFile(string fileNameArgument, bool useDirAndExtension = true)
        {
            // PARSING ARGUMENT
            fileNameArgument = fileNameArgument.Trim();

            if (fileNameArgument.Length == 0)
                return "-> Error loading network from a file - no argument with file name entered.";

            string fileName = "";

            int tmpInt = fileNameArgument.IndexOf(' ');

            if (tmpInt > 0)
                fileName = fileNameArgument.Remove(tmpInt);
            else
                fileName = fileNameArgument;

            // CONSTRUCTING FULL FILE NAME USING INPUTED NAME, NETWORK'S DIRECTORY AND FILE EXTENSION, OR JUST USING INPUTED NAME
            string fullFileName;

            if (useDirAndExtension)
                fullFileName = fileDirectory + "\\" + fileName + "." + fileExtension;
            else
                fullFileName = fileName;

            try
            {
                using (System.IO.StreamReader fileForReading = new System.IO.StreamReader(fullFileName))
                {
                    /*
                    ANNetwork file format:

                    ANNetwork
                    <version>
                    <CFType>
                    <alpha>
                    <lyambda>
                    <learningSpeed>
                    <number of layers>
                    layer 1 info...
                    layer 2 info...
                    */

                    /// LOADING GENERAL NETWORK PARAMETERS
                    // READING "ANNETWORK" KEYWORD
                    string result = fileForReading.ReadLine();
                    if (result != "ANNetwork")
                    {
                        return "\n-> Error loading network info from file. Unknown format of first string: " + result + " .";
                    }

                    // READING CLASS VERSION
                    result = fileForReading.ReadLine();
                    if (int.Parse(result) != classesVersion)
                    {
                        return "\n-> Error loading network info from file. Wrong version: " + result + " instead of " + classesVersion.ToString() + " .";
                    }

                    // READING CFTYPE
                    result = fileForReading.ReadLine();
                    switch (int.Parse(result))
                    {
                        case 1:
                            CFType = new CFTypeQuadratic();
                            break;
                        case 2:
                            CFType = new CFTypeCrossEntropy();
                            break;
                        case 3:
                            CFType = new CFTypeLogLikelihood();
                            break;
                        default:
                            return "\n-> Error loading network info from file. Unknown CFType: " + result + " .";
                    }

                    // READING ALPHA
                    result = fileForReading.ReadLine();
                    alpha = float.Parse(result);

                    // READING LYAMBDA
                    result = fileForReading.ReadLine();
                    lyambda = float.Parse(result);

                    // READING LEARNING SPEED
                    result = fileForReading.ReadLine();
                    learningSpeed = float.Parse(result);

                    // READING LAYERS NUMBER
                    result = fileForReading.ReadLine();
                    int layerCount = int.Parse(result);

                    // LOADING LAYER PARAMETERS ONE BY ONE
                    layers.Clear();
                    for (int i = 0; i < layerCount; i++)
                    {

                        layers.Add(new ANNLayer(1, 1, new AFTypeSigmoid()));
                        result = layers[i].LoadFromFile(fileForReading);
                        if (result.Length > 0)
                        {
                            return "\n-> Error loading layer " + i.ToString() + ". " + result;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return "\n-> Error loading network parameters from a file. Exception message: " + e.Message;
            }

            return "";
        }


        /// <summary>
        /// Calculates the outputs of all network's layers on inputed vector and returns output vector of the last network's layer and index of output with largest value.
        /// </summary>
        /// <param name="inputVector">Vector of inputs.</param>
        /// <param name="outputVector">Vector of network's outputs (last layer outputs).</param>
        /// <param name="outputIndex">Index of network's output with latgest value</param>
        /// <returns>Empty string on success or error message.</returns>
        public string React(float[] inputVector, ref float[][] currLayersOutputs, out int outputIndex)
        {
            outputIndex = -1;

            // CHECKING INPUT VECTOR LENGTH TO MATCH THE INPUT NUMBER
            if (layers.Count == 0)
                return "\n-> Error while calculating network reaction without layers (" + layers.Count.ToString() + ").";

            if (inputVector.Length != SayInputNum())
                return "\n-> Error while calculating network reaction with number of layers " + layers.Count.ToString() + " on input vector with length " + inputVector.Length.ToString() + " .";

            // CREATING 2-D ARRAY FOR ALL LAYER OUTPUTS
            layersOutputs = new float[layers.Count][];
            layersOutputs[0] = new float[layers[0].SayNeuronsNum()];

            // CALCULATING FIRST LAYER OUTPUTS BASED ON INPUTED VECTOR
            string result = layers[0].React(inputVector, ref layersOutputs[0]);
            if (result.Length > 0)
                return result;

            // FOR OTHER LAYERS CALCULATING THEIR REACTIONS ON OUTPUTS OF PREVIOUS LAYERS
            for (int layerInd = 1; layerInd < layers.Count; layerInd++)
            {
                layersOutputs[layerInd] = new float[layers[layerInd].SayNeuronsNum()];
                result = layers[layerInd].React(layersOutputs[layerInd - 1], ref layersOutputs[layerInd]);
                if (result.Length > 0)
                    return result;
            }

            currLayersOutputs = layersOutputs;

            // SEARCHING FOR MAX OUTPUT
            float[] outputVector = layersOutputs[layersOutputs.Length - 1];
            float maxOutput = outputVector[0];
            int maxOutputInd = 0;
            for (int outputInd = 1; outputInd < outputVector.Length; outputInd++)
            {
                if (outputVector[outputInd] > maxOutput)
                {
                    maxOutput = outputVector[outputInd];
                    maxOutputInd = outputInd;
                }
            }

            outputIndex = maxOutputInd;
            return "";
        }


        /// <summary>
        /// Calculates summary of all coefficients of the network.
        /// </summary>
        /// <returns>Summary of all coefficients of the network.</returns>
        protected float CalcCoeffsSum()
        {
            // CHECKING LAYERS TO EXIST
            if (layers.Count == 0)
                return -1;

            // CALCULATING SUM
            float coeffsSum = 0;

            for (int layerInd = 0; layerInd < layers.Count; layerInd++)
            {
                layers[layerInd].CalcCoeffsSum(ref coeffsSum);
            }

            return coeffsSum;
        }


        /// <summary>
        /// Trains the network using a batch of examples (with type of <i>Example</i> class) with a single pass returning cost function value for inputet batch and optionally showing details of trainings and.
        /// </summary>
        /// <param name="examplesBatch">Array of <i>Examples</i> class with examples.</param>
        /// <param name="cost">The value of cost function for inputed examples.</param>
        /// <param name="showDetails">Set to <i>true</i> - to print delta sums during training, or <i>false</i> - not to print them.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string TrainBatch(Example[] examplesBatch, out float cost, bool showDetails = false)
        {
            cost = -1;

            // CHECKIGN LAYERS NUMBER
            if (layers.Count == 0)
                return "\n-> Error teaching the network with zero layers number.";
            
            // CHACKING INPUTES EXAMPLES BATCH TO BE NON-ZERO
            if (examplesBatch.Length == 0)
                return "\n-> Error teaching the network with batch - batch is empty.";

            // RESETING LAYERS TRAINING PROGRESS
            ResetLessonsNum();

            float cSum = 0;                                 // Sum for cost function values over batch examles
            float[][] outputs = null;                         // Outputs of all layers of the network afthe a run (React)
            string result = "";

            // TRAINING OVER ALL EXAMPLES IN THE BATCH
            float[] derivatives = null;
            for (int exampleInd = 0; exampleInd < examplesBatch.Length; exampleInd++)
            {
                // CHECKING EXAMPLE
                if (examplesBatch[exampleInd].SayX().Length != layers[0].SayInputsNum())
                    return "\n-> Error teaching the network: example with index " + exampleInd.ToString() + " has number of inputs (" + examplesBatch[exampleInd].SayX().Length.ToString() + ") less then the first layer of network (" + layers[0].SayInputsNum().ToString() + ").";

                if (examplesBatch[exampleInd].SayY().Length != layers[layers.Count - 1].SayNeuronsNum())
                    return "\n-> Error teaching the network: example with index " + exampleInd.ToString() + " has number of outputs (" + examplesBatch[exampleInd].SayY().Length.ToString() + ") less then the number of neurons in the last layer of network (" + layers[layers.Count - 1].SayNeuronsNum().ToString() + ").";

                // GETTING NETWORK REACTION ON CURRENT EXAMPLE'S INPUTS
                result = React(examplesBatch[exampleInd].SayX(), ref outputs, out int outputInd);
                if (result.Length > 0)
                    return result;

                // CALCULATING COST BASED ON CURRENT EXAMPLE OUTPUTS AND NETWORK REACTIONS FOR THE TOTAL SUM
                cSum += CFType.CalcSingleCost(outputs[outputs.Length - 1], examplesBatch[exampleInd].SayY());

                // CALCULATING COST DERIVATIVES FOR LAST LAYER BASED ON CURRENT EXAMPLES OUTPUTS AND NETWORK REACTIONS
                try
                {
                    derivatives = CFType.CalcDerivatives(outputs[outputs.Length - 1], examplesBatch[exampleInd].SayY());
                }
                catch (Exception e)
                {
                    return "\n-> Error calculating cost function derivatives. " + e.Message;
                }

                /// PROPOGATING GRADIENT THROUGH ALL LAYERS STARTING FROM LAST TO SECOND
                if (layers.Count > 1)
                {
                    // IF THERE ARE MORE THEN ONE LAYER
                    for (int layerInd = layers.Count - 1; layerInd >= 1; layerInd--)
                    {
                        result = layers[layerInd].BPropogate(layersOutputs[layerInd - 1], layersOutputs[layerInd], derivatives, ref derivatives, learningSpeed, examplesBatch.Length);
                        if (result.Length > 0)
                            return "\n-> Error while back propogation through layer " + layerInd.ToString() + " . " + result;
                    }
                }

                // FOR FIRST LAYER INPUTS ARE INPUTS OF THE EXAMPLE
                result = layers[0].BPropogate(examplesBatch[exampleInd].SayX(), layersOutputs[0], derivatives, ref derivatives, learningSpeed, examplesBatch.Length);
                if (result.Length > 0)
                    return "\n-> Error while back propogation through layer 0 . " + result;
            }

            // APPLYING DELTA SUMS OF LAYERS
            for (int layerInd = 0; layerInd < layers.Count; layerInd++)
            {
                if (showDetails)
                {
                    PrintClass.PrintLine("\n  - Layer " + layerInd.ToString() + " delta sums:");
                }
                layers[layerInd].ApplyDeltaSums(showDetails, examplesBatch.Length);
            }

            // CALCULATING AVERAGE COST
            cost = cSum / examplesBatch.Length;

            // CALCULATING L2 REGULARIZATION
            float coeffsSum = CalcCoeffsSum();

            cost += lyambda * coeffsSum / 2 / examplesBatch.Length;

            return "";
        }


        /// <summary>
        /// Trains all examples from its learning examples bank, devided in batches with certain size
        /// </summary>
        /// <param name="epochsNum">Number of epochs to proceed.</param>
        /// <param name="batchSize">Optional: batch size, default is <i>100</i>.</param>
        /// <peram name="showEpochProgress">Optionsl: set true to print progress of training network through epochs, default is <i>false</i>.</peram>
        /// <peram name="showBatchProgress">Optionsl: set true to print progress of training network through batches, default is <i>false</i>.</peram>
        /// <peram name="showDetails">Optionsl: set true to print details of network training process, e. g. delta sums, default is <i>false</i>.</peram>
        /// <returns>Empty string on success or error message.</returns>
        public string TrainExamples(int epochsNum, int batchSize = 100, bool showEpochProgress = false, bool showBatchProgress = false, bool showDetails = false)
        {
            // CHECKING EXAMPLES TO EXIST
            if (!examples.ExamplesExist())
                return "\n-> Error training network - no examples exist.";

            // SETTING BATCH SIZE OF EXAMPLES
            examples.SetBatchSize(batchSize);

            float costEpoch = -1;
            float costBatch = -1;

            string result = CalcAllCost(out costEpoch);
            if (result.Length > 0)
                return result;

            PrintClass.PrintLine("\n-> Starting training of network through " + epochsNum.ToString() + " epochs with " + examples.SayExamplesNum().ToString() + " examples, starting total cost is " + costEpoch.ToString() + "...");
            DateTime trainingStart = DateTime.Now;
            DateTime epochStart = DateTime.Now;
            DateTime batchStart = DateTime.Now;

            for (int epochInd = 0; epochInd < epochsNum; epochInd++)
            {
                // SHUFFLING TRAINING EXAMPLES
                examples.ShuffleTrainExamples();

                // TRAINING ONE EPOCH THROUGH ALL BATCHES
                int batchsNum = examples.SayBatchNumber();

                if (showEpochProgress)
                {
                    epochStart = DateTime.Now;
                    PrintClass.PrintLine("   - Starting epoch " + (epochInd + 1).ToString() + " / " + epochsNum.ToString() + " through " + batchsNum.ToString() + " batches...");
                }

                for (int batchInd = 0; batchInd < batchsNum; batchInd++)
                {
                    // TRAIN BATCH USING BATCH EXAMPLES
                    if (showBatchProgress)
                    {
                        batchStart = DateTime.Now;
                        PrintClass.PrintLine("      - Training batch " + (batchInd + 1).ToString() + " / " + batchsNum.ToString() + " through " + examples.SayBatchSize(batchInd).ToString() + " examples...");
                    }

                    result = TrainBatch(examples.GiveBatchByInd(batchInd), out costBatch, showDetails);
                    if (result.Length > 0)
                        return "\n-> Error training epoch " + epochInd.ToString() + ", batch " + batchInd.ToString() + "." + result;

                    if (showBatchProgress)
                    {
                        PrintClass.PrintLine("         Batch training finished in " + DateTime.Now.Subtract(batchStart).ToString() + " with batch cost " + costBatch.ToString() + ".");
                    }
                }

                if (showEpochProgress)
                {
                    result = CalcAllCost(out costEpoch);
                    if (result.Length > 0)
                        return "\n-> Error training epoch " + epochInd.ToString() + "." + result;

                    PrintClass.PrintLine("      Epoch training finished in " + DateTime.Now.Subtract(epochStart).ToString() + " with total cost " + costEpoch.ToString() + ".");
                }
            }

            result = CalcAllCost(out costEpoch);
            if (result.Length > 0)
                return result;

            PrintClass.PrintLine("\n   Network training through " + epochsNum.ToString() + " epochs with " + examples.SayExamplesNum().ToString() + " examples finished in " + DateTime.Now.Subtract(trainingStart).ToString() + ", final total cost is " + costEpoch.ToString() + ".");

            return "";
        }


        /// <summary>
        /// Calculates cost function for given pack of examples.
        /// </summary>
        /// <param name="packOfExamples">Pack of examples for their cost calculation.</param>
        /// <param name="cost">An <i>out</i> variable to get the resulting value, -1 on unsuccess.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string CalcPackCost(Example[] packOfExamples, out float cost, bool showProgress = false)
        {
            cost = -1;
            
            // CHECKING LAYERS NUMBER
            if (layers.Count == 0)
                return "\n-> Error teaching the network with zero layers number.";

            // CHECKING INPUTES EXAMPLES BATCH TO BE NON-ZERO
            if (packOfExamples.Length == 0)
                return "\n-> Error teaching the network with batch - batch is empty.";

            double cSum = 0;                                 // Sum for cost function values over batch examles
            float[][] outputs = null;
            string result = "";
            float percentStep = packOfExamples.Length / 100;

            if (showProgress)
                PrintClass.PrintLine("\n-> Calculating cost over " + packOfExamples.Length.ToString() + " :");

            // CALCULATING OVER ALL EXAMPLES IN THE BATCH
            for (int exampleInd = 0; exampleInd < packOfExamples.Length; exampleInd++)
            {
                // CHECKING EXAMPLE
                if (packOfExamples[exampleInd].SayX().Length != layers[0].SayInputsNum())
                    return "\n-> Error teaching the network: example with index " + exampleInd.ToString() + " has number of inputs (" + packOfExamples[exampleInd].SayX().Length.ToString() + " less then the first layer of network (" + layers[0].SayInputsNum().ToString() + ").";

                if (packOfExamples[exampleInd].SayY().Length != layers[layers.Count - 1].SayNeuronsNum())
                    return "\n-> Error teaching the network: example with index " + exampleInd.ToString() + " has number of outputs (" + packOfExamples[exampleInd].SayY().Length.ToString() + " less then the number of neurons in the last layer of network (" + layers[layers.Count - 1].SayNeuronsNum().ToString() + ").";

                // GETTING NETWORK REACTION ON CURRENT EXAMPLE'S INPUTS
                result = React(packOfExamples[exampleInd].SayX(), ref outputs, out int tmpInt);
                if (result.Length > 0)
                    return result;

                // CALCULATING COST BASED ON CURRENT EXAMPLE OUTPUTS AND NETWORK REACTIONS FOR THE TOTAL SUM
                cSum += CFType.CalcSingleCost(outputs[outputs.Length - 1], packOfExamples[exampleInd].SayY());

                if (showProgress)
                {
                    float tmpFloat = exampleInd * 100;
                    if ((exampleInd + 1) % percentStep == 0)
                        PrintClass.RePrint(((int)Math.Floor((double)exampleInd / percentStep)).ToString() + "%");
                }
            }

            if (showProgress)
                PrintClass.RePrint(" Calculation cost is done.\n");

            // CALCULATING AVERAGE COST
            cost = (float)(cSum / packOfExamples.Length);

            // CALCULATING L2 REGULARIZATION
            float coeffsSum = CalcCoeffsSum();

            cost += lyambda * coeffsSum / 2 / packOfExamples.Length;

            return "";
        }


        /// <summary>
        /// Calculates cost function for all learning examples in the example bank.
        /// </summary>
        /// <param name="cost">An <i>out</i> variable to get the resulting value, -1 on unsuccess.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string CalcAllCost(out float cost, bool showProgress = false)
        {
            cost = -1;

            if (!examples.ExamplesExist())
                return "Error calculating cost of all examples - no examples exist.";

            string result = CalcPackCost(examples.GiveTrainExamples(), out cost, showProgress);
            if (result.Length > 0)
                return result;

            return "";
        }


        /// <summary>
        /// Calculates percentage of uncorrect answeres of network on all training or test examples and prints it to PrintClass.
        /// </summary>
        /// <param name="ifTestExampleType">Input for choosing examples to calculate: <i>false</i> - for teaching examples (default), <i>true</i> - for test examples.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string CalcError(bool ifTestExampleType = false)
        {
            // CHECKING EXAMPLES TO EXIST
            if (ifTestExampleType)
            {
                if (!examples.TestExamplesExist())
                    return "Error calculating cost of all test examples - no test examples exist.";
            }
            else
            {
                if (!examples.ExamplesExist())
                    return "Error calculating cost of all learning examples - no examples exist.";
            }

            // CALCULATING CORRECT ANSWERS NUMBER
            int correctAnsweresNum = 0;
            string result;

            int exampleNum = examples.SayExamplesNum(ifTestExampleType);

            for (int exampleInd = 0; exampleInd < exampleNum; exampleInd++)
            {
                result = examples.GiveExampleLink(exampleInd, out Example theExample, ifTestExampleType);
                if (result.Length > 0)
                    return result;

                result = CorrectReaction(theExample, out bool correct);
                if (result.Length > 0)
                    return result;

                if (correct)
                    correctAnsweresNum++;
            }

            int errorsNum = examples.SayExamplesNum(ifTestExampleType) - correctAnsweresNum;

            // PRINTING RESULTS OF CALCULATION
            PrintClass.Print("\n-> Network answeres error over all ");

            if (ifTestExampleType)
                PrintClass.Print("test");
            else
                PrintClass.Print("teaching");

            PrintClass.PrintLine(" examples is " + Math.Round((double)errorsNum / exampleNum * 100).ToString() + " (" + errorsNum.ToString() + "/" + exampleNum.ToString() + ").");

            PrintClass.PrintLine("\n-> Correct answeres percentage: " + Math.Round((double)correctAnsweresNum / exampleNum * 100).ToString() + " (" + correctAnsweresNum.ToString() + "/" + exampleNum.ToString() + ").");

            return "";
        }


        ////////// PRIVATE METHODS
        
        /// <summary>
        /// Checks the array elements to be positive and returns their summary on success or -1 on error.
        /// </summary>
        /// <param name="inputArray">Array to be checked.</param>
        /// <returns>Summary of all array elements or -1 if array is empty or has non-positive element.</returns>
        protected int PositiveArraySum(int[] inputArray)
        {
            int sum = 0;
            if (inputArray.Length == 0)
                return -1;

            for (int i = 0; i < inputArray.Length; i++)
            {
                if (inputArray[i] > 0)
                    sum += inputArray[i];
                else
                    return -1;
            }

            return sum;
        }


        /// <summary>
        /// Defines if reaction of the network on the certain inputed example is correct - equal to examples outputs distribution.
        /// </summary>
        /// <param name="inputExample">Input examples to calculate reaction on.</param>
        /// <param name="correct">The <i>out</i> variable for the final result - correct (true) or not (false).</param>
        /// <returns>Empty string on success or error message.</returns>
        protected string CorrectReaction(Example inputExample, out bool correct)
        {
            correct = false;
            
            // CALCULATING INDEX OF CORRECT ANSWER OF GIVEN EXAMPLE
            int correctAnswer = 0;
            correctAnswer = inputExample.SayExpected();

            float[][] tmpFloatArray = null;

            // CALCULATING REACTION OF NETWORK
            string result = React(inputExample.SayX(), ref tmpFloatArray, out int networkAnswer);
            if (result.Length > 0)
                return result;

            correct = correctAnswer == networkAnswer ? true : false;

            return "";
        }


        ////////// PROTECTED METHODS


        ////////// CONSTRUCTORS
        /// <summary>
        /// Constructor of ANNetwork class, used only by static methods.
        /// </summary>
        /// <param name="neuronNums">Array with numbers of neurons of the new network.</param>
        /// <param name="newInputNum">Number of network's inputs (for first layer).</param>
        /// <param name="newLearningSpeed">Learning speed of new network, <i>default</i> is 0.01.</param>
        /// <param name="newAlpha">Alpha of new network, <i>default</i> is 1.</param>
        /// <param name="newLyambda">Lyambda of new network, <i>default</i> is 0.</param>
        /// <param name="newCFType">Cost function type of new network, <i>default</i> will be set depending on the activation function of last layer.</param>
        /// <param name="newAFTypes">Activation function types for layers of the network, <i>default</i> is AFTypeSigmoid class.</param>
        /// <param name="lowerBoundaries">Lower boundaries for the network's coefficients initialization, <i>default</i> is 0.</param>
        /// <param name="upperBoundaries">Upper boundaries for the network's coefficients initialization, <i>default</i> is 0.5.</param>
        /// <param name="initScale">Set to <i>true</i> - for new network's coefficients initialization scale by quare root of inputs number, set to <i>false</i> - for no scaling.</param>
        protected ANNetwork(int[] neuronNums, int newInputNum, float newLearningSpeed = 0.01f, float newAlpha = 1, float newLyambda = 0, CFTypeBase newCFType = null, AFTypeBase[] newAFTypes = null, float[] lowerBoundaries = null, float[] upperBoundaries = null, bool initScale = false)
        {
            // CHECKING IF INPUT PARAMETERS ARE CORRECT
            if ((PositiveArraySum(neuronNums) > 0) && (newInputNum > 0))
            {
                SetLearningSpeed(newLearningSpeed);

                // SETTING ALPHA
                if (newAlpha <= 0)
                {
                    PrintClass.PrintLine("\n-> Attemt to initialize a network with non-positive alpha. Alpha is set to 1.");
                    SetAlpha(1);
                }
                else
                    SetAlpha(newAlpha);

                fileDirectory = "ANN";
                fileExtension = "ann";

                // SETTING LYAMBDA
                if (newLyambda < 0)
                {
                    PrintClass.PrintLine("\n-> Attemt to initialize a network with negative lyambda. Lyambda set to 0.");
                    SetLyambda(0);
                }
                else
                    SetLyambda(newLyambda);

                fileDirectory = "ANN";
                fileExtension = "ann";

                // MAKING DEFAULT AFTYPES - SIGMOID
                if (newAFTypes == null)
                {
                    newAFTypes = new AFTypeBase[neuronNums.Length];
                    for (int i = 0; i < newAFTypes.Length; i++)
                    {
                        newAFTypes[i] = new AFTypeSigmoid();
                    }
                }

                // MAKING DEFAULT CFTYPE - CROSS-ENTROPY (FOR SIGMOID NEURONS) AND LIKEHOOD FOR...
                if (newCFType == null)
                {
                    if (newAFTypes[newAFTypes.Length - 1].ToString() == AFTypeSigmoid.name)
                        // FOR SIGMOID AFTYPE OF LAST LAYER NEURONS USING CROSS-ENTROPY CFTYPE
                        SetCFType(new CFTypeCrossEntropy());
                    else if (newAFTypes[newAFTypes.Length - 1].ToString() == AFTypeSoftMax.name)
                        // FOR SOFTMAX AFTYPE OF LAST LAYER NEURONS USING LOGLIKELIHOOD CFTYPE
                        SetCFType(new CFTypeLogLikelihood());
                    else
                        // FOR OTHER AFTYPES USING QUADRATIC CFTYPE
                        SetCFType(new CFTypeQuadratic());
                }
                else
                    SetCFType(newCFType);

                // MAKING DEFAULT BOUNDARIES
                if (lowerBoundaries == null)
                {
                    lowerBoundaries = new float[neuronNums.Length];
                    upperBoundaries = new float[neuronNums.Length];
                    for (int i = 0; i < lowerBoundaries.Length; i++)
                    {
                        lowerBoundaries[i] = 0;
                        upperBoundaries[i] = 0.5f;
                    }
                }

                // CREATING LAYERS WITH INITIALIZATION
                layers = new List<ANNLayer>();
                layers.Clear();
                float currScale = 1;
                for (int i = 0; i < neuronNums.Length; i++)
                {
                    // CREATING LAYER DEPENDING ON ITS POSITION - FIRST OR NOT (NUMBER OF INPUTS DEFINES IN DIFFERENT WAYS)
                    if (i == 0)
                    {
                        // MAKING SCALE
                        if (initScale == true)
                            currScale = (float)Math.Sqrt(newInputNum);

                        layers.Add(new ANNLayer(neuronNums[i], newInputNum, newAFTypes[i], lowerBoundaries[i], upperBoundaries[i], currScale));
                    }
                    else
                    {
                        // MAKING SCALE
                        if (initScale == true)
                            currScale = (float)Math.Sqrt(neuronNums[i - 1]);
                        try
                        {
                            layers.Add(new ANNLayer(neuronNums[i], neuronNums[i - 1], newAFTypes[i], lowerBoundaries[i], upperBoundaries[i], currScale));
                        }
                        catch (Exception e)
                        {
                            throw new NotImplementedException(e.Message);
                        }
                    }
                }

                // INITIALIZING THE EXAMPLES
                examples = Examples.Init();
            }
            else
            {
                string message = "Illegal attempt to create neuron network with total neurons number " + PositiveArraySum(neuronNums).ToString() + " (-1 - for zero-length array or non-positive elements) and inputs number " + newInputNum.ToString() + " .";
                throw new NotImplementedException(message);
            }
        }
    }
}

