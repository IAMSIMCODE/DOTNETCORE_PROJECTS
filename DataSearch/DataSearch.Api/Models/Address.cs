﻿using Redis.OM.Modeling;

namespace DataSearch.Api.Models
{
    public class Address
    {
        [Indexed]
        public int StreetNumber { get; set; }

        [Indexed]
        public string Unit { get; set; }

        [Searchable]
        public string StreetName { get; set; }

        [Indexed]
        public string City { get; set; }

        [Indexed]
        public string State { get; set; }

        [Indexed]
        public string PostalCode { get; set; }

        [Indexed]
        public string Country { get; set; }

        [Indexed]
        public GeoLoc Location { get; set; }
    }

    [Document(StorageType = StorageType.Json, Prefixes = new[] { "Person" })]
    public class Person
    {
        [RedisIdField][Indexed] public string Id { get; set; }

        [Indexed] public string FirstName { get; set; }

        [Indexed] public string LastName { get; set; }

        [Indexed] public int Age { get; set; }

        [Searchable] public string PersonalStatement { get; set; }

        [Indexed] public string[] Skills { get; set; } = Array.Empty<string>();

        [Indexed(CascadeDepth = 1)] public Address Address { get; set; }
    }
}
