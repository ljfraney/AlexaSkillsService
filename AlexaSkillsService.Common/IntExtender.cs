using System.Linq;

namespace AlexaSkillsService.Common
{
    public static class IntExtender
    {
        public static string GetOrdinal(this int i)
        {
            var result = OrdinalHelper.OrdinalList.FirstOrDefault(o => o.IntValue == i);
            if (result != null)
                return result.StringValue;

            if (i > 0)
                return i + "th";

            return null;
        }
    }
}
