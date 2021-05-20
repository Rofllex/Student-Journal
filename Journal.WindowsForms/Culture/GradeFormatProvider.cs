using Journal.Common.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace Journal.WindowsForms.Culture
{
    public class GradeFormatProvider : IFormatProvider, ICustomFormatter
    {
        public static GradeFormatProvider ShortProvider => new GradeFormatProvider("short");

        public static string ShortFormat(GradeLevel grade)
            => grade switch
                {
                    GradeLevel.Five => "5",
                    GradeLevel.Four => "4",
                    GradeLevel.Three => "3",
                    GradeLevel.Two => "2",
                    GradeLevel.Miss => "НБ",
                    GradeLevel.Offset => "Зач",
                    GradeLevel.Fail => "Н/Зач",
                    _ => throw new FormatException()
                };

        private GradeFormatProvider(string format)
        {
            _format = format;
        }


        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg != null)
            {
                GradeLevel? level = arg as GradeLevel?;
                if (!level.HasValue)
                    throw new FormatException();
                return _format switch
                {
                    "short" => level switch
                    {
                        GradeLevel.Five => "5",
                        GradeLevel.Four => "4",
                        GradeLevel.Three => "3",
                        GradeLevel.Two => "2",
                        GradeLevel.Miss => "НБ",
                        GradeLevel.Offset => "Зач",
                        GradeLevel.Fail => "Н/Зач",
                        _ => throw new FormatException()
                    },
                    _ => throw new FormatException()
                };
            }
            else
                throw new FormatException();
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(GradeFormatProvider))
                return this;
            else
                return null;
        }

        private string _format;

    }
}
