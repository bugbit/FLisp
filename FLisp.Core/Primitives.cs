// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public class Primitives
{
    public async Task<object> EvalAsync(FLisp lisp, Context context, object expr)
    {
        if (expr is SList fn)
        {
            try
            {
                return await AddAsync(lisp, context, fn.Skip(1));
            }
            catch (Exception ex)
            {
                throw new EvalException($"Error in {fn} : {ex.Message}", ex) { Expr = expr };
            }
        }

        return Task.FromResult(expr);
    }

    public async Task<object> AddAsync(FLisp lisp, Context context, object[] args)
    {
        var exprs = await EvalParallelAsync(lisp, context, args);
        object result = 0;

        for (var i = 0; i < args.Length; i++)
        {
            lisp.CancelToken.ThrowIfCancellationRequested();
            var n2 = exprs[i];

            if (!SMath.TryAdd(result, n2, out var r))
                throw new EvalException($"(Argument {i + 1}) NUMBER expected") { NumberArgument = i + 1, Expr = n2 };

            result = r!;
        }

        return result;
    }

    private async Task<object[]> EvalParallelAsync(FLisp lisp, Context context, object[] args)
    {
        var tasks = args.Select(a =>
        {
            (FLisp lisp, Context context, object arg) state = (lisp, context, a);

            Task.Factory.StartNew(async s =>
            {
                var state = ((FLisp lisp, Context context, object arg))s!;

                return await EvalAsync(state.lisp, state.context, state.arg);
            }, (object)state, lisp.CancelToken);
        });

        lisp.CancelToken.ThrowIfCancellationRequested();

        var result = await Task.WhenAll(tasks);

        return result;
    }
}