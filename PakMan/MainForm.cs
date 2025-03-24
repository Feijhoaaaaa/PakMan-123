using System.Configuration;
using System.Diagnostics.Tracing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;
using Timer = System.Windows.Forms.Timer;
namespace PakMan
{
    public partial class MainForm : Form
    {
        // Fields
        private Map _map;
        private Player _spritePlayer;
        private Ghost _spriteGhost;
        private Menu _menu;
        public string? currentDirection;

        // Constructor
        public MainForm()
        {
            InitializeComponent();

            _menu = new Menu(this); // Initialize _menu before using it
            _map = new Map(this);
            _spritePlayer = new Player(this);
            _spriteGhost = new Ghost(this);

            _menu.ApplyWindowAdjustments(this);
            _menu.MenuLoad();
            _menu.LayerSwitcher(0);

            _map.animationTimer = new Timer();
            _map.animationTimer.Interval = 300; // Update every 300 ms
            _map.animationTimer.Tick += AnimationTimer_Tick;

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }

        // Event handler for key down events
        private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            Menu _menu = new Menu(this);

            if (e.KeyCode == Keys.Escape && Menu.currentMenuLayer == 3)
            {
                Menu.currentMenuLayer = 2;
                mainGameTimer.Stop();
                _menu.LayerSwitcher(2);
            }
            else if (e.KeyCode == Keys.Escape && Menu.currentMenuLayer == 2)
            {
                Menu.currentMenuLayer = 3;
                _menu.LayerSwitcher(3);
            }
            else if ((e.KeyCode == Keys.Escape && Menu.currentMenuLayer == 0) || (e.KeyCode == Keys.Escape && Menu.currentLevel == 1))
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Down)
            {
                currentDirection = "down";
            }
            else if (e.KeyCode == Keys.Up)
            {
                currentDirection = "up";
            }
            else if (e.KeyCode == Keys.Left)
            {
                currentDirection = "left";
            }
            else if (e.KeyCode == Keys.Right)
            {
                currentDirection = "right";
            }
        }

        // Event handler for form load
        private void Form1_Load(object sender, EventArgs e)
        {
            // Add any initialization code here
        }

        // Event handler for main game timer
        private void MainGameTimer(object sender, EventArgs e)
        {
            mainGameTimer.Interval = 300;
            

            if (Map._ghosts != null)
            {
                switch (Menu.currentLevel)
                {
                    case 0:
                        _spriteGhost.GhostMovePink(Map._ghosts[0]);
                        _spriteGhost.GhostMoveRed(Map._ghosts[1]);
                        _spriteGhost.GhostMoveBlue(Map._ghosts[2]);
                        break;
                    default:
                        break;
                }
            }
            if (currentDirection != null)
            {
                switch (currentDirection)
                {
                    case "down":
                        _spritePlayer.PlayerMove(0,_spritePlayer.speed);
                        break;
                    case "up":
                        _spritePlayer.PlayerMove(0,-_spritePlayer.speed);
                        break;
                    case "left":
                        _spritePlayer.PlayerMove(-_spritePlayer.speed, 0);
                        break;
                    case "right":
                        _spritePlayer.PlayerMove(_spritePlayer.speed, 0);
                        break;
                    default:
                        break;
                }
            }
        }

        // Method to start the game
        public void StartGame(int level)
        {
            //_menu.RestartGame(this, EventArgs.Empty); // Pass 'this' as sender and EventArgs.Empty as event args
            _map.MapLoad(level);
            if (!mainGameTimer.Enabled)
            {
                mainGameTimer.Start();
            }
            else
            {
                mainGameTimer.Stop();
                mainGameTimer.Start();
            }
            if (!_map.animationTimer.Enabled)
            {
                _map.animationTimer.Start();
            }
            else
            {
                _map.animationTimer.Stop();
                _map.animationTimer.Start();
            }
            //_map.animationTimer.Start();
        }

        // Method to handle game over
        public void GameOver()
        {
            Menu.currentMenuLayer = 5;
            mainGameTimer.Stop();
            _map.animationTimer.Stop();
            _menu.LayerSwitcher(Menu.currentMenuLayer);
        }
        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            _map.isOpen = !_map.isOpen;
            _map.ghostEU = !_map.ghostEU;
            if (Map._player != null) // Use the type name instead of the instance reference
            {
                using (var ms = new MemoryStream(_map.isOpen ? Properties.Resources.PakManO : Properties.Resources.PakManC))
                {
                    Map._player.Image = Image.FromStream(ms); // Use the type name instead of the instance reference
                }
            }
            if (Map._ghosts != null) // Use the type name instead of the instance reference
            {
                using (var ms = new MemoryStream(_map.isOpen ? Properties.Resources.PakManO : Properties.Resources.PakManC))
                {
                    Map._player.Image = Image.FromStream(ms); // Use the type name instead of the instance reference
                }
            }

        }


    }
}
