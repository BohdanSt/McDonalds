using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Common_Classes;

namespace Common
{
    public interface IModel
    {
        void StartWork(int myTime, int myClients);
        void EndWork();
    }

    public interface IController
    {
        void StartWork(int myTime, int myClients);
        void EndWork();
    }

    public interface IView
    {
        void UpdateDishesView(DishesMenu currentDish, int currentValue);
        void UpdateClientView(int currentCashierID, int currentListSize); 
        void UpdateOrderView(int currentCashierID, Dictionary<DishesMenu, int> currentOrder);
        void UpdateRevenueView(int currentCashierID, decimal currentCashDesk);
    }

    public enum DishesMenu
    {
        Hamburger,
        Cheeseburger,
        Mac_chicken,
        Chipped_potato,
        Ice_cream
    };
}
