using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magicaster.SpellCasting
{
    public class Spell
    {
        public Spell()
        {
        }

        public Spell(string words, string keys) : this()
        {
            Words = words;
            KeyList = keys;
        }

        public string Words { get; set; }
        public string KeyList { get; set; }
    }
}
