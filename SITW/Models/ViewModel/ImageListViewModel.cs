using GoogleCloudSamples.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.ViewModel
{
    public class ImageListViewModel
    {
        public string path { get; set; }
        public List<StorageObject> ImageList { get; set; }
    }
}