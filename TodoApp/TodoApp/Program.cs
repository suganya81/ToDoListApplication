using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



namespace TodoApp
{
    class Program
    {
        static List<TaskItem> tasks = new List<TaskItem>();

        static void Main(string[] args)
        {
            LoadTasks();                            //This will  Load tasks from a file if it exists

            bool quit = false;
            while (!quit)
            {
                Console.Clear();
                Console.WriteLine("Todo List Application\n");

                DisplayTasks();

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Edit Task");
                Console.WriteLine("3. Mark Task as Done");
                Console.WriteLine("4. Remove Task");
                Console.WriteLine("5. Save and Quit");
                Console.WriteLine("6. Quit without Saving");

                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        EditTask();
                        break;
                    case "3":
                        MarkTaskAsDone();
                        break;
                    case "4":
                        RemoveTask();
                        break;
                    case "5":
                        SaveTasks();
                        quit = true;
                        break;
                    case "6":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        // Implementing functions to add, edit, mark as done, remove, save, and load tasks
        

        static void DisplayTasks()
        {
            if (tasks.Any())
            {
                Console.WriteLine("Tasks:");
                int count = 1;
                foreach (var task in tasks)
                {
                    Console.WriteLine($"{count}. {task.Title} (Due: {task.DueDate.ToString("yyyy-MM-dd")}, Status: {(task.IsDone ? "Done" : "Not Done")})");
                    count++;
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }
        }

        static void LoadTasks()
        {
            if (File.Exists("tasks.txt"))
            {
                // Load tasks from the file and populate the 'tasks' list
                // Use a suitable file format (CSV) to store task data
                try
                {
                   if (File.Exists("tasks.txt"))
                   {
                      using (StreamReader reader = new StreamReader("tasks.txt"))
                      {
                           string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                string[] taskData = line.Split(',');
                                if (taskData.Length == 3)
                                {
                                    TaskItem loadedTask = new TaskItem
                                    {
                                        Title = taskData[0],
                                        DueDate = DateTime.ParseExact(taskData[1], "yyyy-MM-dd", null),
                                        IsDone = bool.Parse(taskData[2])
                                    };
                                    tasks.Add(loadedTask);
                                }
                            }
                        }
                        Console.WriteLine("Tasks loaded from 'tasks.txt' successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No 'tasks.txt' file found. No tasks loaded.");
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"An error occurred while loading tasks: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }

                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static void SaveTasks()
        {
            // This will Save tasks to a file (e.g., "tasks.txt")
            // Use a suitable file format (e.g., JSON or CSV) to store task data
            try
            {
                using (StreamWriter writer = new StreamWriter("tasks.txt"))
                {
                    foreach (var task in tasks)
                    {
                        // Format each task as a line in the text file
                        string taskLine = $"{task.Title},{task.DueDate.ToString("yyyy-MM-dd")},{task.IsDone}";
                        writer.WriteLine(taskLine);
                    }
                }

                Console.WriteLine("\nTasks saved to 'tasks.txt' successfully!");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while saving tasks: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        // Implement the remaining functions (AddTask, EditTask, MarkTaskAsDone, RemoveTask) as needed
        // ...
        static void AddTask()
        {
            Console.Clear();
            Console.WriteLine("Add Task\n");

            // Create a new TaskItem object
            TaskItem newTask = new TaskItem();

            // Prompt the user for task details
            Console.Write("Enter task title: ");
            newTask.Title = Console.ReadLine();

            Console.Write("Enter due date (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
            {
                newTask.DueDate = dueDate;
            }
            else
            {
                Console.WriteLine("Invalid date format. Task not added.");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return; // Exit the function without adding the task
            }

            // By default, a newly added task is marked as not done
            newTask.IsDone = false;

            // Add the new task to the list of tasks
            tasks.Add(newTask);

            Console.WriteLine("\nTask added successfully!");
            Console.Write("Press any key to continue...");
            Console.ReadKey();

        }

        static void EditTask()
        {
            Console.Clear();
            Console.WriteLine("Edit Task\n");

            DisplayTasks();

            Console.Write("\nEnter the number of the task to edit: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                // Get the task to edit (adjust for 0-based index)
                TaskItem taskToEdit = tasks[taskNumber - 1];

                Console.Write("Enter new task title (leave empty to keep the current title): ");
                string newTitle = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    taskToEdit.Title = newTitle;
                }

                Console.Write("Enter new due date (yyyy-MM-dd) (leave empty to keep the current due date): ");
                string newDueDateInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDueDateInput) && DateTime.TryParse(newDueDateInput, out DateTime newDueDate))
                {
                    taskToEdit.DueDate = newDueDate;
                }

                Console.WriteLine("\nTask edited successfully!");
            }
            else
            {
                Console.WriteLine("Invalid task number. Task not edited.");
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();

        }

        static void RemoveTask()
        {
                Console.Clear();
                Console.WriteLine("Remove Task\n");

                DisplayTasks();

                Console.Write("\nEnter the number of the task to remove: ");
                if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
                {
                    // Get the task to remove (adjust for 0-based index)
                    TaskItem taskToRemove = tasks[taskNumber - 1];

                    // Confirm task removal
                    Console.Write($"Are you sure you want to remove '{taskToRemove.Title}'? (yes/no): ");
                    string confirmation = Console.ReadLine().ToLower();

                    if (confirmation == "yes" || confirmation == "y")
                    {
                        tasks.Remove(taskToRemove);
                        Console.WriteLine("\nTask removed successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nTask not removed.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid task number. Task not removed.");
                }

                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }


        

        static void MarkTaskAsDone()
        {
            Console.Clear();
            Console.WriteLine("Mark Task as Done\n");

            DisplayTasks();

            Console.Write("\nEnter the number of the task to mark as done: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                // Get the task to mark as done (adjust for 0-based index)
                TaskItem taskToMarkDone = tasks[taskNumber - 1];

                taskToMarkDone.IsDone = true;

                Console.WriteLine("\nTask marked as done!");
            }
            else
            {
                Console.WriteLine("Invalid task number. Task not marked as done.");
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();

        }

    }

}
