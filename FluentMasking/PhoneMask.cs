namespace FluentMasking
{
    internal class PhoneMask : AbstractMask
    {
        public PhoneMask()
        {
            AddCharacterFilter(Characters.Numbers);
            SetFormat(phone => phone.Length == 7, phone => phone.Insert(3, "-"));
            SetFormat(phone => phone.Length == 10, FormatLongPhone);
        }

        private static string FormatLongPhone(string phone)
        {
            var areaCode = "(" + phone.Substring(0, 3) + ")";
            var rest = phone.Substring(3).Insert(3, "-");
            return areaCode + " " + rest;
        }
    }
}