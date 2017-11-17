using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xPDB.Utility
{
    class KeyboardMessageFilter : IMessageFilter
    {
        const int WM_KEYDOWN = 0x100;
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                switch ((int)m.WParam)
                {
                    case (int)Keys.Escape:
                        Application.Exit();
                        return true;
                }
            }

            return false;
        }
    }
}
