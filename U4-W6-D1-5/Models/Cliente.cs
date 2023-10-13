namespace U4_W6_D1_5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cliente")]
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            Ordine = new HashSet<Ordine>();
        }

        [Key]
        public int IdCliente { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(50)]
        public string NomeCliente { get; set; }

        [Display(Name = "Cognome")]
        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(50)]
        public string CognomeCliente { get; set; }

        [Display(Name = "Indirizzo")]
        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(50)]
        public string IndirizzoCliente { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(50)]
        public string Password { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ordine> Ordine { get; set; }
    }
}
