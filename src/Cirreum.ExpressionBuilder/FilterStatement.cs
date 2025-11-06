namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using Cirreum.ExpressionBuilder.Exceptions;
using Cirreum.ExpressionBuilder.Helpers;
using Cirreum.ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;

/// <summary>
/// Defines how a property should be filtered.
/// </summary>
[Serializable]
public class FilterStatement<TPropertyType> : IFilterStatement {

	/// <summary>
	/// Establishes how this filter statement will connect to the next one.
	/// </summary>
	public Connector Connector { get; set; }

	/// <summary>
	/// Property identifier conventionalized by for the Expression Builder.
	/// </summary>
	public string PropertyId { get; set; }

	/// <summary>
	/// Express the interaction between the property and the constant value defined in this filter statement.
	/// </summary>
	public IOperator Operator { get; set; }

	/// <summary>
	/// Constant value that will interact with the property defined in this filter statement.
	/// </summary>
	public object? Value { get; set; }

	/// <summary>
	/// Constant value that will interact with the property defined in this filter statement when the operation demands a second value to compare to.
	/// </summary>
	public object? Value2 { get; set; }

	/// <summary>
	/// Instantiates a new <see cref="FilterStatement{TPropertyType}" />.
	/// </summary>
	/// <param name="propertyId"></param>
	/// <param name="operation"></param>
	/// <param name="value"></param>
	/// <param name="value2"></param>
	/// <param name="connector"></param>
	public FilterStatement(string propertyId, IOperator operation, TPropertyType? value, TPropertyType? value2, Connector connector) {
		this.PropertyId = propertyId;
		this.Connector = connector == 0 ? Connector.And : connector;
		this.Operator = operation;
		this.SetValues(value, value2);
		this.Validate();
	}

	private void SetValues(TPropertyType? value, TPropertyType? value2) {

		if (typeof(TPropertyType).IsArray) {

			if (!this.Operator.SupportsLists) {
				throw new ArgumentException("It seems the chosen operation does not support arrays as parameters.");
			}

			var listType = typeof(List<>);
			var elementType = typeof(TPropertyType).GetElementType();
			if (elementType is not null) {
				var constructedListType = listType.MakeGenericType(elementType);
				this.Value = value != null ? Activator.CreateInstance(constructedListType, value) : null;
				this.Value2 = value2 != null ? Activator.CreateInstance(constructedListType, value2) : null;
			} else {
				this.Value = value;
				this.Value2 = value2;
			}
		} else {
			this.Value = value;
			this.Value2 = value2;
		}

	}

	/// <summary>
	/// Validates the FilterStatement regarding the number of provided values and supported operators.
	/// </summary>
	public void Validate() {
		this.ValidateNumberOfValues();
		this.ValidateSupportedOperators();
	}

	private void ValidateNumberOfValues() {

		var numberOfValues = this.Operator.NumberOfValues;
		var failsForSingleValue = numberOfValues == 1 && !Equals(this.Value2, default(TPropertyType));
		var failsForNoValueAtAll = numberOfValues == 0 && (!Equals(this.Value, default(TPropertyType)) || !Equals(this.Value2, default(TPropertyType)));

		if (failsForSingleValue || failsForNoValueAtAll) {
			throw new WrongNumberOfValuesException(this.Operator);
		}

	}

	private void ValidateSupportedOperators() {

		var supportedOperators = OperationHelper.SupportedOperators(typeof(TPropertyType));

		if (!supportedOperators.Contains(this.Operator)) {
			throw new UnsupportedOperationException(this.Operator, typeof(TPropertyType).Name);
		}
	}

	/// <summary>
	/// String representation of <see cref="FilterStatement{TPropertyType}" />.
	/// </summary>
	/// <returns></returns>
	public override string ToString() {
		return this.Operator.NumberOfValues switch {
			0 => string.Format("{0} {1}", this.PropertyId, this.Operator),
			2 => string.Format("{0} {1} {2} And {3}", this.PropertyId, this.Operator, this.Value, this.Value2),
			_ => string.Format("{0} {1} {2}", this.PropertyId, this.Operator, this.Value),
		};
	}

}