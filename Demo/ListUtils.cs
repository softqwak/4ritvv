using System;
using System.Collections.Generic;
namespace Demo
{
    public class ListUtils
    {
        /// <summary>
        /// Удаляет дубликаты из списка строк, сохраняя порядок первых вхождений.
        /// Использует HashSet для эффективного поиска дубликатов.
        /// Модифицирует исходный список.
        /// </summary>
        /// <param name="list">Список строк, из которого удаляются дубликаты.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если список null.</exception>
        public static void RemoveDuplicates(List<string> list)
        {
            // Проверяем, что список не null
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "Список не может быть null.");
            }

            // Создаём HashSet для отслеживания уникальных строк
            var seen = new HashSet<string>();

            // Индекс для записи уникальных элементов
            int writeIndex = 0;

            // Проходим по списку
            for (int readIndex = 0; readIndex < list.Count; readIndex++)
            {
                // Текущая строка
                string item = list[readIndex];

                // Если строка ещё не встречалась, добавляем её
                if (seen.Add(item))
                {
                    // Перемещаем строку на позицию writeIndex
                    list[writeIndex] = item;
                    writeIndex++;
                }
            }

            // Удаляем лишние элементы с конца списка
            if (writeIndex < list.Count)
            {
                list.RemoveRange(writeIndex, list.Count - writeIndex);
            }
        }
    }
}