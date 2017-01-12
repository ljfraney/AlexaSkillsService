using AlexaSkillsService.Common;
using AlexaSkillsService.Hubs;
using AlexaSkillsService.Interfaces;
using AlexaSkillsService.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AlexaSkillsService.Utilities
{
    public class BombStopperRequestHandler : ISkillRequestHandler
    {
        private readonly IBombStopperGameManager _bombStopperGameManager;
        private readonly IConfigurationAdapter _configurationAdapter;

        public BombStopperRequestHandler(IBombStopperGameManager bombStopperGameManager, IConfigurationAdapter configurationAdapter)
        {
            _bombStopperGameManager = bombStopperGameManager;
            _configurationAdapter = configurationAdapter;
        }

        public async Task<AlexaResponse> HandleLaunchRequest(AlexaRequest request, AlexaResponse response)
        {
            try
            { 
                var newGame = _bombStopperGameManager.CreateGame(request.Session.SessionId, request.Session.User.UserId);
                response.AddOrSetSessionItem(SessionKey.BombStopper_GameId, newGame.GameId);
                response.AddOrSetSessionItem(SessionKey.BombStopper_SerialNumber, newGame.SerialNumber);
                response.AddOrSetSessionItem(SessionKey.BombStopper_TimesAnsweredYearQuestion, 0);
                response.Response.OutputSpeech.Type = "SSML";
                response.Response.OutputSpeech.Ssml = 
                "<speak>" + 
                    "Welcome to Bomb Stopper! Begin by going to www.bombstopper.com. Enter serial number" +
                    $"<say-as interpret-as=\"spell-out\">{newGame.SerialNumber}</say-as>. " +
                    "What year is shown on the bomb ? " +
                "</speak>";
                response.Response.Reprompt.OutputSpeech.Type = "SSML";
                response.Response.Reprompt.OutputSpeech.Text =
                "<speak>" +
                    "Go to www.bombstopper.com.Enter serial number" +
                    $"<say-as interpret-as=\"spell-out\">{newGame.SerialNumber}</say-as>. " +
                "</speak>";
                response.SetSkillState(SkillState.BombStopper_WaitingForYear);
                response.Response.ShouldEndSession = false;
                return response;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
        }

        public async Task<AlexaResponse> HandleIntentRequest(AlexaRequest request, AlexaResponse response)
        {
            switch (request.Request.Intent.Name)
            {
                case "AMAZON.RepeatIntent":
                    response = await ProcessRepeatIntent(request, response);
                    break;
                case "YearIntent":
                    response = await ProcessYearIntent(request, response);
                    break;
                case "ColorIntent":
                    response = await ProcessColorIntent(request, response);
                    break;
                case "OrdinalIntent":
                    response = await ProcessOrdinalIntent(request, response);
                    break;
                case "RepeatWiresIntent":
                    response = await ProcessRepeatWiresIntent(request, response);
                    break;
                case "AMAZON.NoIntent":
                    response = await ProcessNoIntent(request, response);
                    break;
                case "AMAZON.YesIntent":
                    response = await ProcessYesIntent(request, response);
                    break;
                //case "AMAZON.Date":
                //    response = await ProcessDateIntent(request, response);
                //    break;
                //case "AMAZON.Time":
                //    response = await ProcessTimeIntent(request, response);
                //    break;
                case "AMAZON.CancelIntent":
                    response = await ProcessCancelIntent(request, response);
                    break;
                case "AMAZON.StopIntent":
                    response = await ProcessCancelIntent(request, response);
                    break;
                case "AMAZON.HelpIntent":
                    response = await ProcessHelpIntent(request, response);
                    break;
            }

            return response;
        }

        public async Task<AlexaResponse> HandleSessionEndedRequest(AlexaRequest request, AlexaResponse response)
        {
            try
            { 
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<BombStopperHub>();
                hubContext.Clients.User(request.Session.User.UserId).goHome();
                response.Response.OutputSpeech.Text = "Thanks for playing! Bye.";
                response.Response.ShouldEndSession = true;
                return response;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
        }

        private async Task<AlexaResponse> ProcessCancelIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            { 
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<BombStopperHub>();
                hubContext.Clients.User(request.Session.User.UserId).goHome();
                response.Response.OutputSpeech.Text = "Thanks for playing! Bye.";
                response.Response.ShouldEndSession = true;
                return response;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
        }

        private async Task<AlexaResponse> ProcessYesIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                IHubContext hubContext;
                switch (request.SkillState())
                {
                    case SkillState.BombStopper_AreYouReady:
                        var gameId = request.SessionItem<int>(SessionKey.BombStopper_GameId);
                        var game = _bombStopperGameManager.GetGameById(gameId);
                        hubContext = GlobalHost.ConnectionManager.GetHubContext<BombStopperHub>();
                        hubContext.Clients.User(request.Session.User.UserId).goToGame();
                        var wiresDescription = game.Wires.Aggregate("", (current, wire) => current + (wire.Color + ", "));
                        response.Response.OutputSpeech.Text = $"OK. I see the following wires on the bomb. {wiresDescription}. Which wire should I cut?";
                        response.Response.Reprompt.OutputSpeech.Text = "Do you need more time?";
                        response.SetSkillState(SkillState.BombStopper_GameOn);
                        response.Response.ShouldEndSession = false;
                        break;
                    case SkillState.BombStopper_GameOn:
                        response.Response.OutputSpeech.Text = "OK.";
                        response.Response.Reprompt.OutputSpeech.Text = "Do you need more time? Say yes or no.";
                        response.Response.ShouldEndSession = false;
                        break;
                    case SkillState.BombStopper_PlayAgain:
                        hubContext = GlobalHost.ConnectionManager.GetHubContext<BombStopperHub>();
                        hubContext.Clients.User(request.Session.User.UserId).goHome();
                        var newGame = _bombStopperGameManager.CreateGame(request.Session.SessionId, request.Session.User.UserId);
                        var readableSerialNumber = newGame.SerialNumber.ToString().Aggregate("", (current, character) => current + (character + " "));
                        response.AddOrSetSessionItem(SessionKey.BombStopper_GameId, newGame.GameId);
                        response.Response.OutputSpeech.Text = $"Enter serial number { readableSerialNumber } and read me the year shown on the bomb.";
                        response.Response.Reprompt.OutputSpeech.Text = $"Enter serial number { readableSerialNumber }. What year is shown on the bomb?.";
                        response.SetSkillState(SkillState.BombStopper_WaitingForYear);
                        response.Response.ShouldEndSession = false;
                        break;
                    default:
                        response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                        response.Response.ShouldEndSession = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
            return response;
        }

        private async Task<AlexaResponse> ProcessNoIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                switch (request.SkillState())
                {
                    case SkillState.BombStopper_AreYouReady:
                    case SkillState.BombStopper_GameOn:
                    case SkillState.BombStopper_PlayAgain:
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<BombStopperHub>();
                        hubContext.Clients.User(request.Session.User.UserId).goHome();
                        response.Response.OutputSpeech.Text = "Ok. Come back when you want to play again.";
                        response.Response.ShouldEndSession = true;
                        response.Response.Reprompt = null;
                        break;
                    default:
                        response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                        response.Response.ShouldEndSession = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
            return response;
        }

        private async Task<AlexaResponse> ProcessYearIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                var userInput = (string)request.Request.Intent.Slots["Year"].value;

                int? year = null;

                int iYear;
                if (int.TryParse(userInput, out iYear))
                    year = iYear;
                else
                {
                    DateTime dt;
                    if (DateTime.TryParse(userInput, out dt))
                        year = dt.Year;
                }

                if (year != null)
                {
                    switch (request.SkillState())
                    {
                        case SkillState.BombStopper_WaitingForYear:
                            var gameId = request.SessionItem<int>(SessionKey.BombStopper_GameId);
                            var game = _bombStopperGameManager.GetGameById(gameId);
                            if (game.Year == year)
                            {
                                response.Response.OutputSpeech.Text = $"I'm sending you the bomb defusal field manual from {year}. I'm going to check out the bomb and tell you what I see. Are you ready?";
                                response.Response.Reprompt.OutputSpeech.Text = "Are you ready to start? Say yes or no.";
                                response.SetSkillState(SkillState.BombStopper_AreYouReady);
                                response.Response.ShouldEndSession = false;
                            }
                            else
                            {
                                var yearQuestionMaxTries = _configurationAdapter.YearQuestionMaxTries;
                                var timesAnsweredYear = request.SessionItem<int>(SessionKey.BombStopper_TimesAnsweredYearQuestion);
                                if (timesAnsweredYear == yearQuestionMaxTries)
                                {
                                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<BombStopperHub>();
                                    hubContext.Clients.User(game.UserId).goHome();
                                    response.Response.OutputSpeech.Text = $"Sorry, {year} was not the right year. Do you want to start a new game?";
                                    response.Response.ShouldEndSession = false;
                                }
                                else
                                {
                                    response.Response.OutputSpeech.Text = $"Sorry, {year} was not the right year. What is the year on the bomb?";
                                    response.Response.ShouldEndSession = false;
                                }
                            }
                            break;
                        default:
                            response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                            response.Response.ShouldEndSession = true;
                            break;
                    }
                }
                else
                {
                    response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                    response.Response.ShouldEndSession = false;
                }
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
            return response;
        }

        private async Task<AlexaResponse> ProcessColorIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                switch (request.SkillState())
                {
                    case SkillState.BombStopper_GameOn:
                        var color = (string)request.Request.Intent.Slots["Color"].value;
                        var gameId = request.SessionItem<int>(SessionKey.BombStopper_GameId);
                        var game = _bombStopperGameManager.GetGameById(gameId);
                        var countOfColor = game.Wires.Count(w => string.Equals(color, w.Color.ToString(), StringComparison.OrdinalIgnoreCase));
                        if (countOfColor == 0)
                        {
                            response.Response.OutputSpeech.Text = $"There are no {color} wires. Which wire should I cut?";
                            response.Response.Reprompt.OutputSpeech.Text = "Do you need more time?";
                        }
                        else if (countOfColor > 1)
                        {
                            response.Response.OutputSpeech.Text = $"There is more than one {color} wire. Which wire should I cut?";
                            response.Response.Reprompt.OutputSpeech.Text = "Do you need more time?";
                        }
                        else
                        {
                            var sortOrder = game.Wires.Single(w => string.Equals(color, w.Color.ToString(), StringComparison.OrdinalIgnoreCase)).SortOrder;
                            Solve(request, response, gameId, sortOrder);
                        }

                        break;
                    default:
                        response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                        break;
                }
                response.Response.ShouldEndSession = false;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
            return response;
        }

        private async Task<AlexaResponse> ProcessOrdinalIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                switch (request.SkillState())
                {
                    case SkillState.BombStopper_GameOn:
                        var userInput = (string)request.Request.Intent.Slots["Ordinal"].value;

                        Ordinal ordinal = null;
                        if (userInput.Equals("last", StringComparison.OrdinalIgnoreCase) || Ordinal.TryParse(userInput, out ordinal))
                        {
                            var gameId = request.SessionItem<int>(SessionKey.BombStopper_GameId);
                            var game = _bombStopperGameManager.GetGameById(gameId);

                            if (userInput.Equals("last", StringComparison.OrdinalIgnoreCase))
                            {
                                var positionOfLast = game.Wires.Count;
                                Solve(request, response, gameId, positionOfLast);
                            }
                            else
                            {
                                Debug.Assert(ordinal != null, "ordinal != null");
                                if (ordinal.IntValue > game.Wires.Count)
                                {
                                    response.Response.OutputSpeech.Text = $"There are fewer than {ordinal.IntValue} wires. Which wire should I cut?";
                                    response.Response.Reprompt.OutputSpeech.Text = "Do you need more time?";
                                }
                                else
                                {
                                    Solve(request, response, gameId, ordinal.IntValue);
                                }
                            }
                        }
                        break;
                    default:
                        response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                        break;
                }
                response.Response.ShouldEndSession = false;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
            return response;
        }

        private void Solve(AlexaRequest request, AlexaResponse response, int gameId, int sortOrder)
        {
            var finishedGame = _bombStopperGameManager.Solve(gameId, sortOrder);
            response.SetSkillState(SkillState.BombStopper_PlayAgain);
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BombStopperHub>();
            hubContext.Clients.User(request.Session.User.UserId).goToResult();
            if (finishedGame.Won == true)
                response.Response.OutputSpeech.Text = $"OK. I cut the {sortOrder.GetOrdinal()} wire. The timer stopped. That was it!. Good job! Do you want to play again?";
            else
            {
                response.Response.OutputSpeech.Ssml = $"<speak>I'm cutting the {sortOrder.GetOrdinal()} wire. Oh no! Something isn't right.<audio src=\"{_configurationAdapter.ExplosionAudioUrl}\" />Do you want to play again?</speak>";
                response.Response.OutputSpeech.Type = "SSML";

            }
        }

        private async Task<AlexaResponse> ProcessRepeatWiresIntent(AlexaRequest request, AlexaResponse response)
        {
            return await ProcessRepeatIntent(request, response);
        }

        private async Task<AlexaResponse> ProcessRepeatIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            { 
                switch (request.SkillState())
                {
                    case SkillState.BombStopper_WaitingForYear:
                        var serialNumber = request.SessionItem<int>(SessionKey.BombStopper_SerialNumber);
                        response.Response.OutputSpeech.Type = "SSML";
                        response.Response.OutputSpeech.Ssml =
                        "<speak>" +
                            "From a web browser, go to www.bombstopper.com. Enter serial number" +
                            $"<say-as interpret-as=\"spell-out\">{serialNumber}</say-as>. " +
                            "and click the check mark. Tell me the year that you see on the bomb." +
                        "</speak>";
                        response.Response.Reprompt.OutputSpeech.Type = "SSML";
                        response.Response.Reprompt.OutputSpeech.Ssml = response.Response.OutputSpeech.Text;
                        response.Response.ShouldEndSession = false;
                        break;
                    case SkillState.BombStopper_AreYouReady:
                        response.Response.OutputSpeech.Text = "Are you ready to start? Say yes or no.";
                        response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                        response.Response.ShouldEndSession = false;
                        break;
                    case SkillState.BombStopper_GameOn:
                        var gameId = request.SessionItem<int>(SessionKey.BombStopper_GameId);
                        var game = _bombStopperGameManager.GetGameById(gameId);
                        var wiresDescription = game.Wires.Aggregate("", (current, wire) => current + (wire.Color + ", "));
                        response.Response.OutputSpeech.Text = $"The wires are {wiresDescription}. Which wire should I cut?";
                        response.Response.Reprompt.OutputSpeech.Text = "Do you need more time?";
                        break;
                    case SkillState.BombStopper_PlayAgain:
                        response.Response.OutputSpeech.Text = "Do you want to play again? Say yes or no.";
                        response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                        response.Response.ShouldEndSession = false;
                        break;
                    default:
                        response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                        response.Response.ShouldEndSession = false;
                        break;
                }

                return response;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
        }

        private async Task<AlexaResponse> ProcessHelpIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            { 
                switch (request.SkillState())
                {
                    case SkillState.BombStopper_WaitingForYear:
                        var serialNumber = request.SessionItem<int>(SessionKey.BombStopper_SerialNumber);
                        response.Response.OutputSpeech.Type = "SSML";
                        response.Response.OutputSpeech.Ssml =
                        "<speak>" +
                            "From a web browser, go to www.bombstopper.com. Enter serial number" +
                            $"<say-as interpret-as=\"spell-out\">{serialNumber}</say-as>. " +
                            "and click the check mark. Tell me the year that you see on the bomb." +
                        "</speak>";
                        response.Response.Reprompt.OutputSpeech.Type = "SSML";
                        response.Response.Reprompt.OutputSpeech.Ssml = response.Response.OutputSpeech.Text;
                        response.Response.ShouldEndSession = false;
                        break;
                    case SkillState.BombStopper_AreYouReady:
                        response.Response.OutputSpeech.Text = "I was just wondering if you are ready. Say yes or no.";
                        response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                        response.Response.ShouldEndSession = false;
                        break;
                    case SkillState.BombStopper_GameOn:
                        response.Response.OutputSpeech.Text = "Tell me which wire to cut. You can say things like, the third wire, or cut the red wire. You can also ask me to repeat the wires.";
                        response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                        response.Response.ShouldEndSession = false;
                        break;
                    case SkillState.BombStopper_PlayAgain:
                        response.Response.OutputSpeech.Text = "Do you want to play again? Say yes or no.";
                        response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                        response.Response.ShouldEndSession = false;
                        break;
                    default:
                        response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                        response.Response.ShouldEndSession = false;
                        break;
                }

                return response;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
        }

        //private async Task<AlexaResponse> ProcessTimeIntent(AlexaRequest request, AlexaResponse response)
        //{
        //    var content = "";
        //    var reprompt = "";
        //    var isValid = false;

        //    try
        //    {
        //        if (request.Request.Intent != null && request.Request.Intent.Slots != null)
        //        {
        //            var slot = request.Request.Intent.Slots;

        //            if (slot["Time"] != null)
        //            {
        //                var result = (string)slot["Time"].value;

        //                isValid = true;

        //                var fixedTime = Convert.ToDateTime(result).ToString("hh:mm");

        //                content = "";
        //                reprompt = content;
        //            }
        //        }

        //        if (!isValid)
        //        {
        //            content = "I didn't understand your last response.";
        //            reprompt = @"";
        //        }

        //        response.Response.ShouldEndSession = false;
        //        response.Response.OutputSpeech.Text = content;
        //        response.Response.Reprompt.OutputSpeech.Text = reprompt;
        //    }
        //    catch (Exception ex)
        //    {

        //        if (ex.Message.Contains("String was not recognized as a valid DateTime"))
        //            return ProcessRepeatIntent(request, response);

        //        return await SafeExceptionHandler.ThrowSafeException(ex, response);
        //    }

        //    return response;
        //}

        //private async Task<AlexaResponse> ProcessDateIntent(AlexaRequest request, AlexaResponse response)
        //{
        //    var content = "";
        //    var isValid = false;

        //    try
        //    {
        //        if (request.Request.Intent != null && request.Request.Intent.Slots != null)
        //        {
        //            var slot = request.Request.Intent.Slots;

        //            if (slot["Date"] != null)
        //            {
        //                var theDate = (string)slot["Date"].value;
        //                var formattedDate = Convert.ToDateTime(theDate);

        //                if (formattedDate > DateTime.Today.AddDays(DateMaxInDays))
        //                {
        //                    content = "Sorry that date was over 60 days from now. You can say things like: July 26th, or next Tuesday, or any day before " + DateTime.Today.AddDays(DateMaxInDays).ToShortDateString() + ".";
        //                    response.Response.ShouldEndSession = false;
        //                    response.Response.Reprompt.OutputSpeech.Text = content;
        //                    response.Response.OutputSpeech.Text = content;
        //                    return response;
        //                }

        //                content = "";
        //                isValid = true;
        //            }
        //        }

        //        if (!isValid)
        //            return ProcessRepeatIntent(request, response);

        //        response.Response.Reprompt.OutputSpeech.Text = content;
        //        response.Response.ShouldEndSession = false;
        //        response.Response.OutputSpeech.Text = content;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("String was not recognized as a valid DateTime"))
        //            return ProcessRepeatIntent(request, response);

        //        return await SafeExceptionHandler.ThrowSafeException(ex, response);
        //    }

        //    return response;
        //}
    }
}
