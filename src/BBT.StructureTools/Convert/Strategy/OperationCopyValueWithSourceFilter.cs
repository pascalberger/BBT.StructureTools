﻿namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCopyValueWithSourceFilter<TSource, TTarget, TValue>
        : IOperationCopyValueWithSourceFilter<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        private Func<TSource, TTarget, TValue> sourceFunc;
        private Expression<Func<TTarget, TValue>> targetExpression;

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TTarget, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            targetExpression.NotNull(nameof(targetExpression));

            this.sourceFunc = sourceFunc;
            this.targetExpression = targetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));

            var sourceValue = this.sourceFunc.Invoke(source, target);
            target.SetPropertyValue(this.targetExpression, sourceValue);
        }
    }
}