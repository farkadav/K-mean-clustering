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
using Microsoft.Win32;


namespace kMeanClust
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _canvasPoints.Children.Clear();
            DrawPoints(Data, CircleType.Normal);
        }

        private void Open_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !_isRunnig;
        }

        private void Open_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != true)
                return;

            // TODO: Load file content
        }

        private void _menuClearPoints_Click(object sender, RoutedEventArgs e)
        {
            Data.Clear();

            _canvasPoints.Children.Clear();
            _canvasResults.Children.Clear();
        }

        private List<Point> Data { get; }= new List<Point>()
        {
            new Point(2, 3),
            new Point(2, 7),
            new Point(4, 5),
            new Point(5, 2),
            new Point(9, 5),
            new Point(10, 10),
            new Point(12, 8),
            new Point(13, 5)
        };

        private void UpdateMenus()
        {
            _menuRun.IsEnabled = !_isRunnig;
            _menuClearPoints.IsEnabled = !_isRunnig;
        }

        private bool _isRunnig = false;

        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            _isRunnig = true;
            UpdateMenus();

            await CalculateAsync(Data);

            _isRunnig = false;
            UpdateMenus();
        }

        private const double eps = 0.0001;
        private async Task CalculateAsync(List<Point> data)
        {
            var newCenters = new List<Point>();
            var centers = new Center(data).Centers();
            var clusters = new List<List<Point>>();

            var n = 0;
            var terminate = false;

            _lblIterations.Text = "";
            while (!terminate)
            {
                await Task.Run(() =>
                {
                    Thread.Sleep(600);
                    clusters = new DistanceCalc(data, centers).SortPointsToClusters();
                    newCenters = clusters.Select(cluster => new CenterOfMass(cluster).Calculate()).ToList();
                });

                for (int i = 0; i < centers.Count; i++)
                {
                    if (double.IsNaN(centers[i].X) | double.IsNaN(centers[i].Y)) centers = new Center(data).Centers();
                    if (clusters[i].Count == 0) centers = new Center(data).Centers();

                }
                for (int i = 0; i < centers.Count; i++)
                {
                   
                    if (Math.Abs(centers[i].X - newCenters[i].X) < eps & Math.Abs(centers[i].Y - newCenters[i].Y) < eps)
                        terminate = true;
                    
                }
                

                centers = newCenters.ToList();

                RedrawClusters(data, newCenters,clusters);

                if (n > 20)
                    break;
                n++;
                _lblIterations.Text = $"Iterations:{n}";
            }
        }

        private const double Scale = 20.0;
        private void RedrawClusters(List<Point> data, List<Point> newCenters, List<List<Point>> clusters)
        {
            _canvasResults.Children.Clear();

            for (var i = 0; i < clusters.Count; ++i)
            {
                var center = newCenters[i];
                var clusterPoints = clusters[i];
                foreach (var point in clusterPoints)
                {
                    var line = new Line()
                    {
                        X1 = point.X*Scale,
                        Y1 = point.Y*Scale,
                        X2 = center.X*Scale,
                        Y2 = center.Y*Scale,
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray
                    };
                    _canvasResults.Children.Add(line);
                }
            }

            DrawPoints(newCenters, CircleType.Center);
        }

        private enum CircleType { Normal, Center }
        private void DrawPoints(List<Point> data, CircleType type)
        {
            var diameter = type == CircleType.Normal ? 10 : 6;
            var radius = diameter / 2.0;
            var color = type == CircleType.Normal ? Brushes.Black : Brushes.Red;
            foreach (var point in data)
            {
                var circle = new Ellipse() {Width = diameter, Height = diameter, Fill = color};
                Canvas.SetLeft(circle, point.X*Scale - radius);
                Canvas.SetTop(circle, point.Y*Scale - radius);
                (type == CircleType.Normal ? _canvasPoints : _canvasResults).Children.Add(circle);
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isRunnig)
                return;

            var position = Mouse.GetPosition(_canvasPoints);
            Data.Add(new Point(position.X/Scale, position.Y/Scale));

            _canvasPoints.Children.Clear();
            _canvasResults.Children.Clear();
            DrawPoints(Data, CircleType.Normal);
        }
    }
}
