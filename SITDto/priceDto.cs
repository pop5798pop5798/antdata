using SITDto.function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITDto
{
    public class priceDto
    {
        public int id { get; set; }
        public Nullable<double> priceMoney { get; set; }
        public Nullable<int> apiCount { get; set; }
        public Nullable<int> apisId { get; set; }
        public Nullable<int> unitSn { get; set; }
        public Nullable<byte> valid { get; set; }
    }
}
