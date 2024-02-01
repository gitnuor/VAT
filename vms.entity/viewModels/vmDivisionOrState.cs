using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class vmDivisionOrState
{
    public vmDivisionOrState()
    {
        CountryList = new List<CustomSelectListItem>();
    }
    public int DivisionOrStateId { get; set; }
    public string EncryptedId { get; set; }
    [DisplayName("Country")]
    [Required(ErrorMessage = "Country is required")]
    public int CountryId { get; set; }
    public IEnumerable<CustomSelectListItem> CountryList { get; set; }
    [DisplayName("Name")]
    [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
    [Required(ErrorMessage = "Name is required")]

    public string DivisionOrStateName { get; set; }
    [DisplayName("Short Name")]
    [StringLength(20, ErrorMessage = "Short Name cannot be longer than 20 characters.")]
    public string DivisionOrStateShortName { get; set; }
}