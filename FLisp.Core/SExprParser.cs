// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

using System;
using System.IO;
using System.Text;

public class SExprParser
{
    //private static readonly Regex sExprRegEx = new Regex(@"(?<token>[\(\)]|""(?<texto>[^""]*)""|\S+)", RegexOptions.Compiled);
    private TextReader reader;
    private string? line;
    private int position;

    public bool EOF { get; private set; }

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

            if (car1 == SExpr.DobleQuoteChar)
                return (true, new SString(token.Substring(1, token.Length - 2)));
        }

        return (true, token);
    }

    private async Task<string?> ReadToken()
    {
        for (; ; )
        {
            if (line == null)
            {
                if (!await ReadLine())
                    return null;
            }
            while (position < line!.Length && char.IsSeparator(line[position])) position++;
            if (position < line!.Length)
            {
                var car1 = line[position];

                if (car1 == SExpr.CommentChar)
                {
                    line = null;

                    continue;
                }

                return car1 switch
                {
                    SExpr.BeginListChar or SExpr.EndListChar => car1.ToString(),
                    SExpr.DobleQuoteChar => ReadString(),
                    _ => ReadWord()
                };
            }
            else
                line = null;
        }
    }

    private string ReadWord()
    {
        var word = new StringBuilder();
        char car;

        while (position < line!.Length && !char.IsSeparator((car = line[position])))
        {
            word.Append(car);
            position++;
        }

        return word.ToString();
    }

    private string ReadString()
    {
        var theString = new StringBuilder();

        if (position < line!.Length)
        {
            theString.Append(line[position++]);
            while (position < line!.Length)
            {
                var car = line[position++];

                if (car == SExpr.DobleQuoteChar)
                {
                    theString.Append(car);

                    break;
                }

                if (car == '\\' && position < line!.Length && line[position] == SExpr.DobleQuoteChar)
                    car = line[position++];

                theString.Append(car);
            }
        }

        return theString.ToString();
    }

    private async Task<bool> ReadLine()
    {
        if (EOF)
            return false;

        line = await reader.ReadLineAsync();

        if (line == null)
        {
            EOF = true;

            return false;
        }

        position = 0;

        return true;
    }
}