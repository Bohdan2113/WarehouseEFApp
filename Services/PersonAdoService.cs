using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Npgsql;
using Microsoft.Extensions.Configuration;
using WarehouseEFApp.Models;

namespace WarehouseEFApp.Services
{
    public class PersonAdoService
    {
        private readonly string? _connectionString;

        public PersonAdoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Person> GetAll()
        {
            var people = new List<Person>();
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString ?? throw new InvalidOperationException("Connection string is null")))
            {
                connection.Open();
                using (var command = new Npgsql.NpgsqlCommand("SELECT * FROM person", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        people.Add(new Person
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Position = reader.IsDBNull(reader.GetOrdinal("position")) ? null : reader.GetString(reader.GetOrdinal("position")),
                        });
                    }
                }
            }
            return people;
        }

        public void Add(Person person)
        {
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString ?? throw new InvalidOperationException("Connection string is null")))
            {
                connection.Open();
                using (var command = new Npgsql.NpgsqlCommand(
                    "INSERT INTO person (first_name, last_name, position) VALUES (@first_name, @last_name, @position)", connection))
                {
                    command.Parameters.AddWithValue("@first_name", person.FirstName);
                    command.Parameters.AddWithValue("@last_name", person.LastName);
                    command.Parameters.AddWithValue("@position", (object?)person.Position ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Person person)
        {
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString ?? throw new InvalidOperationException("Connection string is null")))
            {
                connection.Open();
                using (var command = new Npgsql.NpgsqlCommand(
                    "UPDATE person SET first_name = @first_name, last_name = @last_name, position = @position WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", person.Id);
                    command.Parameters.AddWithValue("@first_name", person.FirstName);
                    command.Parameters.AddWithValue("@last_name", person.LastName);
                    command.Parameters.AddWithValue("@position", (object?)person.Position ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString ?? throw new InvalidOperationException("Connection string is null")))
            {
                connection.Open();
                using (var command = new Npgsql.NpgsqlCommand("DELETE FROM person WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
