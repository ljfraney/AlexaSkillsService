using System;
using System.Linq;
using AlexaSkillsService.Models;

namespace AlexaSkillsService.Common
{
    public static class AlexaRequestExtender
    {
        public static SkillState SkillState(this AlexaRequest alexaRequest)
        {
            return (SkillState)alexaRequest.Session.Attributes.SkillAttributes.SkillState;
        }

        public static T SessionItem<T>(this AlexaRequest alexaRequest, SessionKey key) where T : IConvertible
        {
            var strValue = alexaRequest.Session.Attributes.SkillAttributes.KeyValuePairs.FirstOrDefault(kvp => kvp.Key == key.ToString()).Value;
            return (T)Convert.ChangeType(strValue, typeof(T));
        }
    }
}
