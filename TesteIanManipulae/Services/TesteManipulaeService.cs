using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteIanManipulae.Models;

namespace TesteIanManipulae.Services
{
    public class TesteManipulaeService
    {
        private readonly ApiDbContext _context;

        public TesteManipulaeService(ApiDbContext context)
        {
            _context = context;
        }

        public List<Video> Filtrar(string titulo = null, string autor = null, string q = null)
        {
            var videos = _context.Videos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(titulo))
                videos = _context.Videos.Where(x => x.Titulo.ToLower().Contains(titulo.ToLower()));

            if (!string.IsNullOrWhiteSpace(autor))
                videos = _context.Videos.Where(x => x.Autor.ToLower().Contains(autor.ToLower()));

            if (!string.IsNullOrWhiteSpace(q))
                videos = _context.Videos
                    .Where(x => x.Titulo.ToLower().Contains(q.ToLower()) || x.Descricao.ToLower().Contains(q.ToLower()) || x.Canal.ToLower().Contains(q.ToLower()));
                   
            return videos.ToList();
        }
         public void AtualizarVideo(Video video )
        {
            var videos = _context.Videos.FindAsync(video.Id).Result;
            videos.Autor = video.Autor;
            videos.Canal = video.Canal;
            videos.DataCriacao = video.DataCriacao;
            videos.Descricao = video.Descricao;
            videos.Titulo = video.Titulo;
            videos.YoutubeVideoId = video.YoutubeVideoId;

            _context.SaveChanges();
            
        }

        public void DeletarVideo(int id)
        {
            var video = _context.Videos.FindAsync(id).Result;
            _context.Remove(video);
            _context.SaveChanges();
        }

        public Video CriarVideo(Video video)
        {
            _context.Videos.Add(video);
            _context.SaveChangesAsync();

            return video;
        }
    }
}
