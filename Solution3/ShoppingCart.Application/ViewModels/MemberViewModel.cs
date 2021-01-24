using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class MemberViewModel
    {
        [Required(ErrorMessage = "Please input a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First name required!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name required!")]
        public string LastName { get; set; }
    }
}
