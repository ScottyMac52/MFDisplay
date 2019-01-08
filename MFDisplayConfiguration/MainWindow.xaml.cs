using log4net;
using MFDSettingsManager;
using System.Windows;
using System.Windows.Documents;

namespace MFDisplayConfiguration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Logger for the window
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// The configuration
        /// </summary>
        public MFDConfigurationSection Config { get; set; }

        public string ConfigurationFile { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConfigEditor_Loaded(object sender, RoutedEventArgs e)
        {
            Config = MFDConfigurationSection.GetConfig(ConfigurationFile);

            var model = ConfigSectionModelMapper.MapFromConfigurationSection(Config);
            AvailableModules = model.Modules;

            cbModules.ItemsSource = AvailableModules;
            cbModules.DisplayMemberPath = "DisplayName";
            cbModules.SelectedValuePath = "ModuleName";

        }
    }
}
