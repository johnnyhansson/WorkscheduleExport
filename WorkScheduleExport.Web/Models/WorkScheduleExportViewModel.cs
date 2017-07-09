using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WorkScheduleExport.Web.Models
{
    public class WorkScheduleExportViewModel
    {
        public string Email { get; set; }

        [Required]
        public IFormFile SchemaFile { get; set; }

        public bool ContainsEmail => !string.IsNullOrWhiteSpace(Email);
    }
}
