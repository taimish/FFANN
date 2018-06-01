using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFANN
{
    class Program
    {
        static void Main(string[] args)
        {
            ANNetwork currANN = null;

            string command;
            Commands allCommands = new Commands(ref currANN);

            allCommands.Welcome();

            // WORKING CYCLE
            while (true)
            {
                Console.Write("\n<- ");
                command = Console.ReadLine();
                command = command.Trim();
                string lowerCommand = command.ToLower();
                if (lowerCommand.Length > 0)
                {

                    // HELP COMMAND ENTERED?
                    if (lowerCommand.IndexOf("help") == 0)
                    {
                        allCommands.Help();
                    }


                    // NEW NETWORK COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("newnet") == 0)
                    {
                        string result = allCommands.CreateNewANN(command.Remove(0, 6).Trim(), ref currANN);
                        if (result.Length > 0)
                            Console.WriteLine(result);
                        else
                            Console.WriteLine("\n-> Network created successfully.");
                    }


                    // SAVENET COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("savenet") == 0)
                    {
                        string result = allCommands.SaveNetwork(command.Remove(0, 7).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                        else
                            Console.WriteLine("\n-> Network saved to the file successfully.");
                    }


                    // LOADNET COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("loadnet") == 0)
                    {
                        string result = allCommands.LoadNetwork(command.Remove(0, 7).Trim(), ref currANN);
                        if (result.Length > 0)
                            Console.WriteLine(result);
                        else
                            Console.WriteLine("\n-> Network loaded from the file successfully.");
                    }


                    // SHOW COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("shownet") == 0)
                    {
                        string result = allCommands.ShowNet(command.Remove(0, 7).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // LISTNET COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("listnet") == 0)
                    {
                        allCommands.ListNets();
                    }


                    // LISTEX COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("listex") == 0)
                    {
                        allCommands.ListExs();
                    }


                    // CHANGE INPUTS NUMBER COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("setinpnum") == 0)
                    {
                        string result = allCommands.SetInputsNumber(command.Remove(0, 9).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // CHANGE COST FUNCTION COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("setcf") == 0)
                    {
                        string result = allCommands.SetCFType(command.Remove(0, 5).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // CHANGE ACTIVATION FUNCTION COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("setaf") == 0)
                    {
                        string result = allCommands.SetAFTypes(command.Remove(0, 5).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // CHANGE TRAINING SPEED COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("setls") == 0)
                    {
                        string result = allCommands.SetLearningSpeed(command.Remove(0, 5).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // CHANGE LYAMBDA COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("setlyambda") == 0)
                    {
                        string result = allCommands.SetLyambda(command.Remove(0, 10).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // RESET LESSONS NUMBER COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("resetless") == 0)
                    {
                        string result = allCommands.ResetLessonsNumber(command.Remove(0, 9).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // NEW EXAMPLE COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("newex") == 0)
                    {
                        string result = allCommands.NewExample(command.Remove(0, 5).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // SAVE EXAMPLES COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("saveex") == 0)
                    {
                        string result = allCommands.SaveExamples(command.Remove(0, 6).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                        else
                            Console.WriteLine("\n-> Examples saved to the file successfully.");
                    }

                    
                    // LOADEX COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("loadex") == 0)
                    {
                        string result = allCommands.LoadExamples(command.Remove(0, 6).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                        else
                            Console.WriteLine("\n-> Examples loaded from the file successfully.");
                    }


                    // LOADCSV COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("loadcsv") == 0)
                    {
                        string result = allCommands.LoadExamplesFromCSV(command.Remove(0, 7).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                        else
                            Console.WriteLine("\n-> Examples loaded from the file successfully.");
                    }


                    // SHOW EXAMPLES COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("showex") == 0)
                    {
                        string result = allCommands.ShowExamples(command.Remove(0, 6).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // SHOW EXAMPLE BY INDEX COMMAND?
                    else if (lowerCommand.Trim().IndexOf("show1ex") == 0)
                    {
                        string result = allCommands.ShowExampleWithIndex(command.Remove(0, 7).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }
                    

                    // DIVIDE COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("divex") == 0)
                    {
                        string result = allCommands.DivideExamples(command.Remove(0, 5).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                        else
                            Console.WriteLine("\n-> Examples divided successfully.");
                    }


                    // TRAIN EXAMPLES COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("trainex") == 0)
                    {
                        string result = allCommands.TrainExamples(command.Remove(0, 7).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }
                    

                    // RUN EXAMPLE COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("runex") == 0)
                    {
                        string result = allCommands.RunExample(command.Remove(0, 5).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // CLEAR EXAMPLES COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("clearex") == 0)
                    {
                        string result = allCommands.ClearExamples();
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // COST COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("cost") == 0)
                    {
                        string result = allCommands.Cost();
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // TRAINING EXAMPLES ERROR CALCULATION COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("exerr") == 0)
                    {
                        string result = allCommands.CalcTrainingError(command.Remove(0, 5).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // MINIMAL EXAMPLES ERROR CALCULATION COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("minerr") == 0)
                    {
                        string result = allCommands.CalcMinError(command.Remove(0, 6).Trim());
                        if (result.Length > 0)
                            Console.WriteLine(result);
                    }


                    // EXIT COMMAND ENTERED?
                    else if (lowerCommand.Trim().IndexOf("exit") == 0)
                    {
                        break;
                    }


                    // UNKNOWN COMMAND!
                    else
                    {
                        Console.WriteLine("\n-> Error - unknown command: " + command);
                    }
                }
            }

            allCommands.Goodbye();
            Console.ReadLine();
        }
    }
}
