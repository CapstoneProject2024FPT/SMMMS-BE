using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class GetSpecifyInMachineryResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }

        public GetSpecifyInMachineryResponse()
        {
        }
    }
}
