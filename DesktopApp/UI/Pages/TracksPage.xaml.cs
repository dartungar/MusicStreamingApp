using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Service;
using Service.DTO;
using DesktopApp.Utils;

namespace DesktopApp.UI.Pages
{
    /// <summary>
    /// Interaction logic for TracksPage.xaml
    /// </summary>
    public partial class TracksPage : Page
    {
        private readonly ObservableCollection<TrackDto> _tracks = new ObservableCollection<TrackDto>();
        private readonly TrackService _service;

        public TracksPage()
        {
            InitializeComponent();
            _service = new TrackService();
            
            TracksDataGrid.ItemsSource = _tracks;
            LoadData();
        }

        private void LoadData()
        {
            _tracks.Clear();
            _service.Get().ForEach(_tracks.Add);
        }

        private void TracksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
