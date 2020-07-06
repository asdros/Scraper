using System.Collections.Generic;
using Scraper.Models;

namespace Scraper.Services
{
    public interface IScrapService
    {
        void AddItem(ScrapItem item);
        void DropRows();
        List<ScrapItem> GetItems();
        List<ScrapItem> GetItemsByHigherTemp(int temp);
        List<ScrapItem> GetItemsByDay(byte day);
    }
}
