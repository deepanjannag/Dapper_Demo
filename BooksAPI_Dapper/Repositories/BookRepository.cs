using BooksAPI_Dapper.Models;
using Dapper;
using System.Data.SqlClient;

namespace BooksAPI_Dapper.Repositories
{
    public class BookRepository : IBookRepository
    {

        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BooksConnectionString");
        }

        public async Task<Book> Create(Book book)
        {
            var sqlQuery = "INSERT INTO Books(Title, Author, Description) VALUES (@Title, @Author, @Description)";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sqlQuery, new { book.Title, book.Author, book.Description});
            }
            return book;
        }

        public async Task Delete(int id)
        {
            var sqlQuery = "DELETE FROM Books WHERE Id=@Id";


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sqlQuery, new { Id=id });
            }
        }

        public async Task<IEnumerable<Book>> Get()
        {
            var sqlQuery = "SELECT * FROM Books";
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                //using (var sqlCommand  = new SqlCommand(sqlQuery, connection))
                //{
                //    connection.Open();

                //    using (var reader = await sqlCommand.ExecuteReaderAsync())
                //    {
                //        while (reader.Read())
                //        {
                //            books.Add(new Book { Id = reader.GetInt32(0), Author= reader.GetString(1), Title=reader.GetString(2), Description=reader.GetString(3)});
                //        }
                //    }
                //}
                //return books;

                //Above ADO.NET is being replaced by DAPPER
                return await connection.QueryAsync<Book>(sqlQuery);

            }
        }

        public async Task<Book> Get(int id)
        {
            var sqlQuery = "SELECT * FROM Books WHERE Id=@BookId";
            var book = new Book();

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Book>(sqlQuery, new {BookId = id});
            }
        }

        public async Task Update(Book book)
        {
            var sqlQuery = "UPDATE Books SET Title=@Title, Author=@Author, Description=@Description WHERE Id=@Id";
            

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sqlQuery, new {book.Id, book.Title, book.Author, book.Description});
            }
        }
    
    }
}
