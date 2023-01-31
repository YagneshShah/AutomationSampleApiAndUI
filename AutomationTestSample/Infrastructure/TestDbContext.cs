using AutomationTestSample.Domain;
using Microsoft.EntityFrameworkCore;

namespace AutomationTestSample.Infrastructure;

public class TestDbContext : DbContext, ITestDbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Patient> Patients { get; set; }

    public Task<int> SaveChanges(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "TestDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // timestamps for patient & order creation
        var t1 = DateTimeOffset.Now.AddMinutes(-86);
        var t2 = DateTimeOffset.Now.AddMinutes(-79);
        var t3 = DateTimeOffset.Now.AddMinutes(-76).ToOffset(new TimeSpan(0, 0, 0)); //convert to a UK input
        var t4 = DateTimeOffset.Now.AddMinutes(-53);
        var t5 = DateTimeOffset.Now.AddMinutes(-53);
        var t6 = DateTimeOffset.Now.AddMinutes(-52);
        var t7 = DateTimeOffset.Now.AddMinutes(-27).ToOffset(new TimeSpan(0, 0, 0)); //convert to a UK input
        var t8 = DateTimeOffset.Now.AddMinutes(-14).ToOffset(new TimeSpan(0, 0, 0)); //convert to a UK input
        var t9 = DateTimeOffset.Now.AddMinutes(-02);

        // timestamps for the client studydatetimes, slightly before the order gets to our system
        var s1 = t1.AddSeconds(-101);
        var s2 = t2.AddSeconds(-201);
        var s3 = t3.AddSeconds(-48);
        var s4 = t4.AddSeconds(-120);
        var s5 = t5.AddSeconds(-54);
        var s6 = t6.AddSeconds(-108);
        var s7 = t7.AddSeconds(-77);
        var s8 = t8.AddSeconds(-89);
        var s9 = t9.AddSeconds(-61);


        var p1 = new Patient("P302", "James", "Anderson")   { Id = 1, CreatedAt = t1, UpdatedAt = t1 };
        var p2 = new Patient("P303", "Sarah", "Jones")      { Id = 2, CreatedAt = t2, UpdatedAt = t2 };
        var p3 = new Patient("P312", "Tony", "Jenkins")     { Id = 3, CreatedAt = t3, UpdatedAt = t3 };
        var p4 = new Patient("P067", "Eva", "Larson")       { Id = 4, CreatedAt = t4.AddDays(-221), UpdatedAt = t4 }; //patient created a long time ago, they would have had a study reported by us previously
        var p5 = new Patient("P332", "James", "Martinez")   { Id = 5, CreatedAt = t5, UpdatedAt = t5 };
        var p6 = new Patient("P334", "Jay", "Saunders")     { Id = 6, CreatedAt = t6, UpdatedAt = t6 };
        var p7 = new Patient("P335", "Virgil", "Smith")     { Id = 7, CreatedAt = t7, UpdatedAt = t7 };
        var p8 = new Patient("P337", "William", "Richards") { Id = 8, CreatedAt = t8, UpdatedAt = t8 };
        var p9 = new Patient("P338", "Alma", "Nuez")        { Id = 9, CreatedAt = t9, UpdatedAt = t9 };

        modelBuilder.Entity<Patient>().HasData(new List<Patient> 
        { 
            p1,p2,p3,p4,p5,p6,p7,p8,p9
        });


        modelBuilder.Entity<Order>().HasData(new List<Order>
        {
            new Order(p1.Id,"00486", ClientHelper.Lumus.OrgCode,    ClientHelper.Lumus.Sites[0].Id, ModalityHelper.MR.Code,     s1, Domain.Status.ZZ)  { Id = 1, CreatedAt = t1, UpdatedAt = t1 },
            new Order(p2.Id,"00487", ClientHelper.Lumus.OrgCode,    ClientHelper.Lumus.Sites[1].Id, ModalityHelper.US.Code,     s2, Domain.Status.ZZ)  { Id = 2, CreatedAt = t2, UpdatedAt = t2 },
            new Order(p3.Id,"00492", ClientHelper.CareUK.OrgCode,   ClientHelper.CareUK.Sites[2].Id, ModalityHelper.MR.Code,    s3, Domain.Status.ZZ)  { Id = 3, CreatedAt = t3, UpdatedAt = t3 },
            new Order(p4.Id,"00494", ClientHelper.Lumus.OrgCode,    ClientHelper.Lumus.Sites[2].Id, ModalityHelper.CT.Code,     s4, Domain.Status.CMD) { Id = 4, CreatedAt = t4, UpdatedAt = t4 },
            new Order(p5.Id,"00501", ClientHelper.Lumus.OrgCode,    ClientHelper.Lumus.Sites[4].Id, ModalityHelper.XR.Code,     s5, Domain.Status.CM)  { Id = 5, CreatedAt = t5, UpdatedAt = t5 },
            new Order(p6.Id,"00503", ClientHelper.USClinic.OrgCode, ClientHelper.USClinic.Sites[0].Id, ModalityHelper.US.Code,  s6, Domain.Status.CM)  { Id = 6, CreatedAt = t6, UpdatedAt = t6 },
            new Order(p7.Id,"00504", ClientHelper.CareUK.OrgCode,   ClientHelper.CareUK.Sites[0].Id, ModalityHelper.CT.Code,    s7, Domain.Status.IP)  { Id = 7, CreatedAt = t7, UpdatedAt = t7 },
            new Order(p8.Id,"00506", ClientHelper.CareUK.OrgCode,   ClientHelper.CareUK.Sites[1].Id, ModalityHelper.XR.Code,    s8, Domain.Status.SC)  { Id = 8, CreatedAt = t8, UpdatedAt = t8 },
            new Order(p9.Id,"00507", ClientHelper.Lumus.OrgCode,    ClientHelper.Lumus.Sites[3].Id, ModalityHelper.XR.Code,     s9, Domain.Status.SC)  { Id = 9, CreatedAt = t9, UpdatedAt = t9 }

        });
    }
}
