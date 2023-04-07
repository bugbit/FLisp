// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core.SExpressions;

public class SStrings
{
    public string Str { get; }

    public SStrings(string str) => this.Str = str;

    public override string ToString() => $"""{Str}""";

    public static implicit operator SStrings(string str) => new SStrings(str);
    //public static explicit operator SStrings(string str) => new Digit(b);
}
