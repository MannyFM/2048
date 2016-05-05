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
        public int val;
        public bool empty;

        public title()
        {
            empty = true;
            val = -1;
        }

        public title(title t)
        {
            empty = t.empty;
            val = t.val;
        }

        public title(int setVal)
        {
            empty = false;
            val = setVal;
        }

        public void Initialize()
        {
            empty = false;
            val = share.Rand(9, 1) + 1;
        }

        public void Initialize(int value)
        {
            empty = false;
            val = value;
        }

        public bool canCombine(title t)
        {
            return val != -1 && t.val == val;
        }

        public int Combine(title t)
        {
            if (!canCombine(t))
                return 0;
            val += t.val;
            return val;
        }

        public string getValue()
        {
            if (empty)
                return "";
            return val.ToString();
        }

        public Brush titleBrush
        {
            get
            {
                if (this.empty)
                    return Brushes.Transparent;
                if (this.val == 1)
                    return Brushes.LightYellow;
                if (this.val == 2)
                    return Brushes.LightGoldenrodYellow;
                if (this.val == 4)
                    return Brushes.Yellow;
                if (this.val == 8)
                    return Brushes.GreenYellow;
                if (this.val == 16)
                    return Brushes.YellowGreen;
                if (this.val == 32)
                    return Brushes.Orange;
                if (this.val == 64)
                    return Brushes.Tomato;
                if (this.val == 128)
                    return Brushes.Red;
                if (this.val == 256)
                    return Brushes.LightBlue;
                if (this.val == 512)
                    return Brushes.DeepSkyBlue;
                if (this.val == 1024)
                    return Brushes.Violet;
                if (this.val == 2048)
                    return Brushes.Indigo;

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

        public Map()
        {
            H = 4;
            W = 4;
        }

        public Map(int hight, int width)
        {
            H = hight;
            W = width;
            freeTitles = H * W;
            array = new title[H, W];
            for (int i = 0; i < H; i++)
                for (int j = 0; j < W; j++)
                    array[i, j] = new title();
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

        public void AddTittle()
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
                    array[y, x].Initialize();
                    break;
                }
            }
            freeTitles--;
        }

        public void AddTittle(int value)
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
    }
}
