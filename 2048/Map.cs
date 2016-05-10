using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace _2048
{
    class title
    {
        public int index;
        public bool empty;

        public title()
        {
            empty = true;
            index = 0;
        }

        public title(title t)
        {
            empty = t.empty;
            index = t.index;
        }

        public title(int setVal)
        {
            empty = false;
            index = setVal;
        }

        public void Initialize(int value = 0)
        {
            empty = false;
            if (value == 0)
                index = share.Rand(9, 1) + 1;
            else
                index = value;
        }

        public bool canCombine(title t)
        {
            return index != 0 && t.index == index;
        }

        public int Combine(title t)
        {
            if (!canCombine(t))
                return 0;
            return ++index;
        }

        public string getIndex()
        {
            if (empty || index == 0)
                return "";
            if (index < Map.value.Length)
                return Map.value[index].ToString();
            return "-1";
        }

        public Brush titleBrush
        {
            get
            {
                if (this.empty || index == 0)
                    return Brushes.Transparent;
                if (index < Map.titleColor.Length)
                    return Map.titleColor[index];
                return Brushes.Black;
            }
            set { }
        }
    }

    class Map
    {
        public title[,] array;
        public int points = 0;
        public int H = 4, W = 4;
        public int freeTitles = 16;
        public bool isGameEnded = false;

        public const int maxSize = 15;
        public static Brush[] titleColor =
        {
            Brushes.Transparent, Brushes.LightYellow, Brushes.LightGoldenrodYellow, Brushes.Yellow, Brushes.GreenYellow, Brushes.Orange, Brushes.Tomato, Brushes.Red,
            Brushes.LightBlue, Brushes.DeepSkyBlue, Brushes.Violet, Brushes.Indigo,
        };

        public static int[] value = new int[maxSize];
        
        public Map(int hight = 4, int width = 4)
        {
            H = hight;
            W = width;
            array = new title[H, W];
            freeTitles = H * W;
            for (int i = 0; i < H; i++)
                for (int j = 0; j < W; j++)
                    array[i, j] = new title();
            value[0] = 0;
            value[1] = 2;
            for (int i = 2; i < maxSize; i++)
                value[i] = value[i - 1] + value[i - 1];
        }

        public int nextValue(int prev, int cur)
        {
            return prev + cur;
        }

        public void NewGame(int titleCount)
        {
            for (int i = 0; i < H; i++)
                for (int j = 0; j < W; j++)
                    if (!array[i, j].empty)
                        array[i, j] = new title();
            freeTitles = H * W;
            points = 0;
            isGameEnded = false;
            for (int i = 0; i < titleCount; i++)
                AddTittle();
        }

        public void AddTittle(int value = 0)
        {
            if (freeTitles <= 0)
            {
                MessageBox.Show("No free space :(");
                return;
            }
            while (true)
            {
                int x = share.Rand(W);
                int y = share.Rand(H);
                if (array[y, x].empty)
                {
                    array[y, x].Initialize(value);
                    break;
                }
            }
            freeTitles--;
        }

        void Swap(int i1, int j1, int i2, int j2)
        {
            title tmp = new title(array[i1, j1]);
            array[i1, j1] = new title(array[i2, j2]);
            array[i2, j2] = new title(tmp);
        }

        public bool MoveLeft()
        {
            bool ok = false;
            for (int i = 0; i < H; i++)
                for (int j = 1; j < W; j++)
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (array[i, k].canCombine(array[i, k + 1]))
                        {
                            points += array[i, k].Combine(array[i, k + 1]);
                            array[i, k + 1] = new title();
                            freeTitles++;
                            ok = true;
                            break;
                        }
                        else
                        {
                            if (array[i, k].empty && !array[i, k + 1].empty)
                            {
                                Swap(i, k, i, k + 1);
                                ok = true;
                            }
                            else
                                break;
                        }
                    }  
            return ok;
        }

        public bool MoveRight()
        {
            bool ok = false;
            for (int i = 0; i < H; i++)
                for (int j = W - 2; j >= 0; j--)
                    for (int k = j + 1; k < W; k++)
                    {
                        if (array[i, k].canCombine(array[i, k - 1]))
                        {
                            points += array[i, k].Combine(array[i, k - 1]);
                            array[i, k - 1] = new title();
                            freeTitles++;
                            ok = true;
                            break;
                        }
                        else
                        {
                            if (array[i, k].empty && !array[i, k - 1].empty)
                            {
                                Swap(i, k, i, k - 1);
                                ok = true;
                            }
                            else
                                break;
                        }
                    }
            return ok;
        }

        public bool MoveUp()
        {
            bool ok = false;
            for (int j = 0; j < W; j++)
                for (int i = 1; i < H; i++)
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (array[k, j].canCombine(array[k + 1, j]))
                        {
                            points += array[k, j].Combine(array[k + 1, j]);
                            array[k + 1, j] = new title();
                            freeTitles++;
                            ok = true;
                            break;
                        }
                        else
                        {
                            if (array[k, j].empty && !array[k + 1, j].empty)
                            {
                                Swap(k, j, k + 1, j);
                                ok = true;
                            }
                            else
                                break;
                        }
                    }
            return ok;
        }

        public bool MoveDown()
        {
            bool ok = false;
            for (int j = 0; j < W; j++)
                for (int i = H - 2; i >= 0; i--)
                    for (int k = i + 1; k < H; k++)
                    {
                        if (array[k, j].canCombine(array[k - 1, j]))
                        {
                            points += array[k, j].Combine(array[k - 1, j]);
                            array[k - 1, j] = new title();
                            freeTitles++;
                            ok = true;
                            break;
                        }
                        else
                        {
                            if (array[k, j].empty && !array[k - 1, j].empty)
                            {
                                Swap(k, j, k - 1, j);
                                ok = true;
                            }
                            else
                                break;
                        }
                    }
            return ok;
        }

        public bool CanMove()
        {
            if (freeTitles > 0)
                return true;
            for (int i = 0; i < H; i++)
                for (int j = 0; j < W; j++)
                {
                    if (i + 1 < H && array[i, j].canCombine(array[i + 1, j]))
                        return true;
                    if (j + 1 < W && array[i, j].canCombine(array[i, j + 1]))
                        return true;
                }
            return false;
        }

        public int MaxTitle()
        {
            if (freeTitles == H * W)
                return -1;
            int mx = -1;
            for (int i = 0; i < H; i++)
                for (int j = 0; j < W; j++)
                    mx = Math.Max(mx, array[i, j].index);
            return mx;
        }
    }
}
