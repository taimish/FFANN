using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FFANN
{
    // CLASS FOR CONSOLE USER INTERACTION
    class Commands
    {
        private ANNetwork network = null;                               // The network it works with
        private Examples examples = null;                               // Examples to operate while no network initialized

        private string welcomeMessage = "\nWelcome to AI Robotic Gladiator ANN test unit.\n";

        private string helpMessage = "\nEnter a command in any register, arguments in () are not obligatory, L1 - layer 1, ... - throung all layers:" +
            "\nhelp - show this message" +
            "\nexit - exits the program without saving any progress of network." +
            "\nnewnet <inputsNumber - int>" +
            "\n <layersNumber - int>" +
            "\n <L1NeuronNumber - int>..." +
            "\n (<learningSpeed - float> <alpha - float> <L2Lyambda - float> <CostFunctionIndex - int>" +
            "\n {<L1ActivationFunctionIndex - int>...}" +
            "\n <scaleBoolean - int(0/1)>" +
            "\n {<L1LowerBoundary - float>... <L1UpperBoundary - float>...})" +
            "\n    - create new artificial neuron network, () - optional arguments, {} - arguments, that must be supplied together." +
            "\n    Activation functions (AF) are:" +
            "\n    1 - Step" +
            "\n    2 - Linear" +
            "\n    3 - Sigmoid" +
            "\n    4 - Tanh" +
            "\n    5 - SoftMax" +
            "\n    6 - RectifiedLinear" +
            "\n    7 - NoChange" +
            "\n    Default activation function type is sigmoid. NoChange - the type will not change from previous, not permitted for creation of new network." +
            "\n    Cost functions (CF) are:" +
            "\n    1 - Quadratic" +
            "\n    2 - CrossEntropy" +
            "\n    3 - LogLikelihood" +
            "\n    4 - NoChange" +
            "\n    Default combinations activation function - cost functions are: 3 - 2, 5 - 3, others - 1." +
            "\nshownet - print network params to the console." +
            "\nlistnet - show all saved network files with .ann extension in \\NETS subdirectory of current directory." +
            "\nsavenet <filename> - save network coefficients, biases, activation function types, cost function types and alphas to the specified file in /NETS directory with .ann extension, added automaticly." +
            "\nloadnet <filename> - load network coefficients, biases, activation function types, cost function types and alphas from the specified file in /NETS directory, no need to write .ann extension." +
            "\nreact <imputVectorLengthN> <input1>... <inputN> - calculate network reaction on input vector of floats." +
            "\nsetinpnum <inputNumber - int> - set number of inputs of the network." +
            "\nsetalpha <newAlpha - float> - set new alpha." +
            "\nsetlyambda <newLyambda - float> - set new lyambda for L2 regulation." +
            "\nsetCF <CostFunctionIndex - int> - set cost function type." +
            "\nsetAF <L1ActivationFunctionIndex - int>... - set activation function types of layers." +
            "\nsetLS <trainingSpeed - float> - set train speed of the network" +
            "\nresetless - resets number of passed lessons of network and sets its layers neuron delta sums to zero." +
            "\nnewex <inputsNumber - int> <input1 - float>... <outputsNumber - int> <output1 - float>... - create new example with inputs and outputs" +
            "\nlistex - show all saved examples files with specific extension (default is .exs) in specific subdirectory (default is \\EXS) of current directory." +
            "\nsaveex <filename> - save existing examples to the specified file in specific subdirectory (default is \\EXS) with specific extension (default is .exs)." +
            "\nloadex <filename> (<multiinputs - int(0/1)>) - load examples from the specified file in specific subdirectory (default is \\EXS) with specific extension (default is .exs), erasing previous. If multiinput argument is 1, then every input with limit N will be." +
            "\nloadcsv <filename> - load examples from the specified file with .csv extension." +
            "\ndivex <testPercentage - int> - an inputed percentage of existing training examples are moved to testing examples bank." +
            "\nuniteex - unite examples from two banks into one main training examples bank." +
            "\nshowex  <showDetails - int(0/1)> - print all existing examples to the console." +
            "\nshow1ex <exampleIndex - int> - print example with inputed index." +
            "\ntrainex <epochNumber - int> <showDetails - int(0/1)> - train all existing examples inputed number of epochs." +
            "\nrunex <exampleIndex - int> - run network once on an example with certain index from existing (index starts from 0)." +
            "\ncost - get cost function for all existing examples." +
            "\nexerr (<useTestExamples - int(0/1)>) - calculate network error on training examples or on testing examples if 1 inputed." +
            "\nminerr (<useTestExamples - int(0/1)>) - calculate minimal error of teaching or testing examples, caused by different outputs with same inputs." +
            "\nclearex - removing all existing examples.";

        private string noNetworkMessage = "\n-> Operation with network is illegal because no network is created.\n";

        private string goodbyeMessage = "\nThanks for using me! Goodbye   0/";

        ////////// *******************************************************************************************************************************
        ////////// *******************************************************************************************************************************
        ////////// GENERAL FUNCTIONS

        // WELCOME MESSAGE
        public void Welcome()
        {
            Console.WriteLine(welcomeMessage + helpMessage);
        }


        // GOODBYE MESSAGE
        public void Goodbye()
        {
            Console.WriteLine(goodbyeMessage);
        }


        // HELP MESSAGE
        public void Help()
        {
            PrintClass.PrintLine(helpMessage);
        }


        // SET NETWORK LINK ON EXTERNAL NETWORK
        public void SetExternalLink(ref ANNetwork externalLink)
        {
            network = externalLink;
        }


        ////////// *******************************************************************************************************************************
        ////////// *******************************************************************************************************************************
        ////////// NETWORK FUNCTIONS

        // CREATE NEW NETWORK
        public string CreateNewANN(string tmpString, ref ANNetwork externalNetworkLink)
        {
            /* newANN <inputsNumber> <layersNumber> <L1NeuronNumber>... 
             * (<alpha> <CostFunctionIndex> {<L1ActivationFunctionIndex>...} {<L1LowerBoundary>... <L1UpperBoundary>...} <scaleBoolean>)*/

            bool isLast = true;
            string result = "";
            // PARSING INPUTS NUMBER
            if (tmpString.Length == 0)
                return "\n-> Error creating new network: no <inputsNumber> argument entered.";

            int inputsNumber = -1;
            result = Parsing.ParseInt(ref tmpString, ref inputsNumber, ref isLast, "inputsNumber", Parsing.AfterParsingCheck.Positive);

            if (result.Length > 0)
                return "\n-> Error creating new network. " + result;

            if (isLast)
                return "\n-> Error creating new network: no <layersNumber> argument entered.";


            // PARSING LAYER NUMBER
            int layersNumber = -1;
            result = Parsing.ParseInt(ref tmpString, ref layersNumber, ref isLast, "inputsNumber", Parsing.AfterParsingCheck.Positive);

            if (result.Length > 0)
                return "\n-> Error creating new network. " + result;

            // PARSING LAYER NEURONS NUMBER FOR EACH LAYER
            int[] neuronNumbers = new int[layersNumber];
            for (int layerInd = 0; layerInd < layersNumber; layerInd++)
            {
                result = Parsing.ParseInt(ref tmpString, ref neuronNumbers[layerInd], ref isLast, "<LxNeuronNumber>", Parsing.AfterParsingCheck.Positive);
                if (result.Length > 0)
                    return "\n-> Error creating new network. " + result;

                if ((isLast) && (layerInd < layersNumber - 1))
                    return "\n-> Error creating new network: no <LxNeuronNumber> for layer " + layerInd.ToString() + " .";
            }

            if (isLast)
            {
                network = ANNetwork.NewNetwork(neuronNumbers, inputsNumber);
                externalNetworkLink = network;
                return "";
            }

            // PARSING TRAINING SPEED IF ABLE
            float learningSpeed = -1;

            result = Parsing.ParseFloat(ref tmpString, ref learningSpeed, ref isLast, "trainingSpeed", Parsing.AfterParsingCheck.Positive);
            if (result.Length > 0)
                return "\n-> Error creating new network. " + result;

            if (isLast)
            {
                network = ANNetwork.NewNetwork(neuronNumbers, inputsNumber, learningSpeed);
                externalNetworkLink = network;
                return "";
            }

            // PARSING ALPHA IF ABLE
            float alpha = -1;

            result = Parsing.ParseFloat(ref tmpString, ref alpha, ref isLast, "alpha", Parsing.AfterParsingCheck.Positive);
            if (result.Length > 0)
                return "\n-> Error creating new network. " + result;

            if (isLast)
            {
                network = ANNetwork.NewNetwork(neuronNumbers, inputsNumber, learningSpeed, alpha);
                externalNetworkLink = network;
                return "";
            }

            // PARSING LYAMBDA IF ABLE
            float lyambda = -1;

            result = Parsing.ParseFloat(ref tmpString, ref lyambda, ref isLast, "lyambda", Parsing.AfterParsingCheck.NonNegative);
            if (result.Length > 0)
                return "\n-> Error creating new network. " + result;

            if (isLast)
            {
                network = ANNetwork.NewNetwork(neuronNumbers, inputsNumber, learningSpeed, alpha, lyambda);
                externalNetworkLink = network;
                return "";
            }

            // PARSING COST FUNCTION
            CFTypeBase CFType = null;
            result = Parsing.ParseCFType(ref tmpString, ref CFType, ref isLast);

            if (result.Length > 0)
                return result;

            if (isLast)
            {
                network = ANNetwork.NewNetwork(neuronNumbers, inputsNumber, learningSpeed, alpha, lyambda, CFType);
                externalNetworkLink = network;
                return "";
            }

            // PARSING ACTIVATION FUNCTIONS FOR ALL LAYERS
            AFTypeBase[] AFTypes = new AFTypeBase[layersNumber];
            result = Parsing.ParseAFTypesArray(ref tmpString, ref AFTypes, ref isLast);

            if (result.Length > 0)
                return result;

            if (isLast)
            {
                network = ANNetwork.NewNetwork(neuronNumbers, inputsNumber, learningSpeed, alpha, lyambda, CFType, AFTypes);
                externalNetworkLink = network;
                return "";
            }

            // PARSING INIT SCALE
            bool initScale;
            int initScaleInt = -1;

            result = Parsing.ParseInt(ref tmpString, ref initScaleInt, ref isLast, "scaleBoolean", Parsing.AfterParsingCheck.NonNegative);
            if (result.Length > 0)
                return "\n-> Error creating new network. " + result;

            if ((initScaleInt != 0) && (initScaleInt != 1))
            {
                return "\n-> Error creating new network: non-boolean initScale " + initScaleInt.ToString() + " .";
            }

            initScale = initScaleInt == 0 ? false : true;

            if (isLast)
            {
                network = ANNetwork.NewNetwork(neuronNumbers, inputsNumber, learningSpeed, alpha, lyambda, CFType, AFTypes, initScale);
                externalNetworkLink = network;
                return "";
            }

            // PARSING LOWER BOUNDARY AND UPPER BOUNDARY
            float[] lowerBoundaries = new float[layersNumber];
            result = Parsing.ParseFloatArray(ref tmpString, ref lowerBoundaries, ref isLast, "lower boundaries", Parsing.AfterParsingCheck.NoCheck);

            if (result.Length > 0)
                return "\n-> Error creating new network. " + result;

            if (isLast)
            {
                return "\n-> Error creating new network: no text for upper boundaties ";
            }

            float[] upperBoundaries = new float[layersNumber];
            result = Parsing.ParseFloatArray(ref tmpString, ref upperBoundaries, ref isLast, "upper boundaries", Parsing.AfterParsingCheck.NoCheck);

            if (result.Length > 0)
                return "\n-> Error creating new network. " + result;

            network = ANNetwork.NewNetwork(neuronNumbers, inputsNumber, learningSpeed, alpha, lyambda, CFType, AFTypes, initScale, lowerBoundaries, upperBoundaries);
            externalNetworkLink = network;

            return "";
        }


        // SAVE NETWORK COMMAND
        public string SaveNetwork(string tmpString)
        {
            if (network == null)
            {
                // NO NETWORK TO SAVE
                return noNetworkMessage;
            }
            else
            {
                if (tmpString.Length == 0)
                    return "\n-> Entered no file name for saving.";

                else
                {
                    tmpString = network.SaveToFile(tmpString);
                    if (tmpString.Length > 0)
                        return "\n-> Error saving network info to a file: " + tmpString;
                    else
                        return "";
                }

            }

        }


        // LOAD NETWORK COMMAND
        public string LoadNetwork(string tmpString, ref ANNetwork externalNetworkLink)
        {
            if (network == null)
            {
                // NO NETWORK CREATED - CREATING NEW
                network = ANNetwork.NewNetwork(new int[] { 1 }, 1);
                externalNetworkLink = network;
            }

            string result = network.LoadFromFile(tmpString);
            if (result.Length > 0)
                return result;

            return "";
        }


        // SHOW NETWORK COMMAND
        public string ShowNet(string tmpString)
        {
            if (network == null)
            {
                // NO NETWORK TO PRINT
                return noNetworkMessage;
            }
            else
            {
                int showDetails = 0;

                if (tmpString.Length > 0)
                {
                    bool isLast = true;

                    string result = Parsing.ParseInt(ref tmpString, ref showDetails, ref isLast, "showDetails", Parsing.AfterParsingCheck.NonNegative);
                    if (result.Length > 0)
                        return result;
                }

                bool showDet = showDetails == 0 ? false : true;

                network.ShowParams(showDet);
                return "";
            }
        }


        // LISTNET COMMAND
        public void ListNets()
        {
            if (network == null)
            {
                // NO NETWORK CREATED - CREATING NEW
                network = ANNetwork.NewNetwork(new int[] { 1 }, 1);
            }

            network.ShowFileList();
        }


        // CHANGE INPUTS NUMBER
        public string SetInputsNumber(string tmpString)
        {
            // CHECKING NETWORK
            if (network == null)
            {
                return noNetworkMessage;
            }

            // CHECKING INPUTED STRING
            if (tmpString.Length == 0)
                return "\n-> Error changing network's inputs number: no input text.";

            int newInputsNumber = -1;
            bool isLast = true;
            string result = Parsing.ParseInt(ref tmpString, ref newInputsNumber, ref isLast, "inputsNumber", Parsing.AfterParsingCheck.Positive);
            if (result.Length > 0)
                return result;

            result = network.SetInputsNum(newInputsNumber);
            if (result.Length > 0)
                return result;

            return "\n-> Network's inputs number changed successfuly.";
        }


        // CHANGE ALPHA
        public string SetAlpha(string tmpString)
        {
            // CHECKING NETWORK
            if (network == null)
            {
                return noNetworkMessage;
            }

            // CHECKING INPUTED STRING
            if (tmpString.Length == 0)
                return "\n-> Error changing network's alpha: no input text.";

            int newAlpha = -1;
            bool isLast = true;
            string result = Parsing.ParseInt(ref tmpString, ref newAlpha, ref isLast, "alpha", Parsing.AfterParsingCheck.Positive);
            if (result.Length > 0)
                return result;

            result = network.SetAlpha(newAlpha);
            if (result.Length > 0)
                return result;

            return "\n-> Network's alpha changed successfuly.";
        }


        // CHANGE LYAMBDA
        public string SetLyambda(string tmpString)
        {
            // CHECKING NETWORK
            if (network == null)
            {
                return noNetworkMessage;
            }

            // CHECKING INPUTED STRING
            if (tmpString.Length == 0)
                return "\n-> Error changing network's lyambda: no input text.";

            int newLyambda = -1;
            bool isLast = true;
            string result = Parsing.ParseInt(ref tmpString, ref newLyambda, ref isLast, "lyambda", Parsing.AfterParsingCheck.Positive);
            if (result.Length > 0)
                return result;

            result = network.SetLyambda(newLyambda);
            if (result.Length > 0)
                return result;

            return "\n-> Network's lyambda changed successfuly.";
        }


        // CHANGE COST FUNCTION
        public string SetCFType(string tmpString)
        {
            // CHECKING NETWORK
            if (network == null)
            {
                return noNetworkMessage;
            }

            // CHECKING INPUTED STRING
            if (tmpString.Length == 0)
                return "\n-> Error changing network's cost function type: no input text.";

            CFTypeBase newCFType = null;
            bool isLast = false;

            string answer = Parsing.ParseCFType(ref tmpString, ref newCFType, ref isLast);

            if (answer.Length > 0)
                return answer;

            network.SetCFType(newCFType);
            return "\n-> Cost function type set successful.";
        }


        // CHANGE ACTIVATION FUNCTIONS
        public string SetAFTypes(string tmpString)
        {
            // CHECKING NETWORK
            if (network == null)
            {
                return noNetworkMessage;
            }

            // CHECKING INPUTED STRING
            if (tmpString.Length == 0)
                return "\n-> Error changing network's cost function type: no input text.";

            AFTypeBase[] newAFTypes = new AFTypeBase[network.SayLayerNum()];
            bool isLast = false;

            string result = Parsing.ParseAFTypesArray(ref tmpString, ref newAFTypes, ref isLast);

            if (result.Length > 0)
                return result;

            result = network.SetAFTypes(newAFTypes);
            if (result.Length > 0)
                return result;

            return "\n-> Network layers' activation function types changed successfuly.";
        }


        // CHANGE INPUTS NUMBER
        public string SetLearningSpeed(string tmpString)
        {
            // CHECKING NETWORK
            if (network == null)
            {
                return noNetworkMessage;
            }

            // CHECKING INPUTED STRING
            if (tmpString.Length == 0)
                return "\n-> Error changing network's training speed: no input text.";

            if (!float.TryParse(tmpString, out float newLearningSpeed))
                return "\n-> Error changing network's training speed while parsing (" + tmpString + ").";

            string answer = network.SetLearningSpeed(newLearningSpeed);
            if (answer.Length > 0)
                return answer;

            return "";
        }


        // RESET LESSONS NUMBER
        public string ResetLessonsNumber(string tmpString)
        {
            // CHECKING NETWORK
            if (network == null)
            {
                return noNetworkMessage;
            }

            // CHECKING INPUTED STRING
            if (tmpString.Length == 0)
                return "\n-> Error changing network's cost function type: no input text.";

            network.ResetLessonsNum();
            return "";
        }


        // TRAIN EXAMPLES
        public string TrainExamples(string tmpString)
        {
            // CHECKING NETWORK
            if (network == null)
                return "\n-> Error teaching network - it doesn't exist.";

            int epochsNum = -1;
            bool isLast = true;

            string result = Parsing.ParseInt(ref tmpString, ref epochsNum, ref isLast, "epochsNum", Parsing.AfterParsingCheck.Positive);
            if (result.Length > 0)
                return result;

            if (isLast)
                return "\n-> Error teaching network: not enough input data - no showDetails argument.";

            int showDetails = -1;

            result = Parsing.ParseInt(ref tmpString, ref showDetails, ref isLast, "showDetails", Parsing.AfterParsingCheck.NonNegative);
            if (result.Length > 0)
                return result;

            bool showDet = showDetails == 0 ? false : true;

            result = network.TrainExamples(epochsNum, 100, showDet, false, false);
            if (result.Length > 0)
                return result;

            return "";
        }


        // RUN EXAMPLE
        public string RunExample(string tmpString)
        {
            tmpString = tmpString.Trim();
            
            // CHECKING EXAMPLES, NETWORK AND INPUT
            if (network == null)
                return "\n-> Error running an example - the network doesn't exist.";

            if (network.examples.SayExamplesNum() == 0)
                return "\n-> Error running an example - no existing examples.";

            if (tmpString.Length == 0)
                return "\n-> Error running an example - not enough input data.";

            // PARSING EXAMPLE INDEX
            int exampleInd = -1;
            bool isLast = true;
            string result = Parsing.ParseInt(ref tmpString, ref exampleInd, ref isLast, "examplesInd", Parsing.AfterParsingCheck.NonNegative);
            if (result.Length > 0)
                return result;

            bool allLayersOutputs = false;

            // PARSING OPTION OF PRINTING ALL LAYERS OUTPUTS
            if (!isLast)
            {
                int allLOutputs = -1;
                result = Parsing.ParseInt(ref tmpString, ref allLOutputs, ref isLast, "printAllLayersOutputs", Parsing.AfterParsingCheck.NonNegative);
                if (result.Length > 0)
                    return result;

                allLayersOutputs = allLOutputs == 0 ? false : true;
            }

            float[][] outputs = null;
            result = network.examples.GiveExampleLink(exampleInd, out Example theExample);
            if (result.Length < 0)
                return result;

            result = network.React(theExample.SayX(), ref outputs, out int networkAnswer);
            if (result.Length > 0)
                return result;

            PrintClass.Print("\n\n-> On example N " + exampleInd.ToString() + "\n(" + theExample.SayX(0));
            for (int inputInd = 1; inputInd < theExample.SayXLength(); inputInd++)
            {
                PrintClass.Print(", " + theExample.SayX(inputInd));
            }

            PrintClass.Print(")\n network replied: " + networkAnswer.ToString() + " (" + outputs[outputs.Length - 1][0].ToString());
            for (int outputInd = 1; outputInd < outputs[outputs.Length - 1].Length; outputInd++)
            {
                PrintClass.Print(", " + outputs[outputs.Length - 1][outputInd].ToString());
            }

            PrintClass.Print(").\n while expected is: " + theExample.SayExpected().ToString() + " (" + theExample.SayY(0).ToString());
            for (int outputInd = 1; outputInd < theExample.SayYLength(); outputInd++)
            {
                PrintClass.Print(", " + theExample.SayY(outputInd).ToString());
            }

            PrintClass.Print(").\n");

            if (allLayersOutputs)
                for (int layerInd = 0; layerInd < outputs.Length; layerInd++)
                {
                    PrintClass.Print("\n  - Layer " + layerInd.ToString() + " outputs:\n" + outputs[layerInd][0].ToString());
                    for (int outputInd = 1; outputInd < outputs[layerInd].Length; outputInd++)
                    {
                        PrintClass.Print(", " + outputs[layerInd][outputInd].ToString());
                    }
                }

            return "";
        }


        // COST EXAMPLES
        public string Cost()
        {
            if (network == null)
                return "\n-> Error calculating cost - no network exists.";

            if (!network.examples.ExamplesExist())
                return "\n-> Error calculating cost - no examples exists.";

            string result = network.CalcAllCost(out float cost, true);
            if (result.Length > 0)
                return result;

            PrintClass.PrintLine("\n-> Total cost for existing " + network.examples.SayExamplesNum().ToString() + " examples is: " + cost.ToString() + ".");

            return "";
        }


        // CALCULATING TRAINING EXAMPLES ERROR
        public string CalcTrainingError(string tmpString)
        {
            if (network == null)
                return "\n-> Error clearing examples - no network exists.";

            tmpString = tmpString.Trim();

            string result;
            if (tmpString.Length == 0)
            {
                result = network.CalcError();
            }
            else
            {
                int testExamples = -1;
                bool isLast = true;

                result = Parsing.ParseInt(ref tmpString, ref testExamples, ref isLast, "showDetails", Parsing.AfterParsingCheck.NonNegative);
                if (result.Length > 0)
                    return result;

                bool trainTest = testExamples == 0 ? false : true;

                result = network.CalcError(trainTest);
            }

            if (result.Length > 0)
                return result;

            return "";
        }

        ////////// *******************************************************************************************************************************
        ////////// *******************************************************************************************************************************
        ////////// EXAMPLES FUNCTIONS

        // CREATE NEW EXAMPLE
        public string NewExample(string tmpString)
        {
            // ADDING NEW EXAMPLE
            string result = examples.AddExample(tmpString);
            if (result.Length > 0)
                return result;

            return "\n-> New example created successfully.";
        }


        // SAVE EXAMPLES
        public string SaveExamples(string tmpString)
        {
            string result = examples.SaveToFile(tmpString);
            if (result.Length > 0)
                return result;

            return "-> Examples saved to a file successfuly.";
        }


        // LISTEX COMMAND
        public void ListExs()
        {

            string result = examples.ShowFileList();
            if (result.Length > 0)
                PrintClass.PrintLine(result);
        }


        // LOAD EXAMPLES
        public string LoadExamples(string tmpString)
        {
            // LOADING EXAMPLES
            string result = examples.LoadFromFileReplace(tmpString);
            if (result.Length > 0)
                return result;
            
            return "\n-> Examples loaded from a file successfuly.";
        }


        // LOAD EXAMPLES FROM CSV FILE
        public string LoadExamplesFromCSV(string tmpString)
        {
            // LOADING EXAMPLES
            string result = examples.LoadFromFileReplace(tmpString, false);
            if (result.Length > 0)
                return result;

            return "\n-> Examples loaded from a .csv file successfuly.";
        }


        // SHOW EXAMPLES COMMAND
        public string ShowExamples(string tmpString)
        {
            Examples tmpExamples = Examples.Init();
            if (tmpExamples.SayExamplesNum() == 0)
                return "\n-> Error showing all examples - no examples exist.";

            int showDetails = -1;
            bool isLast = true;

            string result = Parsing.ParseInt(ref tmpString, ref showDetails, ref isLast, "showDetails", Parsing.AfterParsingCheck.NonNegative);
            if (result.Length > 0)
                return result;

            bool showDet = showDetails == 0 ? false : true;

            tmpExamples.ShowExamples(showDet);

            return "";
        }


        // SHOW EXAMPLES WITH INDEX
        public string ShowExampleWithIndex(string tmpString)
        {
            tmpString = tmpString.Trim();

            // CHECKING EXAMPLES AND INPUT
            if (tmpString.Length == 0)
                return "\n-> Error running an example - not enough input data.";

            Examples tmpExamples = Examples.Init();
            if (tmpExamples.SayExamplesNum() == 0)
                return "\n-> Error showing example with an index - no examples exist.";

            // PARSING EXAMPLE INDEX
            int exampleInd = -1;
            bool isLast = true;
            string result = Parsing.ParseInt(ref tmpString, ref exampleInd, ref isLast, "examplesInd", Parsing.AfterParsingCheck.NonNegative);
            if (result.Length > 0)
                return result;

            tmpExamples.ShowExampleWithIndex(exampleInd);

            return "";
        }


        // CLEAR EXAMPLES
        public string ClearExamples()
        {
            Examples tmpExamples = Examples.Init();
            if (tmpExamples.SayExamplesNum() == 0)
                return "\n-> Error clearing examples - no examples exist.";

            tmpExamples.ClearExamples();
            PrintClass.PrintLine("\n-> Training and testing examples lists cleared - no examples exist any more.");

            return "";
        }


        // DIVIDE EXAMPLES
        public string DivideExamples(string tmpString)
        {
            // CHECKING EXAMPLES
            Examples tmpExamples = Examples.Init();
            if (tmpExamples.SayExamplesNum() == 0)
                return "\n-> Error dividing examples - they don't exist.";

            int percentage = -1;
            bool isLast = true;

            string result = Parsing.ParseInt(ref tmpString, ref percentage, ref isLast, "epochsNum", Parsing.AfterParsingCheck.Positive);
            if (result.Length > 0)
                return result;

            result = tmpExamples.DivideExamples(percentage, true);
            if (result.Length > 0)
                return result;

            return "";
        }


        // UNITE EXAMPLES
        public string UniteExamples()
        {
            // CHECKING EXAMPLES
            Examples tmpExamples = Examples.Init();
            if (tmpExamples.SayExamplesNum(true) == 0)
                return "\n-> Error uniting examples - no test examples exist.";

            tmpExamples.UniteExamples();

            return "";
        }


        // CALCULATING MINIMAL ERROR OF EXAMPLE BANK
        public string CalcMinError(string tmpString)
        {
            // CHECKING EXAMPLES
            Examples tmpExamples = Examples.Init();
            if (tmpExamples.SayExamplesNum() == 0)
                return "\n-> Error calculating minimal examples error - they don't exist.";

            tmpString = tmpString.Trim();

            string result;
            if (tmpString.Length == 0)
            {
                result = tmpExamples.CalcMinError(tmpExamples.GiveTrainExamples(), out float minError, true);
                if (result.Length > 0)
                    return result;
            }
            else
            {
                int testExamples = -1;
                bool isLast = true;

                result = Parsing.ParseInt(ref tmpString, ref testExamples, ref isLast, "showDetails", Parsing.AfterParsingCheck.NonNegative);
                if (result.Length > 0)
                    return result;

                bool trainTest = testExamples == 0 ? false : true;

                if (trainTest)
                {
                    result = tmpExamples.CalcMinError(tmpExamples.GiveTestExamples(), out float minError);
                    if (result.Length > 0)
                        return result;
                }
                else
                {
                    result = tmpExamples.CalcMinError(tmpExamples.GiveTrainExamples(), out float minError);
                    if (result.Length > 0)
                        return result;
                }
            }

            return "";
        }


        // CONSTRUNCTOR
        public Commands(ref ANNetwork programNetwork)
        {
            network = programNetwork;
            examples = Examples.Init();
        }


        // **********************************************************************************************
        // OTHER FUNCTIONS ******************************************************************************

    }
}
