using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reviews.ReadModels;

public record ReviewReadModel(string Name, string Description, decimal Rating);
