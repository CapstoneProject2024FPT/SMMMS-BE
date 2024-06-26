﻿using SAM.BusinessTier.Enums.EnumStatus;
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
        public static Dictionary<InventoryStautus, int> CountInventoryEachStatus(this ICollection<Inventory> inventories)
        {
            var statusCount = new Dictionary<InventoryStautus, int>();

            foreach(InventoryStautus status in Enum.GetValues(typeof(InventoryStautus)))
            {
                string statusDes = status.GetDescriptionFromEnum();
                int count = inventories.Count(item => item.Status.Equals(statusDes));
                statusCount.Add(status, count);
            }
            return statusCount;
        }
    }
}
