using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimetableWPF.DTO;

namespace TimetableWPF
{
    public partial class ucDaysMonth : UserControl
    {
        string _day;
        private WindowStateJson windowState;
        private DatePicker _datePicker;

        private void gridTasksWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public ucDaysMonth(string day, DatePicker datePicker)
        {
            InitializeComponent();
            _day = day;
            _datePicker = datePicker;
            ShownDay.Content = day;
            windowState = WindowStateJson.Load();
        }

        private void gridEvents_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            MyEvent myevent = e.Row.Item as MyEvent;
            if (myevent != null)
            {
                System.Drawing.Color color = myevent.Color;
                System.Windows.Media.Color mediaColor = System.Windows.Media.Color.FromArgb(
                        color.A,
                        color.R,
                        color.G,
                        color.B
                );

                e.Row.Background = new SolidColorBrush(mediaColor);
            }
        }

        private void gridEvents_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int day;
            if (int.TryParse(ShownDay.Content.ToString(), out day))
            {
                try
                {
                    DateTime selectedDate = new DateTime(SharedDateInfo.Year, SharedDateInfo.Month, day);
                    _datePicker.SelectedDate = selectedDate;
                }
                catch (ArgumentOutOfRangeException)
                {

                }
            }
        }
    }
}
