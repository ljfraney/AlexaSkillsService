using System;
using System.Linq;

namespace AlexaSkillsService.Common
{
    public class Ordinal
    {
        public int IntValue { get; set; }

        public string StringValue { get; set; }

        public string AlternateValue { get; set; }

        public static bool TryParse(string s, out Ordinal result)
        {
            var ordinal = OrdinalHelper.OrdinalList.FirstOrDefault(o => string.Equals(o.StringValue, s, StringComparison.OrdinalIgnoreCase) || 
                                                                        string.Equals(o.AlternateValue, s, StringComparison.OrdinalIgnoreCase));
            if (ordinal == null)
            {
                result = null;
                return false;
            }

            result = ordinal;
            return true;
        }
    }
}