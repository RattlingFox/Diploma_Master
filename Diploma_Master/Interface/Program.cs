﻿using System;
using System.Diagnostics;
using System.Threading;

namespace Diploma_Master.System
{
    class Program
    {
        static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Для запуска программы нажмите любую клавишу");
            Console.ReadLine();
            Console.Clear();
            
            Console.WriteLine("Запуск инициализации хранилища");
            UI.StorageInitialization(6);
            Console.WriteLine("Хранилище инициализировано");
            UI.StoragePrint();
            Thread.Sleep(5000);
            Console.Clear();

            UI.StoragePrint();
            Console.WriteLine();
            Console.WriteLine("Инициализация начального решения");
            UI.PopulationInitialization();
            Console.WriteLine("Решение инициализировано");
            Console.WriteLine();
            UI.BestObjectiveFunctionValues();
            Thread.Sleep(5000);
            Console.Clear();
            Console.WriteLine();

            Console.WriteLine("Реализация генетического алгоритма");
            Console.WriteLine();
            UI.GARealization();
            Console.WriteLine();

            UI.StoragePrint();
            Console.WriteLine();
            Console.WriteLine("Параметры наилучшего полученного решения");
            UI.BestObjectiveFunctionValues();

            Console.WriteLine("Для выхода из программы нажмите любую клавишу");
            Console.ReadLine();
        }
    }
}
