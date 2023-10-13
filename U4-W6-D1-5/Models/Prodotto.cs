namespace U4_W6_D1_5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prodotto")]
    public partial class Prodotto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prodotto()
        {
            Dettaglio = new HashSet<Dettaglio>();
        }

        [Key]
        public int IdProdotto { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [Display(Name = "Nome Prodotto")]
        [StringLength(50)]
        public string NomeProdotto { get; set; }

        [Display(Name = "Foto Prodotto")]
        [StringLength(50)]
        public string FotoProdotto { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [Display(Name = "Prezzo")]
        [Column(TypeName = "money")]
        public decimal? PrezzoProdotto { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [Display(Name = "Tempo di consegna")]
        public int? TempoConsegna { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        public string Ingredienti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dettaglio> Dettaglio { get; set; }
    }
}
