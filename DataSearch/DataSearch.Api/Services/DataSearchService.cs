using DataSearch.Api.Models;
using Redis.OM;
using Redis.OM.Searching;

namespace DataSearch.Api.Services
{
    public interface IDataSearchService
    {
        Task<Person> AddPerson(Person person);
        IList<Person> FilterByAge(int minAge, int maxAge);
        IList<Person> FilterByGeo(double lon, double lat, double radius, string unit);
        IList<Person> FilterByName(string firstName, string lastName);
        IList<Person> FilterByPersonalStatement(string text);
        IList<Person> FilterByStreetName(string streetName);
        IList<Person> FilterBySkill(string skill);
        void UpdateAge(string id, int newAge);
        void DeletePerson(string id);
    }
    public class DataSearchService : IDataSearchService
    {
        private readonly RedisCollection<Person> _persons;
        private readonly RedisConnectionProvider _connectionProvider;

        public DataSearchService(RedisConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _persons = (RedisCollection <Person>)connectionProvider.RedisCollection<Person>();
        }

        public async Task<Person> AddPerson(Person person)
        {
            await _persons.InsertAsync(person);
            return person;
        }

        public void DeletePerson(string id)
        {
            _connectionProvider.Connection.Unlink($"Person:{id}");
        }

        public IList<Person> FilterByAge(int minAge, int maxAge)
        {
            var persons = _persons.Where(p => p.Age >= minAge && p.Age <= maxAge).ToList();
            return persons;
        }

        public IList<Person> FilterByGeo(double lon, double lat, double radius, string unit)
        {
            return _persons.GeoFilter(x => x.Address!.Location, lon, lat, radius, Enum.Parse<GeoLocDistanceUnit>(unit)).ToList();
        }

        public IList<Person> FilterByName(string firstName, string lastName)
        {
            return _persons.Where(x => x.FirstName == firstName && x.LastName == lastName).ToList();
        }

        public IList<Person> FilterByPersonalStatement(string text)
        {
            return _persons.Where(x => x.PersonalStatement == text).ToList();
        }

        public IList<Person> FilterBySkill(string skill)
        {
            return _persons.Where(x => x.Skills.Contains(skill)).ToList();
        }

        public IList<Person> FilterByStreetName(string streetName)
        {
            return _persons.Where(x => x.Address!.StreetName == streetName).ToList();
        }

        public void UpdateAge(string id, int newAge)
        {
            foreach (var person in _persons.Where(x => x.Id == id))
            {
                person.Age = newAge;
            }
            _persons.Save();
        }
    }
}
