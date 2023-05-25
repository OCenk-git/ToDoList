using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Beschreibung:")]
        [MaxLength(80)]
        public string Beschreibung { get; set; }
        public string Erledigt { get; set; }
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Zieldatum:")]
        public DateTime EndDatum { get; set; }
        public DateTime StartDatum { get; set; }
    }
}
