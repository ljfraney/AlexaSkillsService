using System.Collections.Generic;

namespace AlexaSkillsService.Common
{
    public static class OrdinalHelper
    {
        public static List<Ordinal> OrdinalList => new List<Ordinal>
        {
            new Ordinal { IntValue = 1, StringValue = "first", AlternateValue = "1st" },
            new Ordinal { IntValue = 2, StringValue = "second", AlternateValue = "2nd" },
            new Ordinal { IntValue = 3, StringValue = "third", AlternateValue = "3rd" },
            new Ordinal { IntValue = 4, StringValue = "fourth", AlternateValue = "4th" },
            new Ordinal { IntValue = 5, StringValue = "fifth", AlternateValue = "5th" },
            new Ordinal { IntValue = 6, StringValue = "sixth", AlternateValue = "6th" },
            new Ordinal { IntValue = 7, StringValue = "seventh", AlternateValue = "7th" },
            new Ordinal { IntValue = 8, StringValue = "eighth", AlternateValue = "8th" },
            new Ordinal { IntValue = 9, StringValue = "ninth", AlternateValue = "9th" },
            new Ordinal { IntValue = 10, StringValue = "tenth", AlternateValue = "10th" },
            new Ordinal { IntValue = 11, StringValue = "eleventh", AlternateValue = "11th" },
            new Ordinal { IntValue = 12, StringValue = "twelfth", AlternateValue = "12th" },
            new Ordinal { IntValue = 13, StringValue = "thirteenth", AlternateValue = "13th" },
            new Ordinal { IntValue = 14, StringValue = "fourteenth", AlternateValue = "14th" },
            new Ordinal { IntValue = 15, StringValue = "fifteenth", AlternateValue = "15th" },
            new Ordinal { IntValue = 16, StringValue = "sixteenth", AlternateValue = "16th" },
            new Ordinal { IntValue = 17, StringValue = "seventeenth", AlternateValue = "17th" },
            new Ordinal { IntValue = 18, StringValue = "eighteenth", AlternateValue = "18th" },
            new Ordinal { IntValue = 19, StringValue = "nineteenth", AlternateValue = "19th" }
        };
    }
}
