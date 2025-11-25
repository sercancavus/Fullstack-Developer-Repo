using System.ComponentModel.DataAnnotations;

namespace App.Mvc.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        [MinLength(3, ErrorMessage = "En az 3 karakter olmalı.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fiyat alanı zorunludur.")]
        [Range(0,double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
    }
}
