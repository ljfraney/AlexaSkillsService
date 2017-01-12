using AlexaSkillsService.Common;
using AlexaSkillsService.Interfaces;
using System.Web.Mvc;
using System.Web.Security;

namespace AlexaSkillsService.Controllers
{
    public class BombStopperController : Controller
    {
        private readonly IBombStopperGameManager _bombStopperGameManager;
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly ICacheAdapter _cacheAdapter;

        public BombStopperController(IBombStopperGameManager bombStopperGameManager, IConfigurationAdapter configurationAdapter, ICacheAdapter cacheAdapter)
        {
            _bombStopperGameManager = bombStopperGameManager;
            _configurationAdapter = configurationAdapter;
            _cacheAdapter = cacheAdapter;
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
        public ActionResult Year(string serialNumber)
        {
            //User has five minutes to get to the Year view.
            var game = _bombStopperGameManager.GetGameBySerialNumber(serialNumber, 5.0);
            if (game != null)
            {
                FormsAuthentication.SetAuthCookie(game.UserId, false);
                return View("Year", game);
            }
            ViewBag.BadSerial = true;
            return View("Index");
        }

        [HttpPost]
        public ActionResult Game(string cryptoGameId)
        {
            //User has ten minutes to get to the Game view.
            var gameId = int.Parse(Crypto.DecryptStringAES(cryptoGameId, _configurationAdapter.SharedSecret));
            var game = _bombStopperGameManager.StartGame(gameId, 10.0);
            return View(game);
        }

        [HttpPost]
        public ActionResult Result(string cryptoGameId)
        {
            var gameId = int.Parse(Crypto.DecryptStringAES(cryptoGameId, _configurationAdapter.SharedSecret));
            var game = _bombStopperGameManager.GetGameById(gameId);
            return View(game);
        }
    }
}