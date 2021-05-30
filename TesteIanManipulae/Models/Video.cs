using System;
using System.ComponentModel.DataAnnotations;

namespace TesteIanManipulae.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string YoutubeVideoId { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Canal { get; set; }
        public string Descricao { get; set; }
    }
}
