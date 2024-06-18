using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace produtos.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set;}
        public decimal? Preco { get; set;}
        [ForeignKey("CategoriaId")]
        public int? CategoriaId { get; set;}
        [JsonIgnore]
        public Categoria? Categoria { get; set;}
    }
}