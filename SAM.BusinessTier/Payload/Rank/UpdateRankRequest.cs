using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Rank
{
    public class UpdateRankRequest
    {
        public string? Name { get; set; }

        public int? Range { get; set; }

        public int? Value { get; set; }
    }
}
