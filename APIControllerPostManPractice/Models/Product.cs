using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIControllerPostManPractice.Models
{
    [Table("product")]
    public class Product
    {

        [Key]
        [Column("ID", TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar(40)")]
        public string Name { get; set; }

        [Column(TypeName = "int(10)")]
        public int Quantity { get; set; }

        [Column(TypeName = "tinyint(1)")]
        public bool Discontinued { get; set; }


    }
}


//namespace ProductInformation.Models
//{
//    [Table("Product")]
//    public class Product
//    {
//        [Key]
//        [Column(TypeName = "int(10)")]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int ID { get; set; }

//        // [Column("ProductName", TypeName = "varchar(30)")]
//        [Column(TypeName = "varchar(30)")]
//        [Required]
//        public string Name { get; set; }

//        [Column(TypeName = "int(10)")]
//        public int CategoryID { get; set; }

//        [ForeignKey(nameof(CategoryID))]
//        [InverseProperty(nameof(Models.Category.Products))]
//        public virtual Category Category { get; set; }
//    }
//}
