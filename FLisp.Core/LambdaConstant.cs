// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public sealed class LambdaConstant : Lambda
{
    public LambdaConstant(object? value)
    {
        Result = value;
        IsEval = true;
    }

    public override object? Eval(Context context)
    {
        IsEval = true;

        return Result;
    }

    public override Task<object?> EvalAsync(Context context, CancellationToken cancellationToken)
    {
        IsEval = true;

        return Task.FromResult(Result);
    }

    public override Task<object?> EvalParallel(Context context, CancellationToken cancellationToken)
    {
        IsEval = true;

        return Task.FromResult(Result);
    }
}

