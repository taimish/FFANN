using System;
using System.Collections.Generic;
using System.Text;

namespace FFANN
{
    /// <summary>
    /// Abstract class for cost function subclasses of different types
    /// </summary>
    public abstract class CFTypeBase
    {
        /// <summary>
        /// Calculates cost of single output vector and target vector.
        /// </summary>
        /// <param name="output">Output vector.</param>
        /// <param name="target">Target vector.</param>
        /// <returns>Cost of output and target vectors.</returns>
        public abstract float CalcSingleCost(float[] output, float[] target);


        /// <summary>
        /// Calculates derivative of cost of single output vector and target vector.
        /// </summary>
        /// <param name="output">Output vector.</param>
        /// <param name="target">Target vector.</param>
        /// <returns>Derivative of cost of output and target vectors.</returns>
        public abstract float[] CalcDerivatives(float[] output, float[] target);


        /// <summary>
        /// Returns index of the cost function.
        /// </summary>
        /// <returns>Index of the cost function.</returns>
        public abstract int Index();
    }


    /// <summary>
    /// Subclass for quadratic cost function
    /// </summary>
    class CFTypeQuadratic : CFTypeBase
    {
        public const string name = "QuadraticCFType";           // Name of the cost function
        public const int index = 1;                             // Index of the cost function


        /// <summary>
        /// Calculates cost of single output vector and target vector.
        /// </summary>
        /// <param name="output">Output vector.</param>
        /// <param name="target">Target vector.</param>
        /// <returns>Cost of output and target vectors.</returns>
        public override float CalcSingleCost(float[] output, float[] target)
        {
            // CHECKING LENGTHS OF INPUTED VECTORS TO BE EQUAL
            if (output.Length != target.Length)
                throw new NotImplementedException("Error calculating quadratic cost - different length of outpus and target vectors.");

            float sum = 0;
            for (int outputInd = 0; outputInd < output.Length; outputInd++)
            {
                sum += (target[outputInd] - output[outputInd]) * (target[outputInd] - output[outputInd]);
            }
            return sum / 2;
        }


        /// <summary>
        /// Calculates derivative of cost of single output vector and target vector.
        /// </summary>
        /// <param name="output">Output vector.</param>
        /// <param name="target">Target vector.</param>
        /// <returns>Derivative of cost of output and target vectors.</returns>
        public override float[] CalcDerivatives(float[] output, float[] target)
        {
            // CHECKING LENGTHS OF INPUTED VECTORS TO BE EQUAL
            if (output.Length != target.Length)
                throw new NotImplementedException("Error calculating quadratic derivatives - different length of outpus and target vectors.");

            // CALCULATING DERIVATIVES FOR EACH PAIR OF OUTPUT AND TARGET
            float[] derivatives = new float[output.Length];
            for (int outputInd = 0; outputInd < derivatives.Length; outputInd++)
            {
                derivatives[outputInd] = output[outputInd] - target[outputInd];
            }
            return derivatives;
        }


        /// <summary>
        /// Returns name of the cost function.
        /// </summary>
        /// <returns>Name of cost function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the cost function.
        /// </summary>
        /// <returns>Index of the cost function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for cross-entropy cost function
    /// </summary>
    class CFTypeCrossEntropy : CFTypeBase
    {
        public const string name = "CrossEntropyCFType";        // Name of the cost function
        public const int index = 2;                             // Index of the cost function


        /// <summary>
        /// Calculates cost of single output vector and target vector.
        /// </summary>
        /// <param name="output">Output vector.</param>
        /// <param name="target">Target vector.</param>
        /// <returns>Cost of output and target vectors.</returns>
        public override float CalcSingleCost(float[] output, float[] target)
        {
            // CHECKING LENGTHS OF INPUTED VECTORS TO BE EQUAL
            if (output.Length != target.Length)
                throw new NotImplementedException("Error calculating cross-entropy cost - different length of x and y vectors.");

            float cost = 0;

            for (int outputInd = 0; outputInd < output.Length; outputInd++)
            {
                cost -= (float)(target[outputInd] * Math.Log(output[outputInd]) + (1 - target[outputInd]) * Math.Log(1 - output[outputInd]));
            }
            return cost;
        }


        /// <summary>
        /// Calculates derivative of cost of single output vector and target vector.
        /// </summary>
        /// <param name="output">Output vector.</param>
        /// <param name="target">Target vector.</param>
        /// <returns>Derivative of cost of output and target vectors.</returns>
        public override float[] CalcDerivatives(float[] output, float[] target)
        {
            // CHECKING LENGTHS OF INPUTED VECTORS TO BE EQUAL
            if (output.Length != target.Length)
                throw new NotImplementedException("Error calculating cross-entropy derivatives - different length of outpus and target vectors.");

            float[] derivatives = new float[output.Length];
            for (int neuronInd = 0; neuronInd < derivatives.Length; neuronInd++)
            {
                derivatives[neuronInd] = -(target[neuronInd] / output[neuronInd] - (1 - target[neuronInd]) / (1 - output[neuronInd]));
            }

            return derivatives;
        }


        /// <summary>
        /// Returns name of the cost function.
        /// </summary>
        /// <returns>Name of cost function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the cost function.
        /// </summary>
        /// <returns>Index of the cost function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for log-likelihood cost function
    /// </summary>
    class CFTypeLogLikelihood : CFTypeBase
    {
        public const string name = "LogLikelihoodCFType";       // Name of the cost function
        public const int index = 3;                             // Index of the cost function


        /// <summary>
        /// Calculates cost of single output vector and target vector.
        /// </summary>
        /// <param name="output">Output vector.</param>
        /// <param name="target">Target vector.</param>
        /// <returns>Cost of output and target vectors.</returns>
        public override float CalcSingleCost(float[] output, float[] target)
        {
            // CHECKING LENGTHS OF INPUTED VECTORS TO BE EQUAL
            if (output.Length != target.Length)
                throw new NotImplementedException("Error calculating log-likelihood cost - different length of x and y vectors.");

            // SEARCHING FOR INDEX OF MAXIMUM TAGET OUTPUT - THAT IS MOST LIKELY
            float maxOutput = target[0];
            int maxInd = 0;
            for (int outputInd = 1; outputInd < target.Length; outputInd++)
            {
                if (target[outputInd] > maxOutput)
                {
                    maxOutput = target[outputInd];
                    maxInd = outputInd;
                }
            }

            // RETURNING LN OF THE OUTPUT WITH THAT INDEX, MULTIPLIED ON OUTPUTS NUMBER TO COMPENSATE FINALE COST DIVISION 
            return (float)-Math.Log(output[maxInd]) * target.Length;
        }


        /// <summary>
        /// Calculates derivative of cost of single output vector and target vector.
        /// </summary>
        /// <param name="output">Output vector.</param>
        /// <param name="target">Target vector.</param>
        /// <returns>Derivative of cost of output and target vectors.</returns>
        public override float[] CalcDerivatives(float[] output, float[] target)
        {
            // CHECKING LENGTHS OF INPUTED VECTORS TO BE EQUAL
            if (output.Length != target.Length)
                throw new NotImplementedException("Error calculating log-likelihood derivatives - different length of outpus and target vectors.");

            float[] derivatives = new float[output.Length];

            for (int outputInd = 0; outputInd < derivatives.Length; outputInd++)
            {
                derivatives[outputInd] = (output[outputInd] - target[outputInd]) / (output[outputInd] * (1 - output[outputInd]));
            }

            return derivatives;
        }


        /// <summary>
        /// Returns name of the cost function.
        /// </summary>
        /// <returns>Name of cost function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the cost function.
        /// </summary>
        /// <returns>Index of the cost function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for NoChange type, designed for inputing type of cost function, that indicates, that no changes are needed.
    /// </summary>
    class CFTypeNoChange : CFTypeBase
    {
        public const string name = "NoChangeCFType";            // Name of the cost function
        public const int index = 4;                             // Index of the cost function


        /// <summary>
        /// Realization of cost function calculation for NoChange subclass - rises exception as not mentioned to be called.
        /// </summary>
        /// <param name="output">Useless input.</param>
        /// <param name="target">Useless input.</param>
        /// <returns>Raises exception.</returns>
        public override float CalcSingleCost(float[] output, float[] target)
        {
            throw new NotImplementedException("\n-> Error trying to calculate cost function with type 'NoChange'.");
        }


        /// <summary>
        /// Realization of cost function derivative calculation for NoChange subclass - rises exception as not mentioned to be called.
        /// </summary>
        /// <param name="output">Useless input.</param>
        /// <param name="target">Useless input.</param>
        /// <returns>Raises exception.</returns>
        public override float[] CalcDerivatives(float[] output, float[] target)
        {
            throw new NotImplementedException("\n-> Error trying to calculate derivatives of cost function with type 'NoChange'.");
        }


        /// <summary>
        /// Returns name of the cost function.
        /// </summary>
        /// <returns>Name of cost function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the cost function.
        /// </summary>
        /// <returns>Index of the cost function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Enum for currently supported cost functions.
    /// </summary>
    public enum CFTypeStrings
    {
        QuadraticCFType,
        CrossEntropyCFType,
        LogLikelihood,
        NoChangeCFType
    }
}
