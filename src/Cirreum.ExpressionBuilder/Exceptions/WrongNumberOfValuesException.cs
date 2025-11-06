namespace Cirreum.ExpressionBuilder.Exceptions;

using Cirreum.ExpressionBuilder.Interfaces;
using System;

/// <summary>
/// Represents an attempt to use an operation providing the wrong number of values.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WrongNumberOfValuesException" /> class.
/// </remarks>
/// <param name="operation">Operation used.</param>
[Serializable]
public class WrongNumberOfValuesException(IOperator operation) : Exception {

	/// <summary>
	/// Gets the <see cref="Operation" /> attempted to be used.
	/// </summary>
	public IOperator Operation { get; private set; } = operation;

	/// <summary>
	/// Gets a message that describes the current exception.
	/// </summary>
	public override string Message {
		get {
			return string.Format(
				"The operation '{0}' admits exactly '{1}' values (not more neither less than this).",
				this.Operation.Name,
				this.Operation.NumberOfValues);
		}
	}

}