using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Reflection.Emit;

namespace StudyWithMe
{
    /// <summary>
    /// Interaction logic for ModuleWindow.xaml
    /// </summary>
    public partial class ModuleWindow : Window
    {
        protected string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudyWithMe.ApplicationDbContext;Integrated Security=True";
        private string username;

        public ModuleWindow(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //object of the dashboard window
            Dashboard window = new Dashboard(username);

            //displays Dashboard window
            window.Show();

            //hides current window
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Module module = new Module();
            SqlConnection con = new SqlConnection(connection);

            //checks if module code is entered
            if (string.IsNullOrEmpty(textBox.Text)) { MessageBox.Show("You must enter a module code to proceed.", "No Module Code Entered!", MessageBoxButton.OK); }
            else
            {
                string code = textBox.Text;
                if (string.IsNullOrEmpty(textBox2.Text)) { MessageBox.Show("You must enter a module name to proceed.", "No Module Name Entered!", MessageBoxButton.OK); }
                else
                {
                    string name = textBox2.Text;
                    int numCreds;
                    if (!int.TryParse(textBox3.Text, out numCreds)) { MessageBox.Show("You must enter the number of credits for the module to proceed.", "No Module Credits Entered!", MessageBoxButton.OK); }
                    else
                    {
                        int credits = numCreds;
                        int numHrsWeek;
                        if (!int.TryParse(textBox4.Text, out numHrsWeek)) { MessageBox.Show("You must enter the number of weeks for the module to proceed.", "No Module Weeks Entered!", MessageBoxButton.OK); }
                        else
                        {
                            int classHrsPerWeek = numHrsWeek;
                            try
                            {
                                con.Open();
                                string insertQuery = "INSERT INTO [Modules] (code, name, credits, classHrsPerWeek, username) VALUES (@code, @name, @credits, @classHrsPerWeek, @username)";
                                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                                {
                                    cmd.Parameters.AddWithValue("@code", code);
                                    cmd.Parameters.AddWithValue("@name", name);
                                    cmd.Parameters.AddWithValue("@credits", credits);
                                    cmd.Parameters.AddWithValue("@classHrsPerWeek", numHrsWeek);
                                    cmd.Parameters.AddWithValue("@username", username);
                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Module Infomration Saved.", "Notification:", MessageBoxButton.OK);

                                    //object of the dashboard window
                                    Dashboard window = new Dashboard(username);

                                    //displays Dashboard window
                                    window.Show();

                                    //hides current window
                                    Close();
                                }
                                con.Close();
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show($"Module infromation failed to save --> Error: {ex}");
                            }
                        }

                    }
                }
            }
        }
    }
}
