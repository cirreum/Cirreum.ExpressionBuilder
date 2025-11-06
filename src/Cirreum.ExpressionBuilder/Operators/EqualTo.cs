namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Operation representing an equality comparison.
/// </summary>
public class EqualTo : OperatorBase {

	static readonly Expression stringComparison = Expression.Constant(StringComparison.OrdinalIgnoreCase);
	private readonly MethodInfo equalsMethod = typeof(string).GetMethod("Equals", [typeof(string), typeof(StringComparison)])!;

	/// <inheritdoc />
	public EqualTo()
		: base("EqualTo", 1, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {

		Expression constant = constant1;

		if (member.Type == typeof(string)) {
			return Expression
				.Call(member, equalsMethod, constant, stringComparison)
				.AddNullCheck(member);
		}

		return Expression.Equal(member, constant);
	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {

		Expression constant = constant1;

		if (member.Type == typeof(string)) {
			throw new ArgumentException("Verify the parameters.");
		}

		return Expression.Equal(member, constant);
	}

}