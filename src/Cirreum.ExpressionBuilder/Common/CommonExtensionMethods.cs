namespace Cirreum.ExpressionBuilder.Common;

using System.Linq.Expressions;

public static class CommonExtensionMethods {

	/// <summary>
	/// Gets a member expression for an specific property
	/// </summary>
	/// <param name="param"></param>
	/// <param name="propertyName"></param>
	/// <returns></returns>
	public static MemberExpression GetMemberExpression(this ParameterExpression param, string propertyName) {
		return GetMemberExpression((Expression)param, propertyName);
	}

	private static MemberExpression GetMemberExpression(Expression param, string propertyName) {
		if (!propertyName.Contains('.')) {
			return Expression.PropertyOrField(param, propertyName);
		}

		var index = propertyName.IndexOf('.');
		var subParam = Expression.PropertyOrField(param, propertyName[..index]);
		return GetMemberExpression(subParam, propertyName[(index + 1)..]);
	}

	/// <summary>
	/// Adds a "null check" to the expression (before the original one).
	/// </summary>
	/// <param name="expression">Expression to which the null check will be pre-pended.</param>
	/// <param name="member">Member that will be checked.</param>
	/// <returns></returns>
	public static Expression AddNullCheck(this Expression expression, MemberExpression member) {
		Expression memberIsNotNull = Expression.NotEqual(member, Expression.Constant(null));
		return Expression.AndAlso(memberIsNotNull, expression);
	}

	/// <summary>
	/// Checks if an object is a generic list.
	/// </summary>
	/// <param name="o">Object to be tested.</param>
	/// <returns>TRUE if the object is a generic list.</returns>
	public static bool IsGenericList(this object o) {
		var oType = o.GetType();
		return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>)));
	}

}