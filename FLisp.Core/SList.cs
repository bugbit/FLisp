// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Collections.Immutable;
using FLisp.Core.SExpressions;

namespace FLisp.Core;

public class SList : IEquatable<SList>
{
    private ImmutableArray<object?> exprs;

    public SList(ICollection<object?> exprs) => this.exprs = exprs.ToImmutableArray();

    public override string ToString() => $"({string.Join(' ', exprs.Select(e => SExpr.ToString(e)))})";

    public override bool Equals(object? obj) => (obj is SList list) && Equals(list);
    public bool Equals(SList? other) => other != null && Enumerable.SequenceEqual(exprs, other.exprs);
    public override int GetHashCode() => HashCode.Combine(exprs.Select(s => s ?? DBNull.Value));
}
