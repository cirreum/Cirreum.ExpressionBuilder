namespace Cirreum.ExpressionBuilder.Helpers;

using Cirreum.ExpressionBuilder.Common;
using Cirreum.ExpressionBuilder.Exceptions;
using Cirreum.ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Useful methods regarding <seealso cref="IOperator"></seealso>.
/// </summary>
public static class OperationHelper {

	private static readonly HashSet<IOperator> _operators;
	private static readonly Dictionary<TypeGroup, HashSet<Type>> _typeGroups;

	/// <summary>
	/// List of all operators loaded so far.
	/// </summary>
	public static IEnumerable<IOperator> Operators {
		get {
			return [.. _operators];
		}
	}

	static OperationHelper() {
		var @interface = typeof(IOperator);
		var operatorsFound = @interface.Assembly
			.GetTypes()
			.Where(t => @interface.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
			.Select(t => (IOperator)Activator.CreateInstance(t)!);
		_operators = new HashSet<IOperator>(operatorsFound, new OperationEqualityComparer());
		_typeGroups = new Dictionary<TypeGroup, HashSet<Type>> {
			{ TypeGroup.Text, new HashSet<Type> { typeof(string), typeof(char) } },
			{ TypeGroup.Number, new HashSet<Type> { typeof(int), typeof(uint), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(long), typeof(ulong), typeof(Single), typeof(double), typeof(decimal) } },
			{ TypeGroup.Boolean, new HashSet<Type> { typeof(bool) } },
			{ TypeGroup.Date, new HashSet<Type> { typeof(DateTimeOffset), typeof(DateTime) } },
			{ TypeGroup.Nullable, new HashSet<Type> { typeof(Nullable<>), typeof(string) } }
		};
	}

	/// <summary>
	/// Instantiates an IOperator given its name.
	/// </summary>
	/// <param name="operationName">Name of the operation to be instantiated.</param>
	/// <returns></returns>
	public static IOperator GetOperationByName(string operationName) {

		var operation =
			Operators.SingleOrDefault(o => o.Name == operationName && o.Active) ??
			throw new OperationNotFoundException(operationName);

		return operation;

	}

	/// <summary>
	/// Loads a list of custom operators into the <see cref="Operators"></see> list.
	/// </summary>
	/// <param name="operators">List of operators to load.</param>
	public static void LoadOperators(List<IOperator> operators) {
		LoadOperators(operators, false);
	}

	/// <summary>
	/// Loads a list of custom operators into the <see cref="Operators"></see> list.
	/// </summary>
	/// <param name="operators">List of operators to load.</param>
	/// <param name="overloadExisting">Specifies that any matching pre-existing operators should be replaced by the ones from the list. (Useful to overwrite the default operators)</param>
	public static void LoadOperators(List<IOperator> operators, bool overloadExisting) {
		foreach (var operation in operators) {
			DeactivateOperator(operation.Name, overloadExisting);
			_operators.Add(operation);
		}
	}

	private static void DeactivateOperator(string operationName, bool overloadExisting) {

		if (!overloadExisting) {
			return;
		}

		var op = _operators.FirstOrDefault(o => string.Compare(o.Name, operationName, StringComparison.InvariantCultureIgnoreCase) == 0);
		if (op != null) {
			op.Active = false;
		}

	}


	/// <summary>
	/// Retrieves a list of <see cref="IOperator"></see> supported by a type.
	/// </summary>
	/// <param name="type">Type for which supported operators should be retrieved.</param>
	/// <returns></returns>
	public static HashSet<IOperator> SupportedOperators(Type type) {
		return GetSupportedOperators(type);
	}

	private static HashSet<IOperator> GetSupportedOperators(Type type) {

		var underlyingNullableType = Nullable.GetUnderlyingType(type);
		var typeName = (underlyingNullableType ?? type).Name;

		var supportedOperators = new List<IOperator>();
		if (type.IsArray) {
			typeName = type.GetElementType()?.Name;
			supportedOperators.AddRange(Operators.Where(o => o.SupportsLists && o.Active));
		}

		var typeGroup = TypeGroup.Default;
		if (_typeGroups.Any(i => i.Value.Any(v => v.Name == typeName))) {
			typeGroup = _typeGroups.FirstOrDefault(i => i.Value.Any(v => v.Name == typeName)).Key;
		}

		supportedOperators.AddRange(Operators.Where(o => o.TypeGroup.HasFlag(typeGroup) && !o.SupportsLists && o.Active));

		if (underlyingNullableType != null) {
			supportedOperators.AddRange(Operators.Where(o => o.TypeGroup.HasFlag(TypeGroup.Nullable) && !o.SupportsLists && o.Active));
		}

		return [.. supportedOperators];
	}

}