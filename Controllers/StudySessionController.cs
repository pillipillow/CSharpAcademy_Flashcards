using Flashcards.Models;

namespace Flashcards.Controllers
{
    internal class StudySessionController
    {
        DatabaseManager databaseManager;
        StackController stackController;

        List<FlashcardDto> flashcards = new List<FlashcardDto>();
        int score = 0;

        public StudySessionController(DatabaseManager databaseManager, StackController stackController) 
        { 
            this.databaseManager = databaseManager;
            this.stackController = stackController;
        }

        internal void PrepareStudySession()
        {
            Stack currentStack = null;

            Console.Clear();
            Console.WriteLine("---Start a study session---");

            if (stackController.GetStacks() > 0)
            {
                Console.WriteLine("\nEnter the stack name to start a study session (Press 0 to return to the main menu):");
                string stackName = Console.ReadLine();

                if (stackName == "0") return;

                currentStack = stackController.GetStackByName(stackName);

                if (currentStack != null)
                {
                    flashcards.Clear();
                    flashcards = databaseManager.GetFlashcardsByStackId(currentStack.Id);

                    if (flashcards.Count == 0)
                    {
                        Console.WriteLine("\nNo flashcards found in this stack.");
                    }
                    else
                    {
                        StartStudySession(currentStack);
                    }
                }
            }

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }

        internal void StartStudySession(Stack currentStack)
        {
            Console.WriteLine("\nStarting study session...");
            score = 0;

            //Shuffle and take only the first 10 flashcards for the session, or all if there are less than 10
            var studySelection = flashcards.OrderBy(f => Guid.NewGuid()).Take(10).ToList();

            foreach (var flashcard in studySelection)
            {
                Console.Clear();
                Console.WriteLine($"---Studying: {currentStack.Name}---");
                Console.WriteLine($"Question: {flashcard.Question}");
                Console.Write("What is the answer?: ");
                string answer = Console.ReadLine();

                if (answer == flashcard.Answer)
                {
                    Console.WriteLine("\nCorrect!");
                    score++;
                }
                else
                    Console.WriteLine($"\nThe answer is {flashcard.Answer}.");

                Console.ReadLine();
            }

            Console.Clear();
            Console.WriteLine($"---Studying: {currentStack.Name}---");
            Console.WriteLine("Session completed!");
            Console.WriteLine($"Your score: {score}/{studySelection.Count} ({(double)score / studySelection.Count * 100:0.00}%)");
            databaseManager.CreateStudySession(currentStack.Id, score, studySelection.Count);

            Console.WriteLine($"\nWould you like to study {currentStack.Name} again? y/n");
            string selectInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(selectInput) && selectInput.ToLower() == "y")
                StartStudySession(currentStack);

        }


    }
}
