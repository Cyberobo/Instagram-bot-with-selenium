using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace InstagramBot
{
    internal class Information
    {
        internal string Name;
        internal string Password;

        public string getName()
        {
            return Name;
        }
        public void setName(string Name)
        {
            this.Name = Name;
        }
        public string getPassword()
        {
            return Password;
        }
        public void setPassword(string Password)
        {
            this.Password = Password;
        }
    }
}
