using AutomationTestSample.Domain;
using Microsoft.EntityFrameworkCore;

namespace AutomationTestSample.Infrastructure
{
    public interface ITestDbContext
    {
        public DbSet<Patient> Patients { get; }
        public DbSet<Order> Orders { get; }

        public Task<int> SaveChanges(CancellationToken cancellationToken = default);
    }
}
