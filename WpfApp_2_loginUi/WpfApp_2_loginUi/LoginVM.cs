using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_2_loginUi
{
    public class LoginVM
    {
        private LoginModel _LoginM;
        public LoginModel LoginM
        {
            get { return _LoginM; }
            set { _LoginM = value; }
        }

    }
}
