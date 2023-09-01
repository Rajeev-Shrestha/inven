using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HrevertCRM.AuthServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [StringLength(15)]
        public string FirstName { get; set; }
        [StringLength(15)]
        public string MiddleName { get; set; }
        [StringLength(15)]
        public string LastName { get; set; }
        public int Gender { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
      

        public int CompanyId { get; set; }
        public bool Active { get; set; } = true;
        public byte[] Version { get; set; }
        public bool WebActive { get; set; }

        public DateTime? DateModified { get; set; }
        public DateTime DateCreated { get; set; }
       
    

     
        
        public DateTime AccountExpires { get; set; }
        //public ICollection<EmailSender> EmailSenderModified { get; set; }
        //public ICollection<EmailSender> EmailSenderCreated { get; set; }
        //public ICollection<PurchaseOrder> PurchaseOrderByPurchaseRep { get; set; }
    }
}
