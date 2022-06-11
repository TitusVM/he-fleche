using System;
using System.Numerics;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HeFleche
{
    class Matiere : INotifyPropertyChanged
    {
        /*********************************************************\
         *                 Private Attributes                    *
        \*********************************************************/

        private int id;
        private int idModule;
        private string name;
        private double moyenne;
        private int coefficient;
        private Collection<Note> notes;


        /*********************************************************\
         *                  Public methods                       *
        \*********************************************************/
        public Matiere()
        {
            this.name = "";
            this.moyenne = 0;
            this.coefficient = 0;
            this.notes = new Collection<Note>();
        }
        public void AddNote(Note note)
        {
            this.notes.Add(note);
            UpdateMoyenne();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /*********************************************************\
         *                      Get/Set                          *
        \*********************************************************/
        public int Id
        {
            get { return id; }
            set { id = value; NotifyPropertyChanged("Id"); }
        }
        public int IdModule
        {
            get { return idModule ; }
            set { idModule = value; NotifyPropertyChanged("IdModule"); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged("Name"); }
        }
        public double Moyenne
        {
            get { UpdateMoyenne(); return moyenne; }
        }
        public int Coefficient
        {
            get { return coefficient; }
            set { coefficient = value; NotifyPropertyChanged("Coefficient"); }
        }
        public Collection<Note> Notes
        {
            get { return notes; }
            set { notes = value; UpdateMoyenne(); NotifyPropertyChanged("Notes"); }
        }
        public override string ToString()
        {
            return $"{name}";
        }

        /*********************************************************\
         *                  Private methods                      *
        \*********************************************************/
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UpdateMoyenne()
        {
            this.moyenne = 0;
            double coeffTotal = 0;
            foreach (Note note in this.notes)
            {
                moyenne += note.NoteBrute * note.Coefficient;
                coeffTotal += note.Coefficient;
            }
            moyenne = moyenne / coeffTotal;
            moyenne = Math.Round(moyenne, 3);
        }
    }
}
