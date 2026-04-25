using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
   public class StorageLocation
    {
        public int StorageLocationId { get; set; }
        //tank belirleme
        public string TankNo { get; set; }
        //dikey seviye
        public string CanisterNo { get; set; }
        //cubuk adresi
        public string StrawNo { get; set; }
        //raf adresi
        public string? Shelf { get; set; }
        //her materyal tek konumda
        public int SampleId { get; set; }
        // Navigation
        public Sample Sample { get; set; } = null!;

    }
}
