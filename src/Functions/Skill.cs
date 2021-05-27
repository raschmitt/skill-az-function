using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace AzFunctionSkill.Functions
{
    public static class Skill
    {
        private static SkillResponse _skillResponse;
        private static SkillRequest _skillRequest;
        
        [FunctionName(nameof(Skill))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
        {
            DeserializeRequest(request);
            ProcessResponse();
            
            return new OkObjectResult(_skillResponse);
        }

        private static void DeserializeRequest(HttpRequest request)
        {
            var json = request.ReadAsStringAsync().GetAwaiter().GetResult();
            _skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);
        }
        
        private static void ProcessResponse()
        {
            var request = _skillRequest.Request;

            switch (request)
            {
                case LaunchRequest _:
                    ProcessLaunchRequest();
                    break;
                case IntentRequest r:
                    ProcessIntentRequest(r);
                    break;
                case SessionEndedRequest _:
                    ProcessSessionEndedRequest();
                    break;
            }
        }

        private static void ProcessLaunchRequest()
        {
            _skillResponse = ResponseBuilder.Tell("Welcome to Dot Net!");
            _skillResponse.Response.ShouldEndSession = false;
        }
        
        private static void ProcessIntentRequest(IntentRequest request)
        {
            if (request.Intent.Name == "Hello")
            {
                _skillResponse = ResponseBuilder.Tell("Dot Net Skill says hi!");
                _skillResponse.Response.ShouldEndSession = false;
            }
        }
        
        private static void ProcessSessionEndedRequest()
        {
            _skillResponse.Response.ShouldEndSession = true;
        }
    }
}