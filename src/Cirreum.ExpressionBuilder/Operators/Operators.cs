namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Helpers;
using Cirreum.ExpressionBuilder.Interfaces;
using System.Collections.Generic;

/// <summary>
/// Exposes the default operations supported by the <seealso cref="Builders.FilterBuilder" />.
/// </summary>
public static class Operators {

	/// <summary>
	/// Operation representing a range comparison.
	/// </summary>
	public static IOperator Between { get { return new Between(); } }

	/// <summary>
	/// Operation representing a string "Contains" method call.
	/// </summary>
	public static IOperator Contains { get { return new Contains(); } }

	/// <summary>
	/// Operation that checks for the non-existence of a substring within another string.
	/// </summary>
	public static IOperator DoesNotContain { get { return new DoesNotContain(); } }

	/// <summary>
	/// Operation representing a string "EndsWith" method call.
	/// </summary>
	public static IOperator EndsWith { get { return new EndsWith(); } }

	/// <summary>
	/// Operation representing an equality comparison.
	/// </summary>
	public static IOperator EqualTo { get { return new EqualTo(); } }

	/// <summary>
	/// Operation representing an "greater than" comparison.
	/// </summary>
	public static IOperator GreaterThan { get { return new GreaterThan(); } }

	/// <summary>
	/// Operation representing an "greater than or equal" comparison.
	/// </summary>
	public static IOperator GreaterThanOrEqualTo { get { return new GreaterThanOrEqualTo(); } }

	/// <summary>
	/// Operation representing a list "Contains" method call.
	/// </summary>
	public static IOperator In { get { return new In(); } }

	/// <summary>
	/// Operation representing a check for an empty string.
	/// </summary>
	public static IOperator IsEmpty { get { return new IsEmpty(); } }

	/// <summary>
	/// Operation representing a check for a non-empty string.
	/// </summary>
	public static IOperator IsNotEmpty { get { return new IsNotEmpty(); } }

	/// <summary>
	/// Operation representing a "not-null" check.
	/// </summary>
	public static IOperator IsNotNull { get { return new IsNotNull(); } }

	/// <summary>
	/// Operation representing a "not null nor whitespace" check.
	/// </summary>
	public static IOperator IsNotNullNorWhiteSpace { get { return new IsNotNullNorWhiteSpace(); } }

	/// <summary>
	/// Operation representing a null check.
	/// </summary>
	public static IOperator IsNull { get { return new IsNull(); } }

	/// <summary>
	/// Operation representing a "null or whitespace" check.
	/// </summary>
	public static IOperator IsNullOrWhiteSpace { get { return new IsNullOrWhiteSpace(); } }

	/// <summary>
	/// Operation representing an "less than" comparison.
	/// </summary>
	public static IOperator LessThan { get { return new LessThan(); } }

	/// <summary>
	/// Operation representing an "less than or equal" comparison.
	/// </summary>
	public static IOperator LessThanOrEqualTo { get { return new LessThanOrEqualTo(); } }

	/// <summary>
	/// Operation representing an inequality comparison.
	/// </summary>
	public static IOperator NotEqualTo { get { return new NotEqualTo(); } }

	/// <summary>
	/// Operation representing a string "StartsWith" method call.
	/// </summary>
	public static IOperator StartsWith { get { return new StartsWith(); } }

	/// <summary>
	/// Operation representing the inverse of a list "Contains" method call.
	/// </summary>
	public static IOperator NotIn { get { return new NotIn(); } }

	/// <summary>
	/// Get an IOperator from its name.
	/// </summary>
	/// <param name="operationName">Name of the operation to be instantiated.</param>
	/// <returns></returns>
	public static IOperator FromName(string operationName) {
		return OperationHelper.GetOperationByName(operationName);
	}

	/// <summary>
	/// Loads a list of custom operations.
	/// </summary>
	/// <param name="operators">List of operations to load.</param>
	/// <param name="overloadExisting">Specifies that any matching pre-existing operations should be replaced by the ones from the list. (Useful to overwrite the default operations)</param>
	public static void LoadOperators(List<IOperator> operators, bool overloadExisting = false) {
		OperationHelper.LoadOperators(operators, overloadExisting);
	}

}