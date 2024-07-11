using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Extensions
{
    public static class DataExtensions
    {
        public static Dictionary<InventoryStatus, int> CountInventoryEachStatus(this ICollection<Inventory> inventories)
        {
            var statusCount = new Dictionary<InventoryStatus, int>();

            foreach(InventoryStatus status in Enum.GetValues(typeof(InventoryStatus)))
            {
                string statusDes = status.GetDescriptionFromEnum();
                int count = inventories.Count(item => item.Status.Equals(statusDes));
                statusCount.Add(status, count);
            }
            return statusCount;
        }
        public static Dictionary<WarrantyDetailStatus, int> CountWarrantyDetailEachStatus(this ICollection<WarrantyDetail> warrantyDetails)
        {
            var warrantyDetailsCount = new Dictionary<WarrantyDetailStatus, int>();

            foreach (WarrantyDetailStatus status in Enum.GetValues(typeof(InventoryStatus)))
            {
                string statusDes = status.GetDescriptionFromEnum();
                int count = warrantyDetails.Count(item => item.Status.Equals(statusDes));
                warrantyDetailsCount.Add(status, count);
            }
            return warrantyDetailsCount;
        }
    }
}
