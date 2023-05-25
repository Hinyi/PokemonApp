using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common;

namespace Reviews;

    public class ReviewsModule : IModule
    {
        public string Name => Source.Name;
        public static ActivitySource Source { get; } = new("Reviews", "1.0.0.0");
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public void Use(IApplicationBuilder app)
        {
            throw new NotImplementedException();
        }
    }

