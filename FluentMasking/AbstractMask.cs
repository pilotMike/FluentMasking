using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentMasking
{
    public abstract class AbstractMask
    {
        #region Private Static

        private static readonly Dictionary<Characters, Func<string, string>> ValidFuncs;

        #endregion

        #region Static Properties

        public static char DefaultCharacter { get; set; } = '#';

        #endregion

        #region Fields

        private List<Func<string, string>> FilterFunctions { get; }
        private List<Predicate<string>> Conditions { get; }
        private MaskFunction _maskFunc = (s, c) => s;
        private FormatFunction _format = s => s;
        private char _maskingCharacter = DefaultCharacter;
        private List<MaskCondition> MaskConditions { get; }
        private List<FormatCondition> FormatConditions { get; }

        #endregion

        #region Constructors

        static AbstractMask()
        {
            ValidFuncs = new Dictionary<Characters, Func<string, string>>
            {
                { Characters.Letters, s => string.Join("", s.Where(char.IsLetter)) },
                { Characters.Numbers, s => string.Join("", s.Where(char.IsNumber)) }
            };
        }

        protected AbstractMask()
        {
            Conditions = new List<Predicate<string>>();
            FilterFunctions = new List<Func<string, string>>();
            MaskConditions = new List<MaskCondition>();
            FormatConditions = new List<FormatCondition>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a filter function to remove invalid characters from the string.
        /// </summary>
        protected void AddCharacterFilter(Characters c) => FilterFunctions.Add(ValidFuncs[c]);
        /// <summary>
        /// Adds a filter function to remove invalid characters from the string.
        /// </summary>
        protected void AddCharacterFilter(Func<string, string> filter) => FilterFunctions.Add(filter);

        /// <summary>
        /// Adds a condition to check to verify that the string to mask is valid after illegal
        /// characters have been filtered out.
        /// </summary>
        /// <param name="conditionPredicate">check for whether string can be masked</param>
        protected void MaskWhen(Predicate<string> conditionPredicate) => Conditions.Add(conditionPredicate);
        
        /// <summary>
        /// Sets the function to use for masking.
        /// </summary>
        /// <param name="maskFunc">function to perform the mapping on a filtered string</param>
        protected void SetMask(MaskFunction maskFunc) => _maskFunc = maskFunc;
        
        /// <summary>
        /// Supplies a function to use to perform a mapping if the <param name="maskCondition"></param>
        /// is met for the filtered string.
        /// </summary>
        /// <param name="maskCondition">condition on which to map</param>
        /// <param name="maskFunction">function to call on the filtered string</param>
        protected void SetMask(Predicate<string> maskCondition, Func<string, char, string> maskFunction) =>
            MaskConditions.Add(new MaskCondition(maskCondition, new MaskFunction(maskFunction)));

        /// <summary>
        /// Sets the mask to use when the <paramref name="maskCondition"/> predicate is satisfied.
        /// </summary>
        protected void SetMask(Predicate<string> maskCondition, Func<string, string> func) =>
            MaskConditions.Add(new MaskCondition(maskCondition, (s, c) => func(s)));

        /// <summary>
        /// Sets the format function to perform on a string that has been masked when
        /// the <paramref name="maskCondition"/> predicate has been satisfied, such as
        /// inserting hyphens after characters have been replaced.
        /// </summary>
        protected void SetFormat(Predicate<string> maskCondition, Func<string, string> func) =>
            FormatConditions.Add(new FormatCondition(maskCondition, new FormatFunction(func)));

        /// <summary>
        /// Sets the internal format function to be called after the filter function has been called 
        /// (if applicable).
        /// </summary>
        protected void SetFormat(Func<string, string> format) => _format = new FormatFunction(format);

        /// <summary>
        /// Sets the character to use a replacement character in the mask function.
        /// </summary>
        public AbstractMask SetMaskCharacter(char c)
        {
            _maskingCharacter = c;
            return this;
        }

        /// <summary>
        /// Clears the mask function. Useful for only setting a format for text,
        /// such as using the SSN mask, but only inserting hyphens.
        /// </summary>
        public AbstractMask ClearMask()
        {
            _maskFunc = (s, c) => s;
            return this;
        }

        /// <summary>
        /// Execute the filter and mask function(s) on string <paramref name="s"/>.
        /// </summary>
        /// <param name="s">text to filter and mask</param>
        /// <returns>A new filtered and masked string.</returns>
        public string Mask(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            var original = s;
            s = Filter(s);
            if (!MeetsConditions(s)) return original;
            s = ApplyMask(s);
            s = Format(s);
            return s;
        }
        
        private string Filter(string s) => FilterFunctions.Aggregate(s, (current, filter) => filter(current));
        private bool MeetsConditions(string s) => Conditions.All(condition => condition(s));
        
        private string ApplyMask(string s) =>
            MaskConditions.Where(formatCondition => formatCondition.Predicate(s)).Select(fc => fc.Function)
            .Concat(new [] {_maskFunc})
            .Aggregate(s, (current, func) => func(current, _maskingCharacter));

        private string Format(string s) => 
            FormatConditions.Where(fc => fc.Predicate(s)).Select(fc => fc.Function)
            .Concat(new[] {_format})
            .Aggregate(s, (current, format) => format(current));

        #endregion

        #region Helper Classes

         private class MaskCondition
        {
            public Predicate<string> Predicate { get; }
            public MaskFunction Function { get;  }

            public MaskCondition(Predicate<string> predicate, MaskFunction function)
            {
                Predicate = predicate;
                Function = function;
            }
        }

        private class FormatCondition
        {
            public Predicate<string> Predicate { get; }
            public FormatFunction Function { get; }

            public FormatCondition(Predicate<string> predicate, FormatFunction function)
            {
                Predicate = predicate;
                Function = function;
            }
        }

        #endregion
    }
}
