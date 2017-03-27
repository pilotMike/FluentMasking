namespace FluentMasking
{
    public static class MaskExtension
    {
        /// <summary>
        /// A convenient way of calling mask.Mask(string)
        /// </summary>
        public static string Mask(this string s, AbstractMask mask) => mask.Mask(s);
        
    }
}