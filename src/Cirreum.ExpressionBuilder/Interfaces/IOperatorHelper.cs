namespace Cirreum.ExpressionBuilder.Interfaces;

using System;
using System.Collections.Generic;

/// <summary>
/// Useful methods regarding <seealso cref="IOperator"></seealso>.
/// </summary>
public interface IOperatorHelper {

	#region Properties

	/// <summary>
	/// List of all operations loaded so far.
	/// </summary>
	IEnumerable<IOperator> Operations { get; }

	#endregion Properties

	#region Methods

	/// <summary>
	/// Retrieves a list of Operators (<see cref="IOperator"></see>) supported by a type.
	/// </summary>
	/// <param name="type">Type for which supported operations should be retrieved.</param>
	/// <returns></returns>
	HashSet<IOperator> SupportedOperations(Type type);

	/// <summary>
	/// Instantiates an IOperator given its name.
	/// </summary>
	/// <param name="operationName">Name of the operation to be instantiated.</param>
	/// <returns></returns>
	IOperator GetOperationByName(string operationName);

	/// <summary>
	/// Loads a list of custom operations into the <see cref="Operations"></see> list.
	/// </summary>
	/// <param name="operations">List of operations to load.</param>
	void LoadOperations(List<IOperator> operations);

	/// <summary>
	/// Loads a list of custom operations into the <see cref="Operations"></see> list.
	/// </summary>
	/// <param name="operations">List of operations to load.</param>
	/// <param name="overloadExisting">Specifies that any matching pre-existing operations should be replaced by the ones from the list. (Useful to overwrite the default operations)</param>
	void LoadOperators(List<IOperator> operations, bool overloadExisting);

	#endregion Methods

}