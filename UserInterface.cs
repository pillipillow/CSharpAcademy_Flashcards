using Flashcards.Models;

namespace Flashcards
{
    internal class UserInterface
    {
        DatabaseManager databaseManager = new DatabaseManager();
        Helpers helpers = new Helpers();

        List<Stack> stacks = new List<Stack>();

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
                        ViewFlashcards();
                        break;
                    case "2":
                        CreateStack();
                        break;
                    case "3":
                        CreateFlashcards();
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

        private void ViewFlashcards()
        {
            Console.Clear();
            Console.WriteLine("---View flashcards---");

            GetFlashcards();

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }

        private void CreateStack()
        {
            Console.Clear();
            Console.WriteLine("---Create a new Stack---");

            Console.WriteLine("Enter the name of the new stack (Press 0 to return to the main menu): ");
            string name = Console.ReadLine();

            if (name == "0") return;

            Console.WriteLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Stack name cannot be empty. Please try again.");
            }
            else
            {
                if (databaseManager.CheckStackExist(name))
                    Console.WriteLine($"Stack '{name}' already exists. Please choose a different name.");
                else
                {
                    databaseManager.CreateStack(name);
                    Console.WriteLine($"Stack '{name}' created successfully!");
                }
            }

            Console.WriteLine("Press Enter to return to the main menu...");
            Console.ReadLine();
        }

        private void CreateFlashcards()
        {
            Console.Clear();
            Console.WriteLine("---Create flashcards---");

            if (GetStacks() > 0)
            {
                Console.WriteLine("\nEnter the stack name to start creating a flashcard (Press 0 to return to the main menu):");
                string stackName = Console.ReadLine();

                if (stackName == "0") return;

                var stack = stacks.FirstOrDefault(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase));

                if (stack == null)
                {
                    Console.WriteLine($"Stack '{stackName}' does not exist. Please try again.");
                }
                else
                {
                    Console.WriteLine("\nEnter the question for the flashcard: ");
                    string question = Console.ReadLine();
                    Console.WriteLine("\nEnter the answer for the flashcard: ");
                    string answer = Console.ReadLine();

                    databaseManager.CreateFlashcard(stack.Id, question, answer);
                    Console.WriteLine("\nFlashcard created successfully!");
                }
            }

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }

        private void GetStacks()
        private int GetStacks()
        { 
            stacks.Clear();
            stacks = databaseManager.GetStacks();

            if (stacks.Count == 0)
            {
                Console.WriteLine("No stacks found. Please create a stack first.");
            }
            else
            {
                foreach (var stack in stacks)
                {
                    Console.WriteLine($"- {stack.Name}");
                }
            }

            return stacks.Count;
        }

        private void GetFlashcards()
        {
            if (GetStacks() > 0)
            {
                Console.WriteLine("\nEnter the stack ID to view flashcards (Press 0 to return to the main menu):");
                string stackName = Console.ReadLine();

                if (stackName == "0") return;

                var stack = stacks.FirstOrDefault(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase));

                if (stack == null)
                    Console.WriteLine($"Stack '{stackName}' does not exist. Please try again.");
                else
                {
                    var flashcards = databaseManager.GetFlashcardsByStackId(stack.Id);
                    Console.WriteLine();
                    if (flashcards.Count == 0)
                    {
                        Console.WriteLine("No flashcards found in this stack.");
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0,-7} {1,-15} {2,-15}", "ID", "Question", "Answer"));
                        foreach (var flashcard in flashcards)
                        {
                            Console.WriteLine(string.Format("{0,-7} {1,-15} {2,-15}", flashcard.DisplayId, flashcard.Question, flashcard.Answer));

                        }
                    }
                }
            }
        }
    }
}
