using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using System.Linq;

namespace ThreaditAPI.Services
{
    public class FilterService
    {     
        const double TICKS_PER_DAY = 8.64e+13;
        
        public List<ThreadFull> SortThreads(Models.ThreadFull[] threads, string sortType)
        {
            switch(sortType)
            {
                case "hot":
                    return threads.OrderByDescending(thread => thread.Stitches.Count / ((DateTime.UtcNow.Ticks - thread.DateCreated.Ticks) / TICKS_PER_DAY)).ThenByDescending(thread => thread.DateCreated).ToList();
                case "top":
                    return threads.OrderByDescending(thread => thread.Stitches.Count).ThenByDescending(thread => thread.DateCreated).ToList();
                case "controversial":
                    return threads.OrderByDescending(thread => thread.Rips.Count / Math.Max(thread.Stitches.Count, 0.1)).ThenByDescending(thread => thread.DateCreated).ToList();               
                case "comments":
                    return threads.OrderByDescending(thread => thread.CommentCount).ThenByDescending(thread => thread.DateCreated).ToList();
                default:
                    return threads.OrderByDescending(thread => thread.DateCreated).ToList();
            }
        }

        public List<ThreadFull> SearchThreads(Models.ThreadFull[] threads, string searchWord)
        {
            return threads.Where(thread => thread.Topic.Contains(searchWord) || thread.Title.Contains(searchWord) || thread.Content.Contains(searchWord)).ToList();
        } 
    }
}