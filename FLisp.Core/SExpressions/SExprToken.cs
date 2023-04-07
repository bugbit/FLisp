// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core.SExpressions
{
    public struct SExprToken
    {
        public required ESExprToken TypeToken { get; init; }
        public required string Token { get; init; }
        public required string Value { get; init; }
        public required int Line { get; init; }
        public required int Column { get; init; }
    }
}