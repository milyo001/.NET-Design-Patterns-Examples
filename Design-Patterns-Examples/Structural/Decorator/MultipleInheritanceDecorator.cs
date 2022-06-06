
namespace DotNetDesignPatternDemos.Structural.Decorator
{
    public interface IBird
    {
        void Fly();
    }

    public interface ILizard
    {
        void Crawl();
    }
    
    public class Bird : IBird
    {
        // Let's have a property weight here 
        public int Weight { get; set; }
        public void Fly()
        {
            Console.WriteLine($"Flying high in the sky with weight: {Weight}");
        }
    }

    public class Lizard : ILizard
    {
        // Also here
        public int Weight { get; set; }
        
        public void Crawl()
        {
            Console.WriteLine($"Crawling with a steady pace with weight: {Weight}");
        }
    }

    public class Dragon : IBird, ILizard
    {
        private Bird bird;
        private Lizard lizard;
        private int weight;


        public Dragon(Bird bird, Lizard lizard)
        {
            this.bird = bird ?? throw new ArgumentNullException(paramName: nameof(bird));
            this.lizard = lizard ?? throw new ArgumentNullException(paramName: nameof(lizard));
        }

        public void Crawl()
        {
            lizard.Crawl();
        }

        public void Fly()
        {
            bird.Fly();
        }

        // This is how you set the weight of the dragon, bird and lizard
        public int Weight 
        {
            get => weight;
            set
            {
                weight = value;
                bird.Weight = value;
                lizard.Weight = value;
            }
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var drake = new Dragon(new Bird(), new Lizard());
            drake.Weight = 1233;
            // Will also set the Bird and Lizard weight property 
            Console.WriteLine($"The dragon weight is: {drake.Weight}");
            drake.Fly();
            drake.Crawl();

        }
    }
}
