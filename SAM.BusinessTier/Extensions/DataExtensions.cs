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
        public static Dictionary<InventoryStatus, int> CountComponentInventoryEachStatus(this ICollection<Inventory> inventories, Guid masterInventoryId)
        {
            var statusCount = new Dictionary<InventoryStatus, int>();

            foreach (InventoryStatus status in Enum.GetValues(typeof(InventoryStatus)))
            {
                string statusDes = status.GetDescriptionFromEnum();
                int count = inventories.Count(item => item.MasterInventoryId == masterInventoryId && item.Status.Equals(statusDes));
                statusCount.Add(status, count);
            }
            return statusCount;
        }
        public static Dictionary<OrderStatus, int> CountOrderEachStatus(this ICollection<Order> orders)
        {
            var statusCount = new Dictionary<OrderStatus, int>();

            foreach (OrderStatus status in Enum.GetValues(typeof(OrderStatus)))
            {
                string statusDes = status.GetDescriptionFromEnum();
                int count = orders.Count(item => item.Status.Equals(statusDes));
                statusCount.Add(status, count);
            }
            return statusCount;
        }
        public static Dictionary<TaskManagerStatus, int> CountTaskEachStatus(this ICollection<TaskManager> task)
        {
            var statusCount = new Dictionary<TaskManagerStatus, int>();

            foreach (TaskManagerStatus status in Enum.GetValues(typeof(TaskManagerStatus)))
            {
                string statusDes = status.GetDescriptionFromEnum();
                int count = task.Count(item => item.Status.Equals(statusDes));
                statusCount.Add(status, count);
            }
            return statusCount;
        }
        public static Dictionary<NoteStatus, int> CountNoteEachStatus(this ICollection<Note> note)
        {
            var statusCount = new Dictionary<NoteStatus, int>();

            foreach (NoteStatus status in Enum.GetValues(typeof(NoteStatus)))
            {
                string statusDes = status.GetDescriptionFromEnum();
                int count = note.Count(item => item.Status.Equals(statusDes));
                statusCount.Add(status, count);
            }
            return statusCount;
        }
    }
}
