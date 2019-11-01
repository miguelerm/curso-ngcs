using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"{User.FindFirst("client_id")} Everything Ok";
        }

        [HttpPost]
        public async Task<ActionResult<UploadFileResponse>> PostAsync([FromForm] UploadFileRequest request)
        {
            await Task.CompletedTask;
            return new UploadFileResponse();
        }
    }
}