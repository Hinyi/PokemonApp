using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Reviews.DataAccess;

    internal class MongoDbSettings : AbstractValidator<MongoDbSettings>
{
        public static string SectionName => "Reviews:MongoDb";
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ReviewsCollectionName { get; set; }

        public MongoDbSettings()
        {
            RuleFor(x => ConnectionString).NotEmpty();
        }
    }
