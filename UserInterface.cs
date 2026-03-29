using Flashcards.Models;

namespace Flashcards
{
    internal class UserInterface
    {
        DatabaseManager databaseManager = new DatabaseManager();
        Helpers helpers = new Helpers();

        List<Stack> stacks = new List<Stack>();
        List<FlashcardDto> flashcards = new List<FlashcardDto>();

        internal void MainMenu()
        { 
            bool isCloseApp = false;

            while (!isCloseApp)
            {
                Console.Clear();
                Console.WriteLine("---Welcome to Flashcards!---");
                Console.WriteLine("1 - Manage Stacks");
                Console.WriteLine("2 - Manage Flashcards");
                Console.WriteLine("3 - Study Room");
                Console.WriteLine("0 - Exit");
                Console.Write("Please select an option: ");
                
                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        isCloseApp = true;
                        break;
                    case "1":
                        ManageStacks();
                        break;
                    case "2":
                        break;
                    case "3":
                        StudySession();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ReadLine();
                        break;
                }
            }
        
        }

        private void ManageStacks()
        {
            bool isCloseManageStacks = false;

            while (!isCloseManageStacks)
            {
                Console.Clear();
                Console.WriteLine("---Manage Stacks---");
                Console.WriteLine("1 - Create stacks");
                Console.WriteLine("2 - Delete stacks");
                Console.WriteLine("0 - Return to main menu");
                Console.Write("Please select an option: ");

                string input = Console.ReadLine();

                switch (input)
                { 
                    case "0":
                        isCloseManageStacks = true;
                        break;
                    case "1":
                        CreateStack();
                        break;
                    case "2":
                        DeleteStack();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ReadLine();
                        break;
                }

            }
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

            Console.WriteLine("\nWould you like to create another stack? (y/n): ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && input.ToLower() == "y")
                CreateStack();
            else
            {
                Console.WriteLine("\nPress Enter to return to the main menu...");
                Console.ReadLine();
            }
        }

        private void DeleteStack()
        {
            Console.Clear();
            Console.WriteLine("---Delete Stacks---");

            if (GetStacks() > 0)
            {
                Console.WriteLine("\nEnter the stack name to delete (Press 0 to return to the main menu):");
                string stackName = Console.ReadLine();

                if (stackName == "0") return;

                var stack = stacks.FirstOrDefault(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase));

                if (stack == null)
                {
                    Console.WriteLine($"\nStack '{stackName}' does not exist. Please try again.");
                }
                else
                {
                    Console.WriteLine($"\nAre you sure you want to delete stack '{stackName}' and all its flashcards? (y/n): ");
                    string confirmation = Console.ReadLine();

                    if (confirmation.Trim().ToLower() == "y")
                    {
                        databaseManager.DeleteStack(stack.Id);
                        Console.WriteLine($"\nStack '{stackName}' and all its flashcards deleted successfully!");
                    }
                    else
                        Console.WriteLine("\nDeletion cancelled.");
                }
            }

            Console.WriteLine("\nWould you like to delete another stack? (y/n): ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && input.ToLower() == "y")
                DeleteStack();
            else
            {
                Console.WriteLine("\nPress Enter to return to the main menu...");
                Console.ReadLine();
            }
        }

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

        private void ViewFlashcards()
        {
            Console.Clear();
            Console.WriteLine("---View flashcards---");

            GetFlashcards();

            Console.WriteLine("\nPress Enter to return to the main menu...");
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

        

        private void DeleteFlashcards()
        {
            Console.Clear();
            Console.WriteLine("---Delete flashcards---");
            
            GetFlashcards();
            if (flashcards.Count > 0)
            {
                Console.WriteLine("\nEnter the flashcard id to delete: ");
                int flashcardDisplayId = helpers.CheckIntInput();

                var flashcard = flashcards.FirstOrDefault(f => f.DisplayId == flashcardDisplayId);

                if (flashcard == null)
                    Console.WriteLine($"Flashcard with ID '{flashcardDisplayId}' does not exist. Please try again.");
                else
                {
                    Console.WriteLine($"Are you sure you want to delete flashcard with ID '{flashcardDisplayId}'? (y/n): ");
                    string confirmation = Console.ReadLine();

                    if (confirmation.Trim().ToLower() == "y")
                    {
                        databaseManager.DeleteFlashcard(flashcard.Id);
                        Console.WriteLine($"\nFlashcard with ID '{flashcardDisplayId}' deleted successfully!");
                    }
                    else
                        Console.WriteLine("\nDeletion cancelled.");

                }
            }


            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }

        

        private void GetFlashcards()
        {
            if (GetStacks() > 0)
            {
                Console.WriteLine("\nEnter the stack name to view flashcards (Press 0 to return to the main menu):");
                string stackName = Console.ReadLine();

                if (stackName == "0") return;

                var stack = stacks.FirstOrDefault(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase));

                if (stack == null)
                    Console.WriteLine($"Stack '{stackName}' does not exist. Please try again.");
                else
                {
                    flashcards.Clear();
                    flashcards = databaseManager.GetFlashcardsByStackId(stack.Id);

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
