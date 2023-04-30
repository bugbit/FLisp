// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FLisp.Core;

public class SList : IEnumerable<object>, IEquatable<SList>, IReadOnlyList<object>
{
    private ImmutableArray<object> exprs;

    public int Count => exprs.Length;

    public object this[int index] => exprs[index];

    public SList(ICollection<object> exprs) => this.exprs = exprs.ToImmutableArray();

    public override string ToString() => $"({string.Join(' ', exprs)})";

    public override bool Equals(object? obj) => (obj is SList list) && Equals(list);
    public bool Equals(SList? other) => other != null && Enumerable.SequenceEqual(exprs, other.exprs);
    public override int GetHashCode() => HashCode.Combine(exprs);

    //public object[] Skip(int n) => exprs.Skip(n).ToArray();

    public IEnumerator<object> GetEnumerator() => exprs.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
