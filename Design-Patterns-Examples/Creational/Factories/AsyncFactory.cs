namespace Design_Patterns_Examples.Creational.Factories
{
    public class AsyncFactory
    {
        private AsyncFactory()
        {
            // Ok let's say I need to create a new instance of a class but async, something like this:
            // await Task.Delay(1000);
            // My compiler is not letting me do it, so I need to use a factory to solve it.
        }

        // This method purpose is to do async operation when creating the object
        private async Task<AsyncFactory> InitAsync()
        {
            await Task.Delay(1000);
            // Return AsyncFactory object to make it fluent interface
            return this;
        }

        // Factory method
        public static Task<AsyncFactory> CreateAsync()
        {
            var result = new AsyncFactory();
            return result.InitAsync();
        }
    }
    public class Demo
    {
        public static async void Main(string[] args)
        {
            // Note: Object cannot be inizilized like this 
            // var factory = new AsyncFactory();

            // This is the correct way
            var factory = await AsyncFactory.CreateAsync();
        }
    }
}
