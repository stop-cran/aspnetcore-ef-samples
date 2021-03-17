using System.ComponentModel.DataAnnotations;

namespace EfSamples.Model
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Login { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
