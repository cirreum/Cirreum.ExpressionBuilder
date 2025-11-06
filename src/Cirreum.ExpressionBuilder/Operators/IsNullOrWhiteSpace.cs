namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;

/// <summary>
/// Operation representing a "null or whitespace" check.
/// </summary>
public class IsNullOrWhiteSpace : OperatorBase {

	/// <inheritdoc />
	public IsNullOrWhiteSpace()
		: base("IsNullOrWhiteSpace", 0, TypeGroup.Text, expectNullValues: true) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {
		Expression exprNull = Expression.Constant(null);
		Expression exprEmpty = Expression.Constant(string.Empty);
		return Expression.OrElse(
			Expression.Equal(member, exprNull),
			Expression.AndAlso(
				Expression.NotEqual(member, exprNull),
				Expression.Equal(member, exprEmpty)));
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}