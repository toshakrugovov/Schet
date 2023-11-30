using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YourNamespace;
using Тест_на_скоропечатение;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Введите ваше имя:");
        string userName = Console.ReadLine();

        await Test.StartTestAsync(userName);
        Console.WriteLine("Нажмите Enter для выхода.");
        await Test.StartTestAsync(userName);



    }
}






