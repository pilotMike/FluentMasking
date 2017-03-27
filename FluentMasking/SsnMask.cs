namespace FluentMasking
{
    public class SsnMask : AbstractMask
    {
        internal SsnMask()
        {
            AddCharacterFilter(Characters.Numbers);
            MaskWhen(s => s.Length == 9);

            SetMask((s,c) => s.Substring(s.Length - 5).PadLeft(9, c));
            SetFormat(s => s.Insert(5, "-").Insert(2, "-"));
        }
    }
}