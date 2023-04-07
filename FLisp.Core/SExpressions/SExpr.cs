// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core.SExpressions;

using System.Numerics;

public class SExpr
{
    public ESExprType SExprType { get; }
    public object? Value { get; }

    protected SExpr(ESExprType type, object? value)
    {
        SExprType = type;
        Value = value;
    }

    public static SExpr CreateNumber<T>(T n) where T : INumber<T>
    {
        return
            (T.IsInteger(n))
                ? new SExpr<UInt128>(ESExprType.Numerics, UInt128.CreateChecked(n))
                : new SExpr<double>(ESExprType.Numerics, double.CreateChecked(n));
        //: throw new NotImplementedException();
    }

    public static SExpr<string> CreateSymbol(string symbol) => new SExpr<string>(ESExprType.Symbol, symbol);
    public static SExpr<SStrings> CreateString(string symbol) => new SExpr<SStrings>(ESExprType.String, symbol);
    public static SExpr<SList> CreateList(ICollection<SExpr> list) => new SExpr<SList>(ESExprType.List, new(list));
}

public class SExpr<T> : SExpr
{
    public new T? Value => (T?)base.Value;

    internal SExpr(ESExprType type, T? value) : base(type, value!) { }
}
