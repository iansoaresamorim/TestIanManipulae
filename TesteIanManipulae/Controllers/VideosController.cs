using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteIanManipulae.Models;
using TesteIanManipulae.Services;

namespace TesteIanManipulae.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly YoutubeService _youtubeService;
        private readonly TesteManipulaeService _testeManipulaeService;

        public VideosController(ApiDbContext context, YoutubeService youtubeService, TesteManipulaeService testeManipulaeService)
        {
            _context = context;
            _youtubeService = youtubeService;
            _testeManipulaeService = testeManipulaeService;
        }

        // GET: api/Videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
        {
            _youtubeService.ImportYoutubeVideosToDatabase();

            return await _context.Videos.ToListAsync();
        }

        // GET: api/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }

        // PUT: api/Videos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutVideo(int id, Video video)
        {
            if (id != video.Id)
            {
                return BadRequest();
            }

            _testeManipulaeService.AtualizarVideo(video);

            return NoContent();
        }

        // POST: api/Videos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Video> PostVideo(Video video)
        {
            var criarvideo = _testeManipulaeService.CriarVideo(video);
            
            return CreatedAtAction("GetVideo", new { id = criarvideo.Id }, criarvideo);
        }

        // DELETE: api/Videos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _testeManipulaeService.DeletarVideo(id);

            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult<List<Video>> Search(string titulo = null, string autor = null, string q = null)
        {
           var search = _testeManipulaeService.Filtrar(titulo, autor, q);

           return search;

        }
    }


}
