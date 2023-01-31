using System;

namespace ApiTests.PactTests.ProviderTests.ProviderState
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ProviderStateAttribute : Attribute
    {
        public string State { get; set; }
        public string Consumer { get; set; }
        public ProviderStateAttribute(string consumer, string state)
        {
            Consumer = consumer;
            State = state;
        }

        public ProviderStateAttribute(string state)
        {
            State = state;
        }
    }
}
