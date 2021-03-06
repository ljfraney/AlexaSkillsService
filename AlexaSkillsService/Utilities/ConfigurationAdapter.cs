﻿using System.Configuration;
using AlexaSkillsService.Interfaces;

namespace AlexaSkillsService.Utilities
{
    public class ConfigurationAdapter : IConfigurationAdapter
    {
        public string RedisCache =>  ConfigurationManager.AppSettings["RedisCache"];

        public string BombStopperAppId => ConfigurationManager.AppSettings["BombStopperAppId"];

        public string SharedSecret => ConfigurationManager.AppSettings["SharedSecret"];

        public int YearQuestionMaxTries => int.Parse(ConfigurationManager.AppSettings["YearQuestionMaxTries"]);

        public string ExplosionAudioUrl => ConfigurationManager.AppSettings["ExplosionAudioUrl"];
    }
}