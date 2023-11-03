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
        private Semester semInfo;

        public Dashboard(string username)
        {
            this.username = username;
            InitializeComponent();
            loadInfo();
        }

        public void loadInfo()
        {
            SqlConnection con = new SqlConnection(connection);
            string selectSemesterQuery = "SELECT weeks, startDate, endDate FROM [Semesters] WHERE [Username_username] = @username";
            try
            {
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

                            semInfo = new Semester
                            {
                                weeks= numberOfWeeks,
                                startDate= startDate,
                                endDate = endDate
                            };
                        }
                        else
                        {
                            MessageBox.Show("No Semester information is assigned to your profile.", "Notification", MessageBoxButton.OK);
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Failed to Read Semester --> Error:\n\n{ex}");
            }
            
            string selectModulesQuery = "SELECT module_id, code, name, credits, classHrsPerWeek FROM [Modules] WHERE Username_username = @username";

            List<Module> modules = new List<Module>();

            try
            {
                con.Open();
                using (SqlCommand selectModCmd = new SqlCommand(selectModulesQuery, con))
                {
                    selectModCmd.Parameters.AddWithValue("@username", username);

                    using (SqlDataReader reader = selectModCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Module module = new Module
                            {
                                module_id= reader.GetInt32(0),
                                code = reader.GetString(1),
                               
                                name = reader.GetString(2),
                                credits = reader.GetInt32(3),
                                classHrsPerWeek = reader.GetInt32(4)
                            };
                            modules.Add(module);
                        }
                    }
                }
                con.Close();

                ViewModel view = new ViewModel
                {
                    moduleItems = modules
                };
                this.DataContext = view;
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Failed to read Modules --> error:\n\n{ex}"); ;
            }
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

            //object of the Module Calendar window
            ModuleCalendarWindow window = new ModuleCalendarWindow(semInfo, selectedMod, username);

            //displays Module Calendar window
            window.Show();

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
