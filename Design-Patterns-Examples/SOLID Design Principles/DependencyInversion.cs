namespace Design-Patterns-Examples.SOLID Design Principles
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using static System.Console;

    // Not following the Dependency Inversion Principle  

    // The salary calculator will calculate the salary of a employee by given hoursWorked and hourlyRate.
    class SalaryCalculator
    {
        public float CalculateSalary(int hoursWorked, float hourlyRate) => hoursWorked * hourlyRate;
    }

    // The employee class will have a property called GetSalary executing CalculateSalary property in SalaryCalculator.
    // These classes do not follow the “Dependency Inversion Principle” as the higher-level
    // class EmployeeDetails is directly depending upon the lower level SalaryCalculator class.
    public class EmployeeDetails
    {
        public int HoursWorked { get; set; }
        public int HourlyRate { get; set; }
        public float GetSalary()
        {
            var salaryCalculator = new SalaryCalculator();
            return salaryCalculator.CalculateSalary(HoursWorked, HourlyRate);
        }
    }


    // Following the Dependency Inversion Principle  

    // Let's Create an interface ISalaryCalculator and then we have a class called SalaryCalculatorModified
    // that implements this interface
    public interface ISalaryCalculator
    {
        float CalculateSalary(int hoursWorked, float hourlyRate);
    }
    
    public class SalaryCalculatorModified : ISalaryCalculator
    {
        public float CalculateSalary(int hoursWorked, float hourlyRate) => hoursWorked * hourlyRate;
    }
    
    public class EmployeeDetailsModified
    {
    
        // We are using dependancy injection to provive _salaryCalculator with data
        private readonly ISalaryCalculator _salaryCalculator;
    
        public int HoursWorked { get; set; }
        public int HourlyRate { get; set; }
    
        public EmployeeDetailsModified(ISalaryCalculator salaryCalculator)
        {
            _salaryCalculator = salaryCalculator;
        }

        // In the higher-level class EmployeeDetailsModified, we only depend upon the ISalaryCalculator interface
        // and not the concrete class SalaryCalculator. Hence, when we create the EmployeeDetailsModified class, we specify
        // the abstraction implementation to use.
        public float GetSalary()
        {
        // In addition to this, the details of the CalculateSalary function are hidden from the EmployeeDetailsModified
        // class and any changes to this function will not affect the interface being used.
        return _salaryCalculator.CalculateSalary(HoursWorked, HourlyRate);
        }
    }


    //var employeeDetailsModified = new EmployeeDetailsModified(new SalaryCalculatorModified());
    //employeeDetailsModified.HourlyRate = 50;  
    //    employeeDetailsModified.HoursWorked = 150;  
    //    Console.WriteLine($"The Total Pay is {employeeDetailsModified.GetSalary()}");  


}
