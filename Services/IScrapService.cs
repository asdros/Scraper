using System.Collections.Generic;
using Scraper.Models;

namespace Scraper.Services
{
    public interface IScrapService
    {
        void AddItem(List<ScrapItem> items);
        void DropRows();
        List<ScrapItem> GetAllItems();
        List<ScrapItem> GetItemsAboveTheLowerTempRange(int temp);
        List<ScrapItem> GetItemsByDay(byte day);
    }
}
