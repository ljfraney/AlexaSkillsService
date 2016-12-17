namespace AlexaSkillsService.Data.DontBlowUp
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    // ReSharper disable DoNotCallOverridableMethodsInConstructor
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    public class Rule
    {
        public int RuleId { get; set; }

        public int RuleIndex { get; set; }

        public WireColor WireColor { get; set; }

        //If IsPosition is true, then the value of WirePositionOrCount refers to the position of the wire whos color is being inspected.
        //If IsPosition is false, then the value of WirePositionOrCount refers to the count of the wires of the designated color.
        public bool IsPosition { get; set; }

        //If IsPosition is true, then the value of WirePositionOrCount must be greater than zero, and not greater than NumberOfWires.
        //If IsPosition is false, then the value of WirePositionOrCount must be zero or greater, but not more than the ceiling of 50% of NumberOfWires.
        public int WirePositionOrCount { get; set; }

        public int WireToCutPosition { get; set; }

        public WireCountOperator? Operator { get; set; }

        public string RuleText { get; set; }

        public virtual Game Game { get; set; }
    }
}
