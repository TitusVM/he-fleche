using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows;

namespace HeFleche
{
    class Model
    {
        /*********************************************************\
         *                  Public methods                       *
        \*********************************************************/
        #region CREATE
        public static void CreateNote(SqlConnection cnn, Note note)
        {
            string createQuery = "INSERT INTO Grades(grade,coefficient,subjectId) " +
                "VALUES (" + note.NoteBrute +
                ", " + note.Coefficient +
                ", " + note.IdMatiere +")";
            try
            {
                new SqlCommand(createQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
        }
        public static void CreateMatiere(SqlConnection cnn, Matiere matiere)
        {
            string createQuery = "INSERT INTO Matiere(nameSubject,coefficient,moduleId) " +
                "VALUES ('" + matiere.Name +
                "', " + matiere.Coefficient +
                ", " + matiere.IdModule + ")";
            try
            {
                new SqlCommand(createQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
        }
        public static void CreateModule(SqlConnection cnn, Module module)
        {
            string createQuery = "INSERT INTO Modules (nameModule, mean) " +
                "VALUES ('" + module.Name + "'" +
                ", " + module.Moyenne + ")";
            try
            {
                new SqlCommand(createQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
        }
        #endregion
        #region READ
        public static ObservableCollection<Module> FetchModules(SqlConnection cnn)
        {
            ObservableCollection<Module> modules = new ObservableCollection<Module>();
            string queryModules = "SELECT * FROM Modules";

            using (SqlCommand command = new SqlCommand(queryModules, cnn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        modules.Add(new Module()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        });
                    }
                }
            }

            foreach (Module module in modules)
            {
                module.Matieres = FetchMatieres(cnn, module);
            }
            return modules;
        }

        public static ObservableCollection<Matiere> FetchMatieres(SqlConnection cnn, Module module)
        {
            ObservableCollection<Matiere> matieres = new ObservableCollection<Matiere>();
            string querySubjects = "SELECT * FROM Subjects WHERE moduleId=" + module.Id;
            using (SqlCommand command = new SqlCommand(querySubjects, cnn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        matieres.Add(new Matiere()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Coefficient = reader.GetInt32(2),
                            IdModule = reader.GetInt32(3),
                        });
                    }
                }
            }
            foreach (Matiere matiere in matieres)
            {
                matiere.Notes = FetchNotes(cnn, matiere);
            }
            return matieres;
        }

        public static ObservableCollection<Note> FetchNotes(SqlConnection cnn, Matiere matiere)
        {
            ObservableCollection<Note> notes = new ObservableCollection<Note>();
            string queryGrades = "SELECT * FROM Grades WHERE subjectId=" + matiere.Id;
            using (SqlCommand command = new SqlCommand(queryGrades, cnn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notes.Add(new Note()
                        {
                            Id = reader.GetInt32(0),
                            NoteBrute = reader.GetDouble(1),
                            Coefficient = reader.GetInt32(2),
                            IdMatiere = reader.GetInt32(3),
                        });
                    }
                }
            }
            return notes;
        }
        #endregion
        #region UPDATE
        public static void UpdateNote(SqlConnection cnn, Note note)
        {
            string updateQuery = "UPDATE Grades " +
                "SET grade=" + note.NoteBrute +
                ", coefficient=" + note.Coefficient +
                " WHERE id=" + note.Id;
            try
            {
                new SqlCommand(updateQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
        }

        public static void UpdateMatiere(SqlConnection cnn, Matiere matiere)
        {
            string updateQuery = "UPDATE Subjects " +
                "SET nameSubject='" + matiere.Name +
                "', coefficient=" + matiere.Coefficient +
                " WHERE id=" + matiere.Id;
            try
            {
                new SqlCommand(updateQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
            foreach (Note note in matiere.Notes)
            {
                UpdateNote(cnn, note);
            }
        }

        public static void UpdateModule(SqlConnection cnn, Module module)
        {
            string updateQuery = "UPDATE Modules " +
                "SET nameModule=" + module.Name +
                " WHERE id=" + module.Id;
            try
            {
                new SqlCommand(updateQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
            foreach (Matiere matiere in module.Matieres)
            {
                UpdateMatiere(cnn, matiere);
            }
        }
        #endregion
        #region DELETE
        public static void DeleteNote(SqlConnection cnn, Note note)
        {
            string deleteQuery = "DELETE FROM Grades " +
                "WHERE id=" + note.Id;
            try
            {
                new SqlCommand(deleteQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
        }
        public static void DeleteMatiere(SqlConnection cnn, Matiere matiere)
        {
            string deleteQuery = "DELETE FROM Subjects " +
                "WHERE id=" + matiere.Id;
            try
            {
                new SqlCommand(deleteQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
            foreach(Note note in matiere.Notes)
            {
                DeleteNote(cnn, note);
            }
        }
        public static void DeleteModule(SqlConnection cnn, Module module)
        {
            string deleteQuery = "DELETE FROM Modules " +
                "WHERE id=" + module.Id;
            try
            {
                new SqlCommand(deleteQuery, cnn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem " + e);
            }
            foreach (Matiere matieres in module.Matieres)
            {
                DeleteMatiere(cnn, matieres);
            }
        }
        #endregion
    }
}
