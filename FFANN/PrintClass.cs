using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFANN
{
    /// <summary>
    /// Static class for printing information for user.
    /// </summary>
    public static class PrintClass
    {
        /// <summary>
        /// Prints inputed message at the end of current line without any additinal symbols.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string Print(string message)
        {
            Console.Write(message);
            return "";
        }


        /// <summary>
        /// Prints inputed message at the end of current line additionaly ending it.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string PrintLine(string message)
        {
            Console.WriteLine(message);
            return "";
        }


        /// <summary>
        /// Prints inputed message at the beginning of current line without any additinal symbols.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string RePrint(string message)
        {
            MoveToRewrite();
            Print(message);
            return "";
        }


        /// <summary>
        /// Prints inputed message at the beginning of current line additionaly ending it.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string RePrintLine(string message)
        {
            MoveToRewrite();
            PrintLine(message);
            return "";
        }


        /// <summary>
        /// Moving cursor at the beginning of current line.
        /// </summary>
        public static void MoveToRewrite()
        {
            Console.CursorLeft = 0;
        }
    }
}
