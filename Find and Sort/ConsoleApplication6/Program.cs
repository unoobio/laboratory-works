using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            ///ПОИСК ЭЛЕМЕНТА В МАССИВЕ
            var Ran = new Random();
            int s = 90000;
            long[] mass3 = new long[s];
            int a = Ran.Next(1, s);
            for (int i = 0; i < s; i++)
            {
                mass3[i] = i;
            }
            OurList<long> list2 = new OurList<long>();
            for (int i = 1; i < s; i++)
            {
                list2.Add(i);
            }
            Console.WriteLine("Нашли число " + a + " за " + BinarySearchOurList(list2, a) + " шага(ов).");
            Console.WriteLine("Нашли число " + a + " за " + BinarySearch(mass3, a) + " шага(ов).");             
             
            
            
            
            
            int count = 0;
            Console.WriteLine("Введите длину массива/списка для сортировки");
            int lenght = int.Parse(Console.ReadLine());
            
            int[] mass = new int[lenght];
            int[] mass2 = new int[lenght];
            
            
            for (int i = 0; i < lenght; i++)
            {
                mass[i] = Ran.Next(1, lenght);
            }
            Array.Copy(mass, mass2, mass.Length);
            OurList<int> list = new OurList<int>();
            for (int i = 0; i < mass.Length; i++)
            {
                list.Add(mass[i]);
            }

            
            
            ///СОРТИРОВКА МАССИВА ВСТАВКОЙ
            Console.WriteLine("\nИсходный массив:");
            for (int i = 0; i < mass.Length; i++)
            {
                Console.Write(mass[i] + " ");
            }

            count = InsertSort(mass);
            
            Console.WriteLine("\nОтсортированный массив: ");
            for (int i = 0; i < mass.Length; i++)
            {
                Console.Write(mass[i] + " ");
            }
            Console.WriteLine("\nКоличество итерраций при сортировке массива вставкой длинной " + mass.Length + ": " + count);
           

            ///СОРТИРОВКА СПИСКА ВСТАВКОЙ

            Console.WriteLine("\nИсходный список:");
            for (int i = 0; i < list.Lenght(); i++)
            {
                Console.Write(list.Get(i) + " ");
            }
            
            count = InsertSort(list);

            Console.WriteLine("\nОтсортированный список: ");
            for (int i = 0; i < list.Lenght(); i++)
            {
                Console.Write(list.Get(i) + " ");
            }
            Console.WriteLine("\nКоличество итерраций при сортировке списка вставкой длинной " + list.Lenght() + ": " + count);
             

            ///СОРТИРОВКА МАССИВА КОРЗИНКАМИ

            Console.WriteLine("\nИсходный массив:");
            for (int i = 0; i < mass2.Length; i++)
            {
                Console.Write(mass2[i] + " ");
            }
            Console.WriteLine("\nВведите количество корзинок");
            int n = int.Parse(Console.ReadLine());
            count = BucketSort(mass2, 10);
            
            Console.WriteLine("\nОтсортированный массив: ");
            for (int i = 0; i < mass2.Length; i++)
            {
                Console.Write(mass2[i] + " ");
            }

            Console.WriteLine("\nКоличество итерраций при сортировке массива корзинками длинной " + mass2.Length + ": " + count);
            Console.ReadKey();
        }

        static public int Vlob(long[] mass, int a)
        {
            int length = mass.Length;
            int element = 0;

            for (int k = 0; k < length; k++)
            {
                if ((mass[k]) == a)
                {
                    element = k;
                    break;
                }
            }
            return element;
        }

        static public int BinarySearch(long[] mass, int a)
        {
            int length = mass.Length;
            int element = 0;
            int half = length / 2;
            int half2 = length / 2;
            for (int k = 0; k < length; k++)
            {

                if (a > mass[half])
                {
                    if (half2 % 2 == 0) half2 = half2 / 2;
                    else half2 = half2 / 2 + 1;
                    half = half + half2;
                    element++;
                }
                else
                    if (a < mass[half])
                    {
                        if (half2 % 2 == 0) half2 = half2 / 2;
                        else half2 = half2 / 2 + 1;
                        half = half - half2;
                        element++;
                    }
                    else
                        if (a == mass[half])
                        {
                            element++;
                            break;
                        }
            }
            return element;
        }

        static public int BinarySearchOurList(OurList<long> Ourlist, int a)
        {
            int length = Ourlist.Lenght();
            int element = 0;
            int half = length / 2;
            int half2 = length / 2;
            for (int k = 0; k < length; k++)
            {

                if (a > Ourlist.Get(half))
                {
                    if (half % 2 == 0) half2 = half2 / 2;
                    else half2 = half2 / 2 + 1;
                    half = half + half2;
                    element++;
                    element += half;
                }
                else
                    if (a < Ourlist.Get(half))
                    {
                        if (half % 2 == 0) half2 = half2 / 2;
                        else half2 = half2 / 2 + 1;
                        half = half - half2;
                        element++;
                        element += half;
                    }
                    else
                        if (a == Ourlist.Get(half))
                        {
                            element++;
                            element += half;
                            break;
                        }
            }
            return element;
        }

        public static int InsertSort(int[] mass)
        {
            int count = 0;
            for (int i = 0; i < mass.Length; i++)
            {
                int remember = mass[i];
                int place = 0;
                
                while (remember < mass[place])
                {
                    place = place + 1;
                    count++;
                }
                if (place < i)
                {
                    for (int j = i; j > place; j--)
                    {
                        mass[j] = mass[j - 1];
                        count++;
                    }
                    mass[place] = remember;
                }
                count++;
            }
            return count;
        }

        public static int InsertSort(OurList<int> list)
        {
            int count = 0;
            for (int i = 0; i < list.Lenght(); i++)
            {
                int remember = list.Get(i); count += i;
                int place = 0;
                while (remember < list.Get(place))
                {
                    count += place;
                    place = place + 1;
                    count++;
                }
                if (place < i)
                {
                    for (int j = i; j > place; j--)
                    {
                        list.Set(list.Get(j - 1), j);
                        count += j - 1 + j;
                        count++;
                    }
                    list.Set(remember, place);
                    count += place;
                }
                count++;
            }
            return count;
        }

        public static int BucketSort(int[] mass, int n)
        {
            int[][] buckets = new int[n][];
            SortedDictionary<int, int> map = new SortedDictionary<int, int>();
            try
            {
                int max = mass.Max(), min = mass.Min();
                int count = 0;
                if (mass.Length < 2 || min == max) return count;
                int range = max - min;
                double step = (double)range / n + 0.1;
                for (int i = 0; i < n; i++)
                {
                    int size = mass.Count(a => a >= min + i * step && a < min + (i + 1) * step);
                    buckets[i] = new int[size];
                    map.Add(i, 0);
                    count++;
                }
                for (int i = 0; i < mass.Length; i++)
                {
                    int number = (int)((mass[i] - min) / step);
                    buckets[number][map[number]] = mass[i];
                    map[number]++;
                    count++;
                }
                for (int i = 0; i < n; i++)
                {
                    count += BucketSort(buckets[i], n);
                }
                int k = mass.Length - 1;
                for (int i = 0; i < n; i++)
                {
                    for (int j = buckets[i].Length - 1; j >= 0; j--)
                    {
                        mass[k] = buckets[i][j];
                        k--;
                        count++;
                    }

                }
                return count;
            }
            catch (InvalidOperationException e)
            {
                return 0;
            }
        }

        public class OurList<T>
        {
            private int lenght = 0;
            private Element FElement;
            private class Element
            {
                public T r;
                public Element next;
                public Element(T r)
                {
                    this.r = r;
                }
                public Element Next()
                {
                    return this.next;
                }

            }

            public OurList() {
            }

            public void Add(T r)
            {
                Element n = new Element(r);
                if (lenght == 0)
                {         
                    FElement = n;
                }
                else
                {
                    Element temp = FElement;
                    for (int i = 0; i < lenght - 1; i++)
                    {
                        temp = temp.Next();
                    }
                    temp.next = n;
                }
                lenght++;
            }

            public void Add(T r, int index)
            {
                if (index != lenght)
                {
                    Element n = new Element(r);
                    if (index == 0)
                    {
                        n.next = FElement;
                        FElement = n;
                    }
                    else
                    {
                        Element temp = FElement;
                        for (int i = 0; i < index - 1; i++)
                        {
                            temp = temp.Next();
                        }
                        n.next = temp.Next();
                        temp.next = n;
                    }
                    lenght++;
                }
                else
                {
                    Add(r);
           
                }    
            }

            public void Set(T r, int index)
            {
                    Element temp = FElement;
                    for (int i = 0; i < index; i++)
                    {
                        temp = temp.Next();
                    }
                    temp.r = r;
            }

            public T Get(int index)
            {
                    Element temp = FElement;
                    for (int i = 0; i < index; i++)
                    {
                        temp = temp.Next();
                    }
                    return temp.r;
            }

            public int Lenght()
            {
                return lenght;
            }

        }

        
    }
}

