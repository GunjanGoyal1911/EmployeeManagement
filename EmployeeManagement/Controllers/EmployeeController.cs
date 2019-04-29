using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        //private object employeeList;

        public IConfiguration Configuration { get; }
        public EmployeeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
            
        public IActionResult EmployeeDetails()
        {
            List<Employee> employeeList = new List<Employee>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                string sql = "Select * From tblEmployee"; SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Employee employee = new Employee();
                        employee.Id = Convert.ToInt32(dataReader["Id"]);
                        employee.Name = Convert.ToString(dataReader["Name"]);
                        employee.Skills = Convert.ToString(dataReader["Skills"]);
                        employee.Gender = Convert.ToString(dataReader["Gender"]);
                        employee.Age = Convert.ToInt32(dataReader["Age"]);
                        employee.Salary = Convert.ToDecimal(dataReader["Salary"]);
                        employee.JoiningDate = Convert.ToDateTime(dataReader["JoiningDate"]);
                        employeeList.Add(employee);
                    }
                }
                connection.Close();
            }
            return View(employeeList);
        }

        public IActionResult CreateEmployee()
        {
            return View();
        }        

        [HttpPost]
        [ActionName("CreateEmployee")]
        public IActionResult Create_Post(Employee employee)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = $"Insert Into tblEmployee (Name, Skills, Gender, Age, Salary, JoiningDate) Values ('{employee.Name}', '{employee.Skills}','{employee.Gender}','{employee.Age}','{employee.Salary}','{DateTime.Now}')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    return RedirectToAction("EmployeeDetails");
                }
            }
            else
                return View();
        }
       
        public IActionResult UpdateEmployee(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            Employee employee = new Employee();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * From tblEmployee Where Id='{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        employee.Id = Convert.ToInt32(dataReader["Id"]);
                        employee.Name = Convert.ToString(dataReader["Name"]);
                        employee.Skills = Convert.ToString(dataReader["Skills"]);
                        employee.Gender = Convert.ToString(dataReader["Gender"]);
                        employee.Age = Convert.ToInt32(dataReader["Age"]);
                        employee.Salary = Convert.ToDecimal(dataReader["Salary"]);
                        employee.JoiningDate = Convert.ToDateTime(dataReader["JoiningDate"]);
                    }
                }
                connection.Close();
            }
            return View(employee);
        }
        [HttpPost]
        [ActionName("UpdateEmployee")]
        public IActionResult Update_Post(Employee employee)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update tblEmployee SET Name='{employee.Name}', Skills='{employee.Skills}', Gender='{employee.Gender}',  Age='{employee.Age}', Salary='{employee.Salary}', JoiningDate='{employee.JoiningDate}' Where Id='{employee.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return RedirectToAction("EmployeeDetails");
        }

        [HttpPost]
        public IActionResult DeleteEmployee(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Delete From tblEmployee Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ViewBag.Result = "Operation got error:" + ex.Message;
                    }
                    connection.Close();
                }
            }
            return RedirectToAction("EmployeeDetails");
        }

    }
}
