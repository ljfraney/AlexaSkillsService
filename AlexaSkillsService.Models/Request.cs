using System;

namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    public class Request
    {
        public string Type { get; set; }

        public string RequestId { get; set; }

        public DateTime Timestamp { get; set; }

        public Intent Intent { get; set; }

        public string Locale { get; set; }

        public Request()
        {
            Intent = new Intent();
        }
    }
}