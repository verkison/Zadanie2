using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.RefModels
{
    [Table("tblCustomerStatus")]
    public class CustomerStatus
    {
        public CustomerStatus()
        {
            this.Customers = new HashSet<Customer>();
        }

        public int ID { get; set; }
        [DisplayName("Status klienta")]
        public string Status { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}