using System;
using Xunit;

namespace FluentMasking.Tests
{
    public class MaskingTests
    {
        [Fact]
        public void null_input_returns_empty_string()
        {
            string input = null;
            var result = input.Mask(Masks.SSN);
            Assert.Equal(string.Empty, result);

            input = string.Empty;
            result = input.Mask(Masks.SSN);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void returns_ssn_mask()
        {
            var ssn = "123456789";
            var result = ssn.Mask(Masks.SSN);
            Assert.Equal("###-##-6789", result);
        }

        [Fact]
        public void returns_ssn_mask_with_provided_character_mask()
        {
            var ssn = "123456789";
            var result = ssn.Mask(Masks.SSN.SetMaskCharacter('*'));
            Assert.Equal("***-**-6789", result);
        }

        [Fact]
        public void returns_formatted_ssn_without_changing_characters()
        {
            var ssn = "123456789";
            var result = ssn.Mask(Masks.Formats.SSN);
            Assert.Equal("123-45-6789", result);
        }

        [Fact]
        public void returns_ein_mask()
        {
            var ssn = "123456789";
            var result = ssn.Mask(Masks.EIN);
            Assert.Equal("12-3456789", result);
        }

        [Fact]
        public void returns_ein_mask_with_character()
        {
            var ssn = "123456789";
            var result = ssn.Mask(Masks.EIN);
            Assert.Equal("##-###6789", result);
        }

        [Fact]
        public void masks_long_phone()
        {
            var phone = "1234567890";
            var result = phone.Mask(Masks.Phone);
            Assert.Equal("(123) 456-7890", result);
        }

        [Fact]
        public void masks_short_phone()
        {
            var phone = "4567890";
            var result = phone.Mask(Masks.Phone);
            Assert.Equal("456-7890", result);
        }
    }
}
