﻿// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

[Flags]
public enum EvalFlags
{
    Default = 0, Later = 0x1, Parallel = 0x2
}
