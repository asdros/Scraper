using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scraper.Services.Queries
{
    interface ICommandText
    {
        string AddItem { get; }
        string GetItems { get; }
        string GetItemsByCity { get; }
        string GetItemsByDay { get; }
        string GetItemsByHigherTemp { get; }
        string GetItemsByLowerTemp { get; }
        
    }
}
