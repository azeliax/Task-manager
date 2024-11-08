using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using TimetableWPF.DTO;
using Syncfusion.Windows.Shared;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Threading.Tasks;
using System.Globalization;

namespace TimetableWPF
{
    public partial class MainWindow : Window
    {
        public WindowStateJson _windowState;
        public int _currentDay = 1;
        public DatePicker datePicker => datepicker;
        int _countDays;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            datepicker.SelectedDate = DateTime.Now;
            _windowState = WindowStateJson.Load();
            ListRefresh();
            ShowDays(DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Day);
        }


        void OnChecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Tag is MyTask task)
            {
                task.IsChecked = checkBox.IsChecked == true;
            }

            _windowState.Save();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ListRefresh();
            if (DaysPanel.Children.Count > 0)
            {
                ShowDays(datepicker.SelectedDate.Value.Date.Month, datepicker.SelectedDate.Value.Date.Year, datepicker.SelectedDate.Value.Date.Day);

            }
            else if (WrapPanelDays.Children.Count > 0 && datepicker.SelectedDate.Value.Date.Month != SharedDateInfo.Month)
            {
                ShowDaysMonth(datepicker.SelectedDate.Value.Date.Month, datePicker.SelectedDate.Value.Year);
            }
        }

        private void ListRefresh()
        {
             DateTime selectedDate = datepicker.SelectedDate.Value;
             var list = _windowState?.tasks?.Where(x => x.Date.Date == selectedDate)?.ToList();
             gridTasks.ItemsSource = list;

            var listEvents = _windowState?.events?.Where(x => x.Date.Date == selectedDate)?.ToList();
            gridEvents.ItemsSource = listEvents;
        }

        private void deletetask_Click(object sender, RoutedEventArgs e)
        {
            var temp = _windowState.tasks.Where(task => task.IsChecked).ToList();

            foreach (var task in temp)
            {
                _windowState.tasks.Remove(task);
            }

            ListRefresh();
            ShowDays(SharedDateInfo.Month, SharedDateInfo.Year, _currentDay);
            _windowState.Save();
        }

        private void gridTasks_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            MyTask task = e.Row.Item as MyTask;
            if (task != null)
            {
                Guid categoryId = task.CategoryId;
                var categoryTask = _windowState.categories.Find(x => x.CategoryId == categoryId);
                if (categoryTask != null)
                {
                    task.CategoryName = categoryTask.CategoryName;
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

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            Color color = ColorPick.Color;

            System.Drawing.Color drawingColor = System.Drawing.Color.FromArgb(
                color.A,
                color.R,
                color.G,
                color.B
            );

            var category = Categories.createCategory(CategoryTextBox.Text, drawingColor);
            _windowState.categories.Add(category);
            CategoryTextBox.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DaysPanel.Children.Count > 0)
            {
                _currentDay -= 7;

                if (_currentDay < 1)
                {
                    SharedDateInfo.Month--;

                    if (SharedDateInfo.Month < 1)
                    {
                        SharedDateInfo.Month = 12;
                        SharedDateInfo.Year--;
                    }

                    _currentDay = DateTime.DaysInMonth(SharedDateInfo.Year, SharedDateInfo.Month);
                }

           
                ShowDays(SharedDateInfo.Month, SharedDateInfo.Year, _currentDay);
            }
            else
            {
                SharedDateInfo.Month--;
                _currentDay = 1;

                if (SharedDateInfo.Month < 1)
                {
                    SharedDateInfo.Month = 12;
                    SharedDateInfo.Year--;
                }

                ShowDaysMonth(SharedDateInfo.Month, SharedDateInfo.Year);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DaysPanel.Children.Count > 0)
            {
                _currentDay += 7;

            if (_currentDay > DateTime.DaysInMonth(SharedDateInfo.Year, SharedDateInfo.Month))
            {
                _currentDay = 1;
                SharedDateInfo.Month++;

                if (SharedDateInfo.Month > 12)
                {
                    SharedDateInfo.Month = 1;
                    SharedDateInfo.Year++;
                }
            }
                ShowDays(SharedDateInfo.Month, SharedDateInfo.Year, _currentDay);
            }
            else
            {
                SharedDateInfo.Month++;
                _currentDay = 1;

                if (SharedDateInfo.Month > 12)
                {
                    SharedDateInfo.Month = 1;
                    SharedDateInfo.Year++;
                }

                ShowDaysMonth(SharedDateInfo.Month, SharedDateInfo.Year);
            }
        }

        private void ShowDays(int month, int year, int currentDay)
        {
            DaysPanel.Children.Clear();
            WrapPanelDays.Children.Clear();
            SharedDateInfo.Year = year;
            SharedDateInfo.Month = month;
            _currentDay = currentDay;

            string monthName = new DateTimeFormatInfo().GetMonthName(month);
            MonthName.Content = monthName.ToUpper() + " " + year;

            DateTime startOfWeek = new DateTime(year, month, currentDay);
            int daysToStart = ((int)startOfWeek.DayOfWeek + 6) % 7;
            DateTime startOfWeekDate = startOfWeek.AddDays(-daysToStart);

            ucDaysWeek uc = null;
            bool inMonth = true;

            for (int i = 0; i < 7; i++)
            {
                DateTime dateToDisplay = startOfWeekDate.AddDays(i);
                if (dateToDisplay.Month == month)
                {
                    inMonth = true;
                    uc = new ucDaysWeek(dateToDisplay.Day.ToString(), dateToDisplay, datepicker, ColorPick, gridTasks, _windowState, inMonth, this);
                    DaysPanel.Children.Add(uc);
                    ListRefreshMonth(uc.gridTasksWeek, dateToDisplay);
                }
                else
                {
                    inMonth = false;
                    uc = new ucDaysWeek("", dateToDisplay, datepicker, ColorPick, gridTasks, _windowState, inMonth, this);
                    DaysPanel.Children.Add(uc);
                } 
            }        
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ShowDaysMonth(SharedDateInfo.Month, SharedDateInfo.Year);
        }

        private void ShowDaysMonth(int month, int year)
        {
            DaysPanel.Children.Clear();
            WrapPanelDays.Children.Clear();
            SharedDateInfo.Year = year;
            SharedDateInfo.Month = month;

            string monthName = new DateTimeFormatInfo().GetMonthName(month);
            MonthName.Content = monthName.ToUpper() + " " + year;

            DateTime startOfMonth = new DateTime(year, month, 1);
            int day = DateTime.DaysInMonth(year, month);
            int week = Convert.ToInt32(startOfMonth.DayOfWeek.ToString("d"));

            if (week == 0)
            {
                week = 7;
            }

            for (int i = 1; i < week; i++)
            {
                ucDaysMonth uc = new ucDaysMonth("", datepicker);
                WrapPanelDays.Children.Add(uc);

            }

            for (int i = 1; i <= day; i++)
            {
                ucDaysMonth uc = new ucDaysMonth(i + "", datepicker);
                WrapPanelDays.Children.Add(uc);

               ListRefreshMonth(uc.gridEvents, new DateTime(year, month, i));
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ShowDays(SharedDateInfo.Month, SharedDateInfo.Year, _currentDay);
        }

        private void ListRefreshMonth(DataGrid dataGrid, DateTime date)
        {
            if (_windowState.tasks != null && WrapPanelDays.Children.Count == 0)
            {
                var list = _windowState.tasks.Where(x => x.Date == date.Date).ToList();

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = list;
            }
            if (_windowState.events != null && DaysPanel.Children.Count == 0)
            {
                var list = _windowState.events.Where(x => x.Date == date.Date).ToList();

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = list;
            }
        }

        private void addevent_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = new DateTime();
            date = datepicker.SelectedDate.Value;

            System.Windows.Media.Color mediaColor = ColorPickEvents.Color;

            System.Drawing.Color drawingColor = System.Drawing.Color.FromArgb(
            mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);

            if (textboxevent.Text != "")
            {
                _windowState.events.Add(MyEvent.createMyEvent(textboxevent.Text, date, drawingColor));
                textboxevent.Clear();
                ListRefresh();
                _windowState.Save();
                if (WrapPanelDays.Children.Count > 0)
                {
                    ShowDaysMonth(SharedDateInfo.Month, SharedDateInfo.Year);
                }
            }
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

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridTasks.SelectedItem is MyTask task)
            {
                _windowState.tasks.Remove(gridTasks.SelectedItem as MyTask);
                gridTasks.DataContext = _windowState.tasks;
            }
            if (gridEvents.SelectedItem is MyEvent myEvent)
            {
                _windowState.events.Remove(gridEvents.SelectedItem as MyEvent);
                gridEvents.DataContext = _windowState.events;
            }
            ListRefresh();
            _windowState.Save();
            if (DaysPanel.Children.Count > 0)
            {
                ShowDays(SharedDateInfo.Month, SharedDateInfo.Year, _currentDay);
            }
            else
            {
                ShowDaysMonth(SharedDateInfo.Month, SharedDateInfo.Year);
            }
        }

        public void RefreshWeeklyView()
        {
            ShowDays(SharedDateInfo.Month, SharedDateInfo.Year, _currentDay);
        }
    }
}