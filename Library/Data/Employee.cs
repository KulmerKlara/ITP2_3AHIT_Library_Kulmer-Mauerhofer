namespace Library.Data;
    public class Employee : User
    {
        public int EmployeeId { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }

        public void AddBook(){}

        public void Demote(string newPosition, decimal newSalary)
        {
            Position = newPosition;
            Salary = newSalary;
            Console.WriteLine($"Employee demoted to {newPosition} with a salary of {newSalary}.");
        }
    }