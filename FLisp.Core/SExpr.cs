// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public static class SExpr
{
    public const string NullStr = "nil";

    public static string ToString(object? value) => (value?.ToString()) ?? NullStr;
}
