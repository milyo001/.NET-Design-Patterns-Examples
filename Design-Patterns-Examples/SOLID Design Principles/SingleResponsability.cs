namespace DesignPatternsExamples.SOLID
{
    // Just stores a couple of journal entries and ways of
    // working with them
    public class Journal
	{
		// Initialize the list to avoid any null reference exceptions
		private readonly List<string> entries = new List<string>();

		private static int count = 0;

        // Add entry to the enties field above,
        // That method is considered also as Memento pattern! Check Memento Design Pattern folder
        public int AddEntry(string text)
		{
			entries.Add($"{++count}: {text}");
			return count;
		}

		// Removes an entry from the list. 
		// Note: As you can see the method has a single responsibility, to remove an entry from the list.
		public void Remove(int index)
		{
			entries.RemoveAt(index);
		}
		
		public override string ToString()
		{
			return string.Join(Environment.NewLine, entries);
		}

		// Save,Load(string filename) and Load(Uri uri) methods breaks single responsibility principle, the class will have more methods and 
		// more responsibilities. Not just adding and removing entries, but also saving and loading to/from file.
		public void Save(string filename, bool overwrite = false)
		{
			File.WriteAllText(filename, ToString());
		}

		// Method without any logic just for example
		public void Load(string filename)
		{

		}
        
		// Method without any logic just for example
		public void Load(Uri uri)
		{

		}
	}

    // Handles the responsibility of persisting objects. Now we have separation of concerns.
    // Journal class is responsible to add entry and remove entry and Persistence class is responsible to save to file.
    public class Persistence
	{
		public void SaveToFile(Journal journal, string filename, bool overwrite = false)
		{
			if (overwrite || !File.Exists(filename))
				File.WriteAllText(filename, journal.ToString());
		}
	}

	public class SingleResponsability
	{
		public void Main()
        {
			var j = new Journal();
            j.AddEntry("The first entry in the journal.");
            j.AddEntry("Sometimes the dog can bark loud.");
            j.AddEntry("Sometimes the cat can do more than just meow.");

            // Will call .ToString method which is overriden, see the Journal class
            Console.WriteLine(j);	

			// The file will be saved by given path in property file
			var p = new Persistence();
			var filename = @"c:\temp\journal.txt";
			p.SaveToFile(j, filename);
			Process.Start(filename);
		}
	}
}
