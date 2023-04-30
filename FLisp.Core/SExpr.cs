// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public delegate object EvalDelegate(FLisp lisp, Context context, object[] args, CancellationToken cancellationToken);
public delegate Task<object> EvalTaskDelegate(FLisp lisp, Context context, object[] args, CancellationToken cancellationToken);
public delegate object EvalLaterDelegate();

/// <summary>
/// number => int128
/// identification => string
/// string => SString
/// list => SList
/// EvalLater => eval later
/// EvalTaskDelegate => function
/// </summary>
public static class SExpr
{
    public const string NilStr = "nil";
    public const char CommentChar = ';';
    public const char DobleQuoteChar = '"';
    public const char BeginListChar = '(';
    public const char EndListChar = ')';

    public static string ToString(object? value) => (value?.ToString()) ?? NilStr;
}
