namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Operation representing a string "StartsWith" method call.
/// </summary>
public class StartsWith : OperatorBase {

	static readonly Expression stringComparison = Expression.Constant(StringComparison.OrdinalIgnoreCase);
	private readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", [typeof(string), typeof(StringComparison)])!;

	/// <inheritdoc />
	public StartsWith()
		: base("StartsWith", 1, TypeGroup.Text) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {

		Expression constant = constant1;

		return Expression.Call(member, startsWithMethod, constant, stringComparison)
			   .AddNullCheck(member);
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}