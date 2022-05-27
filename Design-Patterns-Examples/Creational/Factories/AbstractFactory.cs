namespace RefactoringGuru.DesignPatterns.AbstractFactory.Conceptual
{
    // When to use it?

    // We use it when we have a requirement to create a set of related objects, or dependent objects which must 
    // be used together as families of objects.Concrete classes should be decoupled from clients

    // The example contains some interfaces and classes splitted in 5 logical parts.
    // 01. AbstractFactory
    // 02. ConcreteFactory
    // 03. AbstractProduct
    // 04. ConcreteProduct
    // 05. Client

    // 01. AbstractFactory: This is an interface for operations which is used to create abstract product.
    interface IMobilePhone
    {
        ISmartPhone GetSmartPhone();
        INormalPhone GetNormalPhone();
    }

    // 02. ConcreteFactory: This is a class which implements the AbstractFactory interface operations to create concrete products.
    class Nokia : IMobilePhone
    {
        // Smartphones are Products of type A
        public ISmartPhone GetSmartPhone()
        {
            return new NokiaPixel();
        }

        // Normal phones are Products of type B
        public INormalPhone GetNormalPhone()
        {
            return new Nokia1600();
        }
    }

    // And another example with samsung. Also implements the AbstractFactory interface operations to create concrete products.
    class Samsung : IMobilePhone
    {
        // Same thing smartphones are Products of type A
        public ISmartPhone GetSmartPhone()
        {
            return new SamsungGalaxy();
        }

        // And the normal phones are Products of type B
        public INormalPhone GetNormalPhone()
        {
            return new SamsungPro();
        }
    }

    // 03. AbstractProducts: This declares an interfaces for a type of product object (INormalPhone and ISmartPhone).
    interface INormalPhone
    {
        string GetModelDetails();
    }

    interface ISmartPhone
    {
        string GetModelDetails();
    }

    // 04. Products: This defines a product object to be created by the corresponding concrete factory also implements the AbstractProduct interface    
    // Now let's have a class for product nokia which inherits ISmartPhone (ProductA1)
    class NokiaPixel : ISmartPhone
    {
        public string GetModelDetails()
        {
            return "Model: Nokia Pixel\nRAM: 3GB\nCamera: 8MP\n";
        }
    }

    // And then for another smartphone (ProductA2)
    class SamsungGalaxy : ISmartPhone
    {
        public string GetModelDetails()
        {
            return "Model: Samsung Galaxy\nRAM: 2GB\nCamera: 13MP\n";
        }
    }

    // And then for a normal phone (ProductB1)
    class Nokia1600 : INormalPhone
    {
        public string GetModelDetails()
        {
            return "Model: Nokia 1600\nRAM: NA\nCamera: NA\n";
        }
    }

    // And another normal phone (ProductB2)
    class SamsungPro : INormalPhone
    {
        public string GetModelDetails()
        {
            return "Model: Samsung 2001 Pro \nRAM: NA\nCamera: NA\n";
        }
    }

    // 05. Client This is a class which uses AbstractFactory and AbstractProduct interfaces to create a family of related objects.
    class MobileClient
    {
        ISmartPhone smartPhone;
        INormalPhone normalPhone;

        public MobileClient(IMobilePhone factory)
        {
            smartPhone = factory.GetSmartPhone();
            normalPhone = factory.GetNormalPhone();
        }

        public string GetSmartPhoneModelDetails()
        {
            return smartPhone.GetModelDetails();
        }

        public string GetNormalPhoneModelDetails()
        {
            return normalPhone.GetModelDetails();
        }
    }
    class Program
    {
        // Test and debug here
        static void Main()
        {
            IMobilePhone nokiaMobilePhone = new Nokia();
            MobileClient nokiaClient = new MobileClient(nokiaMobilePhone);

            Console.WriteLine("********* NOKIA **********");
            Console.WriteLine(nokiaClient.GetSmartPhoneModelDetails());
            Console.WriteLine(nokiaClient.GetNormalPhoneModelDetails());

            IMobilePhone samsungMobilePhone = new Samsung();
            MobileClient samsungClient = new MobileClient(samsungMobilePhone);

            Console.WriteLine("******* SAMSUNG **********");
            Console.WriteLine(samsungClient.GetSmartPhoneModelDetails());
            Console.WriteLine(samsungClient.GetNormalPhoneModelDetails());

            Console.ReadKey();
        }
    }
}