using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BuenViaje.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetStore.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Введите название товара")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите описание товара")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Please enter price")]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Введите положительную цену")]
        public float PriceBuy { get; set; }

        [Required(ErrorMessage = "Please enter price")]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Введите положительную цену")]
        public float PriceSell { get; set; }

        [ForeignKey("Categories")]
        public int? CategoriesID { get; set; }

        public Categories Categories { get; set; }

        public string ImageId { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; } 

        [NotMapped]
        public bool IsInStock { get; set; } = true;
    }
}
