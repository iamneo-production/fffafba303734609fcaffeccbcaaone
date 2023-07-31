using System;
using System.Data;
using System.Data.SqlClient;


namespace ProductManagement
{
    public static class ConnectionStringProvider
    {
        public static string ConnectionString { get; } = "Replace with Your Connection String";
    }


    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = ConnectionStringProvider.ConnectionString;
            // Write your main method here...
        }

        public static void AddProduct(SqlConnection connection)
        {
            // Session 1
            // Write your logic to add product
        }

        public static void DisplayAllProducts(SqlConnection connection)
        {
            // Session 2
            // Write Your Logic to Display all products
        }

        public static void DeleteProduct(SqlConnection connection)
        {
            // Session 2
            // Write your Logic to Delete Product by using ID
        }

        public static void SearchProduct(SqlConnection connection)
        {
            // Session 3
            // Write your logic to Search Product by Name
        }
        public static void EditProduct(SqlConnection connection)
        {
            // Session 3
            // Write your logic to Edit Product by ID
        }
    }
}
