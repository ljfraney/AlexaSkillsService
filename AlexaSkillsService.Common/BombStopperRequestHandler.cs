using AlexaSkillsService.Interfaces;
using AlexaSkillsService.Models;
using AlexaSkillsService.Models.BombStopper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlexaSkillsService.Common
{
    public class BombStopperRequestHandler : ISkillRequestHandler
    {
        private readonly IBombStopperGameManager _bombStopperGameManager;

        public BombStopperRequestHandler(IBombStopperGameManager bombStopperGameManager)
        {
            _bombStopperGameManager = bombStopperGameManager;
        }

        public AlexaResponse HandleLaunchRequest(AlexaRequest request, AlexaResponse response)
        {
            response.Response.OutputSpeech.Text = "Welcome to Bomb Stopper. Are you ready to start a game?";
            response.Response.Reprompt.OutputSpeech.Text = "Are you ready to start a game? Please answer yes or no.";
            response.SessionAttributes.Strings.Add(GameState.AskedDoYouWantToStart.ToString());
            response.Response.ShouldEndSession = false;
            return response;
        }

        public async Task<AlexaResponse> HandleIntentRequest(AlexaRequest request, AlexaResponse response)
        {
            var shouldSetLastIntent = true;

            switch (request.Request.Intent.Name)
            {
                case "AMAZON.RepeatIntent":
                    response = ProcessRepeatIntent(request, response);
                    shouldSetLastIntent = false;
                    break;
                case "ThanksIntent":
                    response = ProcessThanksIntent(request, response);
                    break;
                case "AMAZON.NoIntent":
                    response = await ProcessNoIntent(request, response);
                    shouldSetLastIntent = false;
                    break;
                case "AMAZON.YesIntent":
                    response = await ProcessYesIntent(request, response);
                    shouldSetLastIntent = false;
                    break;
                case "UnknownIntent":
                    response = ProcessUnknownIntent(request, response);
                    break;
                case "AMAZON.Date":
                    response = await ProcessDateIntent(request, response);
                    break;
                case "AMAZON.Time":
                    response = await ProcessTimeIntent(request, response);
                    break;
                case "AMAZON.CancelIntent":
                    response = ProcessCancelIntent(request, response);
                    break;
                case "AMAZON.StopIntent":
                    response = ProcessCancelIntent(request, response);
                    break;
                case "AMAZON.HelpIntent":
                    response = ProcessHelpIntent(request, response);
                    break;
            }

            if (shouldSetLastIntent)
                response.SessionAttributes.SkillAttributes.LastRequestIntent = request.Request.Intent.Name;

            return response;
        }

        public AlexaResponse HandleSessionEndedRequest(AlexaRequest request, AlexaResponse response)
        {
            response.Response.OutputSpeech.Text = "Thanks for playing!";
            response.Response.ShouldEndSession = true;
            return response;
        }

        private AlexaResponse ProcessCancelIntent(AlexaRequest request, AlexaResponse response)
        {
            response.Response.OutputSpeech.Text = "Goodbye.";
            response.Response.ShouldEndSession = true;
            return response;
        }

        private AlexaResponse ProcessRepeatIntent(AlexaRequest request, AlexaResponse response)
        {
            response.Response.OutputSpeech = request.Session.Attributes.SkillAttributes.OutputSpeech;
            return response;
        }

        private AlexaResponse ProcessUnknownIntent(AlexaRequest request, AlexaResponse response)
        {
            if (string.IsNullOrEmpty(request.Session.Attributes.SkillAttributes.LastRequestIntent))
                return ProcessHelpIntent(request, response);

            return ProcessRepeatIntent(request, response);
        }

        private async Task<AlexaResponse> ProcessYesIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                if (request.Session.Attributes.Strings.Contains(GameState.AskedDoYouWantToStart.ToString()))
                {
                    var newGame = _bombStopperGameManager.CreateGame(request.Session.SessionId, request.Session.User.UserId);
                    var readableSerialNumber = newGame.SerialNumber.ToString().Aggregate("", (current, character) => current + (character + " "));
                    response.SessionAttributes.KeyValuePairs.Add(new KeyValuePair<string, string>("SerialNumber", newGame.SerialNumber.ToString()));
                    response.Response.OutputSpeech.Text = "Great! Begin by going to www.bombstopper.com. Enter serial number " + readableSerialNumber + " to begin.";
                    response.Response.Reprompt.OutputSpeech.Text = "Go to www.bombstopper.com and enter serial number " + readableSerialNumber + " to get started.";
                    response.Response.ShouldEndSession = true;
                    //request.Session.Attributes.Strings.Remove(GameState.AskedDoYouWantToStart.ToString());
                }
                else
                {
                    //This should never happen, but...
                    response.Response.OutputSpeech.Text = "Bye!";
                    response.Response.ShouldEndSession = true;
                }
                
                response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
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
                if (request.Session.Attributes.Strings.Contains(GameState.AskedDoYouWantToStart.ToString()))
                {
                    response.Response.OutputSpeech.Text = "OK, come back when you want to play.";
                    response.Response.ShouldEndSession = true;
                }
                else
                {
                    //This should never happen, but...
                    response.Response.OutputSpeech.Text = "Bye!";
                    response.Response.ShouldEndSession = true;
                }
                
                response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
                response.Response.Reprompt = null;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }
            return response;
        }

        private AlexaResponse ProcessThanksIntent(AlexaRequest request, AlexaResponse response)
        {
            var content = "Have a great day!";
            response.Response.ShouldEndSession = true;
            response.Response.OutputSpeech.Text = content;
            response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
            return response;
        }

        //Example of Amazon.Time Intent
        private async Task<AlexaResponse> ProcessTimeIntent(AlexaRequest request, AlexaResponse response)
        {
            var content = "";
            var reprompt = "";
            var isValid = false;

            try
            {
                if (request.Request.Intent != null && request.Request.Intent.Slots != null)
                {
                    var slot = request.Request.Intent.Slots;

                    if (slot["Time"] != null)
                    {
                        var result = (string)slot["Time"].value;

                        isValid = true;

                        var fixedTime = Convert.ToDateTime(result).ToString("hh:mm");

                        content = "";
                        reprompt = content;
                    }
                }

                if (!isValid)
                {
                    content = "I didn't understand your last response.";
                    reprompt = @"";
                }

                response.Response.ShouldEndSession = false;
                response.Response.OutputSpeech.Text = content;
                response.Response.Reprompt.OutputSpeech.Text = reprompt;
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("String was not recognized as a valid DateTime"))
                    return ProcessRepeatIntent(request, response);

                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }

            return response;
        }

        //Example of Amazon.Date Intent
        private async Task<AlexaResponse> ProcessDateIntent(AlexaRequest request, AlexaResponse response)
        {
            var content = "";
            var isValid = false;

            try
            {
                if (request.Request.Intent != null && request.Request.Intent.Slots != null)
                {
                    var slot = request.Request.Intent.Slots;

                    if (slot["Date"] != null)
                    {
                        var theDate = (string)slot["Date"].value;
                        var formattedDate = Convert.ToDateTime(theDate);

                        //if (formattedDate > DateTime.Today.AddDays(DateMaxInDays))
                        //{
                        //    content = "Sorry that date was over 60 days from now. You can say things like: July 26th, or next Tuesday, or any day before " + DateTime.Today.AddDays(DateMaxInDays).ToShortDateString() + ".";
                        //    response.Response.ShouldEndSession = false;
                        //    response.Response.Reprompt.OutputSpeech.Text = content;
                        //    response.Response.OutputSpeech.Text = content;
                        //    return response;
                        //}

                        content = "";
                        isValid = true;
                    }
                }

                if (!isValid)
                    return ProcessRepeatIntent(request, response);

                response.Response.Reprompt.OutputSpeech.Text = content;
                response.Response.ShouldEndSession = false;
                response.Response.OutputSpeech.Text = content;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("String was not recognized as a valid DateTime"))
                    return ProcessRepeatIntent(request, response);

                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }

            return response;
        }

        private AlexaResponse ProcessHelpIntent(AlexaRequest request, AlexaResponse response)
        {
            if (request.Session.Attributes.Strings.Contains(GameState.AskedDoYouWantToStart.ToString()))
            {
                response.Response.OutputSpeech.Text = "Before I can help, I need to know if you want to start a game. Please answer yes or no.";
                response.Response.Reprompt.OutputSpeech.Text = "Would you like to start a game? Please answer yes or no.";
                response.Response.ShouldEndSession = false;
            }
            else
            {
                response.Response.OutputSpeech.Text = "";
                response.Response.ShouldEndSession = true;
            }

            return response;
        }
    }
}
