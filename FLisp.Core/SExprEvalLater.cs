// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public class SExprEvalLater
{
    public object? SExpr { get; private set; }
    public object? SExprEval { get; private set; }

    public bool IsEvalLater() => SExpr != null;

    public SExprEvalLater(object expr) => SExpr = expr;

    internal void SetExprEval(object? expr)
    {
        SExprEval = expr;
        SExpr = null;
    }

    public static bool IsEvalLater(object expr) => ((expr is SExprEvalLater later) && later.IsEvalLater()) || !(expr is SList);
}
