using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.WarrantyDetail
{
    public class WarrantyDetailNoteResponse
    {
        public Guid Id { get; set; }
        public string? Description {  get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Image {  get; set; }
    }
}
