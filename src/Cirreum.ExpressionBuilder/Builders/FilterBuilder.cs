namespace Cirreum.ExpressionBuilder.Builders;

using Cirreum.ExpressionBuilder.Common;
using Cirreum.ExpressionBuilder.Exceptions;
using Cirreum.ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

internal class FilterBuilder {

	#region Public Methods

	public static Expression<Func<T, bool>> GetExpression<T>(IFilter filter) {

		var param = Expression.Parameter(typeof(T), "x");
		Expression? expression = null;
		var connector = Connector.And;

		foreach (var statementGroup in filter.Statements) {
			var statementGroupConnector = Connector.And;
			var partialExpr = GetPartialExpression(param, ref statementGroupConnector, statementGroup);
			expression = expression == null ? partialExpr : CombineExpressions(expression, partialExpr, connector);
			connector = statementGroupConnector;
		}

		expression ??= Expression.Constant(true);

		return Expression.Lambda<Func<T, bool>>(expression, param);
	}

	public static Expression GetSafePropertyMember(ParameterExpression param, string memberName, Expression expr) {

		if (!memberName.Contains('.')) {
			return expr;
		}

		var index = memberName.LastIndexOf('.');
		var parentName = memberName[..index];
		var subParam = param.GetMemberExpression(parentName);
		var resultExpr = Expression.AndAlso(Expression.NotEqual(subParam, Expression.Constant(null)), expr);
		return GetSafePropertyMember(param, parentName, resultExpr);
	}

	#endregion Public Methods

	#region Protected Methods

	protected static Expression CheckIfParentIsNull(ParameterExpression param, string memberName) {
		var parentMember = GetParentMember(param, memberName);
		return Expression.Equal(parentMember, Expression.Constant(null));
	}

	#endregion Protected Methods

	#region Private Methods

	private static Expression GetPartialExpression(ParameterExpression param, ref Connector connector, IEnumerable<IFilterStatement> statementGroup) {

		Expression? expression = null;

		foreach (var statement in statementGroup) {
			var expr = IsList(statement) ? ProcessListStatement(param, statement) : GetExpression(param, statement);
			expression = expression == null ? expr : CombineExpressions(expression, expr, connector);
			connector = statement.Connector;
		}

		return expression!;

	}

	private static bool IsList(IFilterStatement statement) {
		return statement.PropertyId.Contains('[') && statement.PropertyId.Contains(']');
	}

	private static BinaryExpression CombineExpressions(Expression expr1, Expression expr2, Connector connector) {
		return connector == Connector.And ? Expression.AndAlso(expr1, expr2) : Expression.OrElse(expr1, expr2);
	}

	private static MethodCallExpression ProcessListStatement(ParameterExpression param, IFilterStatement statement) {

		var basePropertyName = statement.PropertyId[..statement.PropertyId.LastIndexOf('[')];
		var propertyName = statement.PropertyId[(statement.PropertyId.LastIndexOf('[') + 1)..].Replace("]", string.Empty);

		var prop = param.Type.GetProperty(basePropertyName) ?? throw new InvalidOperationException($"Property Name '{basePropertyName}' was not found on Type '{param.Type.Name}'.");
		var type = prop.PropertyType.GetGenericArguments()[0];
		var listItemParam = Expression.Parameter(type, "i");

		var lambda = Expression.Lambda(GetExpression(listItemParam, statement, propertyName), listItemParam);
		var member = param.GetMemberExpression(basePropertyName);
		var enumerableType = typeof(Enumerable);
		var anyInfo = enumerableType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Length == 2);
		anyInfo = anyInfo.MakeGenericMethod(type);
		return Expression.Call(anyInfo, member, lambda);
	}

	private static Expression GetExpression(ParameterExpression param, IFilterStatement statement, string? propertyName = null) {

		Expression? resultExpr = null;
		var memberName = propertyName ?? statement.PropertyId;
		var member = param.GetMemberExpression(memberName);

		if (Nullable.GetUnderlyingType(member.Type) != null && statement.Value != null) {
			resultExpr = Expression.Property(member, "HasValue");
			member = Expression.Property(member, "Value");
		}

		var constant1 = Expression.Constant(statement.Value);
		var constant2 = Expression.Constant(statement.Value2);

		CheckPropertyValueMismatch(member, constant1);

		var safeStringExpression = statement.Operator.GetExpression(member, constant1, constant2);
		resultExpr = resultExpr != null ? Expression.AndAlso(resultExpr, safeStringExpression) : safeStringExpression;
		resultExpr = GetSafePropertyMember(param, memberName, resultExpr);

		if (statement.Operator.ExpectNullValues && memberName.Contains('.')) {
			resultExpr = Expression.OrElse(CheckIfParentIsNull(param, memberName), resultExpr);
		}

		return resultExpr;
	}

	private static void CheckPropertyValueMismatch(MemberExpression memberExp, ConstantExpression constant1) {

		var memberType = memberExp.Member.MemberType == MemberTypes.Property
			? (memberExp.Member as PropertyInfo)!.PropertyType
			: (memberExp.Member as FieldInfo)!.FieldType;

		var constant1Type = GetConstantType(constant1);
		var nullableType = constant1Type != null ? Nullable.GetUnderlyingType(constant1Type) : null;

		var constantValueIsNotNull = constant1.Value != null;
		var memberAndConstantTypeDoNotMatch = nullableType == null && memberType != constant1Type;
		var memberAndNullableUnderlyingTypeDoNotMatch = nullableType != null && memberType != nullableType;

		if (constantValueIsNotNull && (memberAndConstantTypeDoNotMatch || memberAndNullableUnderlyingTypeDoNotMatch)) {
			throw new PropertyValueTypeMismatchException(memberExp.Member.Name, memberType.Name, constant1.Type.Name);
		}
	}

	private static Type? GetConstantType(ConstantExpression constant) {
		if (constant != null && constant.Value != null && constant.Value.IsGenericList()) {
			return constant.Value.GetType().GenericTypeArguments[0];
		}

		return constant != null && constant.Value != null ? constant.Value.GetType() : null;
	}
	private static MemberExpression GetParentMember(ParameterExpression param, string memberName) {
		var parentName = memberName[..memberName.IndexOf('.')];
		return param.GetMemberExpression(parentName);
	}

	#endregion Private Methods

}