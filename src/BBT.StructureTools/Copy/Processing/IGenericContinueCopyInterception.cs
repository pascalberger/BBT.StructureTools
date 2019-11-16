﻿namespace BBT.StructureTools.Copy.Processing
{
    /// <summary>
    /// Interface to intercept the copy process.
    /// </summary>
    /// <typeparam name="TType">Typification here.</typeparam>
    public interface IGenericContinueCopyInterception<in TType> : IBaseAdditionalProcessing
        where TType : class
    {
        /// <summary>
        /// Returns true if the object shall be copied.
        /// </summary>
        bool ShallCopy(TType obj);
    }
}