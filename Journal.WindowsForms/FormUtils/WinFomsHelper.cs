using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

#nullable enable

namespace Journal.WindowsForms.FormUtils
{
    public static class WinFomsHelper
    {
        /// <summary>
        ///     Делегат форматирования введенного текста, где остаются только цифры.
        ///     Диапазон значений от <see cref=""/>
        /// </summary>
        public static readonly EventHandler OnlyNumbers = CreateOnlyNumbers();

        /// <summary>
        ///     Метод позволяющий создать делегат для обработки текста.
        /// </summary>
        /// <param name="throwIfInvalidSender">Выбрасывать исключение если sender не является производной типа Control <param>
        /// <returns></returns>
        public static EventHandler CreateOnlyNumbers(bool throwIfInvalidSender = true)
        {
            return new EventHandler((sender, _) =>
            {
                Control? control = sender as Control;
                if (control != null)
                {
                    string text = control.Text;
                    //  Если первый символ минус или плюс, то пропускаем.
                    int iChar = (text[0] == '-' || text[0] == '+') ? 1 : 0;

                    bool containsDecimalSeparator = false;

                    for (; iChar < text.Length; iChar++)
                    {
                        char currentChar = text[iChar];
                        if (!char.IsDigit(currentChar))
                        {
                            if (currentChar == ',' && !containsDecimalSeparator)
                            {
                                containsDecimalSeparator = true;
                                continue;
                            }

                            text = text.Remove(iChar);
                            iChar--;
                        }
                    }

                    if (control.Text.Length != text.Length)
                        control.Text = text;
                }
                else if (throwIfInvalidSender)
                    throw new ArgumentException($"{nameof(sender)} не является производным от {nameof(Control)}");
            });
        }
    }
}
