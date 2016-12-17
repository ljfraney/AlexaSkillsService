namespace AlexaSkillsService.Data.DontBlowUp
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    // ReSharper disable DoNotCallOverridableMethodsInConstructor
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    public class Wire
    {
        public int GameId { get; set; }

        public int SortOrder { get; set; }

        public WireColor Color { get; set; }

        public virtual Game Game { get; set; }
    }
}
