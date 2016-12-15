using System;
using AlexaSkillsService.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlexaSkillsService.Tests
{
    [TestClass]
    public class DontBlowUpGameManagerTests
    {
        [TestMethod]
        public void GetRandomRuleSet_Expected()
        {
            var gameManager = new DontBlowUpGameManager(null);
            gameManager.GetRandomRuleSet(6);

            //TODO: Finish GetRandomRuleSet
        }
    }
}
