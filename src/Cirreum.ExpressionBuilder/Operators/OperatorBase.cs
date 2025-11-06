namespace Cirreum.ExpressionBuilder;

using Cirreum.ExpressionBuilder.Common;
using Cirreum.ExpressionBuilder.Interfaces;
using System;
using System.Linq.Expressions;

/// <summary>
/// Base class for an Operation.
/// </summary>
/// <remarks>
/// Instantiates a new operation.
/// </remarks>
/// <param name="name">Operation name.</param>
/// <param name="numberOfValues">Number of values supported by the operation.</param>
/// <param name="typeGroups">TypeGroup(s) which the operation supports.</param>
/// <param name="active">Determines if the operation is active.</param>
/// <param name="supportsLists">Determines if the operation supports arrays.</param>
/// <param name="expectNullValues"></param>
public abstract class OperatorBase(
	string name,
	int numberOfValues,
	TypeGroup typeGroups,
	bool active = true,
	bool supportsLists = false,
	bool expectNullValues = false
) : IOperator
  , IEquatable<IOperator> {

	/// <inheritdoc />
	public string Name { get; } = name;

	/// <inheritdoc />
	public TypeGroup TypeGroup { get; } = typeGroups;

	/// <inheritdoc />
	public int NumberOfValues { get; } = numberOfValues;

	/// <inheritdoc />
	public bool Active { get; set; } = active;

	/// <inheritdoc />
	public bool SupportsLists { get; } = supportsLists;

	/// <inheritdoc />
	public bool ExpectNullValues { get; } = expectNullValues;

	/// <inheritdoc />
	public abstract Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2);
	public abstract Expression GetExpression(Expression member, ConstantExpression constant1, ConstantExpression constant2);

	/// <inheritdoc />
	public override int GetHashCode() {
		return (this.Name != null ? this.Name.GetHashCode() : 0);
	}

	/// <inheritdoc />
	public override bool Equals(object? obj) {
		if (obj is null) {
			return false;
		}

		if (ReferenceEquals(this, obj)) {
			return true;
		}

		return obj.GetType() == this.GetType() && this.Equals((OperatorBase)obj);
	}

	/// <inheritdoc />
	public override string ToString() {
		return this.Name.Trim();
	}

	public bool Equals(IOperator? other) {
		if (other is null) {
			return false;
		}
		return string.Equals(this.Name, other.Name);
	}

}