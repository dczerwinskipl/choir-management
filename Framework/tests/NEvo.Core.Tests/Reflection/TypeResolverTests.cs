using FluentAssertions;
using NEvo.Core.Reflection;

namespace NEvo.Core.Tests.Reflection
{
    public class TypeResolverTests
    {
        public TypeResolver TypeResolver { get; }
        public TypeResolverTests()
        {
            TypeResolver = new TypeResolver(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.StartsWith("NEvo")));
        }

        [Fact]
        public void GettingTypeName_WithName()
        {
            // arrange
            var type = typeof(TypeResolverTests);

            // act
            var name = TypeResolver.GetName(type, TypeResolvingOptions.Name);

            // assert
            name.Should().Be("TypeResolverTests");
        }

        [Fact]
        public void GettingTypeName_WithNamespace()
        {
            // arrange
            var type = typeof(TypeResolverTests);

            // act
            var name = TypeResolver.GetName(type, TypeResolvingOptions.Namespace);

            // assert
            name.Should().Be("NEvo.Core.Tests.Reflection.TypeResolverTests");
        }

        [Fact]
        public void GettingTypeName_WithAssembly()
        {
            // arrange
            var type = typeof(TypeResolverTests);

            // act
            var name = TypeResolver.GetName(type, TypeResolvingOptions.Assembly);

            // assert
            name.Should().Be("NEvo.Core.Tests.Reflection.TypeResolverTests, NEvo.Core.Tests");
        }

        [Fact]
        public void GettingTypeName_WithVersion()
        {
            // arrange
            var type = typeof(TypeResolverTests);

            // act
            var name = TypeResolver.GetName(type, TypeResolvingOptions.Version);

            // assert
            name.Should().Be($"NEvo.Core.Tests.Reflection.TypeResolverTests, NEvo.Core.Tests, Version={GetType().Assembly.GetName().Version}");
        }

        [Fact]
        public void GettingTypeName_WithAssemblyQualifiedName()
        {
            // arrange
            var type = typeof(TypeResolverTests);

            // act
            var name = TypeResolver.GetName(type, TypeResolvingOptions.AssemblyQualifiedName);

            // assert
            name.Should().Be(GetType().AssemblyQualifiedName);
        }

        [Fact]
        public void ResolvingType_ByName_FromCurrentAssemblyWithoutConflict_ShoudlReturnValidType()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolver), TypeResolvingOptions.Name);

            // act
            var type = TypeResolver.GetType(name);

            // assert
            type.Should()
                .NotBeNull()
                .And.Be(typeof(TypeResolver));
        }

        [Fact]
        public void ResolvingType_ByName_FromExternalAssemblyWithoutConflict_ShoudlReturnValidType()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolverTests), TypeResolvingOptions.Name);

            // act
            var type = TypeResolver.GetType(name);

            // assert
            type.Should()
                .NotBeNull()
                .And.Be(typeof(TypeResolverTests));
        }

        [Fact]
        public void ResolvingType_ByName_WithConflict_ShoudlThrowException()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(Check), TypeResolvingOptions.Name);

            // act
            var act = () => TypeResolver.GetType(name);

            // assert
            act.Should()
                    .Throw<MultipleTypesFoundException>();
        }

        [Fact]
        public void ResolvingType_ByName_ThatNotExists_ShoudlThrowException()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolverTests), TypeResolvingOptions.Name).Replace(nameof(TypeResolverTests), Guid.NewGuid().ToString());

            // act
            var act = () => TypeResolver.GetType(name);

            // assert
            act.Should()
                    .Throw<TypeNotFoundException>();
        }

        [Fact]
        public void ResolvingType_ByNamespace_FromCurrentAssemblyWithoutConflict_ShoudlReturnValidType()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolver), TypeResolvingOptions.Namespace);

            // act
            var type = TypeResolver.GetType(name);

            // assert
            type.Should()
                .NotBeNull()
                .And.Be(typeof(TypeResolver));
        }

        [Fact]
        public void ResolvingType_ByNamespace_FromExternalAssemblyWithoutConflict_ShoudlReturnValidType()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolverTests), TypeResolvingOptions.Namespace);

            // act
            var type = TypeResolver.GetType(name);

            // assert
            type.Should()
                .NotBeNull()
                .And.Be(typeof(TypeResolverTests));
        }

        [Fact]
        public void ResolvingType_ByNamespace_WithConflict_ShoudlThrowException()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(Check), TypeResolvingOptions.Namespace);

            // act
            var act = () => TypeResolver.GetType(name);

            // assert
            act.Should()
                    .Throw<MultipleTypesFoundException>();
        }

        [Fact]
        public void ResolvingType_ByNamespace_ThatNotExists_ShoudlThrowException()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolverTests), TypeResolvingOptions.Namespace).Replace(nameof(TypeResolverTests), Guid.NewGuid().ToString());

            // act
            var act = () => TypeResolver.GetType(name);

            // assert
            act.Should()
                    .Throw<TypeNotFoundException>();
        }

        [Fact]
        public void ResolvingType_ByAssembly_FromCurrentAssemblyWithoutConflict_ShoudlReturnValidType()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolver), TypeResolvingOptions.Assembly);

            // act
            var type = TypeResolver.GetType(name);

            // assert
            type.Should()
                .NotBeNull()
                .And.Be(typeof(TypeResolver));
        }

        [Fact]
        public void ResolvingType_ByAssembly_FromExternalAssemblyWithoutConflict_ShoudlReturnValidType()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolverTests), TypeResolvingOptions.Assembly);

            // act
            var type = TypeResolver.GetType(name);

            // assert
            type.Should()
                .NotBeNull()
                .And.Be(typeof(TypeResolverTests));
        }

        [Fact]
        public void ResolvingType_ByAssembly_WithConflictBetweenAssemblies_ShoudlReturnValidType()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(Check), TypeResolvingOptions.Assembly);

            // act
            // act
            var type = TypeResolver.GetType(name);

            // assert
            type.Should()
                .NotBeNull()
                .And.Be(typeof(Check));
        }

        [Fact]
        public void ResolvingType_ByAssembly_ThatNotExists_ShoudlThrowException()
        {
            // arrange
            var name = TypeResolver.GetName(typeof(TypeResolverTests), TypeResolvingOptions.Assembly).Replace(nameof(TypeResolverTests), Guid.NewGuid().ToString());

            // act
            var act = () => TypeResolver.GetType(name);

            // assert
            act.Should()
                    .Throw<TypeNotFoundException>();
        }
    }
}

namespace NEvo.Core {
   
    // warning: this is class to simulate conflict in names between assemblies
    //          tests can fail when name or namespace of that class in NEvo.Core would change
    public class Check { }
}