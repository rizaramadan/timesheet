using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheet.Domains.Stocks
{
    public interface IStockService
    {
        void AddStock(StockTrx trx, int lot, int price);
    }

    public class StockService : IStockService
    {
        public void AddStock(StockTrx trx, int lot, int price)
        {
            bool addSelf = false;
            if (trx.StockTrxAdditions == null)
            {
                addSelf = true;
                trx.StockTrxAdditions = new List<StockTrxAddition>(2);
            }
            else if (trx.StockTrxAdditions.Count == 0) 
            {
                addSelf = true;
            }

            if (addSelf)
            {
                trx.StockTrxAdditions.Add(new StockTrxAddition
                {
                    StockTrxId = trx.Id,
                    Lot = trx.Lot,
                    BuyPrice = trx.BuyPrice
                });
            }
            trx.StockTrxAdditions.Add(new StockTrxAddition
            {
                StockTrxId = trx.Id,
                Lot = lot,
                BuyPrice = price
            });

            trx.Lot += lot;
            trx.BuyPrice = trx.StockTrxAdditions.Sum(x => x.BuyPrice * x.Lot) / trx.StockTrxAdditions.Sum(x => x.Lot);
        }
    }
}
