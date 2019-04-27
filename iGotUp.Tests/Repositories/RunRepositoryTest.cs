using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using FakeItEasy;
using iGotUp.Api.Data;
using iGotUp.Api.Data.Entities;
using iGotUp.Api.Data.Factories;
using iGotUp.Api.Data.Repositories;
using iGotUp.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace iGotUp.Tests.Repositories
{
    public class RunRepositoryTest
    {
        public abstract class concern : Observes<IRunRepository, RunRepository>
        {
            protected IConfiguration configuration;
            protected GotUpContext ctx;
            protected ILogger<IRunRepository> logger;
            protected IDbConnectionFactory connectionFactory;
            protected Run fake_run;
            protected DbSet<Run> fake_db_set_runs;
            protected IQueryable<Run> fakeIQueryable;

            public override void establish()
            {
                configuration = depends.on<IConfiguration>();
                ctx = fake.an<GotUpContext>();
                logger = depends.on<ILogger<IRunRepository>>();
                connectionFactory = depends.on<IDbConnectionFactory>();
                fake_run = fake.an<Run>();
                fake_db_set_runs = fake.an<DbSet<Run>>();
                fakeIQueryable = new List<Run>().AsQueryable();
            }
        }

        public class when_adding_run : concern
        {
            public override void establish()
            {
                base.establish();
                A.CallTo(() => ctx.Runs).Returns(fake_db_set_runs);
                fake_run.is_active = false;
            }

            [Fact]
            public void it_should_add_run()
            {
                sut.addRun(fake_run);
                A.CallTo(() => ctx.Runs.Add(A<Run>.Ignored)).MustHaveHappenedOnceExactly();
            }
        }
    }
}