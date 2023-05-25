using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reviews.Models
{
    internal class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Rating { get; set; }
        public int PokemonId { get; set; }
    }
}
