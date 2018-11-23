using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magicaster.SpellCasting
{
    class Caster
    {

        private SpeechRecognitionEngine sre;
        private bool isSpellCasting;

        public List<Spell> Spells { get; set; } = new List<Spell>();
        public event EventHandler<CastSpellEventAgrs> SpellCasted;

        public void InitRecognition()
        {
            var t = Task.Run(() =>
            {
                sre?.RecognizeAsyncStop();
                sre?.Dispose();
                if (Spells != null)
                {
                    var spellwords = from s in Spells select s.Words;
                    sre = new SpeechRecognitionEngine(CultureInfo.CurrentCulture);
                    var grammars = CreateGrammar(spellwords);
                    foreach (var grammar in grammars)
                    {
                        sre.LoadGrammar(grammar);
                    }
                    sre.SetInputToDefaultAudioDevice();
                    sre.SpeechRecognized += SpeechRecognizer_SpeechRecognized;
                    sre.EndSilenceTimeout = TimeSpan.FromMilliseconds(50);

                    sre.RecognizeAsync(RecognizeMode.Multiple);
                }
            });
        }

        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            CastSpell(e.Result.Text);
        }

        public static IEnumerable<Grammar> CreateGrammar(IEnumerable<string> candidates)
        {
            var gramars = new List<Grammar>();
            foreach (var w in candidates)
            {

                var choices = new Choices(w);
                var builder = new GrammarBuilder(choices);
                
                gramars.Add(new Grammar(builder));
            }

            return gramars;
        }

        public void CastSpell(string words)
        {
            if (!isSpellCasting)
            {
                var s = from sp in Spells where sp.Words == words select sp;
                var spell = s.FirstOrDefault();
                if (spell != null)
                {
                    Console.WriteLine($"Casting spell:{spell.Words}");
                    SendKeys.SendWait(spell.KeyList);
                    try
                    {
                        SpellCasted?.Invoke(this, new CastSpellEventAgrs(spell));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public class CastSpellEventAgrs : EventArgs
        {
            public CastSpellEventAgrs()
            {
            }

            public CastSpellEventAgrs(Spell s) : this()
            {
                Spell = s;
            }

            public Spell Spell { get; set; }
        }
    }
}
