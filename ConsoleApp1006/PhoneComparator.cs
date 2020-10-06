using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhoneBook
{
    class PhoneComparator : IComparer<PhoneInfo>
    {
        //public int Compare(object x, object y)
        //{
        //    PhoneInfo first = (PhoneInfo)x;
        //    PhoneInfo other = (PhoneInfo)y;

        //    return first.Phone.CompareTo(other.Phone);
        //}

        public int Compare(PhoneInfo x, PhoneInfo y)
        {
            return x.Phone.CompareTo(y.Phone);
        }

        //public int Compare(PhoneInfo x, PhoneInfo y)
        //{
        //    return x.Phone.CompareTo(y.Phone);
        //}
    }

    class NameComparator : IComparer<PhoneInfo>
    {
        //public int Compare(object x, object y)
        //{
        //    PhoneInfo first = (PhoneInfo)x;
        //    PhoneInfo other = (PhoneInfo)y;

        //    return first.Name.CompareTo(other.Name);
        //}

        public int Compare(PhoneInfo x, PhoneInfo y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
