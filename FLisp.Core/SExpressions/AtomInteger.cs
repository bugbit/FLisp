﻿// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core.SExpressions;

public class AtomInteger : SExpr<Int128>
{
    public AtomInteger(Int128 value) : base(ESExprType.Integer, value)
    {
    }
}
