using FoolProof.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GammaltGlimmer.Models
{
    public class Item
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID är obligatoriskt fält.")]
        [RegularExpression(@"^[A-Z]{3}\d{6}$", ErrorMessage = "ID måste bestå av tre bokstäver och sex siffror (Exempelvis: TTT111111)")]
        public string ItemId { get; set; }
        [DisplayName("Namn")]
        [Required(ErrorMessage = "Namn är obligatoriskt fält.")]
        public string Name { get; set; }
        [DisplayName("Beskrivning")]
        [Required(ErrorMessage = "Beskrivning är obligatoriskt fält.")]
        public string Description { get; set; }
        [DisplayName("Utgivningsår")]
        [Required(ErrorMessage = "Utgivningsår är obligatoriskt fält.")]
        public int ReleaseYear { get; set; }
        [DisplayName("Utgångspris")]
        [Required(ErrorMessage = "Utgångspris är obligatoriskt fält.")]
        [GreaterThan("PurchaseCost", ErrorMessage = "Måste vara högre än Inköpskostnaden")]
        public int StartPrice { get; set; }
        [DisplayName("Inköpskostnad")]
        [Required(ErrorMessage = "Inköpskostnad är obligatoriskt fält.")]
        public int PurchaseCost { get; set; }
        public string Status { get; set; }
        [DisplayName("Slutpris")]
        public int FinalPrice { get; set; }
        [Required(ErrorMessage = "Kategori ID är obligatoriskt fält.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [DisplayName("Skapad av")]
        public string CreatedBy { get; set; }
    }
}
