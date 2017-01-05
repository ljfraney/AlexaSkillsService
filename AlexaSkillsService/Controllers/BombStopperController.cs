using AlexaSkillsService.Interfaces;
using System.Web.Mvc;

namespace AlexaSkillsService.Controllers
{
    public class BombStopperController : Controller
    {
        private readonly IBombStopperGameManager _bombStopperGameManager;

        public BombStopperController(IBombStopperGameManager bombStopperGameManager)
        {
            _bombStopperGameManager = bombStopperGameManager;
        }

        // GET: BombStopper
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
            var game = _bombStopperGameManager.CreateGame("test", "test");
            return View("Test2", game);
        }

        [HttpPost]
        public ActionResult GoToGame(string serialNumber)
        {
            var game = _bombStopperGameManager.StartGame(serialNumber, 5.0);
            return View("Game", game);
        }

        [HttpPost]
        public ActionResult Solve(int gameId, int wireToCut)
        {
            //TODO: Encrypt gameId.
            var game = _bombStopperGameManager.Solve(gameId, wireToCut);
            return View("Result", game);
        }
    }
}