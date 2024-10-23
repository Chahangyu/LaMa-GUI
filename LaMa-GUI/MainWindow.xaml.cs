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

using Microsoft.Win32;

namespace LaMa_GUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private double brushSize = 10;
    private bool isSelecting = false;
    private Image? image;
    private ScaleTransform scaleTransform = new ScaleTransform();

    public MainWindow()
    {
        InitializeComponent();

        DrawingCanvas.MouseDown += DrawingCanvas_MouseDown;
        DrawingCanvas.MouseMove += DrawingCanvas_MouseMove;
        DrawingCanvas.MouseUp += DrawingCanvas_MouseUp;

        DrawingCanvas.MouseWheel += DrawingCanvas_MouseWheel;

        DrawingCanvas.RenderTransform = scaleTransform;
    }

    private void GetImageButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "이미지 파일 (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
        if (openFileDialog.ShowDialog() == true)
        {
            if (image != null && DrawingCanvas.Children.Contains(image))
            {
                DrawingCanvas.Children.Remove(image);
            }

            image = new Image();
            image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            DrawingCanvas.Children.Add(image);
        }
    }

    private void SelectObjectButton_Click(object sender, RoutedEventArgs e)
    {
        isSelecting = !isSelecting;
        SelectObjectButton.Content = isSelecting ? "선택 모드 종료" : "객체 선택";
    }

    private void BrushSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        brushSize = e.NewValue;
    }

    private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (isSelecting)
        {
            DrawEllipse(e.GetPosition(DrawingCanvas));
        }
    }

    private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (isSelecting && e.LeftButton == MouseButtonState.Pressed)
        {
            DrawEllipse(e.GetPosition(DrawingCanvas));
        }
    }

    private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
    }

    private void DrawEllipse(Point position)
    {
        Ellipse ellipse = new Ellipse
        {
            Fill = Brushes.Red,
            Width = brushSize,
            Height = brushSize,
            Opacity = 0.5
        };
        Canvas.SetLeft(ellipse, position.X - brushSize / 2);
        Canvas.SetTop(ellipse, position.Y - brushSize / 2);
        DrawingCanvas.Children.Add(ellipse);
    }

    private void DrawingCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        Point mousePosition = e.GetPosition(DrawingCanvas);

        double scaleFactor = e.Delta > 0 ? 1.1 : 0.9;

        scaleTransform.ScaleX *= scaleFactor;
        scaleTransform.ScaleY *= scaleFactor;

        scaleTransform.CenterX = mousePosition.X;
        scaleTransform.CenterY = mousePosition.Y;
    }
}