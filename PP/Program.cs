using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // 1. Конвертер температуры
    static double CelsiusToFahrenheit(double celsius) => (celsius * 9 / 5) + 32;
    static double FahrenheitToCelsius(double fahrenheit) => (fahrenheit - 32) * 5 / 9;

    // 2. Перевод времени
    static string To24HourFormat(string time12)
    {
        return DateTime.Parse(time12).ToString("HH:mm");
    }

    static string To12HourFormat(string time24)
    {
        return DateTime.Parse(time24).ToString("hh:mm tt");
    }

    // 3. Калькулятор возраста
    static (int years, int months, int days) CalculateAge(DateTime birthDate)
    {
        DateTime now = DateTime.Now;
        int years = now.Year - birthDate.Year;
        int months = now.Month - birthDate.Month;
        int days = now.Day - birthDate.Day;

        if (days < 0)
        {
            days += DateTime.DaysInMonth(now.Year, now.Month - 1);
            months--;
        }

        if (months < 0)
        {
            months += 12;
            years--;
        }

        return (years, months, days);
    }

    // 4. Обратный отсчет
    static void Countdown(int seconds)
    {
        while (seconds > 0)
        {
            Console.WriteLine(seconds);
            Thread.Sleep(1000);
            seconds--;
        }
        Console.WriteLine("Time's up!");
    }

    // 5. Анализ текста
    static (int words, int sentences, int paragraphs) AnalyzeText(string text)
    {
        int wordCount = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        int sentenceCount = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        int paragraphCount = text.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries).Length;

        return (wordCount, sentenceCount, paragraphCount);
    }

    // 6. Генератор паролей
    static string GeneratePassword(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    // 7. Конвертер валют (курсы валют фиктивные)
    static double ConvertCurrency(double amount, string fromCurrency, string toCurrency)
    {
        Dictionary<string, double> exchangeRates = new Dictionary<string, double>
        {
            { "USD", 1.0 },
            { "EUR", 0.85 },
            { "RUB", 73.50 }
        };

        double amountInUsd = amount / exchangeRates[fromCurrency];
        return amountInUsd * exchangeRates[toCurrency];
    }

    // 8. Обработка данных о погоде
    static (double avg, double max, double min) AnalyzeTemperatures(double[] temperatures)
    {
        double avg = temperatures.Average();
        double max = temperatures.Max();
        double min = temperatures.Min();
        return (avg, max, min);
    }

    // 9. Простая система управления задачами
    static List<string> todoList = new List<string>();

    static void AddTask(string task)
    {
        todoList.Add(task);
    }

    static void RemoveTask(int index)
    {
        if (index >= 0 && index < todoList.Count)
            todoList.RemoveAt(index);
    }

    static void EditTask(int index, string newTask)
    {
        if (index >= 0 && index < todoList.Count)
            todoList[index] = newTask;
    }

    static void ViewTasks()
    {
        for (int i = 0; i < todoList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {todoList[i]}");
        }
    }

    // 10. API клиент для погоды
    static async Task GetWeatherAsync(string city)
    {
        string apiKey = "ac0346baa98a0bfd3310ce5d659e5807";
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

        using HttpClient client = new HttpClient();
        string response = await client.GetStringAsync(url);
        var weatherData = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

        Console.WriteLine("Weather Data:");
        foreach (var item in weatherData)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }
    }

    static async Task Main(string[] args)
    {
        // 1. Конвертер температуры
        Console.WriteLine("1. Конвертер температуры");
        Console.Write("Введите температуру в Цельсиях: ");
        double celsius = double.Parse(Console.ReadLine());
        Console.WriteLine($"Цельсий в Фаренгейт: {CelsiusToFahrenheit(celsius)}");
        Console.Write("Введите температуру в Фаренгейтах: ");
        double fahrenheit = double.Parse(Console.ReadLine());
        Console.WriteLine($"Фаренгейт в Цельсий: {FahrenheitToCelsius(fahrenheit)}");

        // 2. Перевод времени
        Console.WriteLine("2. Перевод времени");
        Console.Write("Введите время в 12-часовом формате (например, 02:30 PM): ");
        string time12 = Console.ReadLine();
        Console.WriteLine($"12-часовой в 24-часовой: {To24HourFormat(time12)}");
        Console.Write("Введите время в 24-часовом формате (например, 14:30): ");
        string time24 = Console.ReadLine();
        Console.WriteLine($"24-часовой в 12-часовой: {To12HourFormat(time24)}");

        // 3. Калькулятор возраста
        Console.WriteLine("3. Калькулятор возраста");
        Console.Write("Введите вашу дату рождения (гггг-мм-дд): ");
        DateTime birthDate = DateTime.Parse(Console.ReadLine());
        var age = CalculateAge(birthDate);
        Console.WriteLine($"Ваш возраст: {age.years} лет, {age.months} месяцев, {age.days} дней");

        // 4. Обратный отсчет
        Console.WriteLine("4. Обратный отсчет");
        Console.Write("Введите количество секунд для обратного отсчета: ");
        int seconds = int.Parse(Console.ReadLine());
        Countdown(seconds);

        // 5. Анализ текста
        Console.WriteLine("5. Анализ текста");
        Console.WriteLine("Введите текст для анализа:");
        string text = Console.ReadLine();
        var textAnalysis = AnalyzeText(text);
        Console.WriteLine($"Слова: {textAnalysis.words}, Предложения: {textAnalysis.sentences}, Абзацы: {textAnalysis.paragraphs}");

        // 6. Генератор паролей
        Console.WriteLine("6. Генератор паролей");
        Console.Write("Введите длину пароля: ");
        int passwordLength = int.Parse(Console.ReadLine());
        Console.WriteLine($"Сгенерированный пароль: {GeneratePassword(passwordLength)}");

        // 7. Конвертер валют
        Console.WriteLine("7. Конвертер валют");
        Console.Write("Введите сумму: ");
        double amount = double.Parse(Console.ReadLine());
        Console.Write("Введите валюту для конвертации (USD, EUR, RUB): ");
        string fromCurrency = Console.ReadLine();
        Console.Write("Введите валюту в которую нужно конвертировать (USD, EUR, RUB): ");
        string toCurrency = Console.ReadLine();
        Console.WriteLine($"Конвертированная сумма: {ConvertCurrency(amount, fromCurrency, toCurrency)}");

        // 8. Обработка данных о погоде
        Console.WriteLine("8. Обработка данных о погоде");
        Console.Write("Введите количество дней для анализа: ");
        int days = int.Parse(Console.ReadLine());
        double[] temperatures = new double[days];
        for (int i = 0; i < days; i++)
        {
            Console.Write($"Введите температуру за день {i + 1}: ");
            temperatures[i] = double.Parse(Console.ReadLine());
        }
        var temperatureAnalysis = AnalyzeTemperatures(temperatures);
        Console.WriteLine($"Средняя температура: {temperatureAnalysis.avg}, Максимальная: {temperatureAnalysis.max}, Минимальная: {temperatureAnalysis.min}");

        // 9. Простая система управления задачами
        Console.WriteLine("9. Простая система управления задачами");
        while (true)
        {
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Удалить задачу");
            Console.WriteLine("3. Редактировать задачу");
            Console.WriteLine("4. Просмотреть задачи");
            Console.WriteLine("5. Выйти");
            Console.Write("Выберите опцию: ");
            int option = int.Parse(Console.ReadLine());

            if (option == 1)
            {
                Console.Write("Введите задачу: ");
                string task = Console.ReadLine();
                AddTask(task);
            }
            else if (option == 2)
            {
                Console.Write("Введите номер задачи для удаления: ");
                int taskNumber = int.Parse(Console.ReadLine());
                RemoveTask(taskNumber - 1);
            }
            else if (option == 3)
            {
                Console.Write("Введите номер задачи для редактирования: ");
                int taskNumber = int.Parse(Console.ReadLine());
                Console.Write("Введите новую задачу: ");
                string newTask = Console.ReadLine();
                EditTask(taskNumber - 1, newTask);
            }
            else if (option == 4)
            {
                ViewTasks();
            }
            else if (option == 5)
            {
                break;
            }
        }

        // 10. API клиент для погоды
        Console.WriteLine("10. API клиент для погоды");
        await GetWeatherAsync("Moscow");

        Console.ReadLine(); // Чтобы консольное окно не закрывалось сразу
    }
}
