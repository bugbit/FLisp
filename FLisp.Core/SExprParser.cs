// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

using System;
using System.IO;
using System.Text.RegularExpressions;

public class SExprParser
{
    private static readonly Regex sExprRegEx = new Regex(@"(?<token>[\(\)]|""(?<texto>[^""]*)""|\S+)", RegexOptions.Compiled);
    private TextReader reader;
    private Match? match;

    public SExprParser(TextReader reader) => this.reader = reader;

    public async Task<object?[]> ReadSExprs()
    {
        var sexprs = new List<object?>();
        (bool isread, object? value) sexpr;

        while ((sexpr = await TryReadSExpr()).isread)
            sexprs.Add(sexpr.value);

        return sexprs.ToArray();
    }

    public async Task<(bool isread, object? value)> TryReadSExpr()
    {
        var token = await ReadToken();

        if (token == null)
            return (false, null);

        if (string.Equals(token, SExpr.NullStr, StringComparison.OrdinalIgnoreCase))
            return (true, null);

        if (token.Length > 0)
        {
            var car1 = token[0];

            if (char.IsDigit(car1))
                if (Int128.TryParse(token, out var numInt))
                    return (true, numInt);

            if (car1 == '"' && match!.Groups["texto"].Success)
                return (true, new SString(match.Groups["texto"].Value));
        }

        return (true, token);
    }

    private async Task<string?> ReadToken()
    {
        for (; ; )
        {
            if (match == null)
            {
                if (!await ReadLine())
                    return null;
            }
            else
                match = match.NextMatch();

            var t = match!;

            if (t.Success)
                return t.Groups["token"].Value;

            match = null;
        }
    }

    private async Task<bool> ReadLine()
    {
        var line = await reader.ReadLineAsync();

        if (line == null)
            return false;

        match = sExprRegEx.Match(line);

        return true;
    }
}