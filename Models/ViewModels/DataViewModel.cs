using Microsoft.AspNetCore.Mvc.Rendering;
using MVCBlog.Models.Domain;

namespace MVCBlog.Models.ViewModels
{
    public class DataViewModel
    {
        public List<Tag>? Tags { get; set; }
        public List<BlogPost>? Posts { get; set; }
    }
}
