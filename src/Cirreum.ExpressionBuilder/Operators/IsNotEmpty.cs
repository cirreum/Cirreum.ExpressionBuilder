namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;

/// <summary>
/// Operation representing a check for a non-empty string.
/// </summary>
public class IsNotEmpty : OperatorBase {

	/// <inheritdoc />
	public IsNotEmpty()
		: base("IsNotEmpty", 0, TypeGroup.Text) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {
		return Expression.NotEqual(member, Expression.Constant(string.Empty))
			   .AddNullCheck(member);
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}