using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class HuntController : Controller
    {
        [HttpGet, Authorize]
        public IEnumerable<Book> Get()
        {
            var currentUser = HttpContext.User;
            int userAge = 0;
            var resultBookList = new Book[] {
              new Book { Author = "Ray Bradbury", Title = "Fahrenheit 451", AgeRestriction = false },
              new Book { Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude", AgeRestriction = false },
              new Book { Author = "George Orwell", Title = "1984", AgeRestriction = false },
              new Book { Author = "Anais Nin", Title = "Delta of Venus", AgeRestriction = true }
            };

            if (currentUser.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                DateTime birthDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth).Value);
                userAge = DateTime.Today.Year - birthDate.Year;
            }

            if (userAge < 18)
            {
                resultBookList = resultBookList.Where(b => !b.AgeRestriction)
                    .ToArray();
            }

            return resultBookList;
        }

        [Route("/EasyGet")]
        [HttpGet]
        public IEnumerable<Hunt> GetWithNoAuth()
        {
            var currentUser = HttpContext.User;
            int userAge = 0;
            var resultBookList = new Hunt[] {
              new Hunt { Author = "Ray Bradbury", Title = "Puzzle 1" },
              new Hunt { Author = "Gabriel García Márquez", Title = "Scavenger Hunt" },

            };

            if (currentUser.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                DateTime birthDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth).Value);
                userAge = DateTime.Today.Year - birthDate.Year;
            }

            return resultBookList;
        }

        public class Book
        {
            public string Author { get; set; }
            public string Title { get; set; }
            public bool AgeRestriction { get; set; }
        }

        public class Hunt
        {
            public string Author { get; set; }
            public string Title { get; set; }

            public DateTime Created { get; set; }
            public DateTime LastModified { get; set; }
            public string Question { get; set; }
            public string Answer { get; set; }
        }
    }
}