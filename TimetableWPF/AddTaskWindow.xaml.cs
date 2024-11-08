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
using System.Windows.Shapes;
using TimetableWPF.DTO;

namespace TimetableWPF
{
    public partial class AddTaskWindow : Window
    {
        public WindowStateJson _windowState;
        private string choosenCategory;
        private DatePicker _datepicker;
        private ColorPicker _colorPicker;
        private DataGrid _dataGrid;
        private string _day;
        public event Action TaskAdded;

        public AddTaskWindow(DatePicker datepicker, ColorPicker colorPicker, DataGrid dataGrid, WindowStateJson windowState, string day)
        {
            InitializeComponent();
            this.DataContext = this;

            _datepicker = datepicker;
            _colorPicker = colorPicker;
            _dataGrid = dataGrid;
            _windowState = windowState;
            _day = day;
            ListRefresh();
        }

        private void ChooseCategory_Loaded(object sender, RoutedEventArgs e)
        {
            ChooseCategory.SelectedValuePath = "CategoryId";
            ChooseCategory.DisplayMemberPath = "CategoryName";
            foreach (var category in _windowState.categories)
            {
                ChooseCategory?.Items?.Add(category);
            }
        }

        private void ChooseCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            choosenCategory = ChooseCategory?.SelectedItem?.ToString();
        }

        private void addtask_Click(object sender, RoutedEventArgs e)
        {

            if (textbox.Text != null)
            {
                int day = Int32.Parse(_day);
                DateTime date = new DateTime(SharedDateInfo.Year, SharedDateInfo.Month, day);
                
                System.Windows.Media.Color mediaColor = _colorPicker.Color;

                System.Drawing.Color drawingColor = System.Drawing.Color.FromArgb(
                    mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);

                int importanceSelectedIndex = ChooseImportance.SelectedIndex + 1;
                if (importanceSelectedIndex != 0 && textbox.Text != "")
                {
                    Guid id = (Guid)ChooseCategory?.SelectedValue;
                    _windowState.tasks.Add(MyTask.createTask(textbox.Text, date, importanceSelectedIndex, id));
                    textbox.Clear();
                    ListRefresh();
                    _windowState.Save();

                    TaskAdded?.Invoke();
                }
            }
        }

        private void ListRefresh()
        {
            DateTime selectedDate = _datepicker.SelectedDate.Value;
            var list = _windowState?.tasks?.Where(x => x.Date.Date == selectedDate)?.ToList();
            _dataGrid.ItemsSource = list;
        }
    }
}
