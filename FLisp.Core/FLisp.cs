// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public class FLisp
{
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    //public CancellationToken CancelToken => cancellationTokenSource.Token;
    public Primitives Primitives { get; } = new();
}