using Perfi.Core.SplitPartners;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewSplitParnerCommand
    {
        [Required]
        [MaxLength(SplitPartner.NameMaxLength)]
        public string Name { get; set; }
    }
}
