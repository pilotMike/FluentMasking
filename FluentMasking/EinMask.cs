namespace FluentMasking
{
    internal class EinMask : AbstractMask
    {
        public EinMask()
        {
            AddCharacterFilter(Characters.Numbers);
            MaskWhen(s => s.Length == 9);
            SetMask((s, c) => s.Substring(s.Length - 5)
                .PadLeft(9, c)
                .Insert(2, "-"));
        }
    }
}