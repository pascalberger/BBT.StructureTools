﻿namespace BBT.StructureTools.Convert
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <inheritdoc/>
    internal class ConvertHelperFactory<TSource, TTarget, TConcreteTarget, TConvertIntention>
        : IConvertHelperFactory<TSource, TTarget, TConcreteTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <inheritdoc/>
        public ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention> GetConvertHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationFunc)
            where TReverseRelation : class
        {
            reverseRelationFunc.NotNull(nameof(reverseRelationFunc));

            var convertHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention>>();
            convertHelper.SetupReverseRelation(reverseRelationFunc);
            return convertHelper;
        }

        /// <inheritdoc/>
        public ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention> GetConvertHelper()
        {
            var convertHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention>>();
            return convertHelper;
        }
    }
}
