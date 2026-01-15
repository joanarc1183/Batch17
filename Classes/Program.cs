using System;
using System.Security.Cryptography.X509Certificates;

namespace Classes
{
    public class Cat
    {
        /*
            Naming Conventions:
                public --> PascalCased
                private --> camelCased or _camelCased
        */
        /*
            Default value:
                string = null
                int = 0
                bool = false
                char = '\0'
                int* = null
        */

        /*
            Field
            Fields are initialized first (from their declaration).
        */
        public string Name;     // accessible from  everywhere
        internal string Breed;  // accessible within one assembly
        private int _age = 0;    // only accessible in Cat Class
        protected string Owner; // accessible to this class and its descendants 

        private static int _totalCats = 0;      // private field
                
        public volatile bool _isHungry;     // safe visibility across threads
        public new string ToString;         // hides a member from the object class
        public unsafe int* LivesPointer;    // pointer (advance usage)

        /*
            Expression-Bodied Properties
        */
        public static int TotalCats => _totalCats;      // shared acrross all Cat objects, read-only property
        
        /*
            Declaring Multiple Fields
        */
        public readonly int Legs = 4, Eyes = 2;          // can only be set during creation, cannot be changed anymore
        
        public Cat BestFriend;
        
        // Const --> a fixed value that cannot be changed once efined and is determined at compile-time
        public const string Species = "Felis catus";    // Compile-time constant
        public const int MaxLegs = 4;                   // Example numeric constant
        public const char DefaultInitial = 'C';         // Example char constant
        
        /*
        const vs readonly:
            const:      value fixed at compile-time, always static, same for all objects
            readonly:   value set at runtime (in declaration or constructor), can be instance or static
            const:      only simple types (int, bool, char, string, enum)
            readonly:   any type (object, class, struct)
            const:      cannot change
            readonly:   can set once, then fixed
        */


        // Full Constructor
        // Constructor code runs after fields.
        public Cat(string name, string breed, int age, string owner, bool isHungry)
        {
            // Initialization code
            this.Name = name;
            this.Breed = breed;
            this._age = age;
            this.Owner = owner;
            this._isHungry = isHungry;

            // Increment total cats counter
            _totalCats++;
        }

        // Overloaded constructors with defaults
        public Cat() : this("Unknown", "Stray", 0, "Joan", true) { }
        public Cat(string name) : this(name, "Stray", 0, "Joan", true) { }
        public Cat(string name, string breed) : this(name, breed, 0, "Joan", true) { }
        public Cat(string name, string breed, int age) : this(name, breed, age, "Joan", true) { }
        public Cat(string name, string breed, int age, string owner) : this(name, breed, age, owner, true) { }
        public Cat(string name, int age) : this(name, "Stray", age, "Joan", true) { }

        /*
            Method
            Methods can have various modifiers, including:
                static: Belongs to the class, not an instance.
                Access modifiers (public, internal, private, protected).
                Inheritance modifiers (new, virtual, abstract, override, sealed).
                partial: For partial methods.
                Unmanaged code modifiers (unsafe, extern).
                async: For asynchronous operations.
        */

        // 1. Parameterless Meow
        public void Meow() => Console.WriteLine($"{Name ?? "A cat"} says: Meow!");  // with Expression-Bodied Methods (=>)

        // 2. Meow with int
        public void Meow(int times)
        {
            Console.WriteLine($"{Name} meows {times} times.");
        }

        // 3. Meow with double
        public void Meow(double volume)
        {
            Console.WriteLine($"{Name} meows at {volume} dB.");
        }

        // 4. Meow with int + float
        public void Meow(int times, float pitch)
        {
            Console.WriteLine($"{Name} meows {times} times with pitch {pitch}.");
        }

        // 5. Meow with float + int (order swapped)
        public void Meow(float pitch, int times) => Console.WriteLine($"{Name} meows {times} times with pitch {pitch} (swapped order).");   // with Expression-Bodied Methods (=>)
        
        public void Feed()
        {
            _isHungry = false;
            System.Console.WriteLine($"{Name ?? "The cat"} has been fed.");
        }

        // Method that uses a local method
        public void ShowCubesOfLegs()
        {
            // Local variable
            int legs = 4;

            // Local method inside ShowCubesOfLegs
            static int Cube(int value) => value * value * value; // Local method

            Console.WriteLine($"{Name}'s legs cubed: {Cube(legs)}");
        }

        // Deconstructor definition 
        public void Deconstruct(out string name, out int age)
        {
            name = this.Name;
            age = this._age;
        }

        // Method to make this cat best friends with another cat
        public void Befriend(Cat friend)
        {
            this.BestFriend = friend;     // Current cat's BestFriend points to friend
            friend.BestFriend = this;     // Friend's BestFriend points back to this cat
        }

        /* 
            _age is private field, so this is public property that can access _age safely 
            Benefits:
                External code never accesses private field(_age) directly.
                You can add validation or calculation without changing the public interface.
                Object initializers now work with properties.
        */
        public int Age
        {
            get { return _age; }     // The get accessor runs when property is read 
            set { _age = value; }   // The set accessor runs when property is assigned
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Cat kitty = new Cat("Blackie", 5);   // Calls the constructor

            kitty.Meow();
            kitty.Meow(3);
            kitty.Meow(5.5);
            kitty.Meow(2, 1.2f);
            kitty.Meow(0.8f, 4);

            kitty.Feed();
            kitty.ShowCubesOfLegs();


            // Deconstruction call 
            (string name, int age) = kitty;     
            System.Console.WriteLine($"This cat's name is {name} and age is {age}");


            // Using optional parameters
            /*
                + Quick initialization for simple classes.
                + Fewer constructor overloads needed.
                - Changing optional parameters in a public constructor can break binary compatibility for existing code using your library.
            */
            Cat kitty1 = new Cat("Jetty", "Persian", 1);  
            kitty1.Meow();

            // Using object initializer
            /*
                + Works well with immutable objects (init-only properties).
                + Changing or adding properties doesn’t break backward compatibility in public libraries.
                + Useful for complex initialization (e.g., when creating many optional fields).
                - Can’t run logic in the constructor itself for some fields (like counters).
                - Requires properties to be public or init.
            */
            Cat kitty2 = new Cat { Name = "Athy", Breed = "Tabby", _isHungry = true, Age = 10};     
            // age and owner are not accessible due to their protection level
            // so we need public property to access age and owner safely, for this example use field age only
            kitty2.Meow();

            // Parameterized constructor + object initializer
            Cat kitty3 = new Cat("Whitley", "Siamese", 3, "Alice") { Name = "Whitely", _isHungry = false};    // this cat's name will be Whitely not Whitley
            kitty3.Meow();

            System.Console.WriteLine($"Total cats created: {Cat.TotalCats}");

            // The this Reference
            kitty1.Befriend(kitty2);
            System.Console.WriteLine($"{kitty1.Name}'s best friend is {kitty1.BestFriend.Name}"); 
            System.Console.WriteLine($"{kitty2.Name}'s best friend is {kitty2.BestFriend.Name}");

            // Implicit Parameterless Constructor
            Bird birdy = new Bird();
            System.Console.WriteLine($"My name is {birdy.Name} and my fur is {birdy.Color}.");
        
            // Nonpublic Constructor
            // Fish f1 = new Fish("Goldfish", 1); // Error: constructor is private
            Fish gold = Fish.CreateGoldfish();
            Fish shark = Fish.CreateShark();

            System.Console.WriteLine($"{gold.Species}, {gold.Age} year(s) old"); 
            System.Console.WriteLine($"{shark.Species}, {shark.Age} year(s) old"); 
        
            // Init-property
            Dog doggy = new Dog("Beatrice", "Mixed"){ Color = "Black" };
            doggy.Name = "Bellas";      // ✅ can change after creation
            // doggy.Color = "White";   // ❌ Error: cannot change after creation
            System.Console.WriteLine($"{doggy.Name}, {doggy.Breed}, Age {doggy.Age}, Color {doggy.Color}");
        
            // Indexers
            Sentence s = new Sentence();
            System.Console.WriteLine("Before: " + s[2]);
            s[2] = "the";
            System.Console.WriteLine("After: " + s[2]);
            System.Console.WriteLine(s[^2]);                        // last element using Index
            System.Console.WriteLine(string.Join(" ", s[2..]));     // range from index 2 to end, exclusive
        }
    }

    class Sentence
    {
        string[] words = "Sherlock Holmes The Final Problem".Split();
        
        public string this[int i]{      // Indexer definition
            get { return words[i]; }
            set { words[i] = value; }
        }
        public string this[Index index] => words[index];        // Indexer for System.Index
        public string[] this[Range range] => words[range];      // Indexer for System.Range
    }
    
    public class Dog
    {
        // Automatic properties 
        public string Name { get; set; }    // can change anytime
        public string Breed { get; set; }   // can change anytime
        
        // Automatic properties with default values, Property Initializers Example
        public int Age { get; set; } = 0;

        // Init-only property
        public string Color {get; init;} = "Brown";     // can only set when creating the object
        
        public Dog(string name, string breed)
        {
            Name = name;
            Breed = breed;
        }


    }

    // Implicit Parameterless Constructor
    public class Bird
    {
        public string Name = "Purr";
        public string Color = "Yellow";
    }

    // Nonpublic Constructor
    public class Fish
    {

        public string Species { get; private set; }

        // CLR Property Implementation basically explains how C# properties work internally in the runtime
        // When there is a property like this:
        public int Age { get; private set; }
        // The compiler actually creates two methods in the background
        // public int get_Age() { return _age; }
        // public void set_Age(int value) { _age = value; }

        // Private constructor
        private Fish(string species, int age)
        {
            Species = species;
            Age = age;
        }

        // Public static factory method
        public static Fish CreateGoldfish()
        {
            return new Fish("Goldfish", 1);
        }

        public static Fish CreateShark()
        {
            return new Fish("Shark", 5);
        }
    }
}