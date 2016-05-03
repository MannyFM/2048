using System;
using System.Collections.Generic;
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
using System.Drawing;

namespace _2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Canvas canvas;
        Map map;
        Grid grid;
        TextBox[,] textBox;
        public MainWindow()
        {
            canvas = new Canvas();
            InitializeComponent();
            map = new Map();
            grid = new Grid
            {
                ShowGridLines = true,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = share.H * share.title_h,
                Width = share.W * share.title_w,
                //Background = Brushes.Aqua
            };
            mainGrid.Children.Add(grid);
            for (int i = 0; i < share.H; i++)
            {
                RowDefinition row = new RowDefinition
                {
                    Height = new GridLength(share.title_h),
                    IsEnabled = true
                };
                grid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < share.W; i++)
            {
                ColumnDefinition column = new ColumnDefinition
                {
                    Width = new GridLength(share.title_w)
                };
                grid.ColumnDefinitions.Add(column);
            }
            int zIndex = Canvas.GetZIndex(grid);
            textBox = new TextBox[share.H, share.W];
            for (int i = 0; i < share.H; i++)
            {
                for (int j = 0; j < share.W; j++)
                {
                    textBox[i, j] = new TextBox
                    {
                        Text = map.array[i, j].val.ToString(),
                    };
                    zIndex++;
                    grid.Children.Add(textBox[i, j]);
                    Grid.SetZIndex(textBox[i, j], zIndex);
                    Grid.SetColumn(textBox[i, j], j);
                    Grid.SetRow(textBox[i, j], i);
                }
            }
            Draw();
        }

        void Draw()
        {
            pointsLabel.Content = "Pts: " + map.points.ToString();
            for (int i = 0; i < share.H; i++)
                for (int j = 0; j < share.W; j++)
                {
                    textBox[i, j].Text = map.array[i, j].getValue();
                    //MessageBox.Show("text: |" + textBox[i, j].Text + "| " + i + " " + j + " " + Canvas.GetZIndex(textBox[i, j]));
                }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            map.AddTittle();
            Draw();
        }

        private void moveLeftButoon_Click(object sender, RoutedEventArgs e)
        {
            map.MoveLeft();
            Draw();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            map.MoveRight();
            Draw();
        }

        private void moveUpButton_Click(object sender, RoutedEventArgs e)
        {
            map.MoveUp();
            Draw();
        }

        private void moveDownButton_Click(object sender, RoutedEventArgs e)
        {
            map.MoveDown();
            Draw();
        }
    }
    static public class share
    {
        static Random random = new Random();

        public static int Points = 0;
        public const int H = 4;
        public const int W = 4;

        public const int title_h = 50;
        public const int title_w = 50;

        public static int Rand()
        {
            return random.Next();
        }

        public static int Rand(int MaxVal)
        {
            return random.Next(MaxVal);
        }

        public static int Rand(int Prob1, int Prob2)
        {
            int val = random.Next(Prob1 + Prob2);
            if (val < Prob1)
                return 0;
            else
                return 1;
        }
    }
}
