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
                        ManageFlashcards();
                        break;
                    case "3":
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

            Console.WriteLine("Enter the name of the new stack (Press 0 to return to the manage stacks menu): ");
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
                Console.WriteLine("\nPress Enter to return to the manage stacks menu...");
                Console.ReadLine();
            }
        }

        private void DeleteStack()
        {
            Console.Clear();
            Console.WriteLine("---Delete Stacks---");

            if (GetStacks() > 0)
            {
                Console.WriteLine("\nEnter the stack name to delete (Press 0 to return to the manage stacks menu):");
                string stackName = Console.ReadLine();

                if (stackName == "0") return;

                Stack stack = GetStackByName(stackName);
                if (stack != null)
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

                Console.WriteLine("\nWould you like to delete another stack? (y/n): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.ToLower() == "y")
                    DeleteStack();
                else
                {
                    Console.WriteLine("\nPress Enter to return to the manage stack menu...");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("\nPress Enter to return to the manage stack menu...");
                Console.ReadLine();
            }
        }

        private int GetStacks()
        {
            stacks.Clear();
            stacks = databaseManager.GetStacks();

            if (stacks.Count == 0)
                Console.WriteLine("No stacks found. Please create a stack first.");
            else
            {
                foreach (var stack in stacks)
                {
                    Console.WriteLine($"- {stack.Name}");
                }
            }

            return stacks.Count;
        }

        private Stack GetStackByName(string stackName)
        {
            var stack = stacks.FirstOrDefault(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase));

            if (stack == null)
                Console.WriteLine($"\nStack '{stackName}' does not exist. Please try again.");

            return stack;
        }

        private void ManageFlashcards()
        {
            Stack currentStack = null;

            Console.Clear();
            Console.WriteLine("---Manage Flashcards---");

            if (GetStacks() > 0)
            {
                Console.WriteLine("\nEnter the stack name to manage (Press 0 to return to the main menu):");
                string stackName = Console.ReadLine();

                if (stackName == "0") return;

                currentStack = GetStackByName(stackName);

                if (currentStack == null)
                {
                    Console.WriteLine("\nPress Enter to return to the main menu...");
                    Console.ReadLine();
                }
            }
            else 
            { 
                Console.WriteLine("\nPress Enter to return to the main menu...");
                Console.ReadLine();
            }

            if (currentStack != null)
            { 
                bool isCloseManageFlashcards = false;
                while (!isCloseManageFlashcards)
                { 
                    Console.Clear();
                    Console.WriteLine($"---Manage Flashcards for Stack: {currentStack.Name}---");
                    Console.WriteLine("1 - View flashcards");
                    Console.WriteLine("2 - Create flashcards");
                    Console.WriteLine("3 - Update flashcards");
                    Console.WriteLine("4 - Delete flashcards");
                    Console.WriteLine("0 - Return to main menu");
                    Console.Write("Please select an option: ");

                    string input = Console.ReadLine();

                    switch(input)
                    {
                        case "0":
                            isCloseManageFlashcards = true;
                            break;
                        case "1":
                            ViewFlashcards(currentStack);
                            break;
                        case "2":
                            CreateFlashcards(currentStack);
                            break;
                        case "3":
                            UpdateFlashcards(currentStack);
                            break;
                        case "4":
                            DeleteFlashcards(currentStack);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ReadLine();
                            break;
                    }

                }

            }
        }

        private void ViewFlashcards(Stack stack)
        {
            GetFlashcards(stack);

            Console.WriteLine("\nPress Enter to return to the manage flashcard menu...");
            Console.ReadLine();
        }

        private void CreateFlashcards(Stack stack)
        {
            Console.Clear();
            Console.WriteLine($"---Create flashcards in Stack: {stack.Name}---");

            Console.WriteLine("Enter the question for the flashcard: ");
            string question = Console.ReadLine();

            if (question == "0") return;

            Console.WriteLine("\nEnter the answer for the flashcard: ");
            string answer = Console.ReadLine(); 
            
            if (answer == "0") return;

            databaseManager.CreateFlashcard(stack.Id, question, answer);
            Console.WriteLine("\nFlashcard created successfully!");

            Console.Write("Would you like to create another flashcard? (y/n): ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && input.ToLower() == "y")
                CreateFlashcards(stack);
            else
            {
                Console.WriteLine("\nPress Enter to return to the manage flashcard menu...");
                Console.ReadLine();
            }
        }

        private void UpdateFlashcards(Stack stack)
        {
            Console.Clear();
            Console.WriteLine("---Update flashcards---");
            GetFlashcards(stack);
            if (flashcards.Count > 0)
            {
                Console.WriteLine("\nEnter the flashcard id to update: ");
                int flashcardDisplayId = helpers.CheckIntInput();

                if (flashcardDisplayId == 0) return;

                var flashcard = flashcards.FirstOrDefault(f => f.DisplayId == flashcardDisplayId);

                if (flashcard == null)
                    Console.WriteLine($"Flashcard with ID '{flashcardDisplayId}' does not exist. Please try again.");
                else
                {
                    Console.WriteLine($"\nCurrent question: {flashcard.Question}");
                    Console.WriteLine("Enter the new question for the flashcard (Press Enter to keep the current question): ");
                    string newQuestion = Console.ReadLine();
                    if(!string.IsNullOrWhiteSpace(newQuestion))
                        flashcard.Question = newQuestion;

                    Console.WriteLine($"\nCurrent answer: {flashcard.Answer}");
                    Console.WriteLine("Enter the new answer for the flashcard (Press Enter to keep the current answer): ");
                    string newAnswer = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newAnswer))
                        flashcard.Answer = newAnswer;

                    databaseManager.UpdateFlashcard(flashcard.Id, flashcard.Question, flashcard.Answer);
                    Console.WriteLine($"\nFlashcard with ID '{flashcardDisplayId}' updated successfully!");
                }

                Console.WriteLine("\nWould you like to update another flashcard? (y/n): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.ToLower() == "y")
                    UpdateFlashcards(stack);
                else
                {
                    Console.WriteLine("\nPress Enter to return to the manage flashcard menu...");
                    Console.ReadLine();
                }
            }
            else
            {                 
                Console.WriteLine("\nPress Enter to return to the manage flashcard menu...");
                Console.ReadLine();
            }
        }


        private void DeleteFlashcards(Stack stack)
        {
            Console.Clear();
            Console.WriteLine("---Delete flashcards---");
            
            GetFlashcards(stack);
            if (flashcards.Count > 0)
            {
                Console.WriteLine("\nEnter the flashcard id to delete: ");
                int flashcardDisplayId = helpers.CheckIntInput();

                if (flashcardDisplayId == 0) return;

                var flashcard = flashcards.FirstOrDefault(f => f.DisplayId == flashcardDisplayId);

                if (flashcard == null)
                    Console.WriteLine($"Flashcard with ID '{flashcardDisplayId}' does not exist. Please try again.");
                else
                {
                    Console.WriteLine($"\nAre you sure you want to delete flashcard with ID '{flashcardDisplayId}'? (y/n): ");
                    string confirmation = Console.ReadLine();

                    if (confirmation.Trim().ToLower() == "y")
                    {
                        databaseManager.DeleteFlashcard(flashcard.Id);
                        Console.WriteLine($"\nFlashcard with ID '{flashcardDisplayId}' deleted successfully!");
                    }
                    else
                        Console.WriteLine("\nDeletion cancelled.");

                }

                Console.WriteLine("\nWould you like to delete another flashcard? (y/n): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.ToLower() == "y")
                    DeleteFlashcards(stack);
                else
                {
                    Console.WriteLine("\nPress Enter to return to the manage flashcard menu...");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("\nPress Enter to return to the manage flashcard menu...");
                Console.ReadLine();
            }
        }

        

        private void GetFlashcards(Stack stack)
        {
            flashcards.Clear();
            flashcards = databaseManager.GetFlashcardsByStackId(stack.Id);

            Console.Clear();
            Console.WriteLine($"---Flashcards in Stack: {stack.Name}---");
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
