using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectHotels.Views
{
    public class SearchPageMasterMenuItem
    {
        public SearchPageMasterMenuItem()
        {
            TargetType = typeof(SearchPageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        string iconSourcePath;
        public string IconSourcePath
        {
            get { return iconSourcePath; }
            set
            {
                iconSourcePath = value;
                if(Uri.TryCreate(value, UriKind.Absolute, out Uri uri))
                    IconSource = ImageSource.FromUri(uri);
            }
         }
        public ImageSource IconSource { get; set; }
        public Type TargetType { get; set; }
    }
}