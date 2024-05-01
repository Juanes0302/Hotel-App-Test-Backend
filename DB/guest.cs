using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DB
{
    public class guest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_guest { get; set; }
        public string guest_fullname { get; set; }
        public string guest_dni { get; set; }
        public int guest_phone_number { get; set; }
        public DateTimeConverter admission_date { get; set; }
        public DateTimeConverter departure_date { get; set; }
        public int id_room { get; set; }
    }
}
