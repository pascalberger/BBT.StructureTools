﻿namespace BBT.StructureTools.Tests.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationCrossReferenceProcessingIntTests
    {
        private readonly ICopy<TestClass> testCandidate;

        public CopyOperationCrossReferenceProcessingIntTests()
        {
            var kernel = TestIocContainer.Initialize();

            kernel.Bind<ICopyRegistrations<TestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<TestClassChild>>().To<TestClassChildCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<TestClassCrossReferencedChild>>().To<TestClassCrossReferencedChildCopyRegistrations>();

            this.testCandidate = kernel.Get<ICopy<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void MustExecuteAndSetCrossReferenceRegistrations()
        {
            // Arrange
            var crossReferenceSource = new TestClassCrossReferencedChild();
            var source = new TestClass();
            crossReferenceSource.Parent = source;
            source.TestClassCrossReferencedChild = crossReferenceSource;
            source.TestClassChild = new TestClassChild
            {
                Parent = source,
                CrossReference = crossReferenceSource,
            };
            var target = new TestClass();

            // Act
            this.testCandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            crossReferenceSource.Should().NotBeSameAs(target.TestClassCrossReferencedChild);
            target.TestClassCrossReferencedChild.Should().BeSameAs(target.TestClassChild.CrossReference);
        }

        private class TestClass
        {
            public TestClassChild TestClassChild { get; set; }

            public TestClassCrossReferencedChild TestClassCrossReferencedChild { get; set; }
        }

        private class TestClassChild
        {
            public TestClass Parent { get; set; }

            public TestClassCrossReferencedChild CrossReference { get; set; }
        }

        private class TestClassCrossReferencedChild
        {
            public TestClass Parent { get; set; }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<TestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterCrossReferenceProcessing<TestClassCrossReferencedChild, TestClassChild>(
                        x => x.CrossReference)
                    .RegisterCreateToOneWithReverseRelation<TestClassCrossReferencedChild, TestClassCrossReferencedChild>(
                        x => x.TestClassCrossReferencedChild,
                        x => x.Parent)
                    .RegisterCreateToOneWithReverseRelation<TestClassChild, TestClassChild>(
                        x => x.TestClassChild,
                        x => x.Parent);
            }
        }

        private class TestClassCrossReferencedChildCopyRegistrations : ICopyRegistrations<TestClassCrossReferencedChild>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassCrossReferencedChild> registrations)
            {
                registrations.Should().NotBeNull();
            }
        }

        private class TestClassChildCopyRegistrations : ICopyRegistrations<TestClassChild>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassChild> registrations)
            {
                registrations.Should().NotBeNull();
            }
        }
    }
}