namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Builders;
using Cirreum.ExpressionBuilder.Common;
using Cirreum.ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Aggregates <see cref="FilterStatement{TPropertyType}" /> and build them into a LINQ expression.
/// </summary>
/// <typeparam name="TClass"></typeparam>
[Serializable]
public class Filter<TClass> : IFilter {

	private readonly List<List<IFilterStatement>> _statements;
	private List<IFilterStatement> CurrentStatementGroup {
		get {
			return this._statements.Last();
		}
	}

	/// <summary>
	/// Creates a new Group and return this current instance.
	/// </summary>
	public IFilter Group {
		get {
			this.StartGroup();
			return this;
		}
	}

	/// <summary>
	/// List of <see cref="IFilterStatement" /> groups that will be combined and built into a LINQ expression.
	/// </summary>
	public IEnumerable<IEnumerable<IFilterStatement>> Statements {
		get {
			return [.. this._statements];
		}
	}


	/// <summary>
	/// Instantiates a new <see cref="Filter{TClass}" />
	/// </summary>
	public Filter() {
		this._statements = [[]];
	}


	/// <summary>
	/// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
	/// (To be used by <see cref="IOperator" /> that need no values)
	/// </summary>
	/// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
	/// <param name="operation">Operation to be used.</param>
	/// <param name="connector"></param>
	/// <returns></returns>
	public IFilterStatementConnection By(string propertyId, IOperator operation, Connector connector) {
		return this.By<string>(propertyId, operation, null, null, connector);
	}

	/// <summary>
	/// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
	/// (To be used by <see cref="IOperator" /> that need no values)
	/// </summary>
	/// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
	/// <param name="operation">Operation to be used.</param>
	/// <returns></returns>
	public IFilterStatementConnection By(string propertyId, IOperator operation) {
		return this.By<string>(propertyId, operation, null, null, Connector.And);
	}

	/// <summary>
	/// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
	/// </summary>
	/// <typeparam name="TPropertyType"></typeparam>
	/// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
	/// <param name="operation">Operation to be used.</param>
	/// <param name="value"></param>
	/// <returns></returns>
	public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperator operation, TPropertyType? value) {
		return this.By(propertyId, operation, value, default(TPropertyType));
	}

	/// <summary>
	/// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
	/// </summary>
	/// <typeparam name="TPropertyType"></typeparam>
	/// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
	/// <param name="operation">Operation to be used.</param>
	/// <param name="value"></param>
	/// <param name="connector"></param>
	/// <returns></returns>
	public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperator operation, TPropertyType? value, Connector connector) {
		return this.By(propertyId, operation, value, default, connector);
	}

	/// <summary>
	/// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
	/// </summary>
	/// <typeparam name="TPropertyType"></typeparam>
	/// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
	/// <param name="operation">Operation to be used.</param>
	/// <param name="value"></param>
	/// <param name="value2"></param>
	/// <returns></returns>
	public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperator operation, TPropertyType? value, TPropertyType? value2) {
		return this.By(propertyId, operation, value, value2, Connector.And);
	}

	/// <summary>
	/// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
	/// </summary>
	/// <typeparam name="TPropertyType"></typeparam>
	/// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
	/// <param name="operation">Operation to be used.</param>
	/// <param name="value"></param>
	/// <param name="value2"></param>
	/// <param name="connector"></param>
	/// <returns></returns>
	public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperator operation, TPropertyType? value, TPropertyType? value2, Connector connector) {
		IFilterStatement statement = new FilterStatement<TPropertyType>(propertyId, operation, value, value2, connector);
		this.CurrentStatementGroup.Add(statement);
		return new FilterStatementConnection(this, statement);
	}

	public IFilterStatementConnection By(IFilterStatement statement) {
		this.CurrentStatementGroup.Add(statement);
		return new FilterStatementConnection(this, statement);
	}

	/// <summary>
	/// Starts a new group denoting that every subsequent filter statement should be grouped together (as if using a parenthesis).
	/// </summary>
	public void StartGroup() {
		if (this.CurrentStatementGroup.Count != 0) {
			this._statements.Add([]);
		}
	}

	/// <summary>
	/// Removes all <see cref="FilterStatement{TPropertyType}" />, leaving the <see cref="Filter{TClass}" /> empty.
	/// </summary>
	public void Clear() {
		this._statements.Clear();
		this._statements.Add([]);
	}

	/// <summary>
	/// Implicitly converts a <see cref="Filter{TClass}" /> into a <see cref="Func{TClass, TResult}" />.
	/// </summary>
	/// <param name="filter"></param>
	public static implicit operator Func<TClass, bool>(Filter<TClass> filter) {
		return FilterBuilder.GetExpression<TClass>(filter).Compile();
	}

	/// <summary>
	/// Implicitly converts a <see cref="Filter{TClass}" /> into a <see cref="System.Linq.Expressions.Expression" />.
	/// </summary>
	/// <param name="filter"></param>
	public static implicit operator System.Linq.Expressions.Expression<Func<TClass, bool>>(Filter<TClass> filter) {
		return FilterBuilder.GetExpression<TClass>(filter);
	}

	/// <summary>
	/// String representation of <see cref="Filter{TClass}" />.
	/// </summary>
	/// <returns></returns>
	public override string ToString() {
		var result = new System.Text.StringBuilder();
		var lastConnector = Connector.And;

		foreach (var statementGroup in this._statements) {
			if (this._statements.Count > 1) {
				result.Append('(');
			}

			var groupResult = new System.Text.StringBuilder();
			foreach (var statement in statementGroup) {
				if (groupResult.Length > 0) {
					groupResult.Append(" " + lastConnector + " ");
				}

				groupResult.Append(statement);
				lastConnector = statement.Connector;
			}

			result.Append(groupResult.ToString().Trim());
			if (this._statements.Count > 1) {
				result.Append(')');
			}
		}

		return result.ToString();
	}

}