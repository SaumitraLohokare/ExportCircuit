using Prosumergrid.Domain.Attributes;
using System.Text.RegularExpressions;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    /// <summary>
    /// Base class for any distribution element.
    /// </summary>
    public abstract class DisplayableElement : DistributionElement
    {
        public string? GroupId { get; set; }
        
        [GlmName("phases")]
        public string? Phases { get; set; }

        public int NumPhases
        {
            get => PrimaryPhases.Count;
        }

        public List<string> PrimaryPhases
        {
            get
            {
                string pattern = @"[ABC]";
                var regex = new Regex(pattern);
                var phases = Phases ?? String.Empty;
                var matches = regex.Matches(phases).Select(m => m.ToString()).ToList();
                return matches;
            }
        }
    }
}
