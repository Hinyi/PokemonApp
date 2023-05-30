using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Reviews.Models;

namespace Reviews.Query.GetAllReviews
{
    public class GetAllReviewsQuery : IRequest<List<Review>>
    {
    }
}
