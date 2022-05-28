using static System.Console;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace DotNetDesignPatternDemos.Creational.Prototype
{
    // Extention methods that work on any type of object
    public static class ExtensionMethods
    {
        // T is the type being copied
        public static T DeepCopy<T>(this T self)
        {
            MemoryStream stream = new MemoryStream();
            // Everytime the we use the BinaryFormatter we also need to use the atrubute [Serializable]
            BinaryFormatter formatter = new BinaryFormatter();
            
            // Inicialize the memory stream with a serialized version of self object
            formatter.Serialize(stream, self);
            
            // Now we need to get it out of the stream, with offset 0 from the beginning
            stream.Seek(0, SeekOrigin.Begin);
            
            // And ofcourse make a copy of the object
            object copy = formatter.Deserialize(stream);
            
            // Then close the stream
            stream.Close();
            
            // And return the copy
            return (T)copy;
        }


        // XML serialization is slower than binary serialization
        public static T DeepCopyXml<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer s = new XmlSerializer(typeof(T));
                s.Serialize(ms, self);
                ms.Position = 0;
                // Deserialize whatever is in the stream,cast it to T and return it 
                return (T)s.Deserialize(ms);
            }
        }
    }

    [Serializable] // This is, unfortunately, required
    public class Animal
    {
        public string Name;
        public uint Weight;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Weight)}: {Weight}";
        }
    }

    public static class CopyThroughSerialization
    {
        static void Main()
        {
            Animal elephant = new Animal { Name = "Topcho", Weight = 25 };
            
            Animal elephantCopy = elephant.DeepCopy();
            //Aniaml elephantCopy = elephant.DeepCopy(); // Crashes without [Serializable]
            
            Animal elephantCopyXml = elephant.DeepCopyXml();

            elephantCopyXml.Name = "Topcho2";
            
            WriteLine(elephant);
            WriteLine(elephantCopy);
            WriteLine(elephantCopyXml);
        }
    }
}
