using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace PakMan
{
    class Menu
    {
        public static int currentMenuLayer;
        public static bool infinityGameModeEnabled;
        public static bool levelGameModeEnabled;
        public static int currentLevel;

        public static Button[] buttonMenuL1 = Array.Empty<Button>();
        public static Button[] buttonMenuL2 = Array.Empty<Button>();
        public static Button[] buttonMenuL3 = Array.Empty<Button>();
        public static Control[] scoreBoardL4 = Array.Empty<Control>();
        public static Control[] gameOverL4 = Array.Empty<Control>();

        private readonly Form _form;
        private readonly Map _map;

        public Menu(Form form)
        {
            _form = form;
            _map = new Map(form);
        }

        public void ApplyWindowAdjustments(Form form)
        {
            try
            {
                int width = int.Parse(ConfigurationManager.AppSettings["WindowWithSize"] ?? "800");
                int height = int.Parse(ConfigurationManager.AppSettings["WindwoHeightSize"] ?? "600");
                string backgraundColor = ConfigurationManager.AppSettings["WindowBackgraundColor"] ?? "#FFFFFF";
                string windowsName = ConfigurationManager.AppSettings["WindowName"] ?? "PakMan";
                string windowFormBorderStyle = ConfigurationManager.AppSettings["WindowFormBorderStyle"] ?? "FixedSingle";

                form.Size = new Size(width, height);
                form.BackColor = ColorTranslator.FromHtml(backgraundColor);
                form.Text = windowsName;

                switch (windowFormBorderStyle)
                {
                    case "FixedDialog":
                        form.FormBorderStyle = FormBorderStyle.FixedDialog;
                        break;
                    case "Fixed3D":
                        form.FormBorderStyle = FormBorderStyle.Fixed3D;
                        break;
                    case "FixedSingle":
                        form.FormBorderStyle = FormBorderStyle.FixedSingle;
                        break;
                    case "Sizable":
                        form.FormBorderStyle = FormBorderStyle.Sizable;
                        break;
                    case "None":
                        form.FormBorderStyle = FormBorderStyle.None;
                        break;
                    case "FixedToolWindow":
                        form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                        break;
                    case "SizableToolWindow":
                        form.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                        break;
                    default:
                        form.FormBorderStyle = FormBorderStyle.FixedSingle;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading config" + ex.Message);
            }
        }

        public static Button CreateButton(string text, int fontSize, Size size, string color, EventHandler onClick, Point location)
        {
            string buttonFontName = ConfigurationManager.AppSettings["ButtonFontName"] ?? "Arial";
            string buttonFontColor = ConfigurationManager.AppSettings["ButtonFontColor"] ?? "#000000";
            string buttonBorderColor = ConfigurationManager.AppSettings["ButtonBorderColor"] ?? "#000000";

            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                Font = new Font(buttonFontName, fontSize, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml(color),
                ForeColor = ColorTranslator.FromHtml(buttonFontColor),
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };
            button.FlatAppearance.BorderColor = ColorTranslator.FromHtml(buttonBorderColor);
            button.FlatAppearance.BorderSize = 3;

            // Attach the event handler
            button.Click += onClick;

            return button;
        }

        public static Label CreateLabel(string text, int fontSize, string color, string fontColor, Size size, Point location)
        {
            string buttonFontName = ConfigurationManager.AppSettings["ButtonFontName"] ?? "Arial";
            string labelFontColor = fontColor ?? "#000000";

            Label label = new Label
            {
                Text = text,
                Font = new Font(buttonFontName, fontSize, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml(labelFontColor),
                BackColor = ColorTranslator.FromHtml(color),
                Size = size,
                Location = location,
            };
            return label;
        }

        public static Panel CreatePanel(string color, Size size, Point location)
        {
            Panel panel = new Panel
            {
                BackColor = ColorTranslator.FromHtml(color),
                Size = size,
                Location = location,
                Visible = true
            };

            return panel;
        }

        public void MenuLoad()
        {
            int Window_W = int.Parse(ConfigurationManager.AppSettings["WindowWithSize"] ?? "800");
            int Window_H = int.Parse(ConfigurationManager.AppSettings["WindwoHeightSize"] ?? "600");

            int NormalButtonW = int.Parse(ConfigurationManager.AppSettings["NormalButtonW"] ?? "100");
            int NormalButtonH = int.Parse(ConfigurationManager.AppSettings["NormalButtonH"] ?? "50");
            int NormalButtonFontSize = int.Parse(ConfigurationManager.AppSettings["NormalButtonFontSize"] ?? "12");

            int WideButtonW = int.Parse(ConfigurationManager.AppSettings["WideButtonW"] ?? "200");
            int WideButtonH = int.Parse(ConfigurationManager.AppSettings["WideButtonH"] ?? "50");
            int WideButtonFontSize = int.Parse(ConfigurationManager.AppSettings["WideButtonFontSize"] ?? "14");

            int HugeButtonW = int.Parse(ConfigurationManager.AppSettings["HugeButtonW"] ?? "300");
            int HugeButtonH = int.Parse(ConfigurationManager.AppSettings["HugeButtonH"] ?? "70");
            int HugeButtonFontSize = int.Parse(ConfigurationManager.AppSettings["HugeButtonFontSize"] ?? "16");

            int BackButtonW = int.Parse(ConfigurationManager.AppSettings["BackButtonW"] ?? "50");
            int BackButtonFontSize = int.Parse(ConfigurationManager.AppSettings["BackButtonFontSize"] ?? "12");

            int MenuBackGraundL3W = int.Parse(ConfigurationManager.AppSettings["MenuBackGraundL3W"] ?? "400");
            int MenuBackGraundL3H = int.Parse(ConfigurationManager.AppSettings["MenuBackGraundL3H"] ?? "300");

            string MenuBackgraundColor = ConfigurationManager.AppSettings["MenuBackgraundColor"] ?? "#FFFFFF";
            string ButtonBackgraundColor = ConfigurationManager.AppSettings["ButtonBackgraundColor"] ?? "#000000";
            string ButtonFontColor = ConfigurationManager.AppSettings["ButtonFontColor"] ?? "#000000";

            int padding = 2;

            // Creating buttons
            Button startButton = Menu.CreateButton("Start", WideButtonFontSize, new Size(WideButtonW, WideButtonH), ButtonBackgraundColor, StartGame, new Point((Window_W - WideButtonW) / 2, (Window_H - WideButtonH) / 2 - WideButtonH / 2 - padding));
            _form.Controls.Add(startButton);

            Button infinityMode = Menu.CreateButton("Infinity Mode", NormalButtonFontSize, new Size(NormalButtonW, NormalButtonH), ButtonBackgraundColor, InfinityMode, new Point(Window_W / 2 - NormalButtonW - padding, Window_H / 2 + padding));
            _form.Controls.Add(infinityMode);

            Button levelMode = Menu.CreateButton("Level Mode", NormalButtonFontSize, new Size(NormalButtonW, NormalButtonH), ButtonBackgraundColor, LevelMode, new Point(Window_W / 2 + padding, Window_H / 2 + padding));
            _form.Controls.Add(levelMode);

            // Adding first layers buttons to their arr
            buttonMenuL1 = new Button[] { startButton, infinityMode, levelMode };

            Button backButton = Menu.CreateButton("<", BackButtonFontSize, new Size(BackButtonW, BackButtonW), ButtonBackgraundColor, BackButton, new Point(padding * 5, padding * 5));
            _form.Controls.Add(backButton);

            Button l2ButtonLVL1 = Menu.CreateButton("1", NormalButtonFontSize, new Size(HugeButtonW, HugeButtonH), ButtonBackgraundColor, L2ButtonLVL1, new Point(Window_W / 2 - (HugeButtonW + padding) * 2, (Window_H - HugeButtonH) / 2));
            _form.Controls.Add(l2ButtonLVL1);

            Button l2ButtonLVL2 = Menu.CreateButton("2", NormalButtonFontSize, new Size(HugeButtonW, HugeButtonH), ButtonBackgraundColor, L2ButtonLVL1, new Point(Window_W / 2 - HugeButtonW + padding * 2, (Window_H - HugeButtonH) / 2));
            _form.Controls.Add(l2ButtonLVL2);

            Button l2ButtonLVL3 = Menu.CreateButton("3", NormalButtonFontSize, new Size(HugeButtonW, HugeButtonH), ButtonBackgraundColor, L2ButtonLVL1, new Point(Window_W / 2 + padding * 6, (Window_H - HugeButtonH) / 2));
            _form.Controls.Add(l2ButtonLVL3);

            Button l2ButtonLVL4 = Menu.CreateButton("4", NormalButtonFontSize, new Size(HugeButtonW, HugeButtonH), ButtonBackgraundColor, L2ButtonLVL1, new Point(Window_W / 2 + HugeButtonW + padding * 10, (Window_H - HugeButtonH) / 2));
            _form.Controls.Add(l2ButtonLVL4);

            // Adding second layers buttons to their arr
            buttonMenuL2 = new Button[] { l2ButtonLVL1, l2ButtonLVL2, l2ButtonLVL3, l2ButtonLVL4, backButton };

            Button fadeButton = Menu.CreateButton(" ", WideButtonFontSize, new Size(MenuBackGraundL3W, MenuBackGraundL3H), MenuBackgraundColor, FadeButton, new Point((Window_W - MenuBackGraundL3W) / 2, (Window_H - MenuBackGraundL3H) / 2));
            _form.Controls.Add(fadeButton);

            Button resumeButton = Menu.CreateButton("Resume", WideButtonFontSize, new Size(WideButtonW, WideButtonH), ButtonBackgraundColor, ResumeGame, new Point((Window_W - WideButtonW) / 2, (Window_H - WideButtonH) / 2 - WideButtonH / 2 - padding));
            _form.Controls.Add(resumeButton);

            Button backButtonL3 = Menu.CreateButton("Back", NormalButtonFontSize, new Size(NormalButtonW, NormalButtonH), ButtonBackgraundColor, BackButton, new Point(Window_W / 2 - NormalButtonW - padding, Window_H / 2 + padding));
            _form.Controls.Add(backButtonL3);

            Button quitButton = Menu.CreateButton("Quit", NormalButtonFontSize, new Size(NormalButtonW, NormalButtonH), ButtonBackgraundColor, QuitButton, new Point(Window_W / 2 + padding, Window_H / 2 + padding));
            _form.Controls.Add(quitButton);

            // Adding third layers buttons to their arr
            buttonMenuL3 = new Button[] { fadeButton, resumeButton, backButtonL3, quitButton };
            fadeButton.SendToBack();
            resumeButton.BringToFront();
            backButtonL3.BringToFront();
            quitButton.BringToFront();

            Label fakeBorder = Menu.CreateLabel("", 28, ConfigurationManager.AppSettings["ButtonFontColor"] ?? "#000000", ButtonBackgraundColor, new Size(200, 720), new Point(1080, 0));
            _form.Controls.Add(fakeBorder);
            fakeBorder.SendToBack();

            Panel panel = Menu.CreatePanel(ButtonBackgraundColor, new Size(200, 720 - padding * 2), new Point(1080 + padding * 2, padding * 2));
            _form.Controls.Add(panel);
            panel.BringToFront();

            Label Text = Menu.CreateLabel("PakMan", 28, ButtonBackgraundColor, "#ffffff", new Size(200, 50), new Point(8, 10));
            panel.Controls.Add(Text);

            Label score = Menu.CreateLabel("Score: 0", 18, ButtonBackgraundColor, "#ffffff", new Size(200, 50), new Point(12, 60));
            panel.Controls.Add(score);

            Panel leaderboardTable = Menu.CreatePanel(ButtonBackgraundColor, new Size(160, 400), new Point(10, 170));
            leaderboardTable.BringToFront();
            panel.Controls.Add(leaderboardTable);

            scoreBoardL4 = new Control[] { fakeBorder, panel, Text, score, leaderboardTable };


            Panel gameOverPanel = Menu.CreatePanel(ButtonBackgraundColor, new Size(MenuBackGraundL3W, MenuBackGraundL3H), new Point((Window_W - MenuBackGraundL3W) / 2, (Window_H - MenuBackGraundL3H) / 2));
            _form.Controls.Add(gameOverPanel);

            Button gameOverBackButton = Menu.CreateButton("Back", NormalButtonFontSize, new Size(NormalButtonW, NormalButtonH), ButtonBackgraundColor, GameOverBackButton, new Point(MenuBackGraundL3W / 2 - NormalButtonW - padding, MenuBackGraundL3H / 2 + padding));
            gameOverPanel.Controls.Add(gameOverBackButton);

            Button gameOverQuitButton = Menu.CreateButton("Quit", NormalButtonFontSize, new Size(NormalButtonW,NormalButtonH),ButtonBackgraundColor, GameOverQuitButton, new Point(MenuBackGraundL3W / 2 + padding, MenuBackGraundL3H / 2 + padding));
            gameOverPanel.Controls.Add(gameOverQuitButton);

            Button restart = Menu.CreateButton("Restart", WideButtonFontSize, new Size(WideButtonW, WideButtonH), ButtonBackgraundColor, RestartGame, new Point((MenuBackGraundL3W - WideButtonW) / 2, (MenuBackGraundL3H - WideButtonH) / 2 - WideButtonH / 2 - padding));
            gameOverPanel.Controls.Add(restart);


            gameOverL4 = new Control[] {gameOverPanel, gameOverBackButton, gameOverQuitButton, restart };


        }

        // Buttons handlers, action by button click
        public void StartGame(object? sender, EventArgs e)
        {
            if (levelGameModeEnabled)
            {
                currentMenuLayer = 1;
                LayerSwitcher(1);
            }
            else if (infinityGameModeEnabled)
            {
                // Start infinity mode 
                currentMenuLayer = 3;
                LayerSwitcher(3);
                ClearMap();
                if (_form is MainForm mainForm)  // Cast _form to MainForm safely
                {
                    currentLevel = 0;
                    
                    mainForm.StartGame(currentLevel);  // Call function from MainForm
                }
                else
                {
                    MessageBox.Show("MainForm reference is invalid!");
                }
            }
            else
            {
                MessageBox.Show("You must choose game mode");
            }
        }

        public void InfinityMode(object? sender, EventArgs e)
        {
            string ButtonClicked = ConfigurationManager.AppSettings["MenuBackgraundColor"] ?? "#FFFFFF";
            string ButtonBackgraundColor = ConfigurationManager.AppSettings["ButtonBackgraundColor"] ?? "#000000";

            if ((!infinityGameModeEnabled && !levelGameModeEnabled) || (!infinityGameModeEnabled && levelGameModeEnabled))
            {
                buttonMenuL1[1].BackColor = ColorTranslator.FromHtml(ButtonClicked);
                buttonMenuL1[2].BackColor = ColorTranslator.FromHtml(ButtonBackgraundColor);
                infinityGameModeEnabled = true;
                levelGameModeEnabled = false;
            }
            else if (infinityGameModeEnabled && !levelGameModeEnabled)
            {
                buttonMenuL1[1].BackColor = ColorTranslator.FromHtml(ButtonBackgraundColor);
                buttonMenuL1[2].BackColor = ColorTranslator.FromHtml(ButtonBackgraundColor);
                infinityGameModeEnabled = false;
                levelGameModeEnabled = false;
            }
        }

        public void LevelMode(object? sender, EventArgs e)
        {
            string ButtonClicked = ConfigurationManager.AppSettings["MenuBackgraundColor"] ?? "#FFFFFF";
            string ButtonBackgraundColor = ConfigurationManager.AppSettings["ButtonBackgraundColor"] ?? "#000000";

            if ((!infinityGameModeEnabled && !levelGameModeEnabled) || (infinityGameModeEnabled && !levelGameModeEnabled))
            {
                buttonMenuL1[1].BackColor = ColorTranslator.FromHtml(ButtonBackgraundColor);
                buttonMenuL1[2].BackColor = ColorTranslator.FromHtml(ButtonClicked);
                infinityGameModeEnabled = false;
                levelGameModeEnabled = true;
            }
            else if (!infinityGameModeEnabled && levelGameModeEnabled)
            {
                buttonMenuL1[1].BackColor = ColorTranslator.FromHtml(ButtonBackgraundColor);
                buttonMenuL1[2].BackColor = ColorTranslator.FromHtml(ButtonBackgraundColor);
                infinityGameModeEnabled = false;
                levelGameModeEnabled = false;
            }
        }

        public void BackButton(object? sender, EventArgs e)
        {
            ClearMap(); 
       
            // With the following code
            if (_form is MainForm mainForm)
            {
                mainForm.GameOver();
            }
            else
            {
                MessageBox.Show("MainForm reference is invalid!");
            }
            currentMenuLayer = 0;

            Thread.Sleep(100);

            LayerSwitcher(0);
        }

        public void L2ButtonLVL1(object? sender, EventArgs e)
        {
            currentMenuLayer = 3;
            LayerSwitcher(3);
            if (_form is MainForm mainForm)  // Cast _form to MainForm safely
            {
                currentLevel = 1;
                mainForm.StartGame(currentLevel);  // Call function from MainForm
            }
            else
            {
                MessageBox.Show("MainForm reference is invalid!");
            }
        }

        public void L2ButtonLVL2(object? sender, EventArgs e)
        {
        }

        public void L2ButtonLVL3(object? sender, EventArgs e)
        {
        }

        public void L2ButtonLVL4(object? sender, EventArgs e)
        {
        }

        public void FadeButton(object? sender, EventArgs e)
        {
        }

        public void ResumeGame(object? sender, EventArgs e)
        {
            if (_form is MainForm mainForm)  // Cast _form to MainForm safely
            {
                mainForm.mainGameTimer.Start();
            }
            else
            {
                MessageBox.Show("MainForm reference is invalid!");
            }
            LayerSwitcher(3);
        }

        public void QuitButton(object? sender, EventArgs e)
        {
            Application.Exit();
        }
        public void GameOverBackButton(object? sender, EventArgs e)
        {

        }
        public void GameOverQuitButton(object? sender, EventArgs e)
        {
            Application.Exit();
        }
        public void RestartGame(object? sender, EventArgs e)
        {
            ClearMap();
            if (_form is MainForm mainForm)  // Cast _form to MainForm safely
            {
                mainForm.GameOver();
                if (Map._ghosts != null)
                {
                    Map._ghosts.Clear();
                }
                mainForm.StartGame(currentLevel);  // Call function from MainForm
                Debug.WriteLine(currentLevel);
                UpdateScore(0);
            }
            else
            {
                MessageBox.Show("MainForm reference is invalid!");
            }
            LayerSwitcher(3);
        }
        // Layer switcher function 
        public void LayerSwitcher(int layer)
        {
            switch (layer)
            {
                case 0:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = true;
                    }
                    foreach (Button button in buttonMenuL2)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = false;
                    }
                    foreach (Control control in scoreBoardL4)
                    {
                        control.Visible = false;
                    }
                    foreach (Control control in gameOverL4)
                    {
                        control.Visible = false;
                    }
                    break;

                case 1:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL2)
                    {
                        button.Visible = true;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = false;
                    }
                    foreach (Control control in scoreBoardL4)
                    {
                        control.Visible = false;
                    }
                    foreach (Control control in gameOverL4)
                    {
                        control.Visible = false;
                    }
                    break;

                case 2:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL2)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = true;
                    }
                    foreach (Control control in scoreBoardL4)
                    {
                        control.Visible = true;
                    }
                    foreach (Control control in gameOverL4)
                    {
                        control.Visible = false;
                    }
                    break;
                case 3:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL2)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = false;
                    }
                    foreach (Control control in scoreBoardL4)
                    {
                        control.Visible = true;
                    }
                    foreach (Control control in gameOverL4)
                    {
                        control.Visible = false;
                    }
                    break;
                case 4:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL2)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = false;
                    }
                    foreach (Control control in scoreBoardL4)
                    {
                        control.Visible = false;
                    }
                    foreach (Control control in gameOverL4)
                    {
                        control.Visible = false;
                    }
                    break;
                case 5:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL2)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = false;
                    }
                    foreach (Control control in scoreBoardL4)
                    {
                        control.Visible = true;
                    }
                    foreach (Control control in gameOverL4)
                    {
                        control.Visible = true;
                    }
                    break;
                default:
                    break;
            }
        }

        public void UpdateScore(int score)
        {
            foreach (Control control in _form.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control panelControl in panel.Controls)
                    {
                        if (panelControl is Label label)
                        {
                            if (label.Text.Contains("Score"))
                            {
                                label.Text = $"Score: {score}";
                            }
                        }
                    }
                }
            }
        }
        public void ClearMap() 
        {
            for (int i = _form.Controls.Count - 1; i >= 0; i--)
            {
                Control ctrl = _form.Controls[i];

                // Check if the control is a PictureBox and its Name is "wall"
                if (ctrl is PictureBox && (ctrl.Name == "Wall" || ctrl.Name == "Player" || ctrl.Name == "Ghost" || ctrl.Name == "Coin" || ctrl.Name == "Fruit"))
                {
                    // Remove the control
                    _form.Controls.Remove(ctrl);

                    // Dispose of the control to free resources
                    ctrl.Dispose();
                }
            }
        }

    }
}

