// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core.SExpressions;

using System.Numerics;

/*
        1
        "hola mundo"
        suma
        (suma 1 1)
 */

public class SExpr
{
    public ESExprType SExprType { get; }
    public object? Value { get; }

    public SExpr(ESExprType type, object? value)
    {
        SExprType = type;
        Value = value;
    }

    public override string ToString() => Value?.ToString() ?? string.Empty;

}

public class SExpr<T> : SExpr
{
    public new T? Value => (T?)base.Value;

    public SExpr(ESExprType type, T? value) : base(type, value!) { }
}
