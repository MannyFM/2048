﻿using System;
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
        Label[,] labels;
        public MainWindow()
        {
            canvas = new Canvas();
            InitializeComponent();
            map = new Map(share.H, share.W);
            map.NewGame(share.initialTitleCount);
            grid = new Grid
            {
                ShowGridLines = true,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = share.H * (share.title_h),
                Width = share.W * (share.title_w),
                IsEnabled = false
                //Background = Brushes.Aqua
            };
            this.Width = grid.Width + 16;
            this.Height = grid.Height + 38;
            this.ResizeMode = ResizeMode.NoResize;

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
            labels = new Label[share.H, share.W];
            for (int i = 0; i < share.H; i++)
            {
                for (int j = 0; j < share.W; j++)
                {
                    labels[i, j] = new Label
                    {
                        Content = map.array[i, j].index.ToString(),
                        FontSize = share.textSize,
                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                        VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                        //Background = map.array[i, j].titleBrush,
                    };
                    zIndex++;
                    grid.Children.Add(labels[i, j]);
                    Grid.SetZIndex(labels[i, j], zIndex);
                    Grid.SetColumn(labels[i, j], j);
                    Grid.SetRow(labels[i, j], i);
                }
            }

            /*
            int cnt = 1;
            for (int i = 0; i < share.H; i++)
                for (int j = 0; j < share.W; j++)
                {
                    map.array[i, j].Initialize(cnt);
                    cnt += cnt;
                }
            */
            Draw();
            
        }
        
        void Draw()
        {
            this.Title = "Pts: " + map.points.ToString();
            for (int i = 0; i < share.H; i++)
                for (int j = 0; j < share.W; j++)
                {
                    labels[i, j].Content = map.array[i, j].getIndex();
                    labels[i, j].Background = map.array[i, j].titleBrush;
                    //MessageBox.Show("text: |" + textBox[i, j].Text + "| " + i + " " + j + " " + Canvas.GetZIndex(textBox[i, j]));
                }
            int mx = map.MaxTitle();
            if (mx == share.winTitle)
                MessageBox.Show("You won!");
        }

        void CheckCanIMove()
        {
            if (map.CanMove())
                return;
            MessageBox.Show("Game Over. Your Score is " + map.points.ToString(), "Game over");
            map.isGameEnded = true;
        }

        bool StartNewGame()
        {
            if (map.isGameEnded)
            {
                map.NewGame(share.initialTitleCount);
                return true;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "New Game", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    map.NewGame(share.initialTitleCount);
                    return true;
                }
            }
            return false;
        }
        
        void WPFOnKeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.Key.ToString());
            if (e.Key == Key.R && ((Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control)))
            {
                if (StartNewGame())
                    Draw();
            }
            if (e.Key == Key.Up)
                if (map.MoveUp())
                {
                    map.AddTittle();
                    Draw();
                }
            if (e.Key == Key.Down)
                if (map.MoveDown())
                {
                    map.AddTittle();
                    Draw();
                }
            if (e.Key == Key.Right)
                if (map.MoveRight())
                {
                    map.AddTittle();
                    Draw();
                }
            if (e.Key == Key.Left)
                if (map.MoveLeft())
                {
                    map.AddTittle();
                    Draw();
                }
            if (!map.isGameEnded)
                CheckCanIMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.KeyDown += new KeyEventHandler(WPFOnKeyDown);
        }

    }

    static public class share
    {
        static Random random = new Random();

        public static int Points = 0;
        public const int H = 4;
        public const int W = 4;

        public const int title_h = 100;
        public const int title_w = 100;

        public const int textSize = 40;

        public const int initialTitleCount = 4;
        public const int winTitle = 2048;

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
