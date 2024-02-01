using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class vmDistrictOrCity
{
    public vmDistrictOrCity()
    {
        DivisionList = new List<CustomSelectListItem>();
    }

    public int DistrictOrCityId { get; set; }
    public string EncryptedId { get; set; }
    [DisplayName("Division")]
    [Required(ErrorMessage = "Division is required")]
    public int DivisionOrStateId { get; set; }
    public IEnumerable<CustomSelectListItem> DivisionList { get; set; }
    [DisplayName("Name")]
    [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
    [Required(ErrorMessage = "Name is required")]

    public string DistrictOrCityName { get; set; }
    [DisplayName("Short Name")]
    [StringLength(20, ErrorMessage = "Short Name cannot be longer than 20 characters.")]
    public string DistrictOrCityShortName { get; set; }
}