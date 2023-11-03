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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.SqlClient;

namespace StudyWithMe
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        protected string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudyWithMe.ApplicationDbContext;Integrated Security=True";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            //object of the info window
            MainWindow window = new MainWindow();

            //displays info window
            window.Show();

            //hides current window
            Close();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernametextBox.Text;
            string password = passwordtextBox.Text;

            SqlConnection con = new SqlConnection(connection);

            string query = "SELECT COUNT(*) FROM [Users] WHERE [Username] = @Username";
            int count = 0;
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", usernametextBox.Text);
                    count = (int)cmd.ExecuteScalar();
                }
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error finding username: {ex}");
            }

            if(count > 0)
            {
                string storedPassword = "";
                try
                {
                    con.Open();
                    string selectPasswordQuery = "SELECT Password FROM [Users] WHERE Username = @Username";

                    using (SqlCommand selectCmd = new SqlCommand(selectPasswordQuery, con))
                    {
                        selectCmd.Parameters.AddWithValue("@Username", username);
                        storedPassword = selectCmd.ExecuteScalar() as string;
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching password --> {ex}");
                }

                if (password.Equals(storedPassword))
                {
                    MessageBox.Show("Login Successful.", "Notification", MessageBoxButton.OK);

                    int cnt = 0;
                    try
                    {
                        string semesterQuery = "SELECT COUNT(*) FROM [Semesters] WHERE [Username_username] = @username";

                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(semesterQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            cnt = (int)cmd.ExecuteScalar();
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"Error: {ex}");
                    }

                    if(cnt > 0)
                    {
                        //object of the Dashboard window
                        Dashboard window = new Dashboard(username);

                        //displays Dashboard window
                        window.Show();

                        //hides current window
                        Close();
                    }
                    else
                    {
                        //object of the Semester window
                        SemesterWindow window = new SemesterWindow(username);

                        //displays Semester window
                        window.Show();

                        //hides current window
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Login Failed. Password is incorrect, please try again.", "Notification", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Login Failed. Username is incorrect, please try again.", "Notification", MessageBoxButton.OK);
            }
        }
    }
}
