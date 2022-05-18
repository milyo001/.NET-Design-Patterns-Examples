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

		// Save and Load methods breaks single responsibility principle, the class will have more methods and 
		// more responsibilities. Not just adding and removing entries, but also saving and loading to/from file.
		public void Save(string filename, bool overwrite = false)
		{
			File.WriteAllText(filename, ToString());
		}

		public void Load(string filename)
		{

		}

		public void Load(Uri uri)
		{

		}
	}

	// Handles the responsibility of persisting objects
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
		public void Init()
        {
			var j = new Journal();
			j.AddEntry("I cried today.");
			j.AddEntry("I ate a bug.");
			// Will call .ToString method which is overriden, see the Journal class
			Console.WriteLine(j);	

			// The file will be save by given path in property file
			var p = new Persistence();
			var filename = @"c:\temp\journal.txt";
			p.SaveToFile(j, filename);
			Process.Start(filename);
		}
	}
}
