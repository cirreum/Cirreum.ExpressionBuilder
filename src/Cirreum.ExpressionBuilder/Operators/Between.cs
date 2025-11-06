namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System.Linq.Expressions;

/// <summary>
/// Operation representing a range comparison.
/// </summary>
public class Between : OperatorBase {

	/// <inheritdoc />
	public Between()
		: base("Between", 2, TypeGroup.Number | TypeGroup.Date) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {
		var left = Expression.GreaterThanOrEqual(member, constant1);
		var right = Expression.LessThanOrEqual(member, constant2);

		return Expression.AndAlso(left, right);
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		var left = Expression.GreaterThanOrEqual(member, constant1);
		var right = Expression.LessThanOrEqual(member, constant2);

		return Expression.AndAlso(left, right);
	}

}