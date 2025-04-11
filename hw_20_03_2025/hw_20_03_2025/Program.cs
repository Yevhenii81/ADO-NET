//using System;
//using System.Data.SqlClient;

//class Program
//{
//    static void Main()
//    {
//        string connectionString = @"Server=DESKTOP-AQF96SU\SQLEXPRESS;Database=Sklad;Trusted_Connection=True;";

//        using (SqlConnection connection = new SqlConnection(connectionString))
//        {
//            try
//            {
//                connection.Open();
//                Console.WriteLine(" Успішно підключено до бази даних 'Склад'");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(" Помилка підключення: " + ex.Message);
//            }
//        }
//    }
//}

using System;
using System.Data.SqlClient;

class Program
{
    static string connectionString = @"Server=DESKTOP-AQF96SU\SQLEXPRESS;Database=Sklad;Trusted_Connection=True;";

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== МЕНЮ =====");
            Console.WriteLine("1. Підключитися до бази даних");
            Console.WriteLine("2. Відобразити всі товари");
            Console.WriteLine("3. Відобразити всі типи товарів");
            Console.WriteLine("4. Відобразити всіх постачальників");
            Console.WriteLine("5. Показати товари заданої категорії");
            Console.WriteLine("0. Вийти");
            Console.Write("\nВаш вибір: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ConnectToDatabase();
                    break;
                case "2":
                    ShowAllProducts();
                    break;
                case "3":
                    ShowAllProductTypes();
                    break;
                case "4":
                    ShowAllSuppliers();
                    break;
                case "5":
                    ShowProductsByCategory();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невідомий вибір.");
                    break;
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }
    }

    static void ConnectToDatabase()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine(" Успішно підключено до бази даних 'Склад'");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Помилка підключення: " + ex.Message);
            }
        }
    }

    static void ShowAllProducts()
    {
        string query = @"
            SELECT P.ProductName, T.ProductTypeName, S.SupplierName, P.Quantity, P.Cost, P.SupplyDate
            FROM Products P
            JOIN ProductTypes T ON P.ProductTypeID = T.ProductTypeID
            JOIN Suppliers S ON P.SupplierID = S.SupplierID";

        ExecuteQuery(query, "Усі товари");
    }

    static void ShowAllProductTypes()
    {
        string query = "SELECT * FROM ProductTypes";
        ExecuteQuery(query, "Типи товарів");
    }

    static void ShowAllSuppliers()
    {
        string query = "SELECT * FROM Suppliers";
        ExecuteQuery(query, "Постачальники");
    }

    static void ShowProductsByCategory()
    {
        Console.Write("Введіть назву категорії (типу товару): ");
        string category = Console.ReadLine();

        string query = @"
            SELECT P.ProductName, T.ProductTypeName, S.SupplierName, P.Quantity, P.Cost, P.SupplyDate
            FROM Products P
            JOIN ProductTypes T ON P.ProductTypeID = T.ProductTypeID
            JOIN Suppliers S ON P.SupplierID = S.SupplierID
            WHERE T.ProductTypeName = @Category";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Category", category);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine($"\nТовари категорії: {category}");
                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["ProductName"]}, {reader["SupplierName"]}, к-сть: {reader["Quantity"]}, ціна: {reader["Cost"]}, дата: {((DateTime)reader["SupplyDate"]).ToShortDateString()}");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Помилка: " + ex.Message);
            }
        }
    }

    static void ExecuteQuery(string query, string title)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine($"\n{title}:");

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.Write($"{reader.GetName(i)}: {reader[i]}  ");

                    Console.WriteLine();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Помилка: " + ex.Message);
            }
        }
    }
}
