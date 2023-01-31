using AutomationTestSample.Domain;

namespace AutomationTestSample.Infrastructure;

public static class ModalityHelper
{
    public static Modality MR => new("MR", "MRI");

    public static Modality CT => new("CT", "CT");

    public static Modality XR => new("XR", "Xray");

    public static Modality US => new("US", "Ultrasound");

    public static IEnumerable<Modality> GetList()
    {
        return new List<Modality>
        {
            MR, CT, XR, US
        };
    }
}
