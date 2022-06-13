using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace HeFleche
{
    class Module : INotifyPropertyChanged
    {
        /*********************************************************\
         *                 Private Attributes                    *
        \*********************************************************/

        private int id;
        private string name;
        private double moyenne;
        private Collection<Matiere> matieres;


        /*********************************************************\
         *                  Public methods                       *
        \*********************************************************/
        public Module()
        {
            this.id = -1;
            this.name = "";
            this.moyenne = 0.0;
            this.matieres = new Collection<Matiere>();
        }
        public void AddMatiere(Matiere matiere)
        {
            this.matieres.Add(matiere);
            UpdateMoyenne();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /*********************************************************\
         *                      Get/Set                          *
        \*********************************************************/

        public int Id
        {
            get { return id; }
            set { id = value; NotifyPropertyChanged("id"); }
        }
        public double Moyenne
        {
            get { UpdateMoyenne(); return moyenne; }
            set { moyenne = value; NotifyPropertyChanged("moyenne"); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged("Name"); }

        }
        public Collection<Matiere> Matieres
        {
            get { return matieres; }
            set
            {
                matieres = value; UpdateMoyenne(); NotifyPropertyChanged("Matieres");
                UpdateMoyenne();
            }
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

        private void UpdateMoyenne()
        {
            this.moyenne = 0.0;
            double coeffTotal = 0;
            if (this.matieres.Count() != 0)
            {
                foreach (Matiere matiere in this.matieres)
                {
                    moyenne += matiere.Moyenne * matiere.Coefficient;
                    coeffTotal += matiere.Coefficient;
                }
                moyenne = moyenne / coeffTotal;
                moyenne = Math.Round(moyenne, 3);
            }
        }
    }
}
