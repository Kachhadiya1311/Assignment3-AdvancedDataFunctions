using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

partial class Program
{
    static void Main()
    {
        using (var context = new DataContext())
        {
            var result = context.Blogs
                .Include(b => b.BlogType)
                .Include(b => b.Posts)
                    .ThenInclude(p => p.PostType)
                .Include(b => b.Posts)
                    .ThenInclude(p => p.User)
                .Where(b => b.BlogType.IsActive)
                .Select(b => new
                {
                    b.Url,
                    b.IsPublic,
                    BlogTypeName = b.BlogType.Name,
                    Posts = b.Posts
                        .Where(p => p.PostType.IsActive)
                        .GroupBy(p => new { p.User.UserName, p.User.Email })
                        .Select(g => new
                        {
                            g.Key.UserName,
                            g.Key.Email,
                            PostCount = g.Count(),
                            PostTypeNames = g.Select(p => p.PostType.Name).Distinct()
                        })
                        .OrderBy(g => g.UserName)
                })
                .ToList();

            foreach (var blog in result)
            {
                Console.WriteLine($"Blog URL: {blog.Url}");
                Console.WriteLine($"Is Public: {blog.IsPublic}");
                Console.WriteLine($"Blog Type: {blog.BlogTypeName}");

                foreach (var postGroup in blog.Posts)
                {
                    Console.WriteLine($"  User: {postGroup.UserName}");
                    Console.WriteLine($"  Email: {postGroup.Email}");
                    Console.WriteLine($"  Total Posts: {postGroup.PostCount}");
                    Console.WriteLine($"  Post Types: {string.Join(", ", postGroup.PostTypeNames)}");
                }

                Console.WriteLine("--------------------------");
            }
        }
    }
}


//Check that the entities have the proper properties
//Make sure project references are correct
//Create Migration and update the database -> make sure you run your commands against the right project in the solution
//Create a new instance of the data context
//Create a new instance of DataCreator and pass your email into the constructor
//Create a variable and populate it with output of the DataCreator GetData method
//Save the Users, BlogTypes, PostTypes, Blogs, and Posts that are in the output of GetData to the data context and Save the Changes

//In this assignment, you need to return all of the blogs and the associated posts belonging to each blog. 
//Also, and most important, you need to display supporting data names and statuses.
//For instance, in the Blog Entity, we use a foreign key to point to the Blog type.
//Therefore, you will need to include the Blog type and return the BlogType.Name property - not the foreign key value.This is also true to Post types and statuses.

//There are many ways to produce the results and, therefore, multiple solutions will be acceptable.
//There are, of course, efficient ways and not-so-efficient ways but this assignment is primarily focused on the end result.

//Once the query is completed and working correctly, output the results of the query to the screen.
//Below is a minimum list of the data fields that must be returned:

//Blog URL
//Blog IsPublic
//Blog Type Name
//Post User Name
//Post User Email Address
//The total number of blog posts the User has posted
//Post Type Name
//Exclude any post where the Post Type is not Active.

//Exclude any post where the Blog Type is not Active.

//Finally, sort the output using the user name - in alphabetical order(A to Z).


