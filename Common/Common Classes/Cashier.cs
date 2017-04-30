using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Common_Classes
{
    public class Cashier
    {
        const int m_TimeForService = 1000;
        readonly int m_CashierID;
        decimal m_CashDesk;

        Dictionary<DishesMenu, Cook> m_Kitchen;
        List<Client> m_ClientList;
        Client m_CurrentClient;
        
        Thread m_ThrForService;
        EventWaitHandle m_EwhStopWork;

        public delegate void UpdateClient(int myCashierID, int myListSize);
        public delegate void UpdateOrder(int myCashierID, Dictionary<DishesMenu, int> myOrder);
        public delegate void UpdateRevenue(int myCashierID, decimal myCashDesk);
        public event UpdateClient UpdateClientView = delegate(int myCashierID, int myListSize) { };
        public event UpdateOrder UpdateOrderView = delegate(int myCashierID, Dictionary<DishesMenu, int> myOrder) { };
        public event UpdateRevenue UpdateRevenueView = delegate(int myCashierID, decimal myCashDesk) { };

        public int NumberOfClient
        {
            get
            {
                lock (this)
                {
                    return m_ClientList.Count;
                }
            }
        }

        public Cashier(Dictionary<DishesMenu, Cook> myKitchen, int myCashierID)
        {
            m_Kitchen = myKitchen;
            m_ClientList = new List<Client>();
            m_CashierID = myCashierID;
        }

        void GetCurrentClient()
        {
            if (NumberOfClient > 0)
            {
                m_CurrentClient = m_ClientList.First();
            }
        }

        void OnUpdateClientView()
        {
            UpdateClientView(m_CashierID, NumberOfClient);
        }

        void OnUpdateOrderView()
        {
            if (m_CurrentClient == null)   // If current client was served and queue of clients is empty 
            {
                UpdateOrderView(m_CashierID, new Dictionary<DishesMenu, int>());    // Empty order 
            }
            else
            {
                UpdateOrderView(m_CashierID, m_CurrentClient.Orders);
            }
        }

        void OnUpdateRevenueView()
        { 
            UpdateRevenueView(m_CashierID, m_CashDesk);
        }

        void ServiceCurrentClient()
        {
            while (true)
            {
                if (m_EwhStopWork.WaitOne(m_TimeForService))
                {
                    break;
                }
                else
                {
                    lock (this)
                    {
                        if (m_CurrentClient != null)   // If queue of client is not empty
                        {
                            int restOfOrder;
                            bool isComplete = true;
                            foreach (DishesMenu curDish in Enum.GetValues(typeof(DishesMenu)))
                            {
                                restOfOrder = m_Kitchen[curDish].CashierRequest(m_CurrentClient.Orders[curDish]);  // Send request to Cook
                                m_CurrentClient.Orders[curDish] = restOfOrder;
                                if (restOfOrder > 0)
                                    isComplete = false;
                            }
                            if (isComplete)   // If current client was served
                            {
                                m_CashDesk += m_CurrentClient.OrdersCost;
                                m_ClientList.Remove(m_CurrentClient);
                                m_CurrentClient = null;
                                GetCurrentClient();
                                OnUpdateClientView();
                                OnUpdateOrderView();
                                OnUpdateRevenueView();
                            }
                        }
                    }
                }
            }
        }
        
        void StartThread()
        {
            m_ThrForService = new Thread(ServiceCurrentClient);
            m_ThrForService.IsBackground = true;
            m_EwhStopWork = new ManualResetEvent(false);
            m_ThrForService.Start();
        }

        public void AddClient()
        {
            lock (this)
            {
                m_ClientList.Add(new Client());
                if (NumberOfClient == 1)  // If queue of clients was empty
                {
                    GetCurrentClient();
                    OnUpdateOrderView();
                }
                OnUpdateClientView();
            }
        }

        public void StartWork()
        {
            m_CashDesk = 0;
            OnUpdateClientView();
            OnUpdateOrderView();
            OnUpdateRevenueView();
            StartThread();
        }
        
        public void EndWork()
        {
            m_EwhStopWork.Set();
            m_ClientList.Clear();  // All client go home
            m_CurrentClient = null;
            OnUpdateClientView();
            OnUpdateOrderView();
            OnUpdateRevenueView();
        }
        
    }
}
