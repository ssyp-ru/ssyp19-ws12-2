using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using JetBrains.Annotations;
using Ssyp.Communicator.CommonClient;

namespace Ssyp.Communicator.ClientXaml
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeMessageSyncing();
            Task.Run(DisplayNickname);
            ComposeButton.Click += (sender, args) => { CurrentInterlocutor = NickToComposeTo.Text; };

            var sb = new ScrollBar();
            sb.Orientation = Orientation.Vertical;

            SendButton.Click += (sender, args) =>
            {
                if (CurrentInterlocutor != null)
                    Task.Run(() =>
                    {
                        Task t = null;

                        Dispatcher.Invoke(() =>
                        {
                            t = Requests.RequestConversationSend(CurrentInterlocutor, MessageToSend.Text);
                        });

                        return t;
                    });
            };

            Closed += (sender, args) => { Syncing = false; };
        }

        [CanBeNull] private string CurrentInterlocutor { get; set; }
        private bool Syncing { get; set; } = true;
        [CanBeNull] private string NicknameCache { get; set; }

        private void InitializeMessageSyncing() => new Thread(() =>
        {
            while (Syncing)
            {
                var conversations1 = Requests.RequestConversationList()?.Result?.Conversations;
                Debug.Assert(conversations1 != null, nameof(conversations1) + " != null");
                Dispatcher.Invoke(() => ConversationsList.Children.Clear());

                conversations1.ForEach(it =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        var button = new Button
                        {
                            Content = it.Interlocutor,
                            Width = 182,
                            Height = 50,
                            FontFamily = new FontFamily("Segoe UI"),
                            FontSize = 20
                        };

                        button.Click += (sender, args) => { CurrentInterlocutor = it.Interlocutor; };
                        ConversationsList.Children.Add(button);
                    });
                });

                Dispatcher.Invoke(() => Messages.Children.Clear());

                conversations1
                    .Find(it => it.Interlocutor == CurrentInterlocutor)
                    ?.Messages
                    .ForEach(it => Dispatcher.Invoke(() =>
                        Messages.Children.Add(new TextBlock()
                        {
                            FontFamily = new FontFamily("Segoe UI"),
                            FontSize = 20,
                            Text = $"{it.Sender}: {it.Value}"
                        })));

                Thread.Sleep(5000);
            }
        }).Start();

        private async Task DisplayNickname()
        {
            var t = Requests.RequestUserInfoOwn();

            if (t == null)
                return;

            var nameFromRequest = (await t).Name;
            Debug.Assert(nameFromRequest != null, nameof(nameFromRequest) + " != null");

            Dispatcher.Invoke(() =>
            {
                NicknameCache = nameFromRequest;
                Nickname.Text = nameFromRequest;

                Nickname.TextChanged += (sender, args) =>
                {
                    var text = Nickname.Text;
                    var l = text.Length;

                    if (l != 0 && l <= 16 && !text.Contains(" "))
                    {
                        NicknameCache = Nickname.Text;
                        Task.Run(() =>
                        {
                            Task task = null;
                            Dispatcher.Invoke(() => { task = Requests.RequestUserModify(Nickname.Text); });
                            return task;
                        });
                    }
                    else
                        Nickname.Text = NicknameCache;
                };
            });
        }
    }
}