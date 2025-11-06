namespace Cirreum.ExpressionBuilder.Exceptions;

using Cirreum.ExpressionBuilder.Interfaces;
using System;

/// <summary>
/// Represents an attempt to use an operation not currently supported by a type.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UnsupportedOperationException" /> class.
/// </remarks>
/// <param name="operation">Operation used.</param>
/// <param name="typeName">Name of the type.</param>
[Serializable]
public class UnsupportedOperationException(IOperator operation, String typeName) : Exception {

	/// <summary>
	/// Gets the <see cref="Operation" /> attempted to be used.
	/// </summary>
	public IOperator Operation { get; private set; } = operation;

	/// <summary>
	/// Gets name of the type.
	/// </summary>
	public string TypeName { get; private set; } = typeName;

	/// <summary>
	/// Gets a message that describes the current exception.
	/// </summary>
	public override string Message {
		get {
			return
				string.Format(
					"The type '{0}' does not have support for the operation '{1}'.",
					this.TypeName,
					this.Operation);
		}
	}
}