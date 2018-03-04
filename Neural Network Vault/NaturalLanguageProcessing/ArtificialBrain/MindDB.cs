using System;
using System.Collections.Generic;
using NaturalLanguageProcessing;

namespace ArtificialBrain
{
    public class MindDB
    {
        // This class is used to control what the ai knows about its surroundings, and its word.
        // conversationMem holds important data about the current conversation.
        // longTermMem holds all of the important information that the ai knows.
        // wordDB contains every word that the ai knows.

        private Dictionary<string, object> longTermMem;
        private Dictionary<string, object> conversationMem;
        private List<Word> wordDB;

        public MindDB()
        {
            longTermMem = new Dictionary<string, object>();
            conversationMem = new Dictionary<string, object>();
            wordDB = new List<Word>();
        }

        public T FindSomething<T>(string subject = "", List<string> subtopics = null)
        {
            bool found = false;
            if(subject == "")
            {
                foreach(object obj in longTermMem)
                {
                    if (obj.GetType() == Type.GetType("Dictionary<string, object>"))
                        FindInDict((Dictionary<string, object>)obj, subtopics);
                }
            }

            object FindInDict(Dictionary<string, object> longTermMem, List<string> topics)
            {
                if (longTermMem.ContainsKey(topics[0]))
                {
                    if (longTermMem.GetValueOrDefault(topics[0]).GetType() == Type.GetType("Dictionary<string, object>"))
                    {
                        string key = topics[0];
                        topics.RemoveAt(0);
                        return FindInDict((Dictionary<string, object>)longTermMem[key], topics);
                    }
                    else
                    {
                        return longTermMem[topics[0]];
                    }
                }
                else
                    return null;
            }
        }

        public void LearnWord(Word word)
        {
            wordDB.Add(word);
        }

        public void LearnWord(List<Word> words)
        {
            wordDB.AddRange(words);
        }

        public static MindDB operator +(MindDB mind, Word word)
        {
            mind.LearnWord(word);
            return mind;
        }

        public static MindDB operator +(MindDB mind, List<Word> words)
        {
            mind.LearnWord(words);
            return mind;
        }
    }
}
