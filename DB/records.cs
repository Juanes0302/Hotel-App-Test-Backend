using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class records
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id_record { get; set; }
        public string record_fullname { get; set; }
        public string record_dni { get; set; }
        public int record_phone_number { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime record_admission_date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime record_departure_date { get; set; }
        public string record_room { get; set; }
        public int? id_guest { get; set; }
        public int? id_room { get; set;}


    }
}
