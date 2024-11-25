using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    public class CustomDivideByZeroException : Exception
    {
        public CustomDivideByZeroException() : base("Attempted to divide by zero.") { }
        public static double CheckDivision(double numerator, double denominator)
        {
            if (denominator == 0)
            {
                throw new CustomDivideByZeroException();
            }
            return numerator / denominator;
        }
    }

    internal class Program
    {
        static void CreateTxtFiles(string directoryPath)
        {
            // Перевірка чи існує каталог; створюємо новий каталог якщо він відсутній
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            for (int i = 10; i <= 29; i++)
            {
                string filePath = Path.Combine(directoryPath, $"{i}.txt");

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose(); // Dispose для закриття потоку
                }
            }

            CreateTxtFilesForExceptions(directoryPath);
        }

        static void CreateTxtFilesForExceptions(string directoryPath)
        {
            string[] fileNames = { "no_file", "bad_data", "overflow" };

            for (int i = 0; i < fileNames.Length; i++)
            {
                string filePath = Path.Combine(directoryPath, $"{fileNames[i]}.txt");

                try
                {
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Dispose();
                    }
                    else
                    {
                        File.WriteAllText(filePath, ""); // Очищення файлу
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Помилка створення або очищення файлу {fileNames[i]}.txt: {ex.Message}");
                    Environment.Exit(1);
                }
            }
        }

        static void FillTxtFiles(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Каталог не знайдено.");
                return;
            }

            Random random = new Random();
            for (int i = 10; i <= 29; i++)
            {
                string filePath = Path.Combine(directoryPath, $"{i}.txt");

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(random.Next(1, 21));
                    writer.WriteLine(random.Next(1, 21));
                }
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string directoryPath = @"C:\Users\User\source\repos\Лабораторна робота 3\txt_files2";
            CreateTxtFiles(directoryPath);
            FillTxtFiles(directoryPath);

            double sumOfProducts = 0;
            int countOfProducts = 0;

            string noFilePath = Path.Combine(directoryPath, "no_file.txt");
            string badDataPath = Path.Combine(directoryPath, "bad_data.txt");
            string overflowPath = Path.Combine(directoryPath, "overflow.txt"); ;

            for (int i = 10; i <= 29; i++)
            {
                try
                {
                    string filePath = Path.Combine(directoryPath, $"{i}.txt");
                    int n1;
                    int n2;
                    int product;

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        n1 = int.Parse(reader.ReadLine());
                        n2 = int.Parse(reader.ReadLine());
                    }

                    checked
                    {
                        product = n1 * n2;
                        sumOfProducts += product;
                        countOfProducts++;
                    }

                }
                catch (FileNotFoundException ex)
                {
                    using (StreamWriter writer = new StreamWriter(noFilePath, true)) // true для додавання в кінець файлу
                    {
                        writer.WriteLine($"{i}.txt"); // Записуємо назву файлу
                    }
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    using (StreamWriter writer = new StreamWriter(badDataPath, true))
                    {
                        writer.WriteLine($"{i}.txt");
                    }
                    Console.WriteLine($"Помилка: {ex.Message} 'File: {i}.txt'");
                }
                catch (OverflowException ex)
                {
                    using (StreamWriter writer = new StreamWriter(overflowPath, true))
                    {
                        writer.WriteLine($"{i}.txt");
                    }
                    Console.WriteLine($"Помилка: {ex.Message} 'File: {i}.txt'");
                }
                catch (Exception ex)
                {
                    using (StreamWriter writer = new StreamWriter(badDataPath, true)) // true для додавання в кінець файлу
                    {
                        writer.WriteLine($"{i}.txt"); // Записуємо назву файлу
                    }
                    Console.WriteLine($"Помилка: {ex.Message} 'File: {i}.txt'");
                }
            }

            Console.WriteLine(" ");

            countOfProducts = 0;
            try
            {
                double averageOfProducts = CustomDivideByZeroException.CheckDivision(sumOfProducts, countOfProducts);
                Console.WriteLine($"Сума добутків: {sumOfProducts}");
                Console.WriteLine($"Середнє арифметичне добутків: {averageOfProducts}");
            }
            catch (CustomDivideByZeroException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
