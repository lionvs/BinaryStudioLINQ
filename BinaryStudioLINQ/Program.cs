using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudioLINQ
{
    class Program
    {
       static List<User> users = new List<User>();
       static List<Test> tests = new List<Test>();
       static List<TestWork> works = new List<TestWork>();
         enum Citys
       {
           Lviv,
           Kyev,
           London,
           VirginiaBeach,
           NY,
       }
        static void GenerateData()
        {
            for (int i = 0; i < 20; i++)
            {
                users.Add(new User() { Name = "User" + i, Age = 20 + i, Email = "myemail" + i + "@gmail.com", University = "SomeUniver" + i, UserCategory = (Category)(i % 6), City = ((Citys)(i % 5)).ToString() });
            }
            Random rd = new Random();
            for (int i = 0; i < 20; i++)
            {
                List<Question> questions = new List<Question>();
                for (int j = 0; j < 10; j++)
                {
                    questions.Add(new Question() { Text = "Some Question text " + i + j, QuestionCategory = (Category)(j% 6) });
                }
                tests.Add(new Test() { MarkNeeded = 15, Questions = questions, MaxTime = TimeSpan.FromMinutes(45), TestCategory = (Category)(i % 6) ,TestName = "This Test #" + i});
            }
            for (int i = 0; i < 40; i++)
            {
                int random = rd.Next(10, 20);
                int random1 = rd.Next(25, 50);

                works.Add(new TestWork() { CurrTest = tests[i%20], Result = random , TestUser = users[i%20], TimeUsed = TimeSpan.FromMinutes(random1) });
            }
        }
        static void Main(string[] args)
        {
            GenerateData();
            var passTest = from work in works
                where work.Result >= work.CurrTest.MarkNeeded
                select work.TestUser;
            var passTestAndTime = from work in works
                           where work.Result >= work.CurrTest.MarkNeeded && work.TimeUsed <= work.CurrTest.MaxTime
                           select work.TestUser;
            var passTestAndNoTime = from work in works
                                  where work.Result >= work.CurrTest.MarkNeeded && work.TimeUsed > work.CurrTest.MaxTime
                                  select work.TestUser;

            Console.WriteLine("Pass test:\n");

            foreach (var user in passTest)
            {
                Console.WriteLine(user.Name);
            }
            Console.WriteLine("\nPass test and enough time:\n");

            foreach (var user in passTestAndTime)
            {
                Console.WriteLine(user.Name);
            }
            Console.WriteLine("\nSort by city\n");

            var studentsCity = users.Select(user =>user.City).Distinct();
            foreach (var city in studentsCity)
            {
                var usersFromCity = from user in users where user.City == city select user;
                Console.WriteLine("\n" + city + "\n");
                foreach (var user in usersFromCity)
                {
                    Console.WriteLine(user.Name);
                }
            }
            Console.WriteLine("\nSuccessfull in city\n");

            foreach (var city in studentsCity)
            {
                var usersFromCity = from work in works
                                    where work.Result >= work.CurrTest.MarkNeeded &&  work.TestUser.City == city
                                    select work.TestUser;
                Console.WriteLine("\n" + city + "\n");
                foreach (var user in usersFromCity)
                {
                    Console.WriteLine(user.Name);
                }
            }

            Console.WriteLine("\nStudent results\n");

            foreach (var student in users)
            {
                var result1 = works.Where(work => work.TestUser ==student ).GroupBy(work => work.CurrTest.TestCategory)
    .Select(
        g => new
        {
            Key = g.Key,
            Result = g.Sum(s => s.Result),
            //Time = g.Sum(s => s.TimeUsed),
            Category = g.First().CurrTest.TestCategory
        });
                //var result = from work in works
                //             where work.TestUser == student
                //            // group work by work.TestUser into q
                //             select new
                //             {
                //                 res = work.Result,
                //                 time = work.TimeUsed,
                //                 percent = (double)work.Result / (double)work.CurrTest.MarkNeeded,
                //                 cat = work.CurrTest.TestCategory
                //             }; 
                foreach (var stud in result1)
                {
                    Console.WriteLine("{2}: \t Mark: {0} , Time: {1} ", stud.Result, stud.Category,student.Name);
                }
            }
            
        }
    }
}
