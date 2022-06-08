

namespace DotNetDesignPatternDemos.Structural.Proxy.Composite
{
    // Let's say we have a checkbox for all properties with boolean value
    public class MasonrySettings
    {
        public bool? All
        {
            get
            {
                // Make sure that the first element is equal to all of the other elemets in flags array
                if (flags.Skip(1).All(f => f == flags[0]))
                    return flags[0];
                return null;
            }

            set
            {
                if (!value.HasValue) return;
                for (int i = 0; i < flags.Length; ++i)
                    flags[i] = value.Value;
            }
        }

        // We can use a bit array to store the flags
        private bool[] flags = new bool[3];

        public bool Pillars
        {
            get => flags[0];
            set => flags[0] = value;
        }

        public bool Walls
        {
            get => flags[1];
            set => flags[1] = value;
        }

        public bool Floors
        {
            get => flags[2];
            set => flags[2] = value;
        }

    }
}