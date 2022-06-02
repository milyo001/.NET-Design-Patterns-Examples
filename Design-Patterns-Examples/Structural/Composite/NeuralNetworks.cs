using System.Collections;
using System.Collections.ObjectModel;

// In this example we are going to emulate a neural network (like in machine learning)
namespace DotNetDesignPatternDemos.Structural.Composite.NeuralNetworks
{
    // We cannot use a base class 

    public static class ExtensionMethods
    {
        // This is how you connect the neurons to one another
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other)) return;

            foreach (var from in self)
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
        }
    }
    // You have lots of neurons connected to one another 
    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        public List<Neuron> In, Out;

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this;
        }
    }

    // The collection of neurons, you can use also a list or any collection type
    public class NeuronLayer : Collection<Neuron>
    {

    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();
            var layer1 = new NeuronLayer();
            var layer2 = new NeuronLayer();

            neuron1.ConnectTo(neuron2);
            neuron1.ConnectTo(layer1);
            layer1.ConnectTo(layer2);
        }
    }
}
