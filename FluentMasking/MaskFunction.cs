namespace FluentMasking
{
    /// <summary>
    /// Function to apply a character mask over a string.
    /// </summary>
    /// <param name="textToMask">text to mask</param>
    /// <param name="maskingCharacter">character to use for masking</param>
    /// <returns>A masked string.</returns>
    public delegate string MaskFunction(string textToMask, char maskingCharacter);
}