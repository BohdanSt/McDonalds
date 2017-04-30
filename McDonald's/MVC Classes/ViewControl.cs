using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using Common.Common_Classes;

namespace McDonald_s.MVC_Classes
{
    public partial class ViewControl : UserControl, IView
    {
        List<Label> m_CashiersQueuesLabels;
        List<ListBox> m_OrdersListBoxes;
        List<decimal> m_CashDesks;
        decimal m_TotalCash;
        List<Label> m_CashDeskLabels;
        Dictionary<DishesMenu, Label> m_TableOfDishesLabels;
        
        public ViewControl()
        {
            InitializeComponent();

            // m_CashiersQueuesLabels init
            m_CashiersQueuesLabels = new List<Label>
            {
                labelQueue1,
                labelQueue2,
                labelQueue3
            };

            // m_OrdersListBoxes init
            m_OrdersListBoxes = new List<ListBox>
            {
                listBoxOrder1,
                listBoxOrder2,
                listBoxOrder3
            };

            // m_CashDesks init
            m_CashDesks = new List<decimal> { 0, 0, 0};

            // m_CashDeskLabels init
            m_CashDeskLabels = new List<Label>
            {
                labelCashDesk1,
                labelCashDesk2,
                labelCashDesk3
            };

            // m_TableOfDishesLabels init
            m_TableOfDishesLabels = new Dictionary<DishesMenu, Label>();
            m_TableOfDishesLabels.Add(DishesMenu.Hamburger, labelHamburger);
            m_TableOfDishesLabels.Add(DishesMenu.Cheeseburger, labelCheeseburger);
            m_TableOfDishesLabels.Add(DishesMenu.Chipped_potato, labelChipped_potato);
            m_TableOfDishesLabels.Add(DishesMenu.Mac_chicken, labelMac_chicken);
            m_TableOfDishesLabels.Add(DishesMenu.Ice_cream, labelIce_cream);

        }

        public void UpdateDishesView(DishesMenu currentDish, int currentValue)
        {
            Invoke(new Action(() => m_TableOfDishesLabels[currentDish].Text = currentValue.ToString()));
            if (currentValue == 10)
            {
                Invoke(new Action(() => m_TableOfDishesLabels[currentDish].ForeColor = Color.Red));
            }
            else
            {
                Invoke(new Action(() => m_TableOfDishesLabels[currentDish].ForeColor = Color.Green));
            }
        }

        public void UpdateClientView(int currentCashierID, int currentListSize)
        {
            Invoke(new Action(() => m_CashiersQueuesLabels[currentCashierID].Text = currentListSize.ToString()));
            if (currentListSize == 0)
            {
                Invoke(new Action(() => m_CashiersQueuesLabels[currentCashierID].ForeColor = Color.Red));
            }
            else
            {
                Invoke(new Action(() => m_CashiersQueuesLabels[currentCashierID].ForeColor = Color.Green));
            }
        }

        public void UpdateOrderView(int currentCashierID, Dictionary<DishesMenu, int> currentOrder)
        {
            if (currentOrder.Count == 0)  // If current order is empty (queue of clients is empty)
            {
                Invoke(new Action(() => m_OrdersListBoxes[currentCashierID].Items.Clear()));
                Invoke(new Action(() => m_OrdersListBoxes[currentCashierID].ForeColor = Color.Red));
                Invoke(new Action(() => m_OrdersListBoxes[currentCashierID].Items.Add("FREE!!!")));
            }
            else 
            {
                Invoke(new Action(() => m_OrdersListBoxes[currentCashierID].Items.Clear()));
                Invoke(new Action(() => m_OrdersListBoxes[currentCashierID].ForeColor = Color.Black));
                foreach (DishesMenu curDish in Enum.GetValues(typeof(DishesMenu)))
                {
                    Invoke(new Action(() => m_OrdersListBoxes[currentCashierID].Items.Add(curDish.ToString() + ": " + currentOrder[curDish].ToString())));
                }
            }
        }

        public void UpdateRevenueView(int currentCashierID, decimal currentCashDesk)
        {
            m_TotalCash += currentCashDesk - m_CashDesks[currentCashierID];
            Invoke(new Action(() => labelTotal.Text = m_TotalCash.ToString()));

            m_CashDesks[currentCashierID] = currentCashDesk;
            Invoke(new Action(() => m_CashDeskLabels[currentCashierID].Text = currentCashDesk.ToString()));
        }
    }
}
