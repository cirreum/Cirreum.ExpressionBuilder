namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using System;
using System.Collections;
using System.Linq.Expressions;

/// <summary>
/// Operation representing the inverse of a list "Contains" method call.
/// </summary>
public class NotIn : OperatorBase {

	/// <inheritdoc />
	public NotIn()
		: base("NotIn", 1, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text, true, true) { }

	/// <inheritdoc />
	public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) {

		if (constant1.Value is not IList || !constant1.Value.GetType().IsGenericType) {
			throw new ArgumentException("The 'NotIn' operation only supports lists as parameters.");
		}

		var type = constant1.Value.GetType();
		var inInfo = type.GetMethod("Contains", [type.GetGenericArguments()[0]])!;
		var contains = Expression.Call(constant1, inInfo, member);
		return Expression.Not(contains);

	}

	public override Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2) {
		throw new NotImplementedException("Verify the parameters.");
	}

}