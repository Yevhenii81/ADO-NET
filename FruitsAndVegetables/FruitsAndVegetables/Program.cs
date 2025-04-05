using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FruitsAndVegetables
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-AQF96SU;Database=FruitsAndVegetables;Integrated Security=True;";

            Console.WriteLine("Натисніть Enter для підключення до бази даних...");
            Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("[Підключення успішне!]");

                    List<(string Name, string Type, string Color, int Calories)> products = new List<(string, string, string, int)>();
                    string query = "SELECT Name, Type, Color, Calories FROM FruitsAndVegetables";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Список овочів та фруктів:");
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            string type = reader.GetString(1);
                            string color = reader.GetString(2);
                            int calories = reader.GetInt32(3);
                            products.Add((name, type, color, calories));
                            Console.WriteLine($"{name} ({type}) - Колір: {color}, Калорійність: {calories} ккал");
                        }
                    }

                    Console.WriteLine("\nВведіть значення калорійності для фільтрації:");
                    int userCalories;
                    while (!int.TryParse(Console.ReadLine(), out userCalories))
                    {
                        Console.WriteLine("Будь ласка, введіть коректне число:");
                    }

                    Console.WriteLine("\nПродукти з калорійністю нижче {0} ккал:", userCalories);
                    foreach (var product in products.Where(p => p.Calories < userCalories))
                    {
                        Console.WriteLine($"{product.Name} ({product.Type}) - Колір: {product.Color}, Калорійність: {product.Calories} ккал");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }

            Console.WriteLine("\nНатисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
