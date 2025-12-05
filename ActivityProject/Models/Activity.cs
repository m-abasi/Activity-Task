using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ActivityProject.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Activity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public bool IsActive { get; set; }

    }
}
