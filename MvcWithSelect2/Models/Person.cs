namespace MvcWithSelect2.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Person
    {
        [DisplayName("Code")]
        [Required(ErrorMessage = "{0}: Madatory field.")]
        public int Code { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "{0}: Madatory field.")]
        [StringLength(150, ErrorMessage = "{0}: Maximum length {1} chars.")]
        public string Name { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "{0}: Madatory field.")]
        [StringLength(2, ErrorMessage = "{0}: Maximum length {1} chars.")]
        [MvcWithSelect2.Infrastructure.Mvc.Select2Attribute(dataPlaceholder: "Choose Country", dataOption: "CountryDescription")]
        public string Country { get; set; }
        public string CountryDescription { get; set; }

        [DisplayName("Countries")]
        [Required(ErrorMessage = "{0}: Madatory field.")]
        [StringLength(1000, ErrorMessage = "{0}: Maximum length {1} chars.")]
        [MvcWithSelect2.Infrastructure.Mvc.Select2Attribute(dataPlaceholder: "Choose Country")]
        public string Countries { get; set; }
    }
}