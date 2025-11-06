namespace Cirreum.ExpressionBuilder.Helpers;

using Cirreum.ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;

internal class OperationEqualityComparer : IEqualityComparer<IOperator> {

	public bool Equals(IOperator? x, IOperator? y) {
		if (x == null && y == null) {
			return true;
		}

		if (x == null || y == null) {
			return false;
		}

		return
			string.Compare(
				x.Name,
				y.Name,
				StringComparison.InvariantCultureIgnoreCase) == 0 &&
			x.Active &&
			y.Active;

	}

	public int GetHashCode(IOperator obj) {
		return obj.Name.GetHashCode() ^ obj.Active.GetHashCode();
	}

}