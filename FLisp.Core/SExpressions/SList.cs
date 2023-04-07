// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Collections.Immutable;

namespace FLisp.Core.SExpressions;

public class SList
{
    private ImmutableArray<SExpr> lista;

    public SList(ICollection<SExpr> lista) => this.lista = lista.ToImmutableArray();

    public override string ToString() => $"({string.Join(',', lista)})";
}
