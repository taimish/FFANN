using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFANN
{
    /// <summary>
    /// Class for different parsing of inputed string argument
    /// </summary>
    public static class Parsing
    {
        /// <summary>
        /// Parses inputed string, trying to read float array, passed as a reference.
        /// </summary>
        /// <param name="tmpString">Input string argument to be parsed.</param>
        /// <param name="resultArray">Reference on the result array.</param>
        /// <param name="isLast">Reference on a variable, indicating on success if read array was last in inputed string or not.</param>
        /// <param name="parseName">Name of parsed parameter for error messages.</param>
        /// <param name="checkMode">Mode of checking read values to be positive, negative, etc.</param>
        /// <param name="delimiter">The delimiter string for parsing, default is ' '.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string ParseFloatArray(ref string tmpString, ref float[] resultArray, ref bool isLast, string parseName, AfterParsingCheck checkMode, string delimiter = " ")
        {
            // CHECKING INPUT
            if (tmpString.Length == 0)
                return "\n-> Error parsing " + parseName + ": empty input text.";

            string result = "";
            // RUNNING THROUGH ALL ALREADY CREATED ARRAY
            for (int i = 0; i < resultArray.Length; i++)
            {
                result = ParseFloat(ref tmpString, ref resultArray[i], ref isLast, parseName, checkMode, delimiter);

                if (result.Length > 0)
                    return result;

                if ((isLast) && (i < resultArray.Length - 1))
                    return "\n-> Error parsing " + parseName + ": no input text for unit " + i.ToString() + " .";
            }
            return "";
        }


        /// <summary>
        /// Parses inputed string, trying to read array of indexes of activation functions (see AFTypeBase class and its subclasses), passed as a reference.
        /// </summary>
        /// <param name="tmpString">Input string argument to be parsed.</param>
        /// <param name="resultAFTypes">Reference on result array with subclass objects.</param>
        /// <param name="isLast">Reference on a variable, indicating on success if read array was last in inputed string or not.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string ParseAFTypesArray(ref string tmpString, ref AFTypeBase[] resultAFTypes, ref bool isLast)
        {

            // CHECKING INPUT
            if (tmpString.Length == 0)
                return "\n-> Error parsing AFType indexes: empty input text.";

            int AFTypeInd = -1;
            string result = "";

            // RUNNING THROUGH ALL ALREADY CREATED ARRAY
            for (int i = 0; i < resultAFTypes.Length; i++)
            {
                result = ParseInt(ref tmpString, ref AFTypeInd, ref isLast, "LxActivationFunctionIndex", AfterParsingCheck.Positive);

                if (result.Length > 0)
                    return "\n-> Error parsing AFType indexes. " + result;

                if ((isLast) && (i < resultAFTypes.Length - 1))
                    return "\n-> Error parsing AFType indexes: no <LxActivationFunctionIndex> for layer " + i.ToString() + " .";

                // DETERMINING THE TYPE OF ACTIVATION FAUNCTION SUBCLASS
                switch (AFTypeInd)
                {
                    case 1:
                        resultAFTypes[i] = new AFTypeStep();
                        break;
                    case 2:
                        resultAFTypes[i] = new AFTypeLinear();
                        break;
                    case 3:
                        resultAFTypes[i] = new AFTypeSigmoid();
                        break;
                    case 4:
                        resultAFTypes[i] = new AFTypeTanh();
                        break;
                    case 5:
                        resultAFTypes[i] = new AFTypeSoftMax();
                        break;
                    case 6:
                        resultAFTypes[i] = new AFTypeRectifiedLinear();
                        break;
                    default:
                        return "\n-> Error parsing AFType indexes: unknown AFType index " + AFTypeInd.ToString() + " .";
                }
            }
            return "";
        }


        /// <summary>
        /// Parses inputed string, trying to read array of indexes of cost functions (see CFTypeBase class and its subclasses), passed as a reference.
        /// </summary>
        /// <param name="tmpString">Input string argument to be parsed.</param>
        /// <param name="resultCFType">Reference on result array with subclass objects.</param>
        /// <param name="isLast">Reference on a variable, indicating on success if read array was last in inputed string or not.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string ParseCFType(ref string tmpString, ref CFTypeBase resultCFType, ref bool isLast)
        {
            int CFTypeInd = -1;
            string result = ParseInt(ref tmpString, ref CFTypeInd, ref isLast, "CFType index", AfterParsingCheck.Positive);

            if (result.Length > 0)
                return "\n-> Error parsing CFType index. " + result;

            // DETERMINING THE TYPE OF COST FAUNCTION SUBCLASS
            switch (CFTypeInd)
            {
                case 1:
                    resultCFType = new CFTypeQuadratic();
                    break;
                case 2:
                    resultCFType = new CFTypeCrossEntropy();
                    break;
                case 3:
                    resultCFType = new CFTypeLogLikelihood();
                    break;
                default:
                    return "\n-> Error parsing CFType index: unknown CFType index (" + CFTypeInd.ToString() + ").";
            }

            return "";
        }


        /// <summary>
        /// Parses inputed string, trying to read a float, passed as a reference.
        /// </summary>
        /// <param name="tmpString">Input string argument to be parsed.</param>
        /// <param name="resultFloat">Reference on the result float.</param>
        /// <param name="isLast">Reference on a variable, indicating on success if read value was last in inputed string or not.</param>
        /// <param name="variableName">Name of parsed variable for error messages.</param>
        /// <param name="checkMode">Mode of checking read value to be positive, negative, etc.</param>
        /// <param name="delimiter">The delimiter string for parsing, default is ' '.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string ParseFloat(ref string tmpString, ref float resultFloat, ref bool isLast, string variableName, AfterParsingCheck checkMode, string delimiter = " ")
        {
            tmpString = tmpString.Trim();

            if (delimiter.Length == 1)
                tmpString = tmpString.Trim(delimiter.ToCharArray());

            if (tmpString.Length == 0)
                return "\n-> Error parsing <" + variableName + "> - no argument inputed.";

            if (delimiter.Length == 0)
                return "\n-> Error parsing <" + variableName + "> - empty delimiter inputed.";

            int tmpInt = tmpString.IndexOf(delimiter);

            // CHECKING IF CURRENT ARGUMENT IS THE LAST
            if (tmpInt == -1)
            {
                // CURRENT ARGUMENT IS LAST
                isLast = true;

                // TRYING TO PARSE THE INPUTED STRING
                if (!float.TryParse(tmpString, out resultFloat))
                    return "\n-> Error parsing <" + variableName + "> argument (" + tmpString + ").";

                // EMPTYING ARGUMENT STRING
                tmpString = "";
            }
            else
            {
                // CURRENT ARGUMENT IS NOT THE LAST
                isLast = false;

                if (!float.TryParse(tmpString.Remove(tmpInt), out resultFloat))
                    return "\n-> Error parsing <" + variableName + "> argument (" + tmpString.Remove(tmpInt) + ").";

                tmpString = tmpString.Remove(0, tmpInt + 1).Trim();
                if (delimiter.Length == 1)
                    tmpString = tmpString.Trim(delimiter.ToCharArray());
            }

            switch (checkMode)
            {
                case AfterParsingCheck.NoCheck:
                    break;
                case AfterParsingCheck.NonNegative:
                    if (resultFloat < 0)
                        return "\n-> Error: argument <" + variableName + "> is not non-negative (" + resultFloat.ToString() + ").";
                    break;
                case AfterParsingCheck.Positive:
                    if (resultFloat <= 0)
                        return "\n-> Error: argument <" + variableName + "> is not positive (" + resultFloat.ToString() + ").";
                    break;
                case AfterParsingCheck.Negative:
                    if (resultFloat >= 0)
                        return "\n-> Error: argument <" + variableName + "> is not negative (" + resultFloat.ToString() + ").";
                    break;
                case AfterParsingCheck.NonPositive:
                    if (resultFloat > 0)
                        return "\n-> Error: argument <" + variableName + "> is not non-positive (" + resultFloat.ToString() + ").";
                    break;
                case AfterParsingCheck.Zero:
                    if (resultFloat != 0)
                        return "\n-> Error: argument <" + variableName + "> is not a zero (" + resultFloat.ToString() + ").";
                    break;
                case AfterParsingCheck.NonZero:
                    if (resultFloat == 0)
                        return "\n-> Error: argument <" + variableName + "> is not non-zero (" + resultFloat.ToString() + ").";
                    break;
                default:
                    break;
            }

            return "";
        }


        /// <summary>
        /// Parses inputed string, trying to read an int, passed as a reference.
        /// </summary>
        /// <param name="tmpString">Input string argument to be parsed.</param>
        /// <param name="resultInt">Reference on the result int.</param>
        /// <param name="isLast">Reference on a variable, indicating on success if read value was last in inputed string or not.</param>
        /// <param name="variableName">Name of parsed variable for error messages.</param>
        /// <param name="checkMode">Mode of checking read value to be positive, negative, etc.</param>
        /// <param name="delimiter">The delimiter string for parsing, default is ' '.</param>
        /// <returns>Empty string on success or error message.</returns>
        public static string ParseInt(ref string tmpString, ref int resultInt, ref bool isLast, string variableName, AfterParsingCheck checkMode, string delimiter = " ")
        {
            tmpString = tmpString.Trim();
            if (delimiter.Length == 1)
                tmpString = tmpString.Trim(delimiter.ToCharArray());

            if (tmpString.Length == 0)
                return "Error parsing <" + variableName + "> - no argument inputed.";

            int tmpInt = tmpString.IndexOf(delimiter);

            // CHECKING IF CURRENT ARGUMENT IS THE LAST
            if (tmpInt == -1)
            {
                // THE ARGUMENT IS LAST
                isLast = true;

                // TRYING TO PARSE THE INPUTED STRING
                if (!int.TryParse(tmpString, out resultInt))
                    return "\n-> Error parsing <" + variableName + "> argument (" + tmpString + ").";

                // EMPTYING ARGUMENT STRING
                tmpString = "";
            }
            else
            {
                // CURRENT ARGUMENT IS NOT THE LAST
                isLast = false;

                if (!int.TryParse(tmpString.Remove(tmpInt), out resultInt))
                    return "\n-> Error parsing <" + variableName + "> argument (" + tmpString.Remove(tmpInt) + ").";

                tmpString = tmpString.Remove(0, tmpInt + 1).Trim();
                if (delimiter.Length == 1)
                    tmpString = tmpString.Trim(delimiter.ToCharArray());
            }

            switch (checkMode)
            {
                case AfterParsingCheck.NoCheck:
                    break;
                case AfterParsingCheck.NonNegative:
                    if (resultInt < 0)
                        return "\n-> Error: argument <" + variableName + "> is not non-negative (" + resultInt.ToString() + ").";
                    break;
                case AfterParsingCheck.Positive:
                    if (resultInt <= 0)
                        return "\n-> Error: argument <" + variableName + "> is not positive (" + resultInt.ToString() + ").";
                    break;
                case AfterParsingCheck.Negative:
                    if (resultInt >= 0)
                        return "\n-> Error: argument <" + variableName + "> is not negative (" + resultInt.ToString() + ").";
                    break;
                case AfterParsingCheck.NonPositive:
                    if (resultInt > 0)
                        return "\n-> Error: argument <" + variableName + "> is not non-positive (" + resultInt.ToString() + ").";
                    break;
                case AfterParsingCheck.Zero:
                    if (resultInt != 0)
                        return "\n-> Error: argument <" + variableName + "> is not a zero (" + resultInt.ToString() + ").";
                    break;
                case AfterParsingCheck.NonZero:
                    if (resultInt == 0)
                        return "\n-> Error: argument <" + variableName + "> is not non-zero (" + resultInt.ToString() + ").";
                    break;
                default:
                    break;
            }

            return "";
        }


        /// <summary>
        /// Enum of supported after-parsing value's checks
        /// </summary>
        public enum AfterParsingCheck
        {
            NoCheck,
            NonNegative,
            Positive,
            Negative,
            NonPositive,
            Zero,
            NonZero
        }
    }
}
