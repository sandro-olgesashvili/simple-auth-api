using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace authAPI
{
	public class ProductUpdateDto
	{
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public string ImageSrc { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string PdfName { get; set; }

        [NotMapped]
        public IFormFile PdfFile { get; set; }

        [NotMapped]
        public string PdfSrc { get; set; }
    }
}

