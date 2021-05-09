using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Journal.Common.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum @enum) where TEnum : Enum
            => new EnumFlagsEnumerable<TEnum>(@enum);

    
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
