using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CleanConsoleApplication
{
    class Program
    {
        public static string allPerson = "allPerson";
        public static string cleanedPerson = "cleanedPerson";
        static void Main(string[] args)
        {
            Help();
            bool isGo = true;
            while (isGo)
            {
                string number = Console.ReadLine();

                if (number == "6")
                {
                    isGo = false;
                }
                else if (number == "7")
                {
                    Help();
                }
                else
                {
                    Operation(number);
                }
            }
        }

        private static void Help()
        {
            Console.WriteLine("**********************");
            Console.WriteLine("*1、查看公司所有人员 *");
            Console.WriteLine("*2、查看已值日人员   *");
            Console.WriteLine("*3、增添新人员       *");
            Console.WriteLine("*4、删减人员         *");
            Console.WriteLine("*5、今天谁值日       *");
            Console.WriteLine("*6、退出             *");
            Console.WriteLine("*7、帮助             *");
            Console.WriteLine("**********************");
        }

        private static void Operation(string number)
        {
            if (number == "1")
            {
                string[] persons = Read(allPerson);
                if (persons.Length > 0)
                {
                    foreach (var person in persons)
                    {
                        if (!string.IsNullOrEmpty(person))
                            Console.WriteLine(person);
                    }
                }
                else
                {
                    Console.WriteLine("公司人员为空，请添加人员");
                }
            }
            else if (number == "2")
            {
                string[] persons = Read(cleanedPerson);
                if (persons.Length > 0)
                {
                    foreach (var person in persons)
                    {
                        if(!string.IsNullOrEmpty(person))
                            Console.WriteLine(person);
                    }
                }
                else
                {
                    Console.WriteLine("无已值日人员，需要值日请输入“5”.");
                }
            }
            else if (number == "3")
            {
                if (HasFile(allPerson))//判断文件是否存在
                {
                    Console.WriteLine("请添加相关人员");
                    string newPerson = Console.ReadLine();
                    if (string.IsNullOrEmpty(newPerson))
                    {
                        Console.WriteLine("输入错误");
                    }
                    else
                    {
                        Write(allPerson, true, newPerson);
                        Console.WriteLine("添加成功");
                    }
                }
                else
                {
                    //如果不存在则创建
                    CreateFile(allPerson);

                    Console.WriteLine("请添加相关人员");
                    string newPerson = Console.ReadLine();
                    Write(allPerson, true, newPerson);
                    Console.WriteLine("添加成功");
                }

            }
            else if (number == "4")
            {
                string[] persons = Read(allPerson);
                Console.WriteLine("请输入要删减人员的名字");
                string person = Console.ReadLine();

                string[] newPersons = Update(persons, person);

                Write(allPerson, false, string.Join("\n", newPersons));
                Console.WriteLine("删除成功");
            }
            else if (number == "5")
            {
                string[] cleanedPersons = Read(cleanedPerson);
                string[] allPersons = Read(allPerson);

                string[] noCleadPersons = allPersons.Except(cleanedPersons).ToArray();//A中有B中没有的 查出没有值日的人员

                List<string> list = new List<string>();
                noCleadPersons.ToList().ForEach(
                (s) =>
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        list.Add(s);
                    }
                }
                );
                noCleadPersons = list.ToArray();

                string needPerson = string.Empty;
                if (noCleadPersons.Length > 0)
                {
                    string[] item = noCleadPersons;
                    Random r = new Random();
                    needPerson = item[r.Next(item.Length)];

                    Write(cleanedPerson, true, needPerson);
                }
                else
                {
                    Console.WriteLine("本轮卫生打扫已经结束，新的一轮开始了。请重试！");


                    //System.IO.File.WriteAllText(cleanedPerson, string.Empty);
                    // 删除文件  
                    DeleteFile(cleanedPerson);
                    CreateFile(cleanedPerson);

                    //string[] item = allPersons;
                    //Random r = new Random();
                    //needPerson = item[r.Next(item.Length)];

                    //Write(cleanedPerson, true, needPerson);
                }

                Console.WriteLine(needPerson);

            }
        }

        private static void DeleteFile(string cleanedPerson)
        {
            File.Delete(cleanedPerson);
        }

        private static string[] Update(string[] persons, string person)
        {
            List<string> list = persons.ToList();//把数组转换成泛型类
            list.Remove(person);//利用泛型类remove掉元素
            string[] newPersons = list.ToArray();//再由泛型类转换成数组

            return newPersons;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <returns></returns>
        public static bool HasFile(string fileName)
        {
            bool result = false;
            if (File.Exists(fileName))//判断文件是否存在
            {
                result = true;
            }
            return result;
        }

        public static void CreateFile(string fileName)
        {
            string strFileName = fileName + ".txt";
            File.Create(strFileName).Close();
            Console.WriteLine("存储文件创建成功");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="rewrite"></param>
        /// <param name="data">需要写入的数据</param>
        public static void Write(string fileName, bool rewrite, string data)
        {
            string strFileName = fileName + ".txt";

            //在将文本写入文件前，处理文本行
            //StreamWriter一个参数默认覆盖
            //StreamWriter第二个参数为false覆盖现有文件，为true则把文本追加到文件末尾
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFileName, rewrite))
            {
                file.WriteLine(data);// 直接追加文件末尾，换行   
                file.Close();
                file.Dispose(); //释放资源
            }

        }

        public static string[] Read(string fileName)
        {
            string strFileName = fileName + ".txt";
            string[] lines = new string[] { };
            //List<string> list = new List<string>();

            if (File.Exists(strFileName))//判断文件是否存在
            {
                //直接读取出字符串
                //list.Add(System.IO.File.ReadAllText(strFileName));

                lines = System.IO.File.ReadAllLines(strFileName);
            }

            return lines;
        }
    }
}
