using AlexaSkillsService.Interfaces;
using System.Web.Mvc;

namespace AlexaSkillsService.Controllers
{
    public class DontBlowUpController : Controller
    {
        private readonly IDontBlowUpGameManager _dontBlowUpGameManager;

        public DontBlowUpController(IDontBlowUpGameManager dontBlowUpGameManager)
        {
            _dontBlowUpGameManager = dontBlowUpGameManager;
        }

        // GET: DontBlowUp
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewGame()
        {
            var game = _dontBlowUpGameManager.CreateGame("test", "test");
            return View("Test2", game);
        }

        [HttpPost]
        public ActionResult GoToGame(string serialNumber)
        {
            var savedGame = _dontBlowUpGameManager.GetGameBySerialNumber(serialNumber);
            return View("Game", savedGame);
        }
    }
}