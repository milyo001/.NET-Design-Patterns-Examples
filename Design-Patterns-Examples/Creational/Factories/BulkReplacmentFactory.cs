using System.Text;

namespace BulkReplacement
{
    // A simple interface with two properties
    public interface ITheme
    {
        string TextColor { get; }
        string BgrColor { get; }
    }

    // A simple class that implements the ITheme interface
    class LightTheme : ITheme
    {
        public string TextColor => "black";
        public string BgrColor => "white";
    }

    // A simple class that implements the ITheme interface with reversed properties values
    class DarkTheme : ITheme
    {
        public string TextColor => "white";
        public string BgrColor => "dark gray";
    }

    // Here is the factory class that creates the themes
    public class TrackingThemeFactory
    {
        // When you use a weak reference, the application can still obtain a strong reference to the object,
        // which prevents it from being collected. Store all themes in a weak reference dictionary.
        private readonly List<WeakReference<ITheme>> themes = new();

        public ITheme CreateTheme(bool dark)
        {
            ITheme theme = dark ? new DarkTheme() : new LightTheme();
            themes.Add(new WeakReference<ITheme>(theme));
            return theme;
        }

        // Returns a string  of all themes
        public string Info
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var reference in themes)
                {
                    if (reference.TryGetTarget(out var theme))
                    {
                        bool dark = theme is DarkTheme;
                        sb.Append(dark ? "Dark" : "Light")
                          .AppendLine(" theme");
                    }
                }
                return sb.ToString();
            }
        }
    }

    public class ReplaceableThemeFactory
    {
        // Slightly different from the previous one
        private readonly List<WeakReference<Ref<ITheme>>> themes
          = new();

        private ITheme createThemeImpl(bool dark)
        {
            return dark ? new DarkTheme() : new LightTheme();
        }

        public Ref<ITheme> CreateTheme(bool dark)
        {
            var r = new Ref<ITheme>(createThemeImpl(dark));
            // Add wrapper, using shorthand new() syntax
            themes.Add(new(r));
            return r;
        }

        // Bulk replacement method, replace all themes in the List collection with a given value
        public void ReplaceTheme(bool dark)
        {
            // Loop through all WeakReferences
            foreach (var wr in themes)
            {
                // Tries to get the reference in the List collection
                // Out keyword is telling our compiler that we are passing parameter by reference
                if (wr.TryGetTarget(out var reference))
                {
                    reference.Value = createThemeImpl(dark);
                }
            }
        }
    }

    // This class works only with reference types only. This class can perform bulk replacement of themes.
    public class Ref<T> where T : class
    {
        public T Value;

        public Ref(T value)
        {
            Value = value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new TrackingThemeFactory();
            var theme = factory.CreateTheme(true);
            var theme2 = factory.CreateTheme(false);
            Console.WriteLine(factory.Info);
            // Output:
            // Dark theme
            // Light theme


            // Bulk replacement factory
            var factory2 = new ReplaceableThemeFactory();
            var magicTheme = factory2.CreateTheme(true);
            Console.WriteLine(magicTheme.Value.BgrColor); 
            factory2.ReplaceTheme(false);
            Console.WriteLine(magicTheme.Value.BgrColor);
            // Output of background color depending on the theme:
            // Dark gray (See DarkTheme class)
            // White (See LightTheme class)

        }
    }
}