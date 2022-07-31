using WebApi.Models;

namespace WebApi.Data
{
    public class DummyData
    {
        public List<User> Users = new()
        {
            new User { Id = Guid.Parse(Guid.NewGuid().ToString()), Forename = "Tony", Surname = "Hawk", Age = 20 },
            new User { Id = Guid.Parse(Guid.NewGuid().ToString()), Forename = "Charlie", Surname = "Day", Age = 30 },
            new User { Id = Guid.Parse(Guid.NewGuid().ToString()), Forename = "Walter", Surname = "White", Age = 10 },
            new User { Id = Guid.Parse(Guid.NewGuid().ToString()), Forename = "Tony", Surname = "Hawk", Age = 20 }
        };
    }
}
