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
using Domain.Models;
using Service;
using Service.DTO;

namespace DesktopApp.UI.Pages
{
    /// <summary>
    /// Interaction logic for ArtistsPage.xaml
    /// </summary>
    public partial class ArtistsPage : Page
    {
        private readonly ObservableCollection<ArtistDto> _artists = new ObservableCollection<ArtistDto>();
        private readonly ArtistService _service;

        public ArtistsPage()
        {
            InitializeComponent();
            _service = new ArtistService();

            ArtistsDataGrid.ItemsSource = _artists;
            LoadData();
        }

        private void LoadData()
        {
            _artists.Clear();
            _service.Get().ForEach(_artists.Add);
        }

    }
}
