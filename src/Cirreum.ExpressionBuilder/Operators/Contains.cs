namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Operation representing a string "Contains" method call.
/// </summary>
public class Contains : OperatorBase {

	private static readonly Expression stringComparison = Expression.Constant(StringComparison.OrdinalIgnoreCase);
	private readonly MethodInfo? stringContainsMethod = typeof(string).GetMethod("Contains", [typeof(string), typeof(StringComparison)]);

	/// <inheritdoc />
	public Contains()
		: base("Contains", 1, TypeGroup.Text) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {

		if (stringContainsMethod == null) {
			throw new InvalidOperationException("Contains method not found.");
		}

		var constant = constant1;

		return Expression
			.Call(member, stringContainsMethod, constant, stringComparison)
			.AddNullCheck(member);

	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}