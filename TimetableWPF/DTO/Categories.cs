using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableWPF.DTO
{
    public class Categories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryColorHex { get; set; }
        [NotMapped]
        public Color CategoryColor
        {
            get
            {
                return ColorTranslator.FromHtml(CategoryColorHex);
            }
            set
            {
                CategoryColorHex = ColorTranslator.ToHtml(value);
            }
        }

        public static Categories createCategory(string categoryName, Color color)
        {
            Categories category = new Categories();
            category.CategoryId = Guid.NewGuid();
            category.CategoryName = categoryName;
            category.CategoryColor = color;

            return category;
        }
    }
}
