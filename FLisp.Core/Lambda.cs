// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Runtime.CompilerServices;

namespace FLisp.Core;

public abstract class Lambda
{
    public object? Result { get; protected set; }
    public EEvalType EvalType { get; protected set; }
    public bool IsEval { get; protected set; }
    public bool IsRecursive { get; protected set; }

    public abstract object? Eval(Context context);
    public abstract Task<object?> EvalAsync(Context context, CancellationToken cancellationToken);
    public abstract Task<object?> EvalParallel(Context context, CancellationToken cancellationToken);
    public static bool HasEvalAsync(Lambda[] fns) => fns.Any(f => f.EvalType switch { EEvalType.EvalAsync or EEvalType.EvalParallel => true, _ => false });
    public static object?[] Eval(Context context, Lambda[] fns)
    {
        if (HasEvalAsync(fns))
            throw new InvalidOperationException();

        return fns.Select(f => f.Eval(context)).ToArray();
    }
    public static async Task<object?[]> EvalAsync(Context context, Lambda[] fns, CancellationToken cancellationToken)
    {
        var result = new object?[fns.Length];
        var tasks = new List<Task>();
        var indexsAsync = new List<(int idx, Lambda fn)>();
        var IndexsRun = new List<(int idx, Lambda fn)>();

        for (var i = 0; i < fns.Length; i++)
        {
            var fn = fns[i];
            switch (fn.EvalType)
            {
                case EEvalType.EvalParallel:
                    tasks.Add(Task.Factory.StartNew(() => fn.EvalParallel(context, cancellationToken).ContinueWith(t =>
                    {
                        if (t.IsCompleted)
                            result[i] = t.Result;
                        if (t.IsFaulted)
                            throw t.Exception!;
                    }), cancellationToken));
                    break;
                case EEvalType.EvalAsync:
                    indexsAsync.Add((i, fn));
                    break;
                default:
                    IndexsRun.Add((i, fn));
                    break;
            }
        }
        if (indexsAsync.Count > 0)
            tasks.Add(EvalAsync());

        foreach (var i in IndexsRun)
        {
            cancellationToken.ThrowIfCancellationRequested();

            result![i.idx] = i.fn.Eval(context);
        }

        await Task.WhenAll(tasks);

        cancellationToken.ThrowIfCancellationRequested();

        return result;

        async Task EvalAsync()
        {
            foreach (var i in indexsAsync)
            {
                cancellationToken.ThrowIfCancellationRequested();

                result![i.idx] = await i.fn.EvalAsync(context, cancellationToken);
            }
        }
    }
}