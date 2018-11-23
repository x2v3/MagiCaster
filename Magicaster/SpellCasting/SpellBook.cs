using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;

namespace Magicaster.SpellCasting
{
    public class SpellBook:ReactiveObject
    {
        private ObservableCollection<Spell> spells=new ObservableCollection<Spell>();

        [JsonProperty]
        public ObservableCollection<Spell> Spells
        {
            get => spells;
            set => this.RaiseAndSetIfChanged(ref spells, value);
        }

        public void SaveToFile(string filename)
        {
            var jsonString = JsonConvert.SerializeObject(this);
            File.WriteAllText(filename,jsonString,Encoding.UTF8);
        }

        public static SpellBook LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return new SpellBook();
            }
            var json = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<SpellBook>(json);
        }
    }
}
