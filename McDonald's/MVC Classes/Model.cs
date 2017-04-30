using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Common_Classes;

namespace McDonald_s.MVC_Classes
{
    public class Model : IModel
    {
        const int m_NumberOfCashier = 3;
        int m_TimeForNewClient;
        int m_NumberOfNewClient;

        IView m_View;
        Dictionary<DishesMenu, Cook> m_Kitchen;
        List<Cashier> m_Service;

        Thread m_ThrForNewClient;
        EventWaitHandle m_EwhStopWork;

        public Model(IView myView)
        {
            m_View = myView;

            // Kitchen init
            m_Kitchen = new Dictionary<DishesMenu, Cook>();
            foreach (DishesMenu curDish in Enum.GetValues(typeof(DishesMenu)))
            {
                m_Kitchen.Add(curDish, new Cook(curDish));

                // Sign Model to Cooks event 
                m_Kitchen[curDish].UpdateDishView += this.UpdateDishesView;  
            }

            // Service init
            m_Service = new List<Cashier>();
            for (int curID = 0; curID < m_NumberOfCashier; curID++)
            { 
                m_Service.Add(new Cashier(m_Kitchen, curID));

                // Sign Model to Cashiers events 
                m_Service[curID].UpdateClientView += this.UpdateClientView;
                m_Service[curID].UpdateOrderView += this.UpdateOrderView;
                m_Service[curID].UpdateRevenueView += this.UpdateRevenueView;
            }        
        }

        void AddClient()
        {
            while (true)
            {
                if (m_EwhStopWork.WaitOne(m_TimeForNewClient))
                {
                    break;
                }
                else
                {
                    for (int count = 0; count < m_NumberOfNewClient; count++)
                    {
                        // Find cashier with smallest queue size
                        int myID = 0, myCount = -1;
                        for (int curID = 0; curID < m_NumberOfCashier; curID++)
                        {
                            if (myCount < 0 || myCount > m_Service[curID].NumberOfClient)
                            {
                                myCount = m_Service[curID].NumberOfClient;
                                myID = curID;
                            }
                        }

                        m_Service[myID].AddClient();
                    }
                }
            }
        }

        void UpdateDishesView(DishesMenu currentDish, int currentValue)
        { 
            m_View.UpdateDishesView(currentDish, currentValue);
        }

        void UpdateClientView(int currentCashierID, int currentListSize)
        {
            m_View.UpdateClientView(currentCashierID, currentListSize);
        }

        void UpdateOrderView(int currentCashierID, Dictionary<DishesMenu, int> currentOrder)
        {
            m_View.UpdateOrderView(currentCashierID, currentOrder);
        }

        void UpdateRevenueView(int currentCashierID, decimal currentCashDesk)
        {
            m_View.UpdateRevenueView(currentCashierID, currentCashDesk);
        }

        void StartThread()
        {
            m_ThrForNewClient = new Thread(AddClient);
            m_ThrForNewClient.IsBackground = true;
            m_EwhStopWork = new ManualResetEvent(false);
            m_ThrForNewClient.Start();
        }
        
        public void StartWork(int myTime, int myClients)
        {
            m_TimeForNewClient = myTime * 1000;  // time in milliseconds
            m_NumberOfNewClient = myClients;
            foreach (DishesMenu curDish in Enum.GetValues(typeof(DishesMenu)))
            {
                m_Kitchen[curDish].StartWork();
            }
            for (int curID = 0; curID < m_NumberOfCashier; curID++)
            {
                m_Service[curID].StartWork();
            }
            StartThread();
        }

        public void EndWork()
        {
            m_EwhStopWork.Set();
            foreach (DishesMenu curDish in Enum.GetValues(typeof(DishesMenu)))
            {
                m_Kitchen[curDish].EndWork();
            }
            for (int curID = 0; curID < m_NumberOfCashier; curID++)
            {
                m_Service[curID].EndWork();
            }
        }
    }
}
