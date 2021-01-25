
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class ProductViewModel
    {
       
        public Guid Id { get; set; }

        [Required(ErrorMessage="Please input a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a price")]
        [Range(typeof(double),"0","99999",ErrorMessage ="Must be a number between 0 and 99999")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

     
        public CategoryViewModel Category { get; set; }

        public string ImageUrl { get; set; }

        public bool Disable { get; set; }

    }
}
