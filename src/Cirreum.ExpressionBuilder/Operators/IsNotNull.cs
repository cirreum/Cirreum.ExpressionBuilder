namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;

/// <summary>
/// Operation representing a "not-null" check.
/// </summary>
public class IsNotNull : OperatorBase {

	/// <inheritdoc />
	public IsNotNull()
		: base("IsNotNull", 0, TypeGroup.Text | TypeGroup.Nullable) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {
		return Expression.NotEqual(member, Expression.Constant(null));
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}