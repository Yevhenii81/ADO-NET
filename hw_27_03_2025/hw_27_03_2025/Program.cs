using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = @"Server=DESKTOP-AQF96SU\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;";
        string query = "SELECT * FROM Coffee";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Подключено к базе данных.\n");

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("Данные из таблицы Coffee:\n");

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]}");
                    Console.WriteLine($"Название: {reader["CoffeeName"]}");
                    Console.WriteLine($"Страна: {reader["OriginCountry"]}");
                    Console.WriteLine($"Тип: {reader["CoffeeType"]}");
                    Console.WriteLine($"Описание: {reader["Description"]}");
                    Console.WriteLine($"Количество: {reader["QuantityGrams"]} г");
                    Console.WriteLine($"Себестоимость: {reader["CostPrice"]} грн");
                    Console.WriteLine("-----------------------------");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при подключении или выполнении запроса: " + ex.Message);
            }
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
