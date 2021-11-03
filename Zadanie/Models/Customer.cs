using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.RefModels;

namespace Models
{
    [Table("tblCustomer")]
    public class Customer
    {
        [DisplayName("Dane klienta"), Column(Order = 0)]
        public int ID { get; set; }

        [DisplayName("Imię"), Column(Order = 1), Required(ErrorMessage = "To pole nie może być puste")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko"), Column(Order = 2), Required(ErrorMessage = "To pole nie może być puste")]
        public string LastName { get; set; }

        [DisplayName("Adres"), Column(Order = 3), Required(ErrorMessage = "To pole nie może być puste")]
        public string Address { get; set; }


        [DisplayName("Status klienta"), Column(Order = 4), ForeignKey("CustomerStatus")]
        public int StatusID { get; set; }
        [DisplayName("Status klienta")]
        public virtual CustomerStatus CustomerStatus { get; set; }

        [DisplayName("Emaile")]
        public virtual ICollection<Email> Emails { get; set; }
        [DisplayName("Numery")]
        public virtual ICollection<Phone> Phones { get; set; }
    }
}