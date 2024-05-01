using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class rooms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_room {  get; set; }
        public  string room_identity { get; set; }
        public  string room_type { get; set; }
        public int bedroom_numbers { get; set; }
        public int bed_numbers { get; set; }
        public int number_bathrooms { get; set; }
        public bool status {  get; set; }



    }
}
