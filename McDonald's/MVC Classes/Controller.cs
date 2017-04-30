using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace McDonald_s.MVC_Classes
{
    public class Controller : IController
    {
        IModel m_Model;

        public Controller(IModel myModel)
        {
            m_Model = myModel;
        }

        public void StartWork(int myTime, int myClients)
        {
            m_Model.StartWork(myTime, myClients); 
        }

        public void EndWork()
        {
            m_Model.EndWork();
        }
    }
}
