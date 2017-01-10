using System;
using AlexaSkillsService.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AlexaSkillsService.Common
{
    public static class AlexaResponseExtender
    {
        public static void SetSkillState(this AlexaResponse alexaResponse, SkillState skillState)
        {
            alexaResponse.SessionAttributes.SkillAttributes.SkillState = (int)skillState;
        }

        public static void AddOrSetSessionItem<T>(this AlexaResponse alexaResponse, SessionKey key, T value) where T : IConvertible
        {
            alexaResponse.RemoveSessionItem(key);
            alexaResponse.SessionAttributes.SkillAttributes.KeyValuePairs.Add(new KeyValuePair<string, string>(key.ToString(), value.ToString(CultureInfo.InvariantCulture)));
        }

        public static void RemoveSessionItem(this AlexaResponse alexaResponse, SessionKey key)
        {
            if (alexaResponse.SessionAttributes.SkillAttributes.KeyValuePairs.Any(kvp => kvp.Key == key.ToString()))
                alexaResponse.SessionAttributes.SkillAttributes.KeyValuePairs.Remove(alexaResponse.SessionAttributes.SkillAttributes.KeyValuePairs.First(kvp => kvp.Key == key.ToString()));
        }
    }
}
