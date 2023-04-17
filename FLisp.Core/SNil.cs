// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.ComponentModel;

namespace FLisp.Core;

public class SNil
{
    private static readonly Lazy<SNil> instance = new Lazy<SNil>(() => new());

    public static SNil Instance => instance.Value;

    private SNil() { }
}