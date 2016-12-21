using AlexaSkillsService.Models;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AlexaSkillsService.Common
{
    public static class SafeExceptionHandler
    {
        //use this within each method of controller to trap errors
        public static async Task<AlexaResponse> ThrowSafeException(Exception exception, AlexaResponse response, [CallerMemberName] string methodName = "")
        {
            var content = @"Oops. We encountered some trouble. Sorry about that. We'll look into this. Please try again later.";

            //TODO: Log exception

            response.Response.OutputSpeech.Text = content;
            response.Response.ShouldEndSession = true;

            return response;
        }
    }
}