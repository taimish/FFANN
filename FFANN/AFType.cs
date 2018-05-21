using System;
using System.Collections.Generic;
using System.Text;

namespace FFANN
{
    /// <summary>
    /// Abstract class for activation function subclasses of different types
    /// </summary>
    public abstract class AFTypeBase
    {
        /// <summary>
        /// Calculates some activation function of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of some activation function.</returns>
        public abstract float CalcActivation(float sum, float alpha);


        /// <summary>
        /// Calculates some activation function derivative of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of some activation function derivative.</returns>
        public abstract float CalcDerivative(float sum, float alpha);


        /// <summary>
        /// Returns index of the activation function.
        /// </summary>
        /// <returns>Index of the activation function.</returns>
        public abstract int Index();
    }


    /// <summary>
    /// Subclass for step activation function
    /// </summary>
    class AFTypeStep : AFTypeBase
    {
        public const string name = "StepAFType";                    // Name of the activation function
        public const int index = 1;                                 // Index of the activation function

        
        /// <summary>
        /// Calculates step activation function of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of step activation function.</returns>
        public override float CalcActivation(float sum, float alpha)
        {
            return sum >= 0 ? alpha : 0;
        }


        /// <summary>
        /// Calculates step activation function derivative of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of step activation function derivative.</returns>
        public override float CalcDerivative(float sum, float alpha)
        {
            return 0;
        }


        /// <summary>
        /// Returns name of the activation function.
        /// </summary>
        /// <returns>Name of activation function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the activation function.
        /// </summary>
        /// <returns>Index of the activation function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for linear activation function
    /// </summary>
    class AFTypeLinear : AFTypeBase
    {
        public const string name = "LinearAFType";                  // Name of the activation function
        public const int index = 2;                                 // Index of the activation function


        /// <summary>
        /// Calculates linear activation function of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of linear activation function.</returns>
        public override float CalcActivation(float sum, float alpha)
        {
            return sum * alpha;
        }

        
        /// <summary>
        /// Calculates linear activation function derivative of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of linear activation function derivative.</returns>
        public override float CalcDerivative(float sum, float alpha)
        {
            return alpha;
        }

        
        /// <summary>
        /// Returns name of the activation function.
        /// </summary>
        /// <returns>Name of activation function.</returns>
        public override string ToString()
        {
            return name;
        }

        
        /// <summary>
        /// Returns index of the activation function.
        /// </summary>
        /// <returns>Index of the activation function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for sigma activation function
    /// </summary>
    class AFTypeSigmoid : AFTypeBase
    {
        public const string name = "SigmoidAFType";                 // Name of the activation function
        public const int index = 3;                                 // Index of the activation function


        /// <summary>
        /// Calculates sigma activation function of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of sigma activation function.</returns>
        public override float CalcActivation(float sum, float alpha)
        {
            return (float)(1 /(1 + Math.Exp(-2 * alpha * sum)));
        }


        /// <summary>
        /// Calculates sigmoid activation function derivative of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of sigmoid activation function derivative.</returns>
        public override float CalcDerivative(float sum, float alpha)
        {
            return 2*alpha*sum*(1 - sum);
        }


        /// <summary>
        /// Returns name of the activation function.
        /// </summary>
        /// <returns>Name of activation function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the activation function.
        /// </summary>
        /// <returns>Index of the activation function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for tanh activation function
    /// </summary>
    class AFTypeTanh : AFTypeBase
    {
        public const string name = "TanhAFType";                    // Name of the activation function
        public const int index = 4;                                 // Index of the activation function


        /// <summary>
        /// Calculates tanh activation function of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of tanh activation function.</returns>
        public override float CalcActivation(float sum, float alpha)
        {
            return (float)((Math.Exp(2 * alpha * sum) - 1) / (Math.Exp(2 * alpha * sum) + 1));
        }


        /// <summary>
        /// Calculates tanh activation function derivative of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of tanh activation function derivative.</returns>
        public override float CalcDerivative(float sum, float alpha)
        {
            return alpha * (1 - sum);
        }


        /// <summary>
        /// Returns name of the activation function.
        /// </summary>
        /// <returns>Name of activation function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the activation function.
        /// </summary>
        /// <returns>Index of the activation function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for softmax activation function
    /// </summary>
    class AFTypeSoftMax : AFTypeBase
    {
        public const string name = "SoftMaxAFType";                 // Name of the activation function
        public const int index = 5;                                 // Index of the activation function


        /// <summary>
        /// Calculates softmax activation function of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of softmax activation function.</returns>
        public override float CalcActivation(float sum, float alpha)
        {
            return (float)Math.Exp(alpha * sum);
        }


        /// <summary>
        /// Calculates softmax activation function derivative of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of softmax activation function derivative.</returns>
        public override float CalcDerivative(float sum, float alpha)
        {
            return alpha * sum * (1 - sum);
        }


        /// <summary>
        /// Returns name of the activation function.
        /// </summary>
        /// <returns>Name of activation function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the activation function.
        /// </summary>
        /// <returns>Index of the activation function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for rectified linear activation function
    /// </summary>
    class AFTypeRectifiedLinear : AFTypeBase
    {
        public const string name = "RectifiedLinearAFType";         // Name of the activation function
        public const int index = 6;                                 // Index of the activation function


        /// <summary>
        /// Calculates rectified linear activation function of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of rectified activation function.</returns>
        public override float CalcActivation(float sum, float alpha)
        {
            return sum >= 0 ? sum * alpha : 0;
        }


        /// <summary>
        /// Calculates rectified linear activation function derivative of inputed neuron summary with inputed alpha coefficient.
        /// </summary>
        /// <param name="sum">Neuron summary.</param>
        /// <param name="alpha">Alpha coefficient of the activation function.</param>
        /// <returns>The value of rectified linear activation function derivative.</returns>
        public override float CalcDerivative(float sum, float alpha)
        {
            return sum >= 0 ? alpha : 0;
        }


        /// <summary>
        /// Returns name of the activation function.
        /// </summary>
        /// <returns>Name of activation function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the activation function.
        /// </summary>
        /// <returns>Index of the activation function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Subclass for NoChange type, designed for inputing type of activation function, that indicates, that no changes are needed.
    /// </summary>
    class AFTypeNoChange : AFTypeBase
    {
        public const string name = "NoChangeAFType";                // Name of the activation function
        public const int index = 7;                                 // Index of the activation function


        /// <summary>
        /// Realization of activation function calculation for NoChange subclass - rises exception as not mentioned to be called.
        /// </summary>
        /// <param name="sum">Useless input.</param>
        /// <param name="alpha">Useless input.</param>
        /// <returns>Raises exception.</returns>
        public override float CalcActivation(float sum, float alpha)
        {
            throw new NotImplementedException("Error trying to calculate activation function with type 'NoChange'.");
        }


        /// <summary>
        /// Realization of activation function derivative calculation for NoChange subclass - rises exception as not mentioned to be called.
        /// </summary>
        /// <param name="sum">Useless input.</param>
        /// <param name="alpha">Useless input.</param>
        /// <returns>Raises exception.</returns>
        public override float CalcDerivative(float sum, float alpha)
        {
            throw new NotImplementedException("Error trying to calculate derivative of activation function with type 'NoChange'.");
        }


        /// <summary>
        /// Returns name of the activation function.
        /// </summary>
        /// <returns>Name of activation function.</returns>
        public override string ToString()
        {
            return name;
        }


        /// <summary>
        /// Returns index of the activation function.
        /// </summary>
        /// <returns>Index of the activation function.</returns>
        public override int Index()
        {
            return index;
        }
    }


    /// <summary>
    /// Enum for currently supported activation functions.
    /// </summary>
    public enum AFTypeStrings
    {
        StepAFType,
        LinearAFType,
        SigmoidAFType,
        TanhAFType,
        SoftMaxAFType,
        RectifiedLinearAFType,
        NoChangeAFType
    }
}

