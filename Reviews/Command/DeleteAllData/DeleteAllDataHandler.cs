using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using Reviews.Models;

namespace Reviews.Command.DeleteAllData
{
    public class DeleteAllDataHandler : IRequestHandler<DeleteAllDataCommand,Unit>
    {
        private readonly IMongoCollection<Review> _collection;

        public DeleteAllDataHandler(IMongoCollection<Review> collection)
        {
            _collection = collection;
        }
        public async Task<Unit> Handle(DeleteAllDataCommand request, CancellationToken cancellationToken)
        {
            await _collection.DeleteManyAsync(_ => true);
            return Unit.Value;
            
        }
    }
}
