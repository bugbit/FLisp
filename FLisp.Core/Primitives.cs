// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public delegate object EvalDelegate(FLisp lisp, Context context, object[] args, CancellationToken cancellationToken);
public delegate Task<object> EvalTaskDelegate(FLisp lisp, Context context, object[] args, CancellationToken cancellationToken);

public class Primitives
{
    private Dictionary<string, (EvalDelegate? eval, EvalTaskDelegate? evalAsParallel, EvalTaskDelegate? evalAsync)> primitives;
    //private Delegate addDelegate;

    public Primitives()
    {
        //addDelegate = AddAsync;
        primitives = new Dictionary<string, (EvalDelegate? eval, EvalTaskDelegate? evalAsParallel, EvalTaskDelegate? evalAsync)>(StringComparer.InvariantCultureIgnoreCase)
        {
            ["+"] = (null, AddAsync, null),
            ["number?"] = (IsNumber, null, null),
            ["define"] = (Define, null, null)
        };
    }

    public object Eval(FLisp lisp, Context context, object expr, CancellationToken cancellationToken, bool evalLater = false)
    {
        if (expr is SExprEvalLater later)
        {
            if (!later.IsEvalLater())
                return later.SExprEval ?? SNil.Value;
        }
        else
        {
            if (evalLater)
                return new SExprEvalLater(expr);
        }

        if (expr is SList fn)
            return Apply(lisp, context, fn.ToArray(), cancellationToken);

        return expr;
    }
    public async Task<object> EvalAsParallel(FLisp lisp, Context context, object expr, CancellationToken cancellationToken)
    {
        if (expr is SList fn)
        {
            try
            {
                return await AddAsync(lisp, context, fn.Skip(1), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new EvalException($"Error in {fn} : {ex.Message}", ex) { Expr = expr };
            }
        }

        return Task.FromResult(expr);
    }

    /*
         apply procedure object object ...
        Calls procedure with the elements of the following list as arguments:

        (cons* object object ...)

        The initial objects may be any objects, but the last object (there must be at least one object) must be a list.

        (apply + (list 3 4 5 6))    =>  18
        (apply + 3 4 '(5 6))        =>  18
        (define no +)
        (apply no 1 2 '(3))         => 6
     */

    public object Apply(FLisp lisp, Context context, object[] args, CancellationToken cancellationToken)
    {
        if (args.Length < 2)
            throw new EvalException("Wrong number of arguments passed to procedure");

        //var proc = (string)args[0];
        if (!(args[0] is string proc))
            throw new EvalException("(Argument 1) PROCEDURE expected");

        if (!(args.Last() is SList argLast))
            throw new EvalException($"(Argument {args.Length}) LIST expected");

        // proc : function o var => function

        if (context.TryGetIdentity(proc, out var identity))
        {
            if (!(identity is string idStr))
                throw new EvalException("(Argument 1) PROCEDURE expected");

            proc = idStr;
        }

        if (!primitives.TryGetValue(proc, out var primitive))
            throw new EvalException($"Unbound variable: {proc}");

        var exprArgs = args.Skip(1).Take(args.Length - 2).Concat(argLast);
        var procArgs = exprArgs.Select(e => Eval(lisp, context, e, cancellationToken, true)).ToArray();

        if (primitive.eval != null)
            return primitive.eval(lisp, context, procArgs, cancellationToken);

        if (primitive.evalAsParallel != null)
        {
            var t = primitive.evalAsParallel(lisp, context, procArgs, cancellationToken);

            t.Wait();

            return t.Result;
        }

        if (primitive.evalAsync != null)
        {
            var caller = primitive.evalAsync;
            var result = caller.BeginInvoke(lisp, context, procArgs, cancellationToken, null, null);

            return caller.EndInvoke(result);
        }

        return SNil.Value;
    }

    // number?
    public object IsNumber(FLisp lisp, Context context, object[] args, CancellationToken cancellationToken)
    {
        var expr0 = Eval(lisp, context, args[0], cancellationToken);

        return SNil.Value;
    }

    public object Define(FLisp lisp, Context context, object[] args, CancellationToken cancellationToken)
    {
        var name = args[0];
        var value = Eval(lisp, context, args[0], cancellationToken, true);

        return SNil.Value;
    }

    public async Task<object> AddAsync(FLisp lisp, Context context, object[] args, CancellationToken cancellationToken)
    {
        /*
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
        */

        return SNil.Value;
    }

    /*
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
    */
}