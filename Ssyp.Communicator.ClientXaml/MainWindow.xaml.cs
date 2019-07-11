using System.Windows;
using Ssyp.Communicator.CommonClient;

namespace Ssyp.Communicator.ClientXaml
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var messageSyncing = MessageSyncing.StartMessageSyncing(
                () => { },
                it => { });

            Closed += (sender, args) => { messageSyncing.Abort(); };
        }
    }
}