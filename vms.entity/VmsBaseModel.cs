using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity
{
    public class VmsBaseModel : URF.Core.EF.Trackable.Entity
    {
        [NotMapped]
        public string EncryptedId { get; set; }
    }
}