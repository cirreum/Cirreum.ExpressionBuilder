namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Operation representing a string "EndsWith" method call.
/// </summary>
public class EndsWith : OperatorBase {

	static readonly Expression stringComparison = Expression.Constant(StringComparison.OrdinalIgnoreCase);
	private readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", [typeof(string), typeof(StringComparison)])!;

	/// <inheritdoc />
	public EndsWith()
		: base("EndsWith", 1, TypeGroup.Text) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {

		Expression constant = constant1;

		return Expression.Call(member, endsWithMethod, constant, stringComparison)
			   .AddNullCheck(member);
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}