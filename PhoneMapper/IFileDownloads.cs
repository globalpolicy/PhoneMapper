using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneMapper
{
    public interface IFileDownloads
    {
        void FileDownloaded(string contents);
        void FileNotDownloaded();

        void NoInternet();
    }
}
