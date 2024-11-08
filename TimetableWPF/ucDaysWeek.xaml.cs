using Syncfusion.Windows.Shared;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimetableWPF.DTO;

namespace TimetableWPF
{
    public partial class ucDaysWeek : UserControl
    {
        string _day;
        private WindowStateJson _windowState;
        private DateTime _taskDate;
        private DatePicker _datepicker;
        private ColorPicker _colorPicker;
        private DataGrid _dataGrid;
        private MainWindow _mainWindow;

        public ucDaysWeek(string day, DateTime taskDate, DatePicker datepicker, ColorPicker colorPicker, DataGrid dataGrid, WindowStateJson windowState, bool inMonth, MainWindow mainWindow)
        {
            InitializeComponent();
            _day = day;
            _taskDate = taskDate;
            ShownDay.Content = day;
            _datepicker = datepicker;
            _colorPicker = colorPicker;
            _dataGrid = dataGrid;
            _windowState = windowState;
            if (inMonth == false)
            {
                gridTasksWeek.Visibility = Visibility.Hidden;
                AddButon.Visibility = Visibility.Hidden;
            }
            else
            {
                gridTasksWeek.Visibility = Visibility.Visible;
                AddButon.Visibility = Visibility.Visible;
            }
            _mainWindow = mainWindow;
        }

        private void gridTasksWeek_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            MyTask task = e.Row.Item as MyTask;
            if (task != null)
            {
                Guid categoryId = task.CategoryId;
                var categoryTask = _windowState?.categories?.Find(x => x.CategoryId == categoryId);

                if (categoryTask != null)
                {
                    System.Drawing.Color color = categoryTask.CategoryColor;
                    System.Windows.Media.Color mediaColor = System.Windows.Media.Color.FromArgb(
                            color.A,
                            color.R,
                            color.G,
                            color.B
                    );

                    e.Row.Background = new SolidColorBrush(mediaColor);
                }
            }
        }

        private void AddButon_Click_1(object sender, RoutedEventArgs e)
        {
            int day = Int32.Parse(_day);
            DateTime date = new DateTime(SharedDateInfo.Year, SharedDateInfo.Month, day);
            _datepicker.SelectedDate = date;
            AddTaskWindow addTaskWindow = new AddTaskWindow(_datepicker, _colorPicker, _dataGrid, _windowState, _day);
            addTaskWindow.TaskAdded += () => _mainWindow.RefreshWeeklyView();
            addTaskWindow.Show();
            _windowState.Save();
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int day;
            if (int.TryParse(ShownDay.Content.ToString(), out day))
            {
                try
                {
                    DateTime selectedDate = new DateTime(SharedDateInfo.Year, SharedDateInfo.Month, day);
                    _datepicker.SelectedDate = selectedDate;
                }
                catch (ArgumentOutOfRangeException)
                {

                }
            }
        }
    }
}
