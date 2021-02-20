namespace Journal.Common.Models
{
    public sealed class InvalidArgumentRequestError : RequestError
    {
        public InvalidArgumentRequestError( string argumentName) : base ($"Неверное значение аргумента \"{ argumentName }\"")
        {

        }
    }
}
