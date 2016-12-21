using AlexaSkillsService.Interfaces;
using AlexaSkillsService.Models;
using AlexaSkillsService.Models.DontBlowUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlexaSkillsService.Common
{
    public class DontBlowUpRequestHandler : ISkillRequestHandler
    {
        private readonly IDontBlowUpGameManager _dontBlowUpGameManager;

        public DontBlowUpRequestHandler(IDontBlowUpGameManager dontBlowUpGameManager)
        {
            _dontBlowUpGameManager = dontBlowUpGameManager;
        }

        public AlexaResponse HandleLaunchRequest(AlexaRequest request, AlexaResponse response)
        {
            response.Response.OutputSpeech.Text = "Welcome to Don't Blow Up. Would you like to start a new game?";
            response.SessionAttributes.Strings.Add(Constants.AskedDoYouWantToStart);
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
                if (request.Session.Attributes.Strings.Contains(Constants.AskedDoYouWantToStart))
                {
                    var newGame = _dontBlowUpGameManager.CreateGame(request.Session.SessionId, request.Session.User.UserId);
                    var readableSerialNumber = newGame.SerialNumber.ToString().Aggregate("", (current, character) => current + (character + " "));
                    response.SessionAttributes.KeyValuePairs.Add(new KeyValuePair<string, string>(Constants.SerialNumber, newGame.SerialNumber.ToString()));
                    response.Response.OutputSpeech.Text = "OK. Begin by going to www.its a bomb.com. Enter serial number " + readableSerialNumber + " to begin. " + newGame.Narrative;
                    response.Response.ShouldEndSession = true;
                    request.Session.Attributes.Strings.Remove(Constants.AskedDoYouWantToStart);
                }
                else
                {
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
            var content = "OK, thanks for listening.";

            try
            {
                response.Response.ShouldEndSession = true;
                response.Response.OutputSpeech.Text = content;
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
            response.Response.OutputSpeech.Text = "";
            response.Response.ShouldEndSession = true;
            return response;
        }
    }
}
