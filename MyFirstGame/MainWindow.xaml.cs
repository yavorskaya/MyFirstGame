using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace MyFirstGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        int secondsElapsed;
        int timeToMemorise = 20000;
        int matchFound = 8;

        public MainWindow()
        {
            InitializeComponent();
            

            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispatcherTimer.Tick += Timer_Tick;
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeToMemorise == 0)
            {
                secondsElapsed += 1000;
                txtblockTime.Text = (secondsElapsed / 1000).ToString("0 s");
                if (matchFound == 8)
                {
                    dispatcherTimer.Stop();
                    txtblockTime.Text = txtblockTime.Text;
                    btnPlay.Visibility = Visibility.Visible;
                    btnPlay.Content = "Play again";
                }
            }
            else
            {
                timeToMemorise -= 1000;
                txtblockTime.Text = (timeToMemorise / 1000).ToString("0 s");
                if (timeToMemorise == 0)
                    HideAnimals();
            }
        }

        private void SetUpGame()
        {
            // Windows + . отображает панель эмодзи
            List<string> animalEmoji = new List<string>()
            {
                "🐙", "🐙",
                "🐬", "🐬",
                "🐘", "🐘",
                "🐳", "🐳",
                "🐪", "🐪",
                "🦕", "🦕",
                "🦘", "🦘",
                "🦔", "🦔"
            };

            Random random = new Random();
            foreach(Grid grid in mainGrid.Children.OfType<Grid>())
            {
                TextBlock tb = grid.Children[0] as TextBlock; 
                int index = random.Next(animalEmoji.Count);
                string nextEmoji = animalEmoji[index];
                tb.Text = nextEmoji;
                animalEmoji.RemoveAt(index);
                tb.Visibility = Visibility.Visible;
            }
            dispatcherTimer.Start();
            matchFound = secondsElapsed = 0;
            timeToMemorise = 20000;
            btnPlay.Visibility=Visibility.Hidden;
            mainGrid.IsHitTestVisible = findingMatch = false;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void HideAnimals()
        {
            mainGrid.IsHitTestVisible = true;
            foreach (Grid grid in mainGrid.Children.OfType<Grid>())
            {
                TextBlock tb = grid.Children[0] as TextBlock;
                tb.Visibility = Visibility.Hidden;
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            TextBlock tb = grid?.Children[0] as TextBlock;
            if (tb == null)
                return;
            tb.Visibility = Visibility.Visible;
            if (!findingMatch)
            {
                lastTextBlockClicked = tb;
                findingMatch = true;
            }
            else if (tb.Text == lastTextBlockClicked.Text)
            {
                findingMatch = false;
                ++matchFound;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (matchFound == 8)
                SetUpGame();
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            TextBlock tb = grid?.Children[0] as TextBlock;
            if (tb == null)
                return;
            tb.Visibility = Visibility.Visible;
            if (findingMatch && tb.Text != lastTextBlockClicked.Text)
            {
                lastTextBlockClicked.Visibility = tb.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
        }
    }
}
