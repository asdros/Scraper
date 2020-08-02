namespace Scraper.Services.Queries
{
    public class IdentityCommands
    {
        public string CreateUser => "INSERT INTO Users (UserName, PasswordHash, PasswordSalt) VALUES (@username, @passwordHash, @passwordSalt)";
        public string DeleteUser => "DELETE FROM Users WHERE Id=@id";
        public string GetById => "SELECT * FROM Users WHERE Id = @id";
        public string GetUsernameColumn => "SELECT UserName FROM Users";
        public string GetAll => "SELECT * FROM Users WHERE UserName=@Username";
    }
}
