using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Common_Classes
{
    public class Client
    {
        const int m_MaxNumberOfDish = 3;
        static Random m_RandDish = new Random(); 

        public Dictionary<DishesMenu, int> Orders;
        public decimal OrdersCost { get; set; }
        
        public Client()
        {
            Orders = new Dictionary<DishesMenu, int>();
            int curNum = m_RandDish.Next(m_MaxNumberOfDish);
            foreach (DishesMenu curDish in Enum.GetValues(typeof(DishesMenu)))
            {
                Orders.Add(curDish, curNum);
                OrdersCost += curNum;
                curNum = m_RandDish.Next(m_MaxNumberOfDish);
            }
        }
    }
}
