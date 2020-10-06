using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhoneBook
{
    /// <summary>
    /// PhoneBookManager : 이클래스는 어떤 역할을 하는 클래스인지.
    /// 작성자 :
    /// 최초작성일:
    /// 수정내용:
    /// </summary>
    public class PhoneBookManager
    {
        //private const int MAX_CNT = 100; 
        //private PhoneInfo[] infoStorage = new PhoneInfo[MAX_CNT];
        //private int curCnt = 0;

        HashSet<PhoneInfo> infoStorage = new HashSet<PhoneInfo>();
        static PhoneBookManager inst = null;
        readonly string dataFile = "PhoneBook.dat";

        private PhoneBookManager()
        {
            ReadToFile();
        }
        private void ReadToFile()
        {
            if (!File.Exists(dataFile))
                return;


            try
            {
                FileStream rs = new FileStream(dataFile, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                infoStorage.Clear();

                infoStorage = (HashSet<PhoneInfo>)formatter.Deserialize(rs);
                rs.Close();
            }

            catch (IOException err)
            {
                Console.WriteLine(err.Message);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }


        public  void WriteToFile()
        {
            try
            {
                using (FileStream fs = new FileStream(dataFile, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, infoStorage);
                    fs.Close();
                }
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PhoneBookManager CreateMangerInstance()
        {
            if (inst == null)
                inst = new PhoneBookManager();

            return inst;
        }

        public void ShowMenu()
        {
            Console.WriteLine("------------------------ 주소록 -----------------------------------");
            Console.WriteLine("1. 입력  |  2. 목록  |  3. 검색  |  4. 정렬  |  5. 삭제  |  6. 종료");
            Console.WriteLine("------------------------------------------------------------------");
            Console.Write("선택: ");
        }

        public void SortData()
        {
            int choice;
            while (true)
            {
                try
                {
                    Console.WriteLine("1.이름 ASC  2.이름 DESC  3.전화번호 ASC  4.전화번호 DESC");
                    Console.Write("선택 >> ");
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice < 1 || choice > 4)
                        {
                            throw new MenuChoiceException(choice);
                            //Console.WriteLine("1.이름 ASC  2.이름 DESC  3.전화번호 ASC  4.전화번호 DESC 중에 선택하십시오.");
                            //return;
                        }
                        else
                            break;
                    }
                }
                catch (MenuChoiceException err)
                {
                    err.ShowWrongChoice();
                }
            }

            //PhoneInfo[] new_arr = new PhoneInfo[curCnt];
            //Array.Copy(infoStorage, new_arr, curCnt);

            List<PhoneInfo> list = new List<PhoneInfo>(infoStorage);

            if (choice == 1)
            {
                list.Sort();

                //list.Sort(new NameComparator());
                //Array.Sort(new_arr, new NameComparator());
            }
            else if (choice == 2)
            {
                list.Sort();
                list.Reverse();
                //Array.Sort(new_arr, new NameComparator());
                //Array.Reverse(new_arr);
            }
            else if (choice == 3)
            {
                list.Sort(new PhoneComparator());
                //Array.Sort(new_arr, new PhoneComparator());
            }
            else if (choice == 4)
            {
                list.Sort(new PhoneComparator());
                list.Reverse();
                //Array.Sort(new_arr, new PhoneComparator());
                //Array.Reverse(new_arr);
            }

            foreach (PhoneInfo curInfo in list)
            {
                curInfo.ShowPhoneInfo();
                Console.WriteLine();
            }
            //for (int i = 0; i < curCnt; i++)
            //{
            //    Console.WriteLine(new_arr[i].ToString());
            //}
        }

        

        public void InputData()
        {
            Console.WriteLine("1.일반  2.대학  3.회사");
            Console.Write("선택 >> ");
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice))
                    break;
            }
            if (choice < 1 || choice > 3)
            {
                Console.WriteLine("1.일반  2.대학  3.회사 중에 선택하십시오.");
                return;
            }

            PhoneInfo info = null;
            switch (choice)
            {
                case 1:
                    info = InputFriendInfo();
                    break;
                case 2:
                    info = InputUnivInfo();
                    break;
                case 3:
                    info = InputCompanyInfo();
                    break;
            }
            if (info != null)
            {
                //infoStorage[curCnt++] = info;

                bool isAdded = infoStorage.Add(info);
                if (isAdded)
                    Console.WriteLine("데이터 입력이 완료되었습니다");
                else
                    Console.WriteLine("이미 저장된 데이터입니다");
            }
        }

        private List<string> InputCommonInfo()
        {
            Console.Write("이름: ");
            string name = Console.ReadLine().Trim();
            //if (name == "") or if (name.Length < 1) or if (name.Equals(""))
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("이름은 필수입력입니다");
                return null;
            }
            else
            {
                if (SearchName(name))
                {
                    Console.WriteLine("이미 등록된 이름입니다. 다른 이름으로 입력하세요");
                    return null;
                }
            }

            Console.Write("전화번호: ");
            string phone = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(phone))
            {
                Console.WriteLine("전화번호는 필수입력입니다");
                return null;
            }

            Console.Write("생일: ");
            string birth = Console.ReadLine().Trim();

            List<string> list = new List<string>();
            list.Add(name);
            list.Add(phone);
            list.Add(birth);

            return list;
        }

        private PhoneInfo InputFriendInfo()
        {
            List<string> comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Count != 3)
                return null;

            return new PhoneInfo(comInfo[0], comInfo[1], comInfo[2]);
        }

        private PhoneInfo InputUnivInfo()
        {
            List<string> comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Count != 3)
                return null;

            Console.Write("전공: ");
            string major = Console.ReadLine().Trim();

            Console.Write("학년: ");
            int year = int.Parse(Console.ReadLine().Trim());

            return new PhoneUnivInfo(comInfo[0], comInfo[1], comInfo[2], major, year);
        }

        private PhoneInfo InputCompanyInfo()
        {
            List<string> comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Count != 3)
                return null;

            Console.Write("회사명: ");
            string company = Console.ReadLine().Trim();

            return new PhoneCompanyInfo(comInfo[0], comInfo[1], comInfo[2], company);
        }

        public void ListData()
        {
            if (infoStorage.Count == 0)
            {
                Console.WriteLine("입력된 데이터가 없습니다.");
                return;
            }

            foreach (PhoneInfo curInfo in infoStorage)
            {
                curInfo.ShowPhoneInfo();
                Console.WriteLine();
            }
            //for(int i=0; i<curCnt; i++)
            //{
            //    //infoStorage[i].ShowPhoneInfo();
            //    //Console.WriteLine();

            //    Console.WriteLine(infoStorage[i].ToString());                
            //}
        }

        public void SearchData()
        {
            Console.WriteLine("주소록 검색을 시작합니다......");
            PhoneInfo findInfo = SearchName();
            if (findInfo == null)
            {
                Console.WriteLine("검색된 데이터가 없습니다");
            }
            else
            {
                findInfo.ShowPhoneInfo();
                Console.WriteLine();
            }
            //int dataIdx = SearchName();
            //if (dataIdx < 0)
            //{
            //    Console.WriteLine("검색된 데이터가 없습니다");
            //}
            //else
            //{
            //    infoStorage[dataIdx].ShowPhoneInfo();
            //    Console.WriteLine();
            //}

            #region 모두 찾기
            //int findCnt = 0;
            //for(int i=0; i<curCnt; i++)
            //{
            //    // ==, Equals(), CompareTo()
            //    if (infoStorage[i].Name.Replace(" ","").CompareTo(name) == 0)
            //    {
            //        infoStorage[i].ShowPhoneInfo();
            //        findCnt++;
            //    }
            //}
            //if (findCnt < 1)
            //{
            //    Console.WriteLine("검색된 데이터가 없습니다");
            //}
            //else
            //{
            //    Console.WriteLine($"총 {findCnt} 명이 검색되었습니다.");
            //}
            #endregion
        }

        private PhoneInfo SearchName()
        {
            Console.Write("이름: ");
            string name = Console.ReadLine().Trim().Replace(" ", "");

            foreach (PhoneInfo curInfo in infoStorage)
            {
                if (name.CompareTo(curInfo.Name) == 0)
                {
                    return curInfo;
                }
            }

            return null;
        }

        //private int SearchName()
        //{
        //    Console.Write("이름: ");
        //    string name = Console.ReadLine().Trim().Replace(" ", "");

        //    for (int i = 0; i < curCnt; i++)
        //    {
        //        if (infoStorage[i].Name.Replace(" ", "").CompareTo(name) == 0)
        //        {
        //            return i;
        //        }
        //    }

        //    return -1;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool SearchName(string name)
        {
            foreach (PhoneInfo curInfo in infoStorage)
            {
                if (curInfo.Name.Equals(name))
                    return true;
            }

            return false;
            //for (int i = 0; i < curCnt; i++)
            //{
            //    if (infoStorage[i].Name.Replace(" ", "").CompareTo(name) == 0)
            //    {
            //        return i;
            //    }
            //}

            //return -1;
        }

        public void DeleteData()
        {
            Console.WriteLine("주소록 삭제를 시작합니다......");

            PhoneInfo delInfo = SearchName();
            if (delInfo == null)
            {
                Console.WriteLine("삭제할 데이터가 없습니다");
            }
            else
            {
                infoStorage.Remove(delInfo);
                //for(int i=dataIdx; i<curCnt; i++)
                //{
                //    infoStorage[i] = infoStorage[i + 1];
                //}
                //curCnt--;
                Console.WriteLine("주소록 삭제가 완료되었습니다");
            }
        }

        internal static PhoneBookManager CreateManagerInstance()
        {
            throw new NotImplementedException();
        }

        
    }
}
