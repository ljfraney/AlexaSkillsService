namespace AlexaSkillsService.Common
{
    public static class IntExtender
    {
        public static string GetOrdinal(this int i)
        {
            switch (i)
            {
                case 1:
                    return "first";
                case 2:
                    return "second";
                case 3:
                    return "third";
                case 4:
                    return "fourth";
                case 5:
                    return "fifth";
                case 6:
                    return "sixth";
                case 7:
                    return "seventh";
                case 8:
                    return "eighth";
                case 9:
                    return "ninth";
                case 10:
                    return "tenth";
                case 11:
                    return "eleventh";
                case 12:
                    return "twelfth";
                case 13:
                    return "thirteenth";
                case 14:
                    return "fourteenth";
                case 15:
                    return "fifteenth";
                case 16:
                    return "sixteenth";
                case 17:
                    return "seventeenth";
                case 18:
                    return "eighteenth";
                case 19:
                    return "nineteenth";
                default:
                    return i + "th";
            }
        }
    }
}
