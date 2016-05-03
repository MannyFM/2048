﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

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
    }
    class Map
    {
        public title[,] array;
        public int points = 0;
        public int freeTitles = share.H * share.W;
        public Map()
        {
            array = new title[share.H, share.W];
            for (int i = 0; i < share.H; i++)
                for (int j = 0; j < share.W; j++)
                    array[i, j] = new title();
        }

        public void AddTittle()
        {
            if (freeTitles <= 0)
            {
                MessageBox.Show("No free space");
                return;
            }
            while (true)
            {
                int x = share.Rand(share.W);
                int y = share.Rand(share.H);
                if (array[x, y].empty)
                {
                    array[x, y].Initialize();
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
            for (int i = 0; i < share.H; i++)
                for (int j = 1; j < share.W; j++)
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
            for (int i = 0; i < share.H; i++)
                for (int j = share.W - 2; j >= 0; j--)
                    for (int k = j + 1; k < share.W; k++)
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
            for (int j = 0; j < share.W; j++)
                for (int i = 1; i < share.H; i++)
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
            for (int j = 0; j < share.W; j++)
                for (int i = share.H - 2; i >= 0; i--)
                    for (int k = i + 1; k < share.H; k++)
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

    }
}