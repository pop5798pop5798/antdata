using SITDto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.ViewModel
{
    public class BetsByUserID
    {       
        public double assets { get; set; }
        public List<BetListDto> BetList { get; set; }
    }
}