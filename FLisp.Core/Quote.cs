// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public sealed class Quote : Lambda
{
    public Lambda Lambda { get; }

    public Quote(Lambda lambda)
    {
        Lambda = lambda;
        EvalType = lambda.EvalType;
    }

    public override object? Eval(Context context)
    {
        if (!IsEval)
        {
            Result = Lambda.Eval(context);
            IsEval = true;
        }

        return Result;
    }

    public async override Task<object?> EvalAsync(Context context, CancellationToken cancellationToken)
    {
        if (!IsEval)
        {
            Result = await Lambda.EvalAsync(context, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            IsEval = true;
        }

        return Result;
    }

    public override Task<object?> EvalParallel(Context context, CancellationToken cancellationToken)
        => (!IsEval)
            ? Task.FromResult(Result)
            : Lambda.EvalParallel(context, cancellationToken).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    throw t.Exception!;

                Result = t.Result;
                IsEval = true;

                return t.Result;
            });
}