using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace authAPI
{
	public class FileDto
	{
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}

