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
using System.Xml.Linq;

namespace StudyWithMe
{
    /// <summary>
    /// Interaction logic for SemesterWindow.xaml
    /// </summary>
    public partial class SemesterWindow : Window
    {
        private string username;
        protected string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudyWithMe.ApplicationDbContext;Integrated Security=True";

        public SemesterWindow(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            //object of the main window
            MainWindow window = new MainWindow();

            //displays main window
            window.Show();

            //hides current window
            Close();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(connection);

            //checks if number of weeks box is empty
            if (string.IsNullOrEmpty(textBox.Text)) { MessageBox.Show("Please enter the number of weeks in a semester.", "Empty Field!", MessageBoxButton.OK); }
            else
            {
                //checks if the number of weeks entered is a valid data type
                int numWeeks;
                if (!int.TryParse(textBox.Text, out numWeeks))
                {
                    MessageBox.Show("Please enter a numerical value for the number of weeks in a semester.", "Invalid Input!", MessageBoxButton.OK);
                }
                //saves the number of weeks value entered
                else
                {
                    int weeks = numWeeks;

                    DateTime? selectedDate = datePicker.SelectedDate;
                    //checks if a start date has been selected by the user
                    if (!selectedDate.HasValue) { MessageBox.Show("Please select a start date for the semseter via the (select a date) tab.", "Unselected Start Date!", MessageBoxButton.OK); }
                    else
                    {
                        DateTime startDate = selectedDate.Value;

                        //calculates and saves the end date of the semester
                        DateTime endDate = startDate.AddDays(weeks * 7);

                        try
                        {
                            con.Open();
                            string insertQuery = "INSERT INTO [Semesters] (weeks, startDate, endDate, username) VALUES (@weeks, @startDate, @endDate, @username)";
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@weeks", weeks);
                                cmd.Parameters.AddWithValue("@startDate", startDate);
                                cmd.Parameters.AddWithValue("@endDate", endDate);
                                cmd.Parameters.AddWithValue("@username", username);
                                cmd.ExecuteNonQuery();

                                MessageBox.Show("Semester Infomration Saved.", "Notification:", MessageBoxButton.OK);

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

                            MessageBox.Show($"Failed to save Semester Information --> Error: {ex}");
                        }
                    }
                }
            }
        }
    }
}
