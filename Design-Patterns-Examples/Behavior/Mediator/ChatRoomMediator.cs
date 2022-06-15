using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Mediator.ChatRoom
{
    public class Person
    {
        public string Name;
        // The actual mediator chat room
        public ChatRoom Room;
        
        // Representing all messages that person has recieved
        private List<string> chatLog = new List<string>();

        public Person(string name)
        {
            Name = name;
        }

        // Recieve a message from a sender
        public void Receive(string sender, string message)
        {
            string s = $"{sender}: '{message}'";
            WriteLine($"[{Name}'s chat session] {s}");
            chatLog.Add(s);
        }

        // Say something in the chatroom
        public void Say(string message)
        {
            Room.Broadcast(Name, message);
        }

        // Specify to who do you want to send the message 
        public void PrivateMessage(string who, string message)
        {
            Room.Message(Name, who, message);
        }
    }

    public class ChatRoom
    {
        // The list of people in the chat room
        private List<Person> people = new List<Person>();

        // Mediator pattern, broadcast to all people in the room
        public void Broadcast(string source, string message)
        {
            // Go through every single person, this is where the chat room is mediating the messages, like a central message hub
            foreach (var p in people)
                if (p.Name != source)
                    p.Receive(source, message);
        }

        // Person can join the room
        public void Join(Person p)
        {
            string joinMsg = $"{p.Name} joins the chat";
            Broadcast("room", joinMsg);

            p.Room = this;
            people.Add(p);
        }

        // Source is the person sending the message, destination is the reciever of the message and message is the message obviously
        public void Message(string source, string destination, string message)
        {
            // FirstOrDefault can retrun null so we use the ? (safe operator) and then receive the message
            people.FirstOrDefault(p => p.Name == destination)?.Receive(source, message);
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var room = new ChatRoom();

            var john = new Person("John");
            var jane = new Person("Jane");

            room.Join(john);
            room.Join(jane);

            john.Say("hi room");
            jane.Say("oh, hey john");

            var simon = new Person("Simon");
            room.Join(simon);
            simon.Say("hi everyone!");

            jane.PrivateMessage("Simon", "glad you could join us!");
        }
    }
}