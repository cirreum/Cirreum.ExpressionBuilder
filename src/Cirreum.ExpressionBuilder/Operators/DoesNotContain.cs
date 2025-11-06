namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Operation that checks for the non-existence of a substring within another string.
/// </summary>
public class DoesNotContain : OperatorBase {

	static readonly Expression stringComparison = Expression.Constant(StringComparison.OrdinalIgnoreCase);
	private readonly MethodInfo stringContainsMethod = typeof(string).GetMethod("Contains", [typeof(string), typeof(StringComparison)])!;

	/// <inheritdoc />
	public DoesNotContain()
		: base("DoesNotContain", 1, TypeGroup.Text) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {

		Expression constant = constant1;

		return Expression.Not(Expression.Call(member, stringContainsMethod, constant, stringComparison))
			   .AddNullCheck(member);

	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}