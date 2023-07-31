using NUnit.Framework;
using System;
using System.IO;
using System.Data.SqlClient;
using ProductManagement;


namespace ProductManagement.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        private string connectionString;
        private string databaseName;
        private string tableName;
        private int lastInsertedProductId; // Store the ID of the last inserted product

        [OneTimeSetUp]
        public void SetUp()
        {
            // Set up the connection string and other variables
            connectionString = ConnectionStringProvider.ConnectionString;
            databaseName = "ProductDB";
            tableName = "Products";

            // Create the database
            // CreateDatabase();

            // Create the Products table
            // CreateTable();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Delete the product added during the test
            DeleteProduct1(lastInsertedProductId);
                    // DeleteTestData();


            // Drop the Products table
            // DropTable();

            // Drop the database
            // DropDatabase();
        }

        [Test]
        public void ConnectToDatabase_ConnectionSuccessful()
        {
            // Arrange
            bool connectionSuccess = false;
            string errorMessage = "";

            // Act
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connectionSuccess = true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(connectionSuccess, "Failed to connect to the database. Error message: " + errorMessage);
        }

        [Test]
public void AddProduct_ShouldAddProductToDatabase()
{
    // Arrange
    string connectionString = ConnectionStringProvider.ConnectionString;
    string databaseName = "ProductDB";
    string productName = "Test Product122";
    decimal productRate = 9.99m;
    int productStock = 50;
    // int lastInsertedProductId = 0;

    // Act
    bool productAdded = false;
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        connection.ChangeDatabase(databaseName);
        using (ConsoleInput input = new ConsoleInput($"{productName}{Environment.NewLine}{productRate}{Environment.NewLine}{productStock}{Environment.NewLine}"))
        {
            using (ConsoleOutput output = new ConsoleOutput())
            {
                Program.AddProduct(connection);
                productAdded = output.GetOutput().Contains("Product added successfully!");
            }
        }

        // Retrieve the last inserted product ID
        // string query = "SELECT MAX(ID) FROM Products";
        // using (SqlCommand command = new SqlCommand(query, connection))
        // {
        //                 object result = command.ExecuteScalar();

            // Console.WriteLine("outside");
            try
            {
                // using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // connection.Open();
                    // connection.ChangeDatabase(databaseName);

                    SqlCommand command = connection.CreateCommand();
                    // Console.WriteLine("inside");
                    command.CommandText = $"DELETE FROM {tableName} WHERE Name = @ID";
                    command.Parameters.AddWithValue("@ID", productName);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                // Handle any exception if necessary
            }
        // }
    }

    // Assert
    Assert.IsTrue(productAdded);
    // Assert.AreNotEqual(0, lastInsertedProductId);
}



        [Test]
        public void Main_FirstLineShouldBeConnectionSuccessful()
        {
            // Arrange
            string expectedFirstLine = "Connection successful!";
            string output = string.Empty;
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                Program.Main(null);
                output = sw.ToString().Trim();
            }

            // Assert
            string[] lines = output.Split(Environment.NewLine);
            Assert.That(lines.Length > 0);
            Assert.AreEqual(expectedFirstLine, lines[0]);
        }

        [Test]
        public void Main_SecondLineShouldBeSelectanoption()
        {
            // Arrange
            string expectedFirstLine = "Select an option:";
            string output = string.Empty;
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                Program.Main(null);
                output = sw.ToString().Trim();
            }

            // Assert
            string[] lines = output.Split(Environment.NewLine);
            Assert.That(lines.Length > 0);
            Assert.AreEqual(expectedFirstLine, lines[1]);
        }

        [Test]
        public void Main_ThirdLineShouldBe1AddProduct()
        {
            // Arrange
            string expectedFirstLine = "1. Add product";
            string output = string.Empty;
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                Program.Main(null);
                output = sw.ToString().Trim();
            }

            // Assert
            string[] lines = output.Split(Environment.NewLine);
            Assert.That(lines.Length > 0);
            Assert.AreEqual(expectedFirstLine, lines[2]);
        }


[Test]
public void DisplayAllProducts_ShouldFetchAtLeastOneProduct()
{
    // Arrange
    string connectionString = ConnectionStringProvider.ConnectionString;
    string databaseName = "ProductDB";

    // Act
    string output = string.Empty;
    using (StringWriter sw = new StringWriter())
    {
        Console.SetOut(sw);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.ChangeDatabase(databaseName);
            Program.DisplayAllProducts(connection);
        }
        output = sw.ToString().Trim();
    }

    // Assert
    Assert.IsFalse(string.IsNullOrEmpty(output));
}

[Test]
public void DeleteProduct_ShouldDeleteProduct()
{
    // Arrange
    string connectionString = ConnectionStringProvider.ConnectionString;
    string databaseName = "ProductDB";

    // Insert a test product
    int productIdToDelete = InsertTestProduct(connectionString, databaseName);

    // Act
    string output = string.Empty;
    using (StringWriter sw = new StringWriter())
    {
        Console.SetOut(sw);
        using (StringReader sr = new StringReader(productIdToDelete.ToString()))
        {
            Console.SetIn(sr);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase(databaseName);
                Program.DeleteProduct(connection);
            }

            output = sw.ToString().Trim();
        }
    }

    // Assert
    Assert.IsTrue(output.Contains("Product deleted successfully!"));
}

[Test]
public void SearchProduct_ShouldDisplayMatchingProducts()
{
    // Arrange
    string connectionString = ConnectionStringProvider.ConnectionString;
    string databaseName = "ProductDB";

    // Insert a test product
    lastInsertedProductId = InsertTestProduct(connectionString, databaseName, "Test Product2", 9.99, 100);

    // Act
    string output = string.Empty;
    using (StringWriter sw = new StringWriter())
    {
        Console.SetOut(sw);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.ChangeDatabase(databaseName);
            using (ConsoleInput input = new ConsoleInput("Test"))
            {
                Program.SearchProduct(connection);
            }
        }
        output = sw.ToString().Trim();
    }

    // Assert
    Assert.IsTrue(output.Contains("ProductID\tName\tRate\tStock"));
    Assert.IsTrue(output.Contains(lastInsertedProductId.ToString()));
}

[Test]
public void EditProduct_ShouldUpdateProductInDatabase()
{
    // Arrange
    string connectionString = ConnectionStringProvider.ConnectionString;
    string databaseName = "ProductDB";

    // Insert a test product
    lastInsertedProductId = InsertTestProduct(connectionString, databaseName, "Test Product", 9.99, 100);

    // Act
    string updatedName = "Updated Product1";
    decimal updatedRate = 19.99m;
    int updatedStock = 50;

    string output = string.Empty;
    using (StringWriter sw = new StringWriter())
    {
        Console.SetOut(sw);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.ChangeDatabase(databaseName);
            using (ConsoleInput input = new ConsoleInput(lastInsertedProductId.ToString() + Environment.NewLine + updatedName + Environment.NewLine + updatedRate + Environment.NewLine + updatedStock))
            {
                Program.EditProduct(connection);
            }
        }
        output = sw.ToString().Trim();
    }

    // Assert
    Assert.IsTrue(output.Contains("Product updated successfully!"));

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        connection.ChangeDatabase(databaseName);

        string query = "SELECT Name, Rate, Stock FROM Products WHERE ID = @ProductID";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@ProductID", lastInsertedProductId);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(updatedName, reader.GetString(0));
                Assert.AreEqual(updatedRate, reader.GetDecimal(1));
                Assert.AreEqual(updatedStock, reader.GetInt32(2));
            }
        }
    }

    try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase(databaseName);

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"DELETE FROM {tableName} WHERE ID = @ID";
                    command.Parameters.AddWithValue("@ID", lastInsertedProductId);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                // Handle any exception if necessary
            }
}

[Test]
public void Main_InvalidOption_ShouldDisplayErrorMessage()
{
    // Arrange
    string connectionString = ConnectionStringProvider.ConnectionString;
    string databaseName = "ProductDB";
    string invalidOption = "6"; // Invalid option that does not exist

    string output = string.Empty;
    using (StringWriter sw = new StringWriter())
    {
        Console.SetOut(sw);
        using (ConsoleInput input = new ConsoleInput(invalidOption))
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase(databaseName);
                Program.Main(null);
            }
        }
        output = sw.ToString().Trim();
    }

    // Assert
    Assert.IsTrue(output.Contains("Invalid option selected!"));
}



private static int InsertTestProduct(string connectionString, string databaseName, string name, double rate, int stock)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        connection.ChangeDatabase(databaseName);

        string insertQuery = "INSERT INTO Products (Name, Rate, Stock) VALUES (@Name, @Rate, @Stock); SELECT SCOPE_IDENTITY();";
        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        {
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Rate", rate);
            command.Parameters.AddWithValue("@Stock", stock);

            // Execute the query and get the last inserted product ID
            int productId = Convert.ToInt32(command.ExecuteScalar());
            // lastInsertedProductId = productId;

            return productId;
        }
    }
}





private static int InsertTestProduct(string connectionString, string databaseName)
{
    
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        connection.ChangeDatabase(databaseName);

        string insertQuery = "INSERT INTO Products (Name, Rate, Stock) VALUES (@Name, @Rate, @Stock); SELECT SCOPE_IDENTITY();";
        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        {
            command.Parameters.AddWithValue("@Name", "Test Product");
            command.Parameters.AddWithValue("@Rate", 9.99);
            command.Parameters.AddWithValue("@Stock", 100);

            // Execute the query and get the last inserted product ID
            int productId = Convert.ToInt32(command.ExecuteScalar());
            
            return productId;
        }
    }
}



      

        private bool AddProductToDatabase(string productName, double productRate, int productStock)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase(databaseName);

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO {tableName} (Name, Rate, Stock) VALUES (@Name, @Rate, @Stock)";
                    command.Parameters.AddWithValue("@Name", productName);
                    command.Parameters.AddWithValue("@Rate", productRate);
                    command.Parameters.AddWithValue("@Stock", productStock);

                    int rowsAffected = command.ExecuteNonQuery();

                    // Store the ID of the last inserted product
                    if (rowsAffected > 0)
                    {
                        command.CommandText = $"SELECT ID from Products where Rate = 9.99 ";
                        lastInsertedProductId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void DeleteProduct1(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase(databaseName);

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"DELETE FROM {tableName} WHERE ID = @ID";
                    command.Parameters.AddWithValue("@ID", productId);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                // Handle any exception if necessary
            }
        }

         private void DeleteTestData()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase(databaseName);

                SqlCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM {tableName} WHERE ID >= @StartID";
                command.Parameters.AddWithValue("@StartID", lastInsertedProductId);

                command.ExecuteNonQuery();
            }
        }
        catch (Exception)
        {
            // Handle any exception if necessary
        }
    }
    }

    public class ConsoleInput : IDisposable
    {
        private readonly StringReader stringReader;
        private readonly TextReader originalInput;

        public ConsoleInput(string input)
        {
            stringReader = new StringReader(input);
            originalInput = Console.In;
            Console.SetIn(stringReader);
        }

        public void Dispose()
        {
            Console.SetIn(originalInput);
            stringReader.Dispose();
        }
    }

    public class ConsoleOutput : IDisposable
    {
        private readonly StringWriter stringWriter;
        private readonly TextWriter originalOutput;

        public ConsoleOutput()
        {
            stringWriter = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        public string GetOutput()
        {
            return stringWriter.ToString().Trim();
        }

        public void Dispose()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }
    }
}
