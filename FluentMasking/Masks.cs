namespace FluentMasking
{
    /// <summary>
    /// A collection of masking a formatting functions provided for easy 
    /// access. This class is partial, so you can add to it by making
    /// a partial class in the FluentMasking namespace.s
    /// </summary>
    public partial class Masks
    {
        /// <summary>Social Security Number</summary>
        public static AbstractMask SSN = new SsnMask();
        /// <summary>Enterprise Identification Number</summary>
        public static AbstractMask EIN = new EinMask();
        /// <summary>Formats a phone number</summary>
        public static AbstractMask Phone = new PhoneMask();
        /// <summary>
        /// A collection of formatters that do not mask characters, but
        /// will format strings.
        /// </summary>
        public static Formats Formats = new Formats();
        /// <summary>
        /// Gets or sets the default character to use for masking across all mask instances.
        /// Default value is '#'.
        /// </summary>
        public static char DefaultCharacter { get; set; } = '#';
    }

    /// <summary>
    /// A container for organizing the Masks class.
    /// </summary>
    public class Formats
    {
        /// <summary>
        /// Provides an SSN mask that only formats with hyphens.
        /// </summary>
        public AbstractMask SSN = new SsnMask().ClearMask();
    }
}