using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Magicaster.SpellCasting;
using ReactiveUI;

namespace Magicaster
{
    class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            StartSpeechEngineCommand = ReactiveCommand.Create(ExecuteStartSpeechEngine);
            SaveSpellBookCommand = ReactiveCommand.Create(ExecuteSaveSpellBook);
            LoadSpellBookCommand = ReactiveCommand.Create(ExecuteLoadSpellBook);
        }

        private void ExecuteLoadSpellBook()
        {
            var filename = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), "spellbook.json");
            Book = SpellBook.LoadFromFile(filename);
        }

        private void ExecuteSaveSpellBook()
        {
            var filename = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), "spellbook.json");
            if (File.Exists(filename) && MessageBox.Show("overwrite??", "Warning", MessageBoxButton.YesNo) !=
                MessageBoxResult.Yes)
            {
                return;
            }

            Book.SaveToFile(filename);
        }

        private Spell selectedSpell;
        private Caster caster = new Caster();
        private string statusMessage = "Stopped";
        private SpellBook spellBook = new SpellBook();

        public SpellBook Book
        {
            get => spellBook;
            set => this.RaiseAndSetIfChanged(ref spellBook, value);
        }


        public ReactiveCommand<Unit, Unit> SaveSpellBookCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadSpellBookCommand { get; }

        public Spell SelectedSpell
        {
            get => selectedSpell;
            set => this.RaiseAndSetIfChanged(ref selectedSpell, value);
        }

        public ICommand StartSpeechEngineCommand { get; }

        public string StatusMessage
        {
            get => statusMessage;
            set => this.RaiseAndSetIfChanged(ref statusMessage, value);
        }

        private void ExecuteStartSpeechEngine()
        {
            caster.Spells = Book.Spells.ToList();
            caster.InitRecognition();
            caster.SpellCasted += Caster_SpellCasted;
            StatusMessage = "Running";
        }

        private void Caster_SpellCasted(object sender, Caster.CastSpellEventAgrs e)
        {
             StatusMessage = $"{DateTime.Now:T} Spell Cast:{e.Spell.Words}"; 
        }
    }
}
