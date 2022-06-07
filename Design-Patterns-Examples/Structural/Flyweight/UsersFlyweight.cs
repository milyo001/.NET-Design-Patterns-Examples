using NUnit.Framework;
using JetBrains.dotMemoryUnit;

// Let's supose we have a user system and they have full name that the users provide 

// Let's imagine that we have 100 000 users with similar names (similar first name or last name etc.)
// and might optimize the memory usage 

namespace DotNetDesignPatternDemos.Structural.Flyweight.Users
{
    
    using System;
    using static System.Console;

    // First example check the TestUser() Unit 
    public class User
    {
        private string fullName;

        public User(string fullName)
        {
            this.fullName = fullName;
        }
    }

    // Second example with flyweight optimization
    public class User2
    {
        // Cache of all possible strings
        static List<string> stringsCache = new List<string>();
        
        // An array with the indexes of the first and last names stored in the stringsCache
        private int[] names;

        public User2(string fullName)
        {
            // An inner function in the constructor
            int getOrAdd(string s)
            {
                int idx = stringsCache.IndexOf(s);
                
                if (idx != -1) return idx;
                else
                {
                    stringsCache.Add(s);
                    // Return the index of last element in cache
                    return stringsCache.Count - 1;
                }
            }

            names = fullName.Split(' ').Select(getOrAdd).ToArray();
        }

        // Obtain the full name if needed
        public string FullName => string.Join(" ", names.Select(i => stringsCache[i]));
    }

    [TestFixture]
    public class Demo
    {
        static void Main(string[] args)
        {

        }

        
        public void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        // The method to generate fake names 
        public static string RandomString()
        {
            Random rand = new Random();
            return new string(
              Enumerable.Range(0, 10).Select(i => (char)('a' + rand.Next(26))).ToArray());
        }
        
        // Right click on the test and click 'Run under dotNetMemory Unit'
        [Test]
        public void TestUser()
        {
            var users = new List<User>();

            // Let's generate a 100 first names
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            // And then 100 random last names
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            foreach (var firstName in firstNames)
                foreach (var lastName in lastNames)
                    users.Add(new User($"{firstName} {lastName}"));

            // Then force garbage collector
            ForceGC();

            // I am using the Jetbrains.dotMemory plugin to check the memory
            dotMemory.Check(memory =>
            {
                WriteLine(memory.SizeInBytes);
            });
        }

        [Test]
        public void TestUser2()
        {
            var users = new List<User2>();

            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            foreach (var firstName in firstNames)
                foreach (var lastName in lastNames)
                    users.Add(new User2($"{firstName} {lastName}"));

            ForceGC();

            dotMemory.Check(memory =>
            {
                WriteLine(memory.SizeInBytes);
            });
        }
    }
}