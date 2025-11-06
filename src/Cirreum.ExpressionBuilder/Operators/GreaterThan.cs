namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System.Linq.Expressions;

/// <summary>
/// Operation representing an "greater than" comparison.
/// </summary>
public class GreaterThan : OperatorBase {

	/// <inheritdoc />
	public GreaterThan()
		: base("GreaterThan", 1, TypeGroup.Number | TypeGroup.Date) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {
		return Expression.GreaterThan(member, constant1);
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		return Expression.GreaterThan(member, constant1);
	}

}