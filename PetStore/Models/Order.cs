using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetStore.Models
{
    public class Order
    {
        #region properties

        [BindNever]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        [BindNever]
        public bool Shipped { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ФИО")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ФИО")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ФИО")]
        public string Patronymic { get; set; }

        public string Code { get; set; }

        public string IssuingAuthority { get; set; }

        public string Email { get; set; }

        public bool HavePassport { get; set; } = false;

        #endregion
    }
}
