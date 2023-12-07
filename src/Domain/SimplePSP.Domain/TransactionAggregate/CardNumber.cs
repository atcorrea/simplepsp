using System.Text.RegularExpressions;

namespace SimplePSP.Domain.TransactionAggregate
{
    public record CardNumber
    {
        public string Number { get; private set; }

        public string Last4Digits => Number.Substring(12, 4);

        public CardNumber(string number)
        {
            Number = GetDigits(number);
        }

        private static string GetDigits(string str) => Regex.Replace(str, @"[_\s\.\-a-zA-Z]", "");
    }
}