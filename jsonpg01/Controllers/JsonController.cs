using System.Collections.Generic;
using System.Linq;
using jsonpg01.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace jsonpg01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class JsonController : ControllerBase
    {
        private  static List<Post> _context = null;
        private static string MyData = "";

        public JsonController()
        {
            if(_context == null)
            {
                MyData = MyJSON.RecieveJSON("https://jsonplaceholder.typicode.com/posts");
                _context = JsonConvert.DeserializeObject<List<Post>>(MyData);
            }
        }
        // http://localhost:5000/api/json
        // Gets and returns all of the json data
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetPosts()
        {
            return Content(JsonConvert.SerializeObject(_context));
        }

        // http://localhost:5000/api/json/3
        // Gets and returns only a selected item e.g. id
        [HttpGet("{id}")]
        public ActionResult<Post> GetPost(int id)
        {
            Post post = _context.FirstOrDefault(x => x.id == id);

            //if(post.id != id)
            if (post == null)
            {
                return BadRequest();
            }
            return Content(JsonConvert.SerializeObject(post));
        }

        // Creates a post
        [HttpPost]
        public ActionResult CreatePost([FromBody] Post post)
        {
            //Create variable id, find the last id and +1 from that (i++)
            int id = _context[_context.Count-1].id + 1;

            Post newPost = new Post();
            newPost.id = id;
            newPost.userId = post.userId;
            newPost.title = post.title;
            newPost.body = post.body;

            _context.Add(newPost);

            string output = JsonConvert.SerializeObject(newPost);

            //Updating local collection
            string json = JsonConvert.SerializeObject(_context);
            MyData = json;

            return CreatedAtAction(nameof(GetPosts), output);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePost([FromBody] Post post, int id)
        {
            Post selectedPost = _context.First(x => x.id == id);
            if(selectedPost.id != id)
            {
                return BadRequest();
            }
            selectedPost.userId = post.userId;
            selectedPost.title = post.title;
            selectedPost.body = post.body;

            List<Post> posts = _context.Where(all => all.id == id).ToList();
            posts[0] = selectedPost;
            string output = JsonConvert.SerializeObject(selectedPost);              
            // Updating
            string json = JsonConvert.SerializeObject(_context);
            MyData = json;
            return CreatedAtAction(nameof(GetPosts), selectedPost);
        }
        
        [HttpDelete("{id}")]
        public ActionResult DeletePost(int id)
        {
            Post selectedPost = _context.First(x => x.id == id);
            _context.Remove(selectedPost);
            // Updating
            string json = JsonConvert.SerializeObject(_context);
            MyData = json;
            return Content($"POST {id} has been removed");
        } 
    }
}