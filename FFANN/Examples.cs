using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFANN
{
    /// <summary>
    /// A complex class for holding training and testing examples banks and methods to work with them.
    /// </summary>
    class Examples
    {
        ////////// VARIABLES

        public static Examples singleton = null;                // Singleton link to a single instance of the class

        private List<Example> trainExamples;                    // Main (learning) examples - examples for training
        private List<Example> testExamples;                     // Examples for testing - are initialized separately
        private string fileExtension;                           // extension of files for saving or loading examples
        private string fileDirectory;                           // Directory for saving or loading examples to or from files

        // SIZE OF BETCH
        private int batchSize;


        ////////// METHODS

        /// <summary>
        /// Returns singleton of Examples class, initializing if still empty.
        /// </summary>
        /// <returns>Examples class link on examples singleton.</returns>
        public static Examples Init()
        {
            if (singleton == null)
                singleton = new Examples();

            return singleton;
        }


        /// <summary>
        /// Says number of examples in the class: training or testing, depending on input bool.
        /// </summary>
        /// <param name="ifTestExampleType">Set to true - to get number of test examples, set to false or by default - to get training examples.</param>
        /// <returns>Number of examples, training or testing.</returns>
        public int SayExamplesNum(bool ifTestExampleType = false)
        {
            if (ifTestExampleType)
            {
                if (testExamples == null)
                    return 0;

                return testExamples.Count;
            }
            else
            {
                if (trainExamples == null)
                    return 0;

                return trainExamples.Count;
            }
        }


        // RETURN GENERAL SIZE OF BATCH
        /// <summary>
        /// Returns current size of default batch (default, certain batch - last one - can be smaller).
        /// </summary>
        /// <returns>Current size of default batch.</returns>
        public int SayBatchSize()
        {
            return batchSize;
        }


        /// <summary>
        /// Returns size of certain batch for current training examples bank.
        /// </summary>
        /// <param name="batchInd">Index of batch to get the size of.</param>
        /// <returns>Size of batch with inputed index.</returns>
        public int SayBatchSize(int batchInd)
        {
            // IF NO EXAMPLE OR OUT OF EXAMPLE VECTOR - RETURN 0
            if (!ExamplesExist() || (batchInd * batchSize > trainExamples.Count))
                return 0;

            // IF BATCH WITH INPUTED INDEX IS FULLY IN EXAMPLES BOUNDARIES, RETURN GENERAL BATCH SIZE, OTHERWISE RETURN REST OF EXAMPLES FOR THIS BATCH
            if (batchSize * (batchInd + 1) <= trainExamples.Count)
                return batchSize;
            else
                return trainExamples.Count - batchSize * batchInd;
        }


        /// <summary>
        /// Returns current extension for work with files of examples (list, open).
        /// </summary>
        /// <returns>Current examples files extension.</returns>
        public string SayFileextension()
        {
            return fileExtension;
        }


        /// <summary>
        /// Returns current name of subdirectory (of program runtime directory) for work with files of examples (list, open).
        /// </summary>
        /// <returns>Current subdirectory name for examples files.</returns>
        public string SayFileDirectory()
        {
            return fileDirectory;
        }


        /// <summary>
        /// Sets the size of default batch for futher trainings.
        /// </summary>
        /// <param name="newBatchSize">New batch size.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SetBatchSize(int newBatchSize)
        {
            if (newBatchSize > trainExamples.Count)
            {
                batchSize = trainExamples.Count;
                return "New batch size exceeds examples number, so batch size set to examples number";
            }

            batchSize = newBatchSize;
            return "";
        }


        /// <summary>
        /// Sets new value of current file extension for work with files of examples (list, open).
        /// </summary>
        /// <param name="newExtension">New extension for files of examples.</param>
        public void SetFileextension(string newExtension)
        {
            if (newExtension.Length > 0)
                fileExtension = newExtension;
        }


        /// <summary>
        /// Sets new value of name of subdirectory (of program runtime directory) for work with files of examples (list, open).
        /// </summary>
        /// <param name="newDirectory">New examples files subdirectory name.</param>
        public void SetFileDirectory(string newDirectory)
        {
            fileDirectory = newDirectory;
        }


        /// <summary>
        /// Returns the number of batches of current size that training examples bank divides on, including last one (that can be shorten), or 0 if no examples exist.
        /// </summary>
        /// <returns>Number of batches of current training examples bank or 0.</returns>
        public int SayBatchNumber()
        {
            if (!ExamplesExist())
                return 0;

            return (int)Math.Ceiling((double)trainExamples.Count / batchSize);
        }


        /// <summary>
        /// Adds an example to the training examples bank, passed as input and output vectors.
        /// </summary>
        /// <param name="inputs">Input vector of new example.</param>
        /// <param name="outputs">Output vector of new example.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string AddExample(float[] inputs, float[] outputs)
        {
            // CHECKING INPUT AND OUTPUT VECTOR TO EXIST AND BE NON-ZERO SIZED
            if ((inputs.Length == 0) || (inputs == null))
                return "\n->Error adding an example: unexisting or zero-sized input vector";

            if ((outputs.Length == 0) || (outputs == null))
                return "\n->Error adding an example: unexisting or zero-sized output vector";

            // CHECKING VECTORS LENGTHS TO MATCH OTHER EXAMPLES IF THEY EXIST
            if (ExamplesExist())
            {
                if (inputs.Length != trainExamples[0].SayXLength())
                    return "\n->Error adding an example: new example's input vector size (" + inputs.Length.ToString() + ") don't match existing examples' input vectors' size (" + trainExamples[0].SayXLength() + ").";

                if (outputs.Length != trainExamples[0].SayYLength())
                    return "\n->Error adding an example: new example's output vector size (" + outputs.Length.ToString() + ") don't match existing examples' output vectors' size (" + trainExamples[0].SayYLength() + ").";
            }

            trainExamples.Add(new Example(inputs, outputs));

            return "";
        }


        /// <summary>
        /// Adds an example to the training examples bank, passed as a string with lengths and values of input and output vectors.
        /// </summary>
        /// <param name="arguments">A string with lengths and values of input and output vectors.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string AddExample(string arguments)
        {
            // PARSING NUMBER OF INPUTS
            if (arguments.IndexOf(' ') == -1)
                return "\n-> Error creating new example - no number of inputs";

            int inputNum = -1;
            bool isLast = true;

            string result = Parsing.ParseInt(ref arguments, ref inputNum, ref isLast, "inputNum", Parsing.AfterParsingCheck.Positive);

            if (result.Length > 0)
                return "\n-> Error creating new example. " + result;

            if (isLast)
                return "\n-> Error creating new exampe - no inputs entered.";

            // PARSING INPUTS
            float[] x = new float[inputNum];

            result = Parsing.ParseFloatArray(ref arguments, ref x, ref isLast, "inputs", Parsing.AfterParsingCheck.NoCheck);

            if (result.Length > 0)
                return "\n-> Error creating new example. " + result;

            if (isLast)
                return "\n-> Error creating new exampe - no ouputs entered.";

            // PARSING NUMBER OF OUTPUTS

            int outputNum = -1;

            result = Parsing.ParseInt(ref arguments, ref outputNum, ref isLast, "outputNum", Parsing.AfterParsingCheck.Positive);

            if (isLast)
                return "\n-> Error creating new exampe - no outputs entered.";

            // PARSING OUTPUTS
            float[] y = new float[outputNum];

            result = Parsing.ParseFloatArray(ref arguments, ref y, ref isLast, "outputs", Parsing.AfterParsingCheck.NoCheck);

            if (result.Length > 0)
                return "\n-> Error creating new example. " + result;

            // CHECKING NEW EXAMPLE TO MATCH PREVIOS INPUTS AND OUTPUTS NUMBERS
            if (trainExamples.Count != 0)
            {
                if (inputNum != trainExamples[0].SayXLength())
                    return "\n-> Error creating new example: it's inputs number (" + inputNum.ToString() + ") don't match existing example inputs number (" + trainExamples[0].SayXLength().ToString() + ").";

                if (outputNum != trainExamples[0].SayYLength())
                    return "\n-> Error creating new example: it's outputs number (" + outputNum.ToString() + ") don't match existing example outputs number (" + trainExamples[0].SayYLength().ToString() + ").";
            }

            // CREATING NEW EXAMPLE
            AddExample(x, y);

            return "";
        }


        /// <summary>
        /// Removes all examples from both training and testing examples banks.
        /// </summary>
        public void ClearExamples()
        {
            if (trainExamples != null)
                trainExamples.Clear();
            else
                trainExamples = new List<Example>();

            if (testExamples != null)
                testExamples.Clear();
            else
                testExamples = new List<Example>();
        }


        /// <summary>
        /// Unites learining and testing examples banks into learing one, emptying the test one.
        /// </summary>
        public void UniteExamples()
        {
            // RETURNING PREVOISLY DEVIDED TEST EXAMPLES TO MAIN BANK
            if (ExamplesExist() && TestExamplesExist())
            {
                while (testExamples.Count > 0)
                {
                    trainExamples.Add(testExamples[testExamples.Count - 1]);
                    testExamples.RemoveAt(testExamples.Count - 1);
                }
            }
        }


        /// <summary>
        /// Moves part of training examples to testing examples bank with inputed percentage of examples moved.
        /// </summary>
        /// <param name="percentToBeTakenToTests">Approximate percentage of training examples to be taken to testing examples bank.</param>
        /// <param name="useRandom">Set ot true - moves examples randomly, using percentage as a degree of randomness, or to false - moves percentage value amount of examples from every 100.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string DivideExamples(int percentToBeTakenToTests, bool useRandom = false)
        {
            if (!ExamplesExist())
                return "\n-> Error dividing examples into main bank and test bank - no examples exist.";

            if ((percentToBeTakenToTests <= 0) || (percentToBeTakenToTests > 95))
                return "\n-> Error dividing examples into main bank and test bank - invalid percent argument (" + percentToBeTakenToTests.ToString() + "), should be 1...95.";

            // UNITING EXAMPLES
            UniteExamples();

            // MAKING NEW SELECTION OF TESTS
            if (testExamples == null)
            {
                testExamples = new List<Example>();
            }

            if (trainExamples.Count < 100)
                return "\n-> Error dividing examples into main bank and test bank - not enough examples for selection (" + trainExamples.Count.ToString() + ", should be at least 100).";

            var randomizer = new Random();
            int exampleInd = 0;
            int percentInd = 0;
            while (exampleInd < trainExamples.Count)
            {
                // IF NO RANDOM: CHECKING CURRENT PERCENTAGE INDEX IF IT IS BEFOR 100% - ENTERED PERCENTAGE, OR IF RANDOM(1-100) IS GREATER THEN THE PERCENTAGE
                if (((!useRandom) && (percentInd < (100 - percentToBeTakenToTests))) || ((useRandom) && (randomizer.Next(100) + 1 > percentToBeTakenToTests)))
                    // CURRENT EXAMPLE SHOULD BE SKIPPED
                    exampleInd++;
                else
                {
                    // CURRENT EXAMPLE SHOULD BE MOVED TO TEST BANK
                    testExamples.Add(trainExamples[exampleInd]);
                    trainExamples.RemoveAt(exampleInd);
                }

                percentInd++;
                if (percentInd == 100)
                    percentInd = 0;
            }

            PrintClass.PrintLine("\n-> Example division result: training examples number - " + trainExamples.Count.ToString() + ", testing examples number - " + testExamples.Count.ToString() + ".");

            return "";
        }


        /// <summary>
        /// Says, are examples divided into training and testing banks or there are no testing examples.
        /// </summary>
        /// <returns>True, if testing examples exist, or false if not.</returns>
        public bool AreExamplesDevided()
        {
            if (TestExamplesExist())
                return true;
            else
                return false;
        }


        /// <summary>
        /// Saves current training and testing examples to a file with inputed name, examples file extension and subdirectory, creating it if it doesn't exist.
        /// </summary>
        /// <param name="fileNameArgument">A string with file name for examples saving. If ' ' is inside the string, the substring before it is used as file name.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SaveToFile(string fileNameArgument)
        {
            // CHECKING MAIN (LEARNING) EXAMPLES TO EXIST
            if (!ExamplesExist())
                return "-> Error saving examples to a file - no existing examples.";

            // TRIMMING FILE NAME ARGUMENT
            fileNameArgument = fileNameArgument.Trim();

            // CHECKING ARGUMENT NOT TO BE EMPTY
            if (fileNameArgument.Length == 0)
                return "-> Error saving examples to a file - no argument with file name entered.";

            string fileName = "";

            int tmpInt = fileNameArgument.IndexOf(' ');
            if (tmpInt > 0)
                fileName = fileNameArgument.Remove(tmpInt);
            else
                fileName = fileNameArgument;

            try
            {
                // CREATING DIRECTORY IF NOT EXIST
                System.IO.Directory.CreateDirectory(fileDirectory);

                using (System.IO.StreamWriter fileForWriting = new System.IO.StreamWriter(fileDirectory + "\\" + fileName + "." + fileExtension, false))
                {
                    //// SAVING EXAMPLES
                    // SAVING KEYWORD
                    fileForWriting.WriteLine("Examples");

                    // SAVING EXAMPLES NUMBER (ONLY LEARNING OR WITH TESTING IF THEY EXIST
                    if (TestExamplesExist())
                        fileForWriting.WriteLine((trainExamples.Count + testExamples.Count).ToString());
                    else
                        fileForWriting.WriteLine(trainExamples.Count.ToString());

                    // SAVING EXAMPLES INPUTS NUMBER
                    fileForWriting.WriteLine(trainExamples[0].SayXLength().ToString());

                    // SAVING EXAMPLES OUTPUTS NUMBER
                    fileForWriting.WriteLine(trainExamples[0].SayYLength().ToString());

                    // SAVING LEARNING EXAMPLES DATA ONE BY ONE
                    for (int exampleInd = 0; exampleInd < trainExamples.Count; exampleInd++)
                    {
                        for (int inputInd = 0; inputInd < trainExamples[exampleInd].SayXLength(); inputInd++)
                        {
                            fileForWriting.WriteLine(trainExamples[exampleInd].SayX(inputInd));
                        }

                        for (int outputInd = 0; outputInd < trainExamples[exampleInd].SayYLength(); outputInd++)
                        {
                            fileForWriting.WriteLine(trainExamples[exampleInd].SayY(outputInd));
                        }
                    }

                    // SAVING TEST EXAMPLES DATA ONE BY ONE IF THEY EXIST
                    if (TestExamplesExist())
                    {
                        for (int exampleInd = 0; exampleInd < testExamples.Count; exampleInd++)
                        {
                            for (int inputInd = 0; inputInd < testExamples[exampleInd].SayXLength(); inputInd++)
                            {
                                fileForWriting.WriteLine(testExamples[exampleInd].SayX(inputInd));
                            }

                            for (int outputInd = 0; outputInd < testExamples[exampleInd].SayYLength(); outputInd++)
                            {
                                fileForWriting.WriteLine(testExamples[exampleInd].SayY(outputInd));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return "Error saving examples to a file. Exception message: " + e.Message;
            }

            return "";
        }


        /// <summary>
        /// Loads examples from a file with inputed file name and examples file extension and subdirectory, is selected.
        /// </summary>
        /// <param name="fileNameArgument">A string with file name for examples loading. If ' ' is inside the string, the substring before it is used as file name.</param>
        /// <param name="useDirAndExtension">Set to true (default) - examples file extension and subdirectory will be used for loading, set to false - won't.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string LoadFromFileReplace(string fileNameArgument, bool useDirAndExtension = true)
        {
            // REMOVING EXAMPLES IF EXISTED
            ClearExamples();

            // LOADING EXAMPLES
            string result = LoadFromFileAdd(fileNameArgument, useDirAndExtension);
            if (result.Length > 0)
                return result;

            result = NormalizeInputs(false);
            if (result.Length > 0)
                return result;

            return "";
        }


        /// <summary>
        /// Loading examples from a file, which name is passed as a string argument.
        /// </summary>
        /// <param name="fileNameArgument">String argument with file name.</param>
        /// <param name="useDirAndExtension">Set to <i>true</i> - to use default examples direction and file extension, <i>false</i> - to use pure file name for loading.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string LoadFromFileAdd(string fileNameArgument, bool useDirAndExtension = true)
        {
            if (trainExamples == null)
                trainExamples = new List<Example>();

            fileNameArgument = fileNameArgument.Trim();

            if (fileNameArgument.Length == 0)
                return "-> Error loading examples from a file - no argument with file name entered.";

            string fileName = "";

            int tmpInt = fileNameArgument.IndexOf(' ');
            if (tmpInt > 0)
                fileName = fileNameArgument.Remove(tmpInt);
            else
                fileName = fileNameArgument;

            string fullFileName;
            if (useDirAndExtension)
                fullFileName = fileDirectory + "\\" + fileName + "." + fileExtension;
            else
                fullFileName = fileName;

            try
            {
                using (System.IO.StreamReader fileForReading = new System.IO.StreamReader(fullFileName))
                {
                    ////////// LOADING EXAMPLES
                    if (fullFileName.LastIndexOf(".csv") != fullFileName.Length - 4)
                    {
                        //////// LOADING EXAMPLES FROM DEFAULT EXAMPLES FILE

                        // READING "EXAMPLES" KEYWORD
                        string tmpString = fileForReading.ReadLine();
                        if (tmpString != "Examples")
                        {
                            return "Error loading examples from the file '" + fileName + "." + fileExtension + "'. Unknown format of first string (" + tmpString + ").";
                        }

                        // READING EXAMPLES NUMBER
                        tmpString = fileForReading.ReadLine();
                        int examplesNum = -1;
                        bool isLast = true;
                        string result = Parsing.ParseInt(ref tmpString, ref examplesNum, ref isLast, "examplesNumber", Parsing.AfterParsingCheck.Positive);
                        if (result.Length > 0)
                            return "Error loading examples from file. " + result;

                        // READING EXAMPLES INPUTS NUMBER
                        tmpString = fileForReading.ReadLine();
                        int examplesInputsNum = -1;
                        result = Parsing.ParseInt(ref tmpString, ref examplesInputsNum, ref isLast, "examplesInputsNumber", Parsing.AfterParsingCheck.Positive);
                        if (result.Length > 0)
                            return "Error loading examples from file. " + result;

                        // READING EXAMPLES OUTPUTS NUMBER
                        tmpString = fileForReading.ReadLine();
                        int examplesOutputsNum = -1;
                        result = Parsing.ParseInt(ref tmpString, ref examplesOutputsNum, ref isLast, "examplesOutputsNumber", Parsing.AfterParsingCheck.Positive);
                        if (result.Length > 0)
                            return "Error loading examples from file. " + result;

                        // READING EXAMPLES DATA ONE BY ONE
                        for (int exampleInd = 0; exampleInd < examplesNum; exampleInd++)
                        {
                            float[] exampleInputs = new float[examplesInputsNum];
                            float[] exampleOutputs = new float[examplesOutputsNum];

                            for (int inputInd = 0; inputInd < examplesInputsNum; inputInd++)
                            {
                                tmpString = fileForReading.ReadLine();
                                result = Parsing.ParseFloat(ref tmpString, ref exampleInputs[inputInd], ref isLast, "exampleInput " + inputInd.ToString(), Parsing.AfterParsingCheck.NoCheck);
                                if (result.Length > 0)
                                    return "Error reading example " + exampleInd.ToString() + " input from file. " + result;
                            }

                            for (int outputInd = 0; outputInd < examplesOutputsNum; outputInd++)
                            {
                                tmpString = fileForReading.ReadLine();
                                result = Parsing.ParseFloat(ref tmpString, ref exampleOutputs[outputInd], ref isLast, "exampleOutput " + outputInd.ToString(), Parsing.AfterParsingCheck.NoCheck);
                                if (result.Length > 0)
                                    return "Error reading example " + exampleInd.ToString() + " output from file. " + result;
                            }

                            // CHECKING NEW EXAMPLE TO MATCH PREVIOUS WITH VECTORS LENGTHS, IF IT IS NOT FIRST
                            if (trainExamples.Count > 0)
                            {
                                if (exampleInputs.Length != trainExamples[trainExamples.Count - 1].SayXLength())
                                    return "\n-> Error adding an example - its inputs number don't match previous example.";

                                if (exampleOutputs.Length != trainExamples[trainExamples.Count - 1].SayYLength())
                                    return "\n-> Error adding an example - its outputs number don't match previous example.";
                            }

                            trainExamples.Add(new Example(exampleInputs, exampleOutputs));
                        }
                    }
                    else
                    {
                        //////// LOADING EXAMPLES FROM CSV FILE ADAPTIVELY

                        // LOADING FIRST STRING AND PASSING IT IF IT CONTAINS CHARACTERS
                        string tmpString = fileForReading.ReadLine();
                        if (tmpString.Length == 0)
                            return "\n-> Error reading examples from a .csv file - no data inside";

                        char[] tmpCharArr = tmpString.ToCharArray();
                        for (int symbolInd = 0; symbolInd < tmpCharArr.Length; symbolInd++)
                        {
                            if (!",.01234567890".Contains(tmpCharArr[symbolInd]))
                            {
                                // THERE IS SOME OTHER SYMBOL THEN DIGITS - READING NEXT LINE
                                tmpString = fileForReading.ReadLine();
                                break;
                            }
                        }

                        if (tmpString.Length == 0)
                            return "\n-> Error reading examples from a .csv file - no data inside";


                        // PARSING FIRST NUMERIC STRING, CONTAINIG VECTOR LENGTHS FOR DISTRIBUTIONAL INPUTS
                        List<int> columnWidths = new List<int>();

                        bool isLast = false;
                        string result;
                        // READING INTS UNTIL END OF STRING
                        while(!isLast)
                        {
                            result = Parsing.ParseInt(ref tmpString, ref tmpInt, ref isLast, "columnWidth", Parsing.AfterParsingCheck.NonNegative, ",");
                            if (result.Length > 0)
                                return result;

                            columnWidths.Add(tmpInt);
                        }

                        // READING STRINGS UNTIL END OF STREAM
                        while (!fileForReading.EndOfStream)
                        {
                            tmpString = fileForReading.ReadLine();

                            if (tmpString.Length == 0)
                                break;

                            float[] tmpFloatArray = new float[columnWidths.Count];

                            result = Parsing.ParseFloatArray(ref tmpString, ref tmpFloatArray, ref isLast, "some csv value", Parsing.AfterParsingCheck.NoCheck, ",");
                            if (result.Length > 0)
                                return result;

                            // CALCULATING SUM OF ALL INPUTS DEPENDING ON THEIR TYPE - VALUE OR DISTRIBUTION
                            int inputsNum = 0;
                            for (int columnInd = 0; columnInd < columnWidths.Count - 1; columnInd++)
                            {
                                inputsNum += columnWidths[columnInd];
                            }

                            List<float> inputs = new List<float>();

                            // FULLFILLING INPUTS ARRAY WIHT VALUES OR DISTRIBUTIONS
                            for (int columnInd = 0; columnInd < columnWidths.Count - 1; columnInd++)
                            {
                                if (columnWidths[columnInd] == 1)
                                {
                                    // COPYING VALUE FROM READ TEMPORARY FLOAT ARRAY TO THE INPUT LIST AS INPUT TYPE IS VALUE - WIDTH IS 1 ONLY
                                    inputs.Add(tmpFloatArray[columnInd]);
                                }
                                else
                                {
                                    // CREATING A DISTRIBUTION INPUTS BASED ON READ COLUMN-INPUTS NUMBER AND INDEX FROM TEMPORARY FLOAT ARRAY
                                    for (int distributionInd = 0; distributionInd < columnWidths[columnInd]; distributionInd++)
                                    {
                                        inputs.Add(distributionInd == Math.Round(tmpFloatArray[columnInd]) ? 1 : 0);
                                    }
                                }
                            }

                            // CREATING OUTPUT
                            List<float> outputs = new List<float>();
                            for (int distributionInd = 0; distributionInd < columnWidths.Last(); distributionInd++)
                            {
                                outputs.Add(distributionInd == Math.Round(tmpFloatArray.Last()) ? 1 : 0);
                            }

                            // CHECKING NEW EXAMPLE TO MATCH PREVIOUS WITH VECTORS LENGTHS, IF IT IS NOT FIRST
                            if (trainExamples.Count > 0)
                            {
                                if (inputs.Count != trainExamples.Last().SayXLength())
                                    return "\n-> Error adding an example - its inputs number don't match previous example.";

                                if (outputs.Count != trainExamples.Last().SayYLength())
                                    return "\n-> Error adding an example - its outputs number don't match previous example.";
                            }

                            // ADDING THE EXAMPLE
                            trainExamples.Add(new Example(inputs.ToArray(), outputs.ToArray()));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return "Error reading examples from a file. Exception message: " + e.Message;
            }

            return "";
        }


        /// <summary>
        /// Normalizes inputs, scaling their outputs to boundaries 0...1 through division by maximum.
        /// </summary>
        /// <param name="showMaximums">Set <i>true</i> - to see found maximums of columns, set <i>false</i> or nothing - no to.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string NormalizeInputs(bool showMaximums = false)
        {
            // CHECKING EXAMPLES TO EXIST
            if (!ExamplesExist())
                return "\n-> Error normalizing examples inputs - no examples exist.";

            // PREPARING ARRAY WITH MAX VALUES OF EXAMPLES INPUTS
            string result = "";
            int inputsNum = trainExamples[0].SayXLength();
            float[] maxValues = new float[inputsNum];
            for (int inputInd = 0; inputInd < maxValues.Length; inputInd++)
            {
                maxValues[inputInd] = float.MinValue;
            }

            // SEATCHING FOR MAXIMUMS OVER ALL TRAINING EXAMPLES
            for (int exampleInd = 0; exampleInd < trainExamples.Count; exampleInd++)
            {
                // GOING THROUGH ALL INPUTS
                for (int inputInd = 0; inputInd < maxValues.Length; inputInd++)
                {
                    if (trainExamples[exampleInd].SayX(inputInd) > maxValues[inputInd])
                        maxValues[inputInd] = trainExamples[exampleInd].SayX(inputInd);
                }
            }

            // SEATCHING FOR MAXIMUMS OVER ALL TESTING EXAMPLES IF THEY EXIST
            if (TestExamplesExist())
            {
                for (int exampleInd = 0; exampleInd < testExamples.Count; exampleInd++)
                {
                    // GOING THROUGH ALL INPUTS
                    for (int inputInd = 0; inputInd < maxValues.Length; inputInd++)
                    {
                        if (testExamples[exampleInd].SayX(inputInd) > maxValues[inputInd])
                            maxValues[inputInd] = testExamples[exampleInd].SayX(inputInd);
                    }
                }
            }

            // MAKING MAX VALUES NON-NEGATIVE
            for (int inputInd = 0; inputInd < maxValues.Length; inputInd++)
            {
                if (maxValues[inputInd] < 0)
                    maxValues[inputInd] = -maxValues[inputInd];

                if (maxValues[inputInd] == 0)
                    maxValues[inputInd] = 1;
            }

            // PRINTING MAXIMUMS IF ASKED
            if (showMaximums)
            {
                PrintClass.PrintLine("\n-> Normalizing inputs using maximums:\n" + maxValues[0].ToString());
                for (int inputInd = 1; inputInd < maxValues.Length; inputInd++)
                {
                    PrintClass.Print(", " + maxValues[inputInd].ToString());
                }
            }

            // SCALING INPUTS OF ALL TRAINING EXAMPLES ACCORDING TO FOUND MAXIMUMS
            for (int exampleInd = 0; exampleInd < trainExamples.Count; exampleInd++)
            {
                // GOING THROUGH ALL INPUTS
                for (int inputInd = 0; inputInd < maxValues.Length; inputInd++)
                {
                    float tmpFloat = trainExamples[exampleInd].SayX(inputInd);
                    tmpFloat /= maxValues[inputInd];
                    result = trainExamples[exampleInd].SetX(inputInd, tmpFloat);
                }
            }

            // SCALING INPUTS OF ALL TESTING EXAMPLES IF THEY EXIST ACCORDING TO FOUND MAXIMUMS
            if (TestExamplesExist())
            {
                for (int exampleInd = 0; exampleInd < testExamples.Count; exampleInd++)
                {
                    // GOING THROUGH ALL INPUTS
                    for (int inputInd = 0; inputInd < maxValues.Length; inputInd++)
                    {
                        float tmpFloat = testExamples[exampleInd].SayX(inputInd);
                        tmpFloat /= maxValues[inputInd];
                        result = trainExamples[exampleInd].SetX(inputInd, tmpFloat);
                    }
                }
            }

            PrintClass.PrintLine("\n-> Inputs normalized to 0...1.");

            return "";
        }


        /// <summary>
        /// Gives a link to an example with inputed index as an <i>out</i> variable.
        /// </summary>
        /// <param name="exampleInd">Index of examples to return.</param>
        /// <param name="theExample">The <i>out</i> variable with link on target example or null on error.</param>
        /// <param name="ifTestExampleType">Input for choosing type of example to return: <i>false</i> - for teaching examples (default), <i>true</i> - for test examples.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string GiveExampleLink(int exampleInd, out Example theExample, bool ifTestExampleType = false)
        {
            theExample = null;
            if (ifTestExampleType)
            {
                if (!TestExamplesExist())
                    return "\n-> Error getting test example by index - no test examples exists.";

                if ((exampleInd < 0) || (exampleInd >= testExamples.Count))
                    return "\n-> Error getting test example by index - index (" + exampleInd.ToString() + ") is out of boundaries (0..." + (testExamples.Count - 1).ToString() + ").";

                theExample = testExamples[exampleInd];
            }
            else
            {
                if (!ExamplesExist())
                    return "\n-> Error getting example by index - no examples exists.";

                if ((exampleInd < 0) || (exampleInd >= trainExamples.Count))
                    return "\n-> Error getting example by index - index (" + exampleInd.ToString() + ") is out of boundaries (0..." + (trainExamples.Count - 1).ToString() + ").";

                theExample = trainExamples[exampleInd];
            }

            return "";
        }
        
        
        /// <summary>
        /// Returns training examples as an array of Example classes.
        /// </summary>
        /// <returns>Array of training examples Example classes.</returns>
        public Example[] GiveTrainExamples()
        {
            return trainExamples.ToArray();
        }


        /// <summary>
        /// Returns a batch of training examples from training examples bank with inputed index, shuffled or not, originals or copies.
        /// </summary>
        /// <param name="batchInd">Index of training examples batch to get.</param>
        /// <param name="shuffled">Set to true - returned batch of examples will be shuffled, set to false (default) - no shuffle.</param>
        /// <param name="cloneExamples">Set to true - returned array holds copies of original training examples Example classes, set to false (default) - array holds original classes.</param>
        /// <returns>Array of Example classes with examples of batch with certain index.</returns>
        public Example[] GiveBatchByInd(int batchInd, bool shuffled = false, bool cloneExamples = false)
        {
            // CHECKING INDEX TO BE INSIDE BOUNDARIES
            if (batchInd >= SayBatchNumber())
                return null;

            List<Example> tmpBatch = new List<Example>();
            List<Example> examplesBatch;

            // COUNTING BATCH SIZE AND THIS BATCH'S SHIFT IN EXAMPLES VECTOR
            int currBatchSize = SayBatchSize(batchInd);
            int shift = batchInd * batchSize;

            if (currBatchSize == 0)
                return null;

            // COPYING EXAMPLES (LINKS OR CLONES) TO A TEMPORARY LIST
            for (int exampleInd = 0; exampleInd < currBatchSize; exampleInd++)
            {
                if (cloneExamples)
                    tmpBatch.Add(trainExamples[shift + exampleInd].Clone());
                else
                    tmpBatch.Add(trainExamples[shift + exampleInd]);
            }

            if (shuffled)
            {
                // SHUFFLING EXAMPLES BEFORE RETURN
                examplesBatch = GiveShuffledExamples(tmpBatch);
            }
            else
                // NO SHUFFLE
                examplesBatch = tmpBatch;

            return examplesBatch.ToArray();
        }


        // RETURNING TEST EXAMPLES
        /// <summary>
        /// Returns testing examples as an array of Example classes.
        /// </summary>
        /// <returns>Array of testing examples Example classes.</returns>
        public Example[] GiveTestExamples()
        {
            return testExamples.ToArray();
        }


        /// <summary>
        /// Prints using PrintClass the names of files with examples file extension from examples subdirectory.
        /// </summary>
        /// <returns>Empty string on success or error message.</returns>
        public string ShowFileList()
        {
            // GETTING FILES
            string[] items;
            items = System.IO.Directory.GetFiles(fileDirectory + "\\", "*." + fileExtension);

            PrintClass.PrintLine("\n-> All examples files in " + fileDirectory + " directory with " + fileExtension + " extension:");
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = items[i].Remove(0, items[i].IndexOf('\\') + 1);
                PrintClass.PrintLine("- " + items[i]);
            }
            PrintClass.PrintLine("\n   End of examples file list.");

            return "";
        }


        /// <summary>
        /// Using PrintClass prints information about training and testing examples amount and input and output vectors lengths, including the vectors values if selected.
        /// </summary>
        /// <param name="showDetailes">Set to true - prints values of input and output vectors of all examples.</param>
        public void ShowExamples(bool showDetailes = false)
        {
            if ((trainExamples == null) || (trainExamples.Count == 0))
                PrintClass.PrintLine("\n-> Can't show examples - no examples exist.");
            else
            {
                // PRINTING MAIN BANK EXAMPLES
                PrintClass.PrintLine("\n-> Total number of examples for learning: " + trainExamples.Count.ToString());
                if (showDetailes)
                {
                    for (int exampleInd = 0; exampleInd < trainExamples.Count; exampleInd++)
                    {
                        PrintExample(ref trainExamples, exampleInd);
                    }
                }
                else
                {
                    PrintClass.PrintLine("Examples have number of inputs: " + trainExamples.First().SayXLength().ToString() + ", number of outputs: " + trainExamples.First().SayYLength().ToString() + ".");
                }

                // PRINTING TEST EXAMPLES IF THEY EXIST
                if ((testExamples != null) && (testExamples.Count > 0))
                {
                    PrintClass.PrintLine("\n-> Total number of examples for testing: " + testExamples.Count.ToString());
                    if (showDetailes)
                    {
                        for (int exampleInd = 0; exampleInd < testExamples.Count; exampleInd++)
                        {
                            PrintExample(ref testExamples, exampleInd, true);
                        }
                    }
                }
            }
        }


        // PRINTING EXAMPLE WITH INDEX
        public void ShowExampleWithIndex(int index, bool testExample = false)
        {
            if (testExample)
            {
                // CHECKING TEST EXAMPLES TO EXIST
                if ((testExamples == null) || (testExamples.Count == 0))
                    PrintClass.PrintLine("\n-> Can't show exact example for testing - no test examples exist.");
                else
                {
                    if ((index < 0) || (index >= testExamples.Count))
                        PrintClass.PrintLine("\n-> Can't show exact example for testing - index (" + index.ToString() + ") out of boundaries (0..." + (testExamples.Count - 1).ToString() + ").");
                    else
                    {
                        PrintExample(ref testExamples, index, true);
                    }
                }
            }
            else
            {
                // CHECKING LEARNING EXAMPLES TO EXIST
                if ((trainExamples == null) || (trainExamples.Count == 0))
                    PrintClass.PrintLine("\n-> Can't show exact example for learning - no learning examples exist.");
                else
                {
                    if ((index < 0) || (index >= trainExamples.Count))
                        PrintClass.PrintLine("\n-> Can't show exact example for learning - index (" + index.ToString() + ") out of boundaries (0..." + (trainExamples.Count - 1).ToString() + ").");
                    else
                    {
                        PrintExample(ref trainExamples, index);
                    }
                }
            }
        }


        /// <summary>
        /// SHUFFLES EXISTING TRAINING EXAMPLES
        /// </summary>
        public void ShuffleTrainExamples()
        {
            if ((trainExamples != null) && (trainExamples.Count != 0))
            {
                // TEMPORARY EXAMPLES BANK WITH EXAMPLES OR THEIR CLONES
                List<Example> tmpExamples = CopyExamples(trainExamples);

                // SHUFFLING EXAMPLES BEFORE RETURN
                Random randomGen = new Random();
                int tmpInd;
                trainExamples.Clear();

                while (tmpExamples.Count > 0)
                {
                    tmpInd = randomGen.Next(tmpExamples.Count);
                    trainExamples.Add(tmpExamples[tmpInd]);
                    tmpExamples.RemoveAt(tmpInd);
                }
            }
        }


        /// <summary>
        /// Calculates minimal error of given examples bank, caused by different outputs of same input vectors.
        /// </summary>
        /// <param name="packOfExamples">The examples bank to analyze, list type.</param>
        /// <param name="minError">Output parameter for result value if it is needed.</param>
        /// <returns></returns>
        public string CalcMinError(Example[] packOfExamples, out float minError, bool showProgress = false)
        {
            minError = -1;

            if ((packOfExamples == null) || (packOfExamples.Length == 0))
                return "\n-> Error calculating minimal examples error - no examples exist.";

            // CLEARING NOTES OF ALL EXAMPLES
            foreach (var calcExample in packOfExamples)
            {
                calcExample.note = "";
            }

            int outputsErrorsSum = 0;                                   // Cumulative variable for errors of different outputs of same input vectors
            List<int> differentOutputs = new List<int>();               // List of outputs, that have the same input, as current
            List<int> differentOutputsNums = new List<int>();           // Number of found duplicate outputs
            float percentStep = (packOfExamples.Length - 1) / 100;

            if (showProgress)
                PrintClass.PrintLine("\n-> Calculating minimal error over " + packOfExamples.Length.ToString() + " :");

            // GOING THROUGH ALL EXAMPLES EXCEPT LAST ONE
            for (int exInd = 0; exInd < packOfExamples.Length - 1; exInd++)
            {
                // CHECKING IF CURRENT EXAMPLE IS NOT MARKED WITH "PASSED"
                if (packOfExamples[exInd].note.ToLower() != "passed")
                {
                    // ADDING CURRENT EXAMPLE OUTPUT TO THE LIST
                    differentOutputs.Add(packOfExamples[exInd].SayExpected());
                    differentOutputsNums.Add(1);

                    // GOING THROUGH ALL OF THE REST EXAMPLES AFTER THE CURRENT ONE
                    for (int otherExInd = exInd + 1; otherExInd < packOfExamples.Length; otherExInd++)
                    {
                        // CHECKING IF EXAMPLE IS NO MARKED AS "PASSED"
                        if (packOfExamples[otherExInd].note.ToLower() != "passed")
                        {
                            // CHECKING IF ITS INPUT IS SAME AS THE CURRENT EXAMPLE HAS
                            if (EqualArrays(packOfExamples[exInd].SayX(), packOfExamples[otherExInd].SayX()))
                            {
                                // MARKING THE OTHER EXAMPLE AS "PASSED"
                                packOfExamples[otherExInd].note = "passed";

                                // CHECKING IF DIFFERENT OUTPUTS LIST ALLREADY HAS OTHER EXAMPLE'S OUTPUT
                                int otherExampleOutputInd = packOfExamples[otherExInd].SayExpected();

                                if (differentOutputs.Contains(otherExampleOutputInd))
                                {
                                    // INCREMENTING THIS DUPLICATE NUMBER
                                    differentOutputsNums[differentOutputs.IndexOf(otherExampleOutputInd)]++;
                                }
                                else
                                {
                                    // REGISTERING THIS DUPLICATE IN THE LIST WITH AMOUNT 1
                                    differentOutputs.Add(otherExampleOutputInd);
                                    differentOutputsNums.Add(1);
                                }
                            }
                        }

                        if (showProgress)
                        {
                            float tmpFloat = exInd * 100;
                            if ((exInd + 1) % percentStep == 0)
                                PrintClass.RePrint(((int)Math.Floor((double)exInd / percentStep)).ToString() + "%");
                        }
                    }

                    // AFTER PASSING ALL EXAMPLES NEXT TO THE CURRENT CALCULATING THE SUM OF NUMBERS OF DIFFERENT OUTPUTS LETS SUM THEM EXCEPT THE MOST OFTEN ONE - IT IS CONSIDERED AS CORRECT ONE
                    int maxDuplicateNum = 0;

                    for (int outputInd = 0; outputInd < differentOutputs.Count; outputInd++)
                    {
                        outputsErrorsSum += differentOutputsNums[outputInd];
                        if (differentOutputsNums[outputInd] > maxDuplicateNum)
                            maxDuplicateNum = differentOutputsNums[outputInd];
                    }

                    outputsErrorsSum -= maxDuplicateNum;

                    // CLEARING LISTS OF DUPLICATES AND THEIR NUMBERS
                    differentOutputs.Clear();
                    differentOutputsNums.Clear();
                }
            }

            if (showProgress)
                PrintClass.RePrint(" Calculation of minimal error is done.\n");

            // REMOVING "PASSED" NOTES
            foreach (var calcExample in packOfExamples)
            {
                calcExample.note = "";
            }

            minError = outputsErrorsSum;

            // PRINTING RESULT
            PrintClass.PrintLine("\n-> Minimal result error : " + (Math.Round((double)outputsErrorsSum * 100 / packOfExamples.Length)).ToString() + "% (" + outputsErrorsSum.ToString() + "/" + packOfExamples.Length.ToString() + "),\n" +
                "maximum precision: " + (Math.Round((double)(packOfExamples.Length - outputsErrorsSum) * 100 / packOfExamples.Length)).ToString() + "% (" + (packOfExamples.Length - outputsErrorsSum).ToString() + "/" + packOfExamples.Length.ToString() + ").");

            return "";
        }


        ////////// PRIVATE METHODS

        /// <summary>
        /// Returns list of inputed Example class shuffled, holding originals or copies.
        /// </summary>
        /// <param name="examplesToShuffle">List of Example classes to shuffle.</param>
        /// <param name="cloneExamples">Set to true - returned list holds copies of classes, set to false (default) - holds originals.</param>
        /// <returns>Empty string on success or error message.</returns>
        private List<Example> GiveShuffledExamples(List<Example> examplesToShuffle, bool cloneExamples = false)
        {
            if ((examplesToShuffle == null) || (examplesToShuffle.Count == 0))
                return null;

            // TEMPORARY EXAMPLES BANK WITH EXAMPLES OR THEIR CLONES
            List<Example> tmpExamples = CopyExamples(examplesToShuffle, cloneExamples);

            // SHUFFLING EXAMPLES BEFORE RETURN
            Random randomGen = new Random();
            int tmpInd;
            List<Example> shuffledExamples = new List<Example>();

            while (tmpExamples.Count > 0)
            {
                tmpInd = randomGen.Next(tmpExamples.Count);
                shuffledExamples.Add(tmpExamples[tmpInd]);
                tmpExamples.RemoveAt(tmpInd);
            }

            return shuffledExamples;
        }


        /// <summary>
        /// Returns a list of originals or copies of Example classes from inputed list.
        /// </summary>
        /// <param name="examplesToCopy">List of Example classes to be copied.</param>
        /// <param name="cloneExamples">Set to true - returned list holds copies of the Example classes, set to false (default) - holds originals.</param>
        /// <returns>Empty string on success or error message.</returns>
        private List<Example> CopyExamples(List<Example> examplesToCopy, bool cloneExamples = false)
        {
            List<Example> examplesCopy = new List<Example>();
            if (cloneExamples)
            {
                foreach (var example in examplesToCopy)
                {
                    examplesCopy.Add(example.Clone());
                }
            }
            else
            {
                foreach (var example in examplesToCopy)
                {
                    examplesCopy.Add(example);
                }
            }

            return examplesCopy;
        }


        /// <summary>
        /// Uses PrintClass to print values of an example with certain index from 
        /// </summary>
        /// <param name="examplesToPrintFrom">List of Example classes to print an example from.</param>
        /// <param name="index">Index of printing example in inputed list.</param>
        /// <param name="testExample">Set to true - to print an example as a testing one, set to false (default) - to print as a training one.</param>
        private void PrintExample(ref List<Example> examplesToPrintFrom, int index, bool testExample = false)
        {
            // PRINTING INPUTS WITH THEIR INDEXES
            float[] x = examplesToPrintFrom[index].SayX();
            PrintClass.Print("\nExample ");

            if (testExample)
                PrintClass.Print("(for testing)");
            else
                PrintClass.Print("(for learning)");

            PrintClass.PrintLine( " N " + index.ToString() + " , inputs:");

            for (int inputInd = 0; inputInd < x.Length; inputInd++)
            {
                PrintClass.PrintLine("Input " + inputInd.ToString() + " = " + x[inputInd].ToString());
            }

            // PRINTING OUTPUTS WITH THEIR INDEXES
            float[] y = examplesToPrintFrom[index].SayY();
            PrintClass.PrintLine("Outputs:");

            for (int outputInd = 0; outputInd < y.Length; outputInd++)
            {
                PrintClass.PrintLine("Output " + outputInd.ToString() + " = " + y[outputInd].ToString());
            }
        }


        /// <summary>
        /// Checking to float arrays to be equal.
        /// </summary>
        /// <param name="arr1">Array 1.</param>
        /// <param name="arr2">Array 2.</param>
        /// <returns>True if both arrays lengths and item values are equal each other, or returns false.</returns>
        public bool EqualArrays(float[] arr1, float[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            for (int inputInd = 0; inputInd < arr1.Length; inputInd++)
            {
                if (arr1[inputInd] != arr2[inputInd])
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Check if training examples exist.
        /// </summary>
        /// <returns>True if training examples bank is not null and empty, or false.</returns>
        public bool ExamplesExist()
        {
            if ((trainExamples != null) && (trainExamples.Count > 0))
                return true;

            return false;
        }


        /// <summary>
        /// Check if training examples exist.
        /// </summary>
        /// <returns>True if training examples bank is not null and empty, or false.</returns>
        public bool TestExamplesExist()
        {
            if ((testExamples != null) && (testExamples.Count > 0))
                return true;

            return false;
        }


        ////////// CONSTRUCTOR

        /// <summary>
        /// Initializes Examples class, private for singleton realization.
        /// </summary>
        private Examples()
        {
            trainExamples = new List<Example>();
            testExamples = new List<Example>();
            testExamples = null;
            batchSize = 1;
            fileExtension = "exs";
            fileDirectory = "EXS";
        }
    }



    // CLASS WITH ONE EXAMPLE - AN ARRAY OF INPUTS AND AN ARRAY OF OUTPUTS
    /// <summary>
    /// A class, representing one example for network routines and methods for working with it.
    /// </summary>
    class Example
    {
        protected float[] x = null;                     // Inputs x of the example
        protected float[] y = null;                     // Outputs y of the example

        public string note = "";                        // For some notes

        /// <summary>
        /// Returns input vector of the examples as a float array.
        /// </summary>
        /// <returns>Input vector of the example as a float array.</returns>
        public float[] SayX()
        {
            return x;
        }


        /// <summary>
        /// Returns value of the example's input with certain index or raises an examption if index is out of the input vector boundaries.
        /// </summary>
        /// <param name="index">Index of input to be returned.</param>
        /// <returns>The value of the input with passed index.</returns>
        public float SayX(int index)
        {
            if (index >= x.Length)
                throw new NotImplementedException("Error saying certain input of the example - index (" + index.ToString() + ") exceeds inputs length (0.." + x.Length.ToString() + ").");

            return x[index];
        }


        /// <summary>
        /// Returns the number of inputs of the example.
        /// </summary>
        /// <returns>Number of inputs of the example.</returns>
        public int SayXLength()
        {
            return x.Length;
        }


        /// <summary>
        /// Returns output vector of the example as a float array.
        /// </summary>
        /// <returns>Output vector of the example as a float array.</returns>
        public float[] SayY()
        {
            return y;
        }


        /// <summary>
        /// Returns value of the example's output with certain index or raises an examption if index is out of the output vector boundaries.
        /// </summary>
        /// <param name="index">Index of output to be returned.</param>
        /// <returns>The value of the output with passed index.</returns>
        public float SayY(int index)
        {
            if (index >= y.Length)
                throw new NotImplementedException("Error saying certain output of the example - index (" + index.ToString() + ") exceeds outputs length (0.." + y.Length.ToString() + ").");

            return y[index];
        }


        /// <summary>
        /// Returns the number of outputs of the example.
        /// </summary>
        /// <returns>Number of outputs of the example.</returns>
        public int SayYLength()
        {
            return y.Length;
        }


        /// <summary>
        /// Returns index of the output with maximum value.
        /// </summary>
        /// <returns>Index of the output with maximum value.</returns>
        public int SayExpected()
        {
            float max = y[0];
            int answer = 0;
            for (int outputInd = 0; outputInd < y.Length; outputInd++)
            {
                if (y[outputInd] > max)
                {
                    max = y[outputInd];
                    answer = outputInd;
                }
            }

            return answer;
        }


        /// <summary>
        /// Sets the inputed vector as the input vector of the example.
        /// </summary>
        /// <param name="newX">New vector to be example's inputs.</param>
        public void SetX(float[] newX)
        {
            x = newX;
        }


        /// <summary>
        /// Changes value of input with certain index to new inputed value.
        /// </summary>
        /// <param name="xInd">Index of input to change.</param>
        /// <param name="newX">New value for changing input.</param>
        /// <returns>Empty string on success or error message.</returns>
        public string SetX(int xInd, float newX)
        {
            if ((xInd < 0) || (xInd >= x.Length))
                return "\n-> Error changing examples input with index " + xInd.ToString() + ", it is out of boundaries (0..." + (x.Length - 1).ToString() + ").";

            x[xInd] = newX;

            return "";
        }


        /// <summary>
        /// Sets the inputed vector as the output vector of the example.
        /// </summary>
        /// <param name="newY">New vector to be example's outputs.</param>
        public void SetY(float[] newY)
        {
            y = newY;
        }


        /// <summary>
        /// Returns a clone of current Example class.
        /// </summary>
        /// <returns>Clone of current Example class.</returns>
        public Example Clone()
        {
            return new Example(SayX(), SayY());
        }


        /// <summary>
        /// Creates new Example class object with inputed input and output vector.
        /// </summary>
        /// <param name="newX">New example's input vector.</param>
        /// <param name="newY">New example's output vector.</param>
        public Example(float[] newX, float[] newY)
        {
            if ((newX.Length == 0) || (newY.Length == 0))
                throw new Exception("\n-> Error initializing new example: inputs or outputs array with zero size.");

            x = newX;
            y = newY;
        }
    }
}
