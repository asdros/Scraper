namespace Scraper.Services.Queries
{
     public interface ICommandText
    {
        string AddItem { get; }
        string GetItems { get; }
        string GetItemsByDay { get; }
        string GetItemsByHigherTemp { get; }
        string DropRows { get; }
    }
}
