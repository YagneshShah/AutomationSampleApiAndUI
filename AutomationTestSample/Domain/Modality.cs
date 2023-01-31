namespace AutomationTestSample.Domain
{
public class Modality
    {
        public string Code { get; init; }
        public string? Name { get; init; }

        public Modality(string code, string? name)
        {
            Code = code;
            Name = name;
        }
    }
}
