// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Collections.Immutable;

namespace FLisp.Core.SExpressions;

public class SList : SExpr<ImmutableArray<SExpr>>
{
    public SList(ICollection<SExpr> value) : base(ESExprType.List, value.ToImmutableArray())
    {
    }

    public override string ToString() => $"({string.Join(',', Value)})";
}
