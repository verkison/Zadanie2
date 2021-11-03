using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.RefModels
{
    [Table("tblEmail")]
    public class Email
    {
        public int ID { get; set; }
        [DisplayName("Adres email"), Required(ErrorMessage = "To pole nie może być puste"), EmailAddress(ErrorMessage = "Niepoprawny format emaila")]
        public string EmailContent { get; set; }

        public virtual Customer Customer { get; set; }
    }
}