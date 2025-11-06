namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;

/// <summary>
/// Operation representing a "not null nor whitespace" check.
/// </summary>
public class IsNotNullNorWhiteSpace : OperatorBase {

	/// <inheritdoc />
	public IsNotNullNorWhiteSpace()
		: base("IsNotNullNorWhiteSpace", 0, TypeGroup.Text) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {
		Expression exprNull = Expression.Constant(null);
		Expression exprEmpty = Expression.Constant(string.Empty);
		return Expression.AndAlso(
			Expression.NotEqual(member, exprNull),
			Expression.NotEqual(member, exprEmpty));
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}