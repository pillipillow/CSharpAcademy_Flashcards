namespace Flashcards
{
    internal class UserInterface
    {
        DatabaseManager databaseManager = new DatabaseManager();

        internal void MainMenu()
        { 
            bool isCloseApp = false;

            while (!isCloseApp)
            {
                Console.Clear();
                Console.WriteLine("---Welcome to Flashcards!---");
                Console.WriteLine("1 - View flashcards");
                Console.WriteLine("2 - Create a Stack");
                Console.WriteLine("3 - Create flashcards");
                Console.WriteLine("4 - Delete a Stack");
                Console.WriteLine("5 - Delete flashcards");
                Console.WriteLine("6 - Study Room");
                Console.WriteLine("0 - Exit");
                Console.Write("Please select an option: ");
                
                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        isCloseApp = true;
                        break;
                    case "1":
                        break;
                    case "2":
                        CreateStack();
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ReadLine();
                        break;
                }
            }
        
        }

        internal void CreateStack()
        {
            Console.Clear();
            Console.WriteLine("---Create a new Stack---");
            Console.Write("Enter the name of the new stack: ");
            string name = Console.ReadLine();

            if (name == "0") return;

            Console.WriteLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Stack name cannot be empty. Please try again.");
            }
            else
            { 
                bool isSuccess = databaseManager.CreateStack(name);
                if(isSuccess)
                    Console.WriteLine($"Stack '{name}' created successfully!");
                else
                    Console.WriteLine($"Stack '{name}' already exist.");
            }

            Console.WriteLine("Press Enter to return to the main menu...");
            Console.ReadLine();
        }
    }
}
