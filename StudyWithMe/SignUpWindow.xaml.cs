using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace StudyWithMe
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        protected string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudyWithMe.ApplicationDbContext;Integrated Security=True";
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void saveButton_Click_1(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(connection);

            string query = "SELECT COUNT(*) FROM [Users] WHERE [Username] = @Username";

            con.Open();
            int count = 0;
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", usernametextBox.Text);
                count = (int)cmd.ExecuteScalar();
            }
            con.Close();

            if (count != 0)
            {
                MessageBox.Show("The username you have entered already exists. Please enter a different username.", "Notification", MessageBoxButton.OK);
            }
            else
            {
                string username = usernametextBox.Text;
                if (String.IsNullOrEmpty(nametextBox.Text))
                {
                    MessageBox.Show("Please enter your name before proceeding.", "Notification", MessageBoxButton.OK);
                }
                else
                {
                    string name = nametextBox.Text;
                    if (string.IsNullOrEmpty(surnametextBox.Text))
                    {
                        MessageBox.Show("Please enter your surname before proceeding.", "Notification", MessageBoxButton.OK);
                    }
                    else
                    {
                        string surname = surnametextBox.Text;
                        if (string.IsNullOrEmpty(usernametextBox.Text))
                        {
                            MessageBox.Show("Please enter a username before proceeding.", "Notification", MessageBoxButton.OK);
                        }
                        else
                        {
                            if (!passwordtextBox.Text.Equals(conPasswordtextBox.Text))
                            {
                                MessageBox.Show("The passwords you entered do not match. Please try again.", "Notification", MessageBoxButton.OK);
                            }
                            else
                            {
                                string password = passwordtextBox.Text;
                                if (string.IsNullOrEmpty(emailTextBox.Text))
                                {
                                    MessageBox.Show("Please enter a email for your account", "Notification", MessageBoxButton.OK);
                                }
                                else
                                {
                                    string email = emailTextBox.Text;

                                    con.Open();
                                    string insertQuery = "INSERT INTO [Users] (username, name, surname, password, email) VALUES (@username, @name, @surname, @password, @email)";
                                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                                    {
                                        cmd.Parameters.AddWithValue("@username", username);
                                        cmd.Parameters.AddWithValue("@name", name);
                                        cmd.Parameters.AddWithValue("@surname", surname);
                                        cmd.Parameters.AddWithValue("@password", password);
                                        cmd.Parameters.AddWithValue("@email", email);
                                        cmd.ExecuteNonQuery();

                                        MessageBox.Show("Registration Successful.", "Notification:", MessageBoxButton.OK);
                                    }
                                    con.Close();

                                    //object of the main window
                                    MainWindow window = new MainWindow();

                                    //displays main window
                                    window.Show();

                                    //hides current window
                                    Close();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void backButton_Click_1(object sender, RoutedEventArgs e)
        {
            //object of the Main window
            MainWindow window = new MainWindow();

            //displays main window
            window.Show();

            //hides current window
            Close();
        }
    }

}
