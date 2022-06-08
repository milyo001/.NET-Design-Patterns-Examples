using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// MVVM 
namespace DotNetDesignPatternDemos.Structural.Proxy
{
    // Model, generate from Database etc
    public class Person
    {
        public string FirstName;
        public string LastName;
    }

    // What if you need to update on changes?

    /// <summary>
    /// A wrapper around a <c>Person</c> that can be
    /// bound to UI controls.
    /// </summary>
    public class PersonViewModel // proxy over the Person itself
      : INotifyPropertyChanged
    {
        private readonly Person person;

        public PersonViewModel(Person person)
        {
            this.person = person;
        }

        public string FirstName
        {
            get => person.FirstName;
            set
            {
                if (person.FirstName == value) return;
                person.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => person.LastName;
            set
            {
                if (person.LastName == value) return;
                person.LastName = value;
                OnPropertyChanged();
            }
        }

        // Decorator functionality (augments original object)
        // Project two properties together into, e.g., an edit box.
        public string FullName
        {
            // Trim the string, because sometimes it could be empty
            get => $"{FirstName} {LastName}".Trim();
            set
            {
                if (value == null)
                {
                    FirstName = LastName = null;
                    return;
                }
                var items = value.Split(); // Split into first and last name
                if (items.Length > 0)
                    FirstName = items[0]; // may cause npc
                if (items.Length > 1)
                    LastName = items[1];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
          [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}