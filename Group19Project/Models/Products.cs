using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group19Project.Models
{
    public class Products
    {
    
    public int ProductID { get; set; }

    [Required]
    [Display(Name = "Product Name")]
    public string ProductName { get; set; }

    [Required]
    [Display(Name = "Product Price")]
    public string ProductPrice { get; set; }

    [Required]
    [Display(Name = "Product Category")]
    public string ProductCategory { get; set; }

    [Required]
    [Display(Name = "Product Description")]
    public string ProductDescription { get; set; }
    }
}
