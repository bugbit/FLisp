// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public static class SExpr
{
    public const string NilStr = "nil";
    public const char CommentChar = ';';
    public const char DobleQuoteChar = '"';
    public const char BeginListChar = '(';
    public const char EndListChar = ')';

    public static string ToString(object? value) => (value?.ToString()) ?? NilStr;
}
