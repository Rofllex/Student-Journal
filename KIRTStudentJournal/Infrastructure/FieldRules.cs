using System;

namespace KIRTStudentJournal.Infrastructure
{
    public class FieldRules
    {
        public IFieldRule LoginRule { get; } = new RelayStringFieldRule((object val, out string errorMessage) =>
        {
            string strVal = (string)val;
            for (int i = 0; i < strVal.Length; i++)
            {
                if (char.IsControl(strVal[i]))
                {
                    errorMessage = "Логин не может содержать управляющие символы";
                    return false;
                }
            }

            if (strVal.Length > 5)
            {
                errorMessage = null;
                return true;
            }
            else
            {
                errorMessage = "Поле не может содержать менее 5 символов";
                return false;
            }

        });

        public IFieldRule PasswordRule => new RelayStringFieldRule((object val, out string message) => 
        {
            string strVal = (string)val;
            for (int i = 0; i < strVal.Length; i++)
                if (char.IsControl(strVal[i]))
                {
                    message = "Пароль содержит недопустимые символы";
                    return false;
                }
            message = null;
            return true;
        });
    }

    public interface IFieldRule
    {
        /// <summary>
        /// Проверка типа на возможность проверки
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CanCheck(Type type);

        /// <summary>
        /// Проверка поля на валидность.
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        /// <returns>
        /// Должен вернуть true если поле верно
        /// </returns>
        bool Check(object value, out string errorMessage);
    }

    public class RelayFieldRule : IFieldRule
    {
        public delegate bool CheckFieldDelegate(object value, out string errorMessage);

        private readonly CheckFieldDelegate checkField;
        private readonly Func<Type, bool> canCheck;
        public RelayFieldRule(CheckFieldDelegate check, Func<Type, bool> canCheck = null)
        {
            checkField = check;
            this.canCheck = canCheck ?? new Func<Type, bool>((_) => true);
        }


        public bool CanCheck(Type type) => canCheck(type);

        public bool Check(object value, out string errorMessage) => checkField(value, out errorMessage);
    }

    public abstract class StringFieldRule : IFieldRule
    {
        public bool CanCheck(Type type) => type == typeof(string);

        public abstract bool Check(object value, out string errorMessage);
    }

    public class RelayStringFieldRule : StringFieldRule 
    {
        private RelayFieldRule.CheckFieldDelegate check;
        public RelayStringFieldRule(RelayFieldRule.CheckFieldDelegate check)
        {
            this.check = check;
        }

        public override bool Check(object value, out string errorMessage) => check(value, out errorMessage);
    }
}
