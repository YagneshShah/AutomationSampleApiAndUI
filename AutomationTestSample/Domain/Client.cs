namespace AutomationTestSample.Domain
{
    public class Client
    {
        public string OrgCode { get; init; }
        public string? Name { get; init; }

        public List<ClientSite> Sites { get; set; } = default!;

        public Client(string orgCode, string? name)
        {
            OrgCode = orgCode;
            Name = name;
        }
    }

    public class ClientSite
    {
        public int Id { get; init; }
        public string? Name { get; init; }

        public ClientSite(int id, string? name)
        {
            Id = id;
            Name = name;
        }
    }
}
