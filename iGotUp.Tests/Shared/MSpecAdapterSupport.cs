using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using FakeItEasy;
using FakeItEasy.Configuration;

namespace iGotUp.Tests.Shared
{
    public abstract class Observes
    {
        protected Observes()
        {
            fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            depends = new DependsOnAdapter(fixture);
            fake = new FakeAnAdapter(fixture);
            setup();
        }

        public virtual void establish()
        {
        }

        public virtual void because()
        {
        }

        public virtual Task because_async()
        {
            return Task.CompletedTask;
        }

        private void setup()
        {
            establish();
            because();
            because_async().GetAwaiter().GetResult();
        }

        public IFixture fixture { get; }

        public DependsOnAdapter depends { get; }

        public FakeAnAdapter fake { get; }

        public class DependsOnAdapter
        {
            private IFixture fixture;

            public DependsOnAdapter(IFixture fixture)
            {
                this.fixture = fixture;
            }

            public Item on<Item>()
            {
                return fixture.Freeze<Item>();
            }
        }

        public class FakeAnAdapter
        {
            private IFixture fixture;

            public FakeAnAdapter(IFixture fixture)
            {
                this.fixture = fixture;
            }

            public Item an<Item>()
            {
                return fixture.Create<Item>();
            }
        }
    }

    public abstract class Observes<TheTarget>
    {
        protected Observes()
        {
            fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            depends = new DependsOnAdapter(fixture);
            fake = new FakeAnAdapter(fixture);
            setup();
        }

        public virtual void establish()
        {
        }

        public virtual void because()
        {
        }

        public virtual Task because_async()
        {
            return Task.CompletedTask;
        }

        private void setup()
        {
            establish();
            sut = fixture.Freeze<TheTarget>();
            because();
            because_async().GetAwaiter().GetResult();
        }

        public TheTarget sut { get; set; }
        public IFixture fixture { get; }

        public DependsOnAdapter depends { get; }

        public FakeAnAdapter fake { get; }

        public class DependsOnAdapter
        {
            private IFixture fixture;

            public DependsOnAdapter(IFixture fixture)
            {
                this.fixture = fixture;
            }

            public Item on<Item>()
            {
                return fixture.Freeze<Item>();
            }
        }

        public class FakeAnAdapter
        {
            private IFixture fixture;

            public FakeAnAdapter(IFixture fixture)
            {
                this.fixture = fixture;
            }

            public Item an<Item>()
            {
                return fixture.Create<Item>();
            }
        }
    }

    public abstract class Observes<TheInterface, TheTarget> where TheTarget : TheInterface
    {
        protected Observes()
        {
            fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            depends = new DependsOnAdapter(fixture);
            fake = new FakeAnAdapter(fixture);
            setup();
        }

        public virtual void establish()
        {
        }

        public virtual void because()
        {
        }

        public virtual Task because_async()
        {
            return Task.CompletedTask;
        }

        private void setup()
        {
            establish();
            sut = fixture.Freeze<TheTarget>();
            because();
            because_async().GetAwaiter().GetResult();
        }

        public TheInterface sut { get; set; }
        public IFixture fixture { get; }

        public DependsOnAdapter depends { get; }

        public FakeAnAdapter fake { get; }

        public class DependsOnAdapter
        {
            private IFixture fixture;

            public DependsOnAdapter(IFixture fixture)
            {
                this.fixture = fixture;
            }

            public Item on<Item>()
            {
                return fixture.Freeze<Item>();
            }

            public void on<Item>(Item item)
            {
                fixture.Inject(item);
            }
        }

        public class FakeAnAdapter
        {
            private IFixture fixture;

            public FakeAnAdapter(IFixture fixture)
            {
                this.fixture = fixture;
            }

            public Item an<Item>()
            {
                return fixture.Create<Item>();
            }
        }
    }

    public static class FakeItEasyExtensions
    {
        public static IVoidArgumentValidationConfiguration setup<Item>(this Item fake, Expression<Action<Item>> action)
        {
            var wrapped_expression = wrap(fake, action);
            var expression = Expression.Lambda<Action>(wrapped_expression);
            return A.CallTo(expression);
        }

        public static IReturnValueArgumentValidationConfiguration<Output> setup<Input, Output>(this Input fake, Expression<Func<Input, Output>> func)
        {
            var wrapped_expression = wrap(fake, func);
            var expression = (Expression<Func<Output>>)Expression.Lambda(wrapped_expression);
            return A.CallTo(expression);
        }

        public static IVoidArgumentValidationConfiguration received<Item>(this Item fake, Expression<Action<Item>> action)
        {
            return fake.setup(action);
        }

        public static IReturnValueArgumentValidationConfiguration<Output> received<Input, Output>(this Input fake, Expression<Func<Input, Output>> func)
        {
            return fake.setup(func);
        }

        public static void never_received<Item>(this Item fake, Expression<Action<Item>> action)
        {
            fake.setup(action).MustNotHaveHappened();
        }

        public static void never_received<Input, Output>(this Input fake, Expression<Func<Input, Output>> func)
        {
            fake.setup(func).MustNotHaveHappened();
        }

        public static void OnlyOnce(this IAssertConfiguration config)
        {
            config.MustHaveHappenedOnceExactly();
        }

        public static void Twice(this IAssertConfiguration config)
        {
            config.MustHaveHappenedTwiceExactly();
        }

        public static void Return<Output>(this IReturnValueArgumentValidationConfiguration<Output> config, Output value)
        {
            config.Returns(value);
        }

        private static Expression wrap<Input, Output>(Input fake, Expression<Output> expression)
        {
            var fake_expression = Expression.Constant(fake, typeof(Input));
            Expression call = null;

            if (expression.Body is MethodCallExpression)
            {
                var method_expression = (MethodCallExpression)expression.Body;
                var method = method_expression.Method;
                call = Expression.Call(fake_expression, method, method_expression.Arguments);
            }

            if (expression.Body is MemberExpression)
            {
                var member_expression = (MemberExpression)expression.Body;
                var member = (PropertyInfo)member_expression.Member;

                call = Expression.Property(fake_expression, member);
            }

            if (call == null)
            {
                throw new InvalidOperationException("Expression is not pointing to a method or property");
            }

            return call;
        }
    }
}