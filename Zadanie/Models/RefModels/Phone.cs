using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.RefModels
{
    [Table("tblPhone")]
    public class Phone
    {
        public int ID { get; set; }
        [DisplayName("Numer telefonu"), Required(ErrorMessage = "To pole nie może być puste"), RegularExpression("^[0-9]{9,9}$", ErrorMessage = "Tylko cyfry lub numer telefonu nie ma 9 cyfr")]
        public string PhoneContent { get; set; }

        public virtual Customer Customer { get; set; }
    }
}