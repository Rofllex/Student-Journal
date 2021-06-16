using System;
using System.ComponentModel;
using System.Linq.Expressions;
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
                    if (text.Length == 0)
                    {
                        control.Text = 0.ToString();
                        TextBox? textBox = control as TextBox;
                        if (textBox != null)
                        {
                            textBox.SelectionLength = 0;
                            textBox.SelectionStart = 1;
                        }

                        return;
                    }
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
       
        /// <summary>
        ///     Метод связывания свойства контрола со свойством модели-представления.
        /// </summary>
        /// <typeparam name="TControl">
        ///     Тип контрола. Контрол должен быть унаследован от интерфейса <see cref="Control"/>
        /// </typeparam>
        /// <typeparam name="TViewModel">
        ///     Тип модели-представления. Должен быть унаследован от интерфейса <see cref="INotifyPropertyChanged"/>
        /// </typeparam>
        /// <param name="control">
        ///     Экземпляра контрола.
        /// </param>
        /// <param name="viewModel">
        ///     Экземпляр модели-представления.
        /// </param>
        /// <param name="controlPropertyExpression">
        ///     Выражение выбора свойства контрола.
        /// </param>
        /// <param name="viewModelPropertyExpression">
        ///     Выражение выбора свойства модели-представления.
        /// </param>
        /// <returns />
        /// <inheritdoc cref="ControlBindingsCollection.Add(string, object, string, bool, DataSourceUpdateMode, object, string, IFormatProvider)"/>
        public static void Bind<TControl, TViewModel>(this TControl control, TViewModel viewModel
                                                , Expression<Func<TControl, object>> controlPropertyExpression
                                                , Expression<Func<TViewModel, object>> viewModelPropertyExpression
                                                , bool formattingEnabled
                                                , DataSourceUpdateMode updateMode
                                                , object nullValue
                                                , string formatString
                                                , IFormatProvider formatProvider)
            where TControl : Control
            where TViewModel : INotifyPropertyChanged
        {
            string controlMemberName = _GetInvocationPropertyName( controlPropertyExpression );
            string viewModelMemberName = _GetInvocationPropertyName( viewModelPropertyExpression );
            control.DataBindings.Add( controlMemberName, viewModel, viewModelMemberName, formattingEnabled, updateMode, nullValue, formatString, formatProvider );
        }

        /// <inheritdoc cref="Bind{TControl, TViewModel}(TControl, TViewModel, Expression{Func{TControl, object}}, Expression{Func{TViewModel, object}}, bool, DataSourceUpdateMode, object, string, IFormatProvider)"/>
        public static void Bind<TControl, TViewModel>( this TControl control
                , TViewModel viewModel
                , Expression<Func<TControl, object>> controlPropertyExpression
                , Expression<Func<TViewModel, object>> viewModelPropertyExpression
                , bool formattingEnabled = true
                , DataSourceUpdateMode updateMode = DataSourceUpdateMode.OnPropertyChanged)
            where TControl : Control
            where TViewModel : INotifyPropertyChanged
        {
            string controlMemberName = _GetInvocationPropertyName( controlPropertyExpression );
            string viewModelMemberName = _GetInvocationPropertyName( viewModelPropertyExpression );
            control.DataBindings.Add( controlMemberName, viewModel, viewModelMemberName, formattingEnabled, updateMode);
        }

        private static string _GetInvocationPropertyName( LambdaExpression lambdaExpr )
        {
            MemberExpression? mExpr = lambdaExpr.Body as MemberExpression;
            if (mExpr == null)
            {
                UnaryExpression unaryExpression = ( UnaryExpression )lambdaExpr.Body;
                mExpr = ( MemberExpression )unaryExpression.Operand;
            }
            
            return mExpr.Member.Name;
        }
    }
}
