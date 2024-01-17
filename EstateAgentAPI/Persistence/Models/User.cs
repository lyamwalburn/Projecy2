using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgentAPI.WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
