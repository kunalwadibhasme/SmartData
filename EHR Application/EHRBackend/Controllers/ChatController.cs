using E_CommerceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetPatientbyProviderId(int providerId)
        {
            var result = await _chatService.GetPatientbyProviderId(providerId);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetProviderByPatientId(int patientId)
        {
            var result = await _chatService.GetProviderByPatientId(patientId);
            return Ok(result);
        }
    }
}
