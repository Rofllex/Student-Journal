using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Journal.Common.Extensions
{
    /// <summary>
    ///     Расширения перечисления.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Получить перечисление всех имеющихся флагов.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetContainsFlags<TEnum>(this TEnum @enum) where TEnum : Enum
            => new EnumFlagsEnumerable<TEnum>(@enum);

        /// <summary>
        ///     Получить все значения флагов.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     Тип перечисления.
        /// </typeparam>
        /// <param name="value">
        ///     Значение 
        /// </param>
        /// <returns>
        ///     Массив всех имеющихся значений во флаге.
        /// </returns>
        public static TEnum[] GetContainsFlagsAsArray<TEnum>(this TEnum value) where TEnum : Enum
        {
            TEnum[] allValues = (TEnum[])Enum.GetValues(typeof(TEnum));
            return allValues.Where(v => value.HasFlag(v)).ToArray();
        }

        private class EnumFlagsEnumerable<TEnum> : IEnumerable<TEnum> where TEnum : Enum
        {
            public class Enumerator<TEnum> : IEnumerator<TEnum> where TEnum : Enum
            {
                public Enumerator(TEnum value, IEnumerator<TEnum> availableValues)
                {
                    _valuesEnumerator = availableValues;
                    _value = value;
                }

                public TEnum Current => _valuesEnumerator.Current;

                object IEnumerator.Current => _valuesEnumerator.Current;

                public void Dispose()
                    => _valuesEnumerator.Dispose();

                public bool MoveNext()
                {
                    while (_valuesEnumerator.MoveNext())
                        if (_value.HasFlag(_valuesEnumerator.Current))
                            return true;
                    return false;
                }

                public void Reset()
                    => _valuesEnumerator.Reset();

                private IEnumerator<TEnum> _valuesEnumerator;
                private TEnum _value;
            }


            public EnumFlagsEnumerable(TEnum value)
            {
                _value = value;
            }

            public IEnumerator<TEnum> GetEnumerator()
                => new Enumerator<TEnum>(_value, Enum.GetValues(typeof(TEnum)).Cast<TEnum>().GetEnumerator());

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            private TEnum _value;
        }
    }
}
