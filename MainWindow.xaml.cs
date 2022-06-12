using System.Windows;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System;

namespace HeFleche
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Module> moduleList = new ObservableCollection<Module>();

        string conString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=Z:\dotnet\groupe-1\HeFleche.mdf;Integrated Security=True";
        public SqlConnection cnn;


        public MainWindow()
        {
            cnn = new SqlConnection(conString);
            cnn.Open();

            InitializeComponent();
            UpdateLists(null);
        }
        private void UpdateLists(Matiere lastMatiere)
        {
            moduleList = Model.FetchModules(cnn);
            listModule.ItemsSource = moduleList;
            cbxModules.ItemsSource = moduleList;

            if(lastMatiere != null)
            {
                listNote.ItemsSource = Model.FetchNotes(cnn, lastMatiere);
            }
            
        }

        private void listModule_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Module selectedModule = (Module)listModule.SelectedItem;
            if(selectedModule != null)
            {
                listMatiere.ItemsSource = selectedModule.Matieres;
                cbxMatiere.ItemsSource = selectedModule.Matieres;
                cbxModules.SelectedItem = selectedModule;
                nomModule.Text = selectedModule.Name;
            }
        }

        private void listMatiere_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Matiere selectedMatiere = (Matiere)listMatiere.SelectedItem;
            if(selectedMatiere != null)
            {
                listNote.ItemsSource = selectedMatiere.Notes;
                moyenneMatiere.Text = selectedMatiere.Moyenne.ToString();
                coeffMat.Text = selectedMatiere.Coefficient.ToString();
                nomMatiere.Text = selectedMatiere.Name;
                coeffMatiere.Text = selectedMatiere.Coefficient.ToString();
                cbxMatiere.SelectedItem = selectedMatiere;
            }
        }

        private void listNote_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Note selectedNote = (Note)listNote.SelectedItem;
            if (selectedNote != null)
            {
                coeffSelect.Text = selectedNote.Coefficient.ToString();
                coeffNote.Text = selectedNote.Coefficient.ToString();
                noteSelect.Text = selectedNote.NoteBrute.ToString();
                note.Text = selectedNote.NoteBrute.ToString();
                cbxMatiere.SelectedItem = listMatiere.SelectedItem;
            }
        }

        #region MODULE
        private void ModuleAjouterBtnClicked(object sender, RoutedEventArgs e)
        {
            if (nomModule.Text != "")
            {
                Model.CreateModule((SqlConnection)cnn, new Module()
                {
                    Name = nomModule.Text,
                });
                UpdateLists(null);
            }
            else
            {
                MessageBox.Show("Il faut un nom de module");
            }
        }

        private void ModuleSauvegarderBtnClicked(object sender, RoutedEventArgs e)
        {
            if (nomModule.Text != "")
            {
                Module module = (Module)listModule.SelectedItem;
                module.Name = nomModule.Text;
                Model.UpdateModule(cnn, module);
                UpdateLists(null);
            }
            else
            {
                MessageBox.Show("Il faut un nom de module");
            }
        }

        private void ModuleSupprimerBtnClicked(object sender, RoutedEventArgs e)
        {
            if (nomModule.Text != "")
            {
                const string message =
                "Êtes vous sûr de vouloi supprimer ce module ?";
                const string caption = "Suppression";

                MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Module module = (Module)listModule.SelectedItem;
                        Model.DeleteModule(cnn, module);
                        UpdateLists(null);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Sélectionnez un module");
            }
        }
        #endregion

        #region MATIERE
        private void MatiereSupprimerBtnClicked(object sender, RoutedEventArgs e)
        {
            if (nomMatiere.Text != "")
            {
                const string message =
                "Êtes vous sûr de vouloi supprimer cette matière ?";
                const string caption = "Suppression";

                MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Matiere matiere = (Matiere)listMatiere.SelectedItem;
                        Model.DeleteMatiere(cnn, matiere);
                        UpdateLists(null);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Sélectionnez une matière");
            }
        }

        private void MatiereAjouterBtnClicked(object sender, RoutedEventArgs e)
        {
            if (nomMatiere.Text != "")
            {
                Model.CreateMatiere((SqlConnection)cnn, new Matiere()
                {
                    Name = nomMatiere.Text,
                });
                UpdateLists(null);
            }
            else
            {
                MessageBox.Show("Il faut un nom de matière");
            }
        }

        private void MatiereSauvegarderBtnClicked(object sender, RoutedEventArgs e)
        {
            if (nomMatiere.Text != "" && cbxModules.SelectedItem != null && coeffMatiere.Text != "")
            {
                Matiere matiere = (Matiere)listMatiere.SelectedItem;
                matiere.Name = nomMatiere.Text;
                Model.UpdateMatiere(cnn, matiere);
                UpdateLists(null);
            }
            else
            {
                MessageBox.Show("Remplissez tous les champs pour ajouter une matière");
            }
        }
        #endregion

        #region NOTE
        private void NoteSupprimerBtnClicked(object sender, RoutedEventArgs e)
        {
            if (note.Text != "")
            {
                const string message =
                "Êtes vous sûr de vouloi supprimer cette note ?";
                const string caption = "Suppression";

                MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Note noteObj = (Note)listNote.SelectedItem;
                        Model.DeleteNote(cnn, noteObj);
                        UpdateLists((Matiere)listMatiere.SelectedItem);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Sélectionnez un module");
            }

        }

        private void NoteAjouterBtnClicked(object sender, RoutedEventArgs e)
        {
            if (note.Text != "")
            {
                Model.CreateNote((SqlConnection)cnn, new Note()
                {
                    IdMatiere = ((Matiere)listMatiere.SelectedItem).Id,
                    NoteBrute = Double.Parse(note.Text),
                    Coefficient = Int32.Parse(coeffNote.Text)
                });
                UpdateLists((Matiere)listMatiere.SelectedItem);
            }
            else
            {
                MessageBox.Show("Il faut une note");
            }
        }

        private void NoteSauvegarderBtnClicked(object sender, RoutedEventArgs e)
        {
            if (note.Text != "" && cbxMatiere.SelectedItem != null && coeffNote.Text != "" && listNote.SelectedItem != null)
            {
                Note noteObj = (Note)listNote.SelectedItem;
                noteObj.NoteBrute = Double.Parse(note.Text);
                noteObj.Coefficient = Int32.Parse(coeffNote.Text);
                Model.UpdateNote(cnn, noteObj);
                UpdateLists((Matiere)listMatiere.SelectedItem);
            }
            else
            {
                MessageBox.Show("Sélectionnez une note pour la modifier et remplissez tous les champs");
            }
        }
        #endregion
    }
}