using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using McDonald_s.MVC_Classes;

namespace McDonald_s
{
    public partial class Form1 : Form
    {
        IController m_Controller;
        IModel m_Model;
        IView m_View;

        public Form1()
        {
            InitializeComponent();
            InitMVCObject();
        }

        public void InitMVCObject()
        {
            m_View = viewControl1;
            m_Model = new Model(m_View);
            m_Controller = new Controller(m_Model);
        }

        private void trackBarX_Scroll(object sender, EventArgs e)
        {
            labelX.Text = trackBarX.Value.ToString(); 
        }

        private void trackBarY_Scroll(object sender, EventArgs e)
        {
            labelY.Text = trackBarY.Value.ToString();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            m_Controller.StartWork(trackBarX.Value, trackBarY.Value);  // Start McDonald's work 
            
            buttonStart.Enabled = false;
            buttonEnd.Enabled = true;
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            m_Controller.EndWork();  // End McDonald's work
            
            buttonEnd.Enabled = false;
            buttonStart.Enabled = true;
        }

    }
}
