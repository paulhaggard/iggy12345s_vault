using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalLanguageProcessing
{
    // This is a list of all word classes that exist, this helps determine what kind of word is being used.
    public enum WordClasses
    {
        Noun = 0, Verb, Adjective, Adverb, Pronoun, Preposition, Conjunction,
        Determiner, Exclamation
    }

    public class Word
    {
        // This class is used to define words that come from either the user, or wordnet
        // The contain the name of the word, it's word type, and it's definitions.

        // Properties
        private string name;
        private List<WordClasses> types;
        private List<List<DefinedWord>> definitions;
        private long id;
        private static long wordCount;

        // Accessor Methods
        public string Name { get => name; set => name = value; }
        public List<WordClasses> Types { get => types; set => types = value; }
        public List<List<DefinedWord>> Definitions { get => definitions; set => definitions = value; }
        public long ID { get => id; set => id = value; }
        public static long WordCount { get => wordCount; }

        // Constructor
        public Word(string name, List<WordClasses> types = null, List<List<DefinedWord>> definitions = null)
        {
            this.name = name;
            this.types = types ?? new List<WordClasses>();
            this.definitions = definitions ?? new List<List<DefinedWord>>();
            id = wordCount++;
        }

        // Method used for finding a definition of a word.
        public virtual void Define(int index = 0)
        {
            foreach(DefinedWord word in definitions[index])
            {
                word.Define();
            }
        }
    }
}
