namespace Contracts
{
    public class ValueEntered
    {
        public ValueEntered(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}