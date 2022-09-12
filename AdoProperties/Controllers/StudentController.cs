using AdoProperties.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdoProperties.Repository;

namespace AdoProperties.Controllers
{
    public class StudentController : Controller
    {
        private readonly IAdoDatabase _adoDatabase;
        public StudentController(IAdoDatabase adoDatabase)
        {
            _adoDatabase = adoDatabase;
        }
        public IActionResult Index()
        {
            return View(_adoDatabase.SqlRead<Student>("select * from Student"));
        }
        [HttpGet]
        public IActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Insert(Student model)
        {
            _adoDatabase.SqlCommand(string.Format("insert into Student(Name,Email) values ('{0}','{1}')", 
                model.Name, model.Email));
            return Redirect("~/Student/Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            SqlConnection connection = new SqlConnection("Data Source=BS-483;Database=" + "UniversityDb" + ";Integrated Security=True");
            connection.Open();
            string query = "select * from Student where id = " + id;
            SqlCommand comand = new SqlCommand(
            query, connection);
            var reading = comand.ExecuteReader();
            Student students = new Student();
            while (reading.Read())
            {
                var model = new Student();
                model.Id = Convert.ToInt32(reading[0]);
                model.Name = reading[1].ToString();
                model.Email = reading[2].ToString();
                students = model;
            }
            return View(students);
        }
        [HttpPost]
        public IActionResult Edit(int id, Student model)
        {
            _adoDatabase.SqlCommand(string.Format("update Student set Name='{0}' , Email='{1}' where Id = {2}", 
                model.Name, model.Email, id));
            return Redirect("~/Student/Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            _adoDatabase.SqlCommand(string.Format("delete from Student where Id = {0}", 
                id));
            return Redirect("~/Student/Index");
        }
        public IActionResult Details(int id)
        {
            return View(_adoDatabase.SqlReadScaler<Student>(string.Format("select * from Student where Id = {0}",
                id)));
        }
    }
}
