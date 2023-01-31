using AutomationTestSample.Domain;

namespace AutomationTestSample.Infrastructure;

public static class ClientHelper
{
    public static Client Lumus=> new("LUM", "Lumus")
    {
        Sites = new List<ClientSite>
        {
            new ClientSite(101, "Northern Beaches Hospital"),
            new ClientSite(102, "Baulkham Hills"),
            new ClientSite(103, "Ingleburn"),
            new ClientSite(104, "Camden Nuclear Medicine"),
            new ClientSite(105, "St George Private Hospital"),
        }
    };

    public static Client CareUK => new("CUK", "Care UK")
    {
        Sites = new List<ClientSite>
        {
            new ClientSite(201, "Sussex"),
            new ClientSite(202, "Lincoln"),
            new ClientSite(203, "Spalding"),
        }
    };

    public static Client USClinic => new("USC", "The Ultrasound Clinic")
    {
        Sites = new List<ClientSite>
        {
            new ClientSite(301, "Clinic")
        }
    };

    public static IEnumerable<Client> GetList()
    {
        return new List<Client> 
        {
            Lumus, CareUK, USClinic
        };
    }
}
