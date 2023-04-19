// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

public class Context
{
    /*
    private Dictionary<string, Lambda> vars = new Dictionary<string, Lambda>();
    private Dictionary<string, Lambda> functions = new Dictionary<string, Lambda>();

    public bool IsScopeGlobal => Parent == null;
    public Context? Parent { get; protected set; }

    public Context NewConext() => new Context { Parent = this };

    public bool TryGetVar(string name, out Lambda? value)
    {
        var context = this;

        do
        {
            if (vars.TryGetValue(name, out var value2))
            {
                value = value2;

                return true;
            }
            context = context.Parent;
        } while (context != null && !context.IsScopeGlobal);

        value = null;

        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="VariableNotFoundException">If the variable has not been declared</exception>
    public Lambda GetVar(string name)
    {
        if (!TryGetVar(name, out var value))
            throw new VariableNotFoundException($"variable {name} not found");

        return value!;
    }
    public bool TryGetFunction(string name, out Lambda? fn)
    {
        var context = this;

        do
        {
            if (functions.TryGetValue(name, out var fn1))
            {
                fn = fn1;

                return true;
            }
            context = context.Parent;
        } while (context != null);

        fn = null;

        return false;
    }
    public Lambda GetFunction(string name)
    {
        if (!TryGetFunction(name, out var fn))
            throw new FunctionNotFoundException($"function {name} not found");

        return fn!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <exception cref="VariableAlreadyDeclaredException">If the variable has been declared</exception>
    public void DeclareVar(string name, Lambda value)
    {
        if (vars.ContainsKey(name))
            throw new VariableAlreadyDeclaredException($"variable {name} has been declared");

        vars[name] = value;
    }
    public void DeclareFunc(string name, Lambda fn)
    {
        if (functions.ContainsKey(name))
            throw new FunctionAlreadyDeclaredException($"function {name} has been declared");

        functions[name] = fn;
    }

    */
}

