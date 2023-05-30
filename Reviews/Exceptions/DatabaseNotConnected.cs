using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shared.Exceptions;

namespace Reviews.Exceptions
{
    public class DatabaseNotConnected : PokemonAppExceptions
    {
        public DatabaseNotConnected() : base("Connection to MongoDb failed!")
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
