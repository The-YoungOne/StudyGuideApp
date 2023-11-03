using StudyGuideDLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using StudyGuideDLL;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;

namespace StudyWithMe
{
    /// <summary>
    /// Interaction logic for ModuleCalendarWindow.xaml
    /// </summary>
    public partial class ModuleCalendarWindow : Window
    {
        protected static string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudyWithMe.ApplicationDbContext;Integrated Security=True";
        private static SqlConnection con = new SqlConnection(connection);

        private Semester semInfo;
        private static Module modInfo;
        private string username;

        private static ClassMethods obj = new ClassMethods();
        private static List<double> weeklyStdHrs = new List<double>();
        private static List<ModuleCalendar> hrsStudied = new List<ModuleCalendar>();
        public ModuleCalendarWindow(Semester semInfo, Module selectedMod, string username)
        {
            InitializeComponent();
            this.semInfo = semInfo;
            modInfo = selectedMod;
            this.username = username;

            //sets up the calendar start and end date
            ModuleCalendar.DisplayDateStart = semInfo.startDate;
            ModuleCalendar.DisplayDateEnd = semInfo.endDate;

            //loads any dates and hours studied for each specific modular
            try
            {
                int module_id = selectedMod.module_id;
                string dataQuery = "SELECT studyDate, hoursStudied [ModuleCalendars] WHERE [Module_module_id] = @module_id";

                con.Open();
                using (SqlCommand selectModCmd = new SqlCommand(dataQuery, con))
                {
                    selectModCmd.Parameters.AddWithValue("@module_id", module_id);

                    using (SqlDataReader reader = selectModCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ModuleCalendar date_hours = new ModuleCalendar
                            {
                                studyDate = reader.GetDateTime(0),
                                hoursStudied = reader.GetInt32(1)
                            };
                            hrsStudied.Add(date_hours);
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Failed to get ant Module data\nError:\n\n{ex}");
            }

            ModuleInfoTxtBox.AppendText(displayModInfo());


            double weeklyStudyHours = obj.weeklyHours(selectedMod.credits, semInfo.weeks, selectedMod.classHrsPerWeek);
            for (int x = 0; x < semInfo.weeks; x++)
            {
                weeklyStdHrs.Add(weeklyStudyHours);
            }

            //modifies the weekly hours studied, if there are any previous dates saved by the user that they have studied on
            int cnter = 0;
            double totHrs = 0.0;
            foreach (var item in hrsStudied)
            {
                totHrs += item.hoursStudied;
            }

            while (cnter < weeklyStdHrs.Count && totHrs != 0)
            {
                if (weeklyStdHrs[cnter] < totHrs)
                {
                    totHrs -= weeklyStdHrs[cnter];
                    weeklyStdHrs[cnter] = 0;
                }
                else
                {
                    weeklyStdHrs[cnter] -= totHrs;
                    totHrs = 0.0;
                }
                cnter++;
            }

            weklyStdHrsTxtBox.AppendText(displayWeeklyHrs());
        }

        public string displayModInfo()
        {
            ModuleInfoTxtBox.Document.Blocks.Clear();
            //gets the total hours studied for the module
            double totHrs = 0.0;
            foreach (var item in hrsStudied)
            {
                totHrs += item.hoursStudied;
            }

            return $"Module Infomration\n\nCode: {modInfo.code}\nName: {modInfo.name}\nCredits: {modInfo.credits}\nClass Hours per Week: {modInfo.classHrsPerWeek}\nTotal Study Hours: {(obj.totStudyHours(modInfo.credits) - totHrs)}";
        }

        public string displayWeeklyHrs()
        {
            int cnter = 1;

            weklyStdHrsTxtBox.Document.Blocks.Clear();
            string joint = "  Study Hours Per Week\n";
            foreach (var item in weeklyStdHrs)
            {
                string formatHrs = item.ToString("0.0");
                string[] hrsSplit = formatHrs.Split('.');
                joint += $"~ Week ({cnter}): {hrsSplit[0]}hrs {int.Parse(hrsSplit[1]) * 6}min\n";
                cnter++;
            }
            return joint;
        }

        private void ModifyHrs(double hours)
        {
            int cnter = 0;
            while (cnter < weeklyStdHrs.Count && hours != 0.0)
            {
                if (weeklyStdHrs[cnter] < hours)
                {
                    hours -= weeklyStdHrs[cnter];
                    weeklyStdHrs[cnter] = 0.0;
                }
                else
                {
                    weeklyStdHrs[cnter] -= hours;
                    hours = 0.0;
                }
                cnter++;
            }
            ModuleInfoTxtBox.AppendText(displayModInfo());
            weklyStdHrsTxtBox.AppendText(displayWeeklyHrs());
        }

        private void datesStudiedButton_Click(object sender, RoutedEventArgs e)
        {
            string joint = "";
            foreach (var item in hrsStudied)
            {
                joint += $"~ Date: {item.studyDate.ToShortDateString()}\nHours Studied: {item.hoursStudied}hrs\n\n";
            }
            textBox.Text = joint;
            textBox.Visibility = Visibility.Visible;
            closeStudiedButton.Visibility = Visibility.Visible;
            datesStudiedButton.Visibility = Visibility.Hidden;
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            //object of the Module Calendar window
            Dashboard window = new Dashboard(username);

            //displays Module Calendar window
            window.Show();

            //hides current window
            Close();
        }

        private void closeStudiedButton_Click(object sender, RoutedEventArgs e)
        {
            textBox.Clear();
            textBox.Visibility = Visibility.Hidden;
            closeStudiedButton.Visibility = Visibility.Hidden;
            datesStudiedButton.Visibility = Visibility.Visible;
        }

        private void ModuleCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = ModuleCalendar.SelectedDate ?? DateTime.MinValue;

            string studyHrs = Interaction.InputBox($"Enter your study hours for {selectedDate.ToLongDateString()}", $"{selectedDate.ToLongDateString()}");
            if (double.TryParse(studyHrs, out double hours))
            {
                hrsStudied.Add(new ModuleCalendar
                {
                    studyDate = selectedDate,
                    hoursStudied = hours
                });
                ModifyHrs(hours);

                //inserting new study times and date
                try
                {
                    con.Open();
                    string insertQuery = "INSERT INTO [ModuleCalendars] (studyDate, hoursStudied, Module_module_id) VALUES (@selectedDate, @hours, @module_id)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@selectedDate", selectedDate);
                        cmd.Parameters.AddWithValue("@hours", hours);
                        cmd.Parameters.AddWithValue("@module_id", modInfo.module_id);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Failed to insert new module calendar info\nerror:\n\n{ex}");
                }
            }
            else
            {
                MessageBox.Show("Please enter a double value for the hours studied IE: 2.0", "Invalid Information Entered!", MessageBoxButton.OK);
            }
        }
    }
}
