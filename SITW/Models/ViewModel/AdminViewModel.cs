using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SITW.Models.ViewModel
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class DetailUserViewModel
    {
        public ApplicationUser user { get; set; }
        public List<string> RolesList { get; set; }
        public List<AssetsViewModel> avList { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public List<AssetsViewModel> avList { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}