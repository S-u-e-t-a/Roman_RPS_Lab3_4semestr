using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace laba3
{
    internal class FileSystem
    {
        /// <summary>
        ///     Функция для получения чисел из файла
        /// </summary>
        /// <param name="path"> Путь к файлу </param>
        /// <returns> Список с данными из файла </returns>
        public static List<double> ReadFromFile(string path)
        {
            string[] stringSeparators = {"\r", "\n", " ", "\t"};
            var numbers = File.ReadAllText(path)
                .Split(stringSeparators,
                    StringSplitOptions.RemoveEmptyEntries) // разбиваем строку по указанным символам
                .Select(n => double.Parse(n)) // преобразуем элементы в double
                .ToList(); // преобразуем результат в список
            return numbers;
        }

        /// <summary>
        /// Записывает переданный текст в файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="text">Текст для записи</param>
        public static void SaveToFile(string path, string text)
        {
            var fileWriter = new StreamWriter(path);
            fileWriter.WriteLine(text);
            fileWriter.Close();
        }
    }
}