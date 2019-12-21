using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace JList
{
    public class JList<T> : IEnumerable<T>
    {
        static int capacityIncrementor = 5;

        public string path = "";
        int count;
        int capacity;
        public void initdata(){
            if (System.IO.Directory.Exists("data"))
            {

            }
            else
            {
                System.IO.Directory.CreateDirectory("data");
            }
        }
        public JList(List<T> oldlist)
        {

            initdata();
            path = System.IO.Path.Combine("data", oldlist.GetHashCode().ToString());
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
                
             
            }
           
            foreach (var item in oldlist)
            {
                this.Add(item);
            }
        }
        public JList(string inp="",bool create = false)
        {       
            initdata();
            if (inp == "")
            {
                path = System.IO.Path.Combine("data",(new Random().Next(0,55464)).ToString());
                System.IO.Directory.CreateDirectory(path);
            }
            else
            {
               
                path = System.IO.Path.Combine("data",inp);
                if (create)
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(path);
                        

                    }
                    catch (Exception)
                    {

                     
                    }
                   
                }
            }
            count = 0;
            
            capacity = 5;
        }

        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < count)
                {

                    BinaryFormatter frmt = new BinaryFormatter();
                    FileStream strm = new FileStream(System.IO.Directory.GetFiles(path)[index], FileMode.Open);
                    var obj = (T) frmt.Deserialize(strm);
                    strm.Close();
                    return obj;
                }

                throw new ArgumentOutOfRangeException();
            }

            set
            {
                if (index >= 0 && index < count)
                {
                    BinaryFormatter frmt = new BinaryFormatter();
                    FileStream strm = new FileStream(System.IO.Directory.GetFiles(path)[index], FileMode.Open);

                    frmt.Serialize(strm, value);
                    strm.Close();
                    
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int Capacity
        {
            get { return capacity; }
        }

        public static int CapacityIncrementor
        {
            get { return capacityIncrementor; }
        }

        public int Count
        {
            get { return System.IO.Directory.GetFiles(path).Length; }
        }

        public static JList<T> operator +(JList<T> listA, JList<T> listB)
        {
            JList<JList<T>> jlists = new JList<JList<T>>();
            JList<T> output = new JList<T>();

            jlists.Add(listA);
            jlists.Add(listB);

            for (int i = 0; i < jlists.Count; i++)
            {
                for (int j = 0; j < jlists[i].Count; j++)
                {
                    output.Add(jlists[i][j]);
                }
            }

            return output;
        }

        public static JList<T> operator -(JList<T> listA, JList<T> listB)
        {
            JList<T> output = new JList<T>();
            int indexFound = listA.Find(listB);

            if (indexFound != -1)
            {
                int indexFoundEnd = indexFound + listB.Count - 1;
                for (int i = 0; i < listA.Count; i++)
                {
                    if (i < indexFound || i > indexFoundEnd)
                    {
                        output.Add(listA[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < listA.Count; i++)
                {
                    output.Add(listA[i]);
                }
            }

            return output;
        }

        public void Add(T value)
        {
           
            BinaryFormatter frmt = new BinaryFormatter();
            FileStream strm = new FileStream(System.IO.Path.Combine(path,value.GetHashCode().ToString()), FileMode.OpenOrCreate);
            frmt.Serialize(strm,value);
            strm.Close();

           
            count++;
        }

        public int Find(T value)
        {
            int index = -1;

            for (int i = 0; i < count; i++)
            {
                if (new FileInfo( System.IO.Directory.GetFiles(path)[i]).Name.Equals(value.GetHashCode().ToString()))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public int Find(T value, int startingIndex)
        {
            int index = -1;

            for (int i = startingIndex; i < count; i++)
            {
                if (new FileInfo(System.IO.Directory.GetFiles(path)[i]).Name.Equals(value.GetHashCode().ToString()))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public int Find(JList<T> findJList)
        {
            int index = -2;
            int searchFromIndex = 0;

            do
            {
                int foundIndex = Find(findJList[0], searchFromIndex);
                if (foundIndex != -1 && foundIndex + findJList.count - 1 < count)
                {
                    int findJListIndex = 1;

                    for (int i = foundIndex + 1; i < foundIndex + findJList.Count; i++, findJListIndex++)
                    {
                        index = foundIndex;
                          if (new FileInfo( System.IO.Directory.GetFiles(path)[i]).Name.Equals(findJList[findJListIndex]))
                       {
                      
                            searchFromIndex = foundIndex + 1;
                            index = -2;
                            break;
                        }
                    }
                }
                else
                {
                    index = -1;
                }

            } while (index == -2);

            return index;
        }
        public T getval(int index)
        {
            BinaryFormatter frmt = new BinaryFormatter();
            FileStream strm = new FileStream(System.IO.Directory.GetFiles(path)[index], FileMode.Open);
            var obj = (T)frmt.Deserialize(strm);
            strm.Close();
            return obj;
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return getval(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Insert(int index, T value)
        {
            if (index >= 0 && index < count)
            {

                BinaryFormatter frmt = new BinaryFormatter();
                FileStream strm = new FileStream(System.IO.Path.Combine(path,value.GetHashCode().ToString()), FileMode.Open);

                frmt.Serialize(strm, value);
                strm.Close();
            
                count++;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsNearCapacity(int expectedCountIncrease)
        {
            return Convert.ToDouble(count + expectedCountIncrease) >= 0.6;
        }

      
        public void Remove(T value)
        {
            int index = Find(value);

            if (index >= 0 && index < count)
            {
                RemoveAt(index);
                System.GC.Collect();
            }
            else if (index != -1)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < count)
            {
                System.IO.File.Delete(System.IO.Directory.GetFiles(path)[index]);
                count--;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

        }


        public static JList<U> Sort<U>(JList<U> list) where U : IComparable
        {
            bool didSwap = false;

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].CompareTo(list[i + 1]) > 0)
                {
                    didSwap = true;
                    U temporary = list[i];
                    list[i] = list[i + 1];
                    list[i + 1] = temporary;
                }
            }

            if (didSwap)
            {
                return JList<U>.Sort<U>(list);
            }
            else
            {
                return list;
            }
        }

        public override string ToString()
        {
            string output = "{ ";

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    output += System.IO.Directory.GetFiles(path)[i].ToString() + ", ";
                }

                output = output.Substring(0, output.Length - 2);
            }

            return output + " }";
        }

        public static JList<T> Zip(JList<T> listA, JList<T> listB)
        {
            var output = new JList<T>();
            int smallestCount = listA.Count < listB.Count ? listA.Count : listB.Count;

            for (int i = 0; i < smallestCount; i++)
            {
                output.Add(listA[i]);
                output.Add(listB[i]);
            }

            return output;
        }
    }
}
