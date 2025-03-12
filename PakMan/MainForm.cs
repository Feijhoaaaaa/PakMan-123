using System.Configuration;
using System.Diagnostics.Tracing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;

namespace PakMan
{
    public partial class MainForm : Form
    {
        public string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Map_Matrix_3D.json");
        public MainForm()
        {
            
            InitializeComponent();
            

            Menu _menu = new Menu(this);
            Map _map = new Map();

            _menu.ApplyWindowAdjustments(this);
            _menu.MenuLoad();
            _menu.LayerSwitcher(0);
            //_map.Save();
            //Map.CheckIfDataWasWritten("Map_Matrix_3D.json");
            //_map.ReadMapMatrixFromJson(jsonPath);
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);




        }


        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Menu _menu = new Menu(this);
            
            if (e.KeyCode == Keys.Escape && Menu.currentMenuLayer == 3 )
            {
                MessageBox.Show("Escape key pressed!");
                Menu.currentMenuLayer = 1;
                _menu.LayerSwitcher(1);
                // Add your desired action here
            }
        }

















        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
