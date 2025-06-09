using System.Diagnostics;
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

namespace SwarmCleaner;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    List<Process> TBApps = new List<Process>();
    Thread? refreshThread = null;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }
    void SwarmClick(object sender, RoutedEventArgs e)
    {
        Task.Run(() => RefreshSwarm());
    }


    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void RefreshSwarm()
    {

        if (SwarmIsOpen())
        {
            foreach (Process process in TBApps)
            {
                if (!process.HasExited)
                {
                    // Graceful close (if supported)
                    process.CloseMainWindow();
                    process.WaitForExit(2000); // wait 2 seconds

                    // If it's still not closed, force kill
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
            }
        }
        Thread.Sleep(3000);
        if (!SwarmIsOpen())
        {


            Process.Start(@"C:\Program Files\Turtle Beach Swarm II\Turtle Beach Swarm II.exe");
            //"C:\Program Files\Turtle Beach Swarm II\Turtle Beach Swarm II.exe"
        }

    }

    private bool SwarmIsOpen()
    {
        TBApps.Clear();
        TBApps.AddRange(Process.GetProcessesByName("Turtle Beach Swarm II"));
        TBApps.AddRange(Process.GetProcessesByName("Turtle Beach Device Service"));

        return TBApps.Count >= 1;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}