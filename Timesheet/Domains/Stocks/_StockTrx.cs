using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.Models;

namespace Timesheet.Domains.Stocks
{
    public class StockTrx : IAuditable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Emiten { get; set; }
        public string State { get; set; }
        public int Lot { get; set; }
        public int BuyPrice { get; set; }
        public int? SellPrice { get; set; }
        public DateTime? SoldAt { get; set; }
        public string LessonLearned { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }

        public List<StockTrxAddition> StockTrxAdditions { get; set; }


        public void UpdateInfo(StockTrx newInfo) 
        {
            Name = newInfo.Name;
            Emiten = newInfo.Emiten;
            Lot = newInfo.Lot;
            BuyPrice = newInfo.BuyPrice;
        }
    }

    public class StockTrxAddition : IAuditable
    {
        public long Id { get; set; }
        public long StockTrxId { get; set; }
        public StockTrx StockTrx { get; set; }
        public int Lot { get; set; }
        public int BuyPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
