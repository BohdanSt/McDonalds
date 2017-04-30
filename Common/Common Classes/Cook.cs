using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Common_Classes
{
    public class Cook
    {
        const int m_TableCapacity = 10;
        const int m_TimeForCooking = 1000;

        int m_NumberOfDishes;
        DishesMenu m_DishType;

        Thread m_ThrForNewDish;
        EventWaitHandle m_EwhStopWork;

        public delegate void UpdateDish(DishesMenu myDish, int myValue);
        public event UpdateDish UpdateDishView = delegate(DishesMenu myDish, int myValue) { }; 

        public Cook(DishesMenu myDishType)
        {
            m_DishType = myDishType;
        }

        void Cooking()
        {
            while (true)
            {
                if (m_EwhStopWork.WaitOne(m_TimeForCooking))
                {
                    break;
                }
                else
                {
                    lock (this)
                    {
                        if (m_TableCapacity > m_NumberOfDishes)  // If table is not full
                        {
                            m_NumberOfDishes++;
                            OnUpdateDishView();
                        }
                    }
                }
            }
        }

        void OnUpdateDishView()
        {
            UpdateDishView(m_DishType, m_NumberOfDishes);
        }

        void StartThread()
        {
            m_ThrForNewDish = new Thread(Cooking);
            m_ThrForNewDish.IsBackground = true;
            m_EwhStopWork = new ManualResetEvent(false);
            m_ThrForNewDish.Start();
        }

        public void StartWork()
        {
            StartThread();
        }

        public void EndWork()
        {
            m_EwhStopWork.Set();
            m_NumberOfDishes = 0;  //All dishes thrown away (new day - new dishes)
            OnUpdateDishView();
        }

        public int CashierRequest(int requestNumber)
        {
            lock (this)
            {
                if (requestNumber <= m_NumberOfDishes)
                {
                    m_NumberOfDishes -= requestNumber;
                    OnUpdateDishView();
                    return 0;
                }
                else
                {
                    int myNumber = requestNumber - m_NumberOfDishes;  // Rest of request
                    m_NumberOfDishes = 0;
                    OnUpdateDishView();
                    return myNumber;
                }
            }
        }
    }
}
