namespace Prosumergrid.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class GlmNameAttribute : Attribute
    {
        public GlmNameAttribute(string name)
        {
            Name = name;

        }

        public string Name { get; }
    }
}
