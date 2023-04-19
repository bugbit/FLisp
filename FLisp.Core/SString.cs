// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Diagnostics.CodeAnalysis;

namespace FLisp.Core
{
    public struct SString : IEquatable<SString>
    {
        private readonly string value;

        public SString(string value)
        {
            this.value = value;
        }

        public bool Equals(SString other) => value == other.value;
        public override bool Equals([NotNullWhen(true)] object? obj) => (obj is SString other) && Equals(other);
        public override int GetHashCode() => value.GetHashCode();

        public override string ToString() => $@"""{value}""";

        public static bool operator ==(SString left, SString right) => left.Equals(right);
        public static bool operator !=(SString left, SString right) => !left.Equals(right);
        public static implicit operator string(SString left) => left.value;
        public static implicit operator SString(string left) => new(left);
    }
}
