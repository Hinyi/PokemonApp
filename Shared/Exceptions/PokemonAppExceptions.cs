using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public abstract class PokemonAppExceptions : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }

        protected PokemonAppExceptions(string message) : base(message)
        {
            
        }
    }
}
