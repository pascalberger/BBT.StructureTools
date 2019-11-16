﻿namespace BBT.StructureTools.Compare
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// See <see cref="IComparer{T}"/>.
    /// Compares two types based on how they inherit each other / are assignable to each other.
    /// A more specific type is considered "higher up" in the inheritance hierarchy, and therefore
    /// "greater" in a comparison.
    /// </summary>
    internal sealed class TypeComparer : IComparer<Type>
    {
        /// <inheritdoc/>
        public int Compare(Type x, Type y)
        {
            x.NotNull(nameof(x));
            y.NotNull(nameof(y));

            // ' X and Y are the same type.
            if (x == y)
            {
                return 0;
            }

            if (x.IsAssignableFrom(y))
            {
                // ' Means x is less.
                return -1;
            }

            if (y.IsAssignableFrom(x))
            {
                // ' Means x is greater.
                return 1;
            }

            // ' If they are completely unrelated, none is greater.
            return 0;
        }
    }
}