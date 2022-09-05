using System.Windows.Controls;

namespace DentalClientWithRESTService.Views
{
    /// <summary>
    /// Interaction logic for HttpResponseView.xaml
    /// </summary>
    public partial class HttpResponseView : UserControl
    {
        public HttpResponseView()
        {
            InitializeComponent();
        }

        public HttpResponseView(string msg) : this()
        {
            httpMessage.Text = msg;
        }

        private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowExtension.ActualPopupWindow.Close();
        }
    }
}
