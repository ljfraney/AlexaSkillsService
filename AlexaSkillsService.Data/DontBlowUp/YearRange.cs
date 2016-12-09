namespace AlexaSkillsService.Data.DontBlowUp
{
    public class YearRange
    {
        public int LanguageYearRangeId { get; set; }

        public int FromYear { get; set; }

        public int ToYear { get; set; }

        public int WireColor1Id { get; set; }

        public int WireColor2Id { get; set; }

        public int WireColor3Id { get; set; }

        public int WireColor4Id { get; set; }

        public int WireColor5Id { get; set; }

        public int LanguageId { get; set; }

        public virtual WireColor Color1 { get; set; }

        public virtual WireColor Color2 { get; set; }

        public virtual WireColor Color3 { get; set; }

        public virtual WireColor Color4 { get; set; }

        public virtual WireColor Color5 { get; set; }

        public virtual Language Language { get; set; }
    }
}
