using Eco.ViewModel.Runtime;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace StudyWithMe
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        //private readonly ViewModel viewModel;
        protected string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudyWithMe.ApplicationDbContext;Integrated Security=True";
        private string username;
        public Dashboard(string username)
        {
            this.username = username;
            InitializeComponent();
            loadInfo();
        }

        public void loadInfo()
        {
            SqlConnection con = new SqlConnection(connection);
            string selectSemesterQuery = "SELECT weeks, startDate, endDate FROM [SemesterTable] WHERE username = @UserId";
            try
            {

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex}");
            }
            con.Open();

            using (SqlCommand selectCmd = new SqlCommand(selectSemesterQuery, con))
            {
                selectCmd.Parameters.AddWithValue("@username", username);

                using (SqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int numberOfWeeks = reader.GetInt32(0);
                        DateTime startDate = reader.GetDateTime(1);
                        DateTime endDate = reader.GetDateTime(2);

                        richTextBox.AppendText($"Number of Weeks: {numberOfWeeks}\nStart Date: {startDate.ToShortDateString()}\nEnd Date: {endDate.ToShortDateString()}");
                    }
                    else
                    {
                        MessageBox.Show("Not Semester information is assigned to your profile.", "Notification", MessageBoxButton.OK);
                    }
                }
            }
            string selectModulesQuery = "SELECT code, name, credits, classHrsPerWeek FROM [SemesterTable] WHERE username = @username";

            List<Module> modules = new List<Module>();


            using (SqlCommand selectModCmd = new SqlCommand(selectModulesQuery, con))
            {
                selectModCmd.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = selectModCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Module module = new Module
                        {
                            code = reader.GetString(0),
                            name = reader.GetString(1),
                            credits = int.Parse(reader.GetString(2)),
                            classHrsPerWeek = int.Parse(reader.GetString(3))
                        };
                        modules.Add(module);
                    }
                }
            }

            ViewModel view = new ViewModel
            {
                moduleItems = modules
            };
            this.DataContext = view;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            //object of the Module window
            ModuleWindow window = new ModuleWindow(username);

            //displays Module window
            window.Show();

            //hides current window
            Close();
        }
        /// <summary>
        /// ///////////////////////////////////////////////////come back to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moduleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Module selectedMod = (Module)moduleDataGrid.SelectedItem;
            //ModuleCalendarWindow window = new ModuleCalendarWindow(semInfo, selectedMod);

            //displays calendar window
            //window.Show();

            //hides current window
            Close();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            //exits the app
            Application.Current.Shutdown();
        }
    }
}
