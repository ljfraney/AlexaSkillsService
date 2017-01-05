using AlexaSkillsService.Interfaces;
using System.Web.Mvc;
using AlexaSkillsService.Helpers;

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
        public ActionResult Year(string serialNumber)
        {
#if DEBUG
            ViewBag.Debug = true;
#endif
            //User has five minutes to get to the Year view.
            var game = _bombStopperGameManager.GetGameBySerialNumber(serialNumber, 5.0);
            return View("Year", game);
        }

        [HttpPost]
        public ActionResult GoToGame(string cryptoGameId)
        {
            //User has ten minutes to get to the Game view.
            var game = _bombStopperGameManager.StartGame(cryptoGameId, 10.0);
            return View("Game", game);
        }

        [HttpPost]
        public ActionResult Solve(string cryptoGameId, int wireToCut)
        {
            var game = _bombStopperGameManager.Solve(cryptoGameId, wireToCut);
            return View("Result", game);
        }
    }
}