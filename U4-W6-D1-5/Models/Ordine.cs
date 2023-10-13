namespace U4_W6_D1_5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordine")]
    public partial class Ordine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordine()
        {
            Dettaglio = new HashSet<Dettaglio>();
        }

        [Key]
        public int IdOrdine { get; set; }

        public int? IdCliente { get; set; }

        public DateTime? DataOrdine { get; set; }

        [StringLength(50)]
        public string StatoOrdine { get; set; }

        [StringLength(50)]
        public string IndirizzoOrdine { get; set; }
        
        [StringLength(50)]
        public string NoteOrdine { get; set; }

        public virtual Cliente Cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dettaglio> Dettaglio { get; set; }
    }
}
