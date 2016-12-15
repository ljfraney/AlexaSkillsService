using System;
using System.Collections.Generic;

namespace AlexaSkillsService.Common
{
    public class RandomHelper : Random
    {
        private static RandomHelper _instance;

        private RandomHelper()
        {
            
        }

        public static RandomHelper Instance => _instance ?? (_instance = new RandomHelper());
    }
}