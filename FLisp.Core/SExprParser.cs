// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

using System.IO;
using System.Text.RegularExpressions;

public class SExprParser
{
    private static readonly Regex SExprRegEx = new Regex(@"(?<token>[\(\)]|\S+|""[^""]+"")", RegexOptions.Compiled);
    private TextReader reader;
}