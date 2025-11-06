namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System.Linq.Expressions;

/// <summary>
/// Operation representing an "less than" comparison.
/// </summary>
public class LessThan : OperatorBase {

	/// <inheritdoc />
	public LessThan()
		: base("LessThan", 1, TypeGroup.Number | TypeGroup.Date) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {
		return Expression.LessThan(member, constant1);
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		return Expression.LessThan(member, constant1);
	}

}