using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace PakMan
{
    class Menu
    {
        public static int currentMenuLayer;
        public static bool infinityGameModeEnabled;
        public static bool levelGameModeEnabled;

        public Button[] buttonMenuL1, buttomMenuL2, buttonMenuL3;
        private Form _form;
        private EventArgs _event;

        public Menu(Form form)
        {
            _form = form;
            

        }


        public void ApplyWindowAdjustments(Form form)
        {
            try
            {
                int width = int.Parse(ConfigurationManager.AppSettings["WindowWithSize"]);
                int height = int.Parse(ConfigurationManager.AppSettings["WindwoHeightSize"]);
                string backgraundColor = ConfigurationManager.AppSettings["WindowBackgraundColor"];
                string windowsName = ConfigurationManager.AppSettings["WindowName"];
                string windowFormBorderStyle = ConfigurationManager.AppSettings["WindowFormBorderStyle"];

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

            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                Font = new Font(ConfigurationManager.AppSettings["ButtonFontName"], fontSize, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml(color),
                ForeColor = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["ButtonFontColor"]),
                FlatStyle = FlatStyle.Flat,
                Visible = false

            };
            button.FlatAppearance.BorderColor = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["ButtonBorderColor"]);
            button.FlatAppearance.BorderSize = 3;

            // Attach the event handler
            button.Click += onClick;

            return button;
        }

        

        public void MenuLoad()
        {
            int Window_W = int.Parse(ConfigurationManager.AppSettings["WindowWithSize"]);
            int Window_H = int.Parse(ConfigurationManager.AppSettings["WindwoHeightSize"]);

            int NormalButtonW = int.Parse(ConfigurationManager.AppSettings["NormalButtonW"]);
            int NormalButtonH = int.Parse(ConfigurationManager.AppSettings["NormalButtonH"]);
            int NormalButtonFontSize = int.Parse(ConfigurationManager.AppSettings["NormalButtonFontSize"]);

            int WideButtonW = int.Parse(ConfigurationManager.AppSettings["WideButtonW"]);
            int WideButtonH = int.Parse(ConfigurationManager.AppSettings["WideButtonH"]);
            int WideButtonFontSize = int.Parse(ConfigurationManager.AppSettings["WideButtonFontSize"]);

            int HugeButtonW = int.Parse(ConfigurationManager.AppSettings["HugeButtonW"]);
            int HugeButtonH = int.Parse(ConfigurationManager.AppSettings["HugeButtonH"]);
            int HugeButtonFontSize = int.Parse(ConfigurationManager.AppSettings["HugeButtonFontSize"]);

            int BackButtonW = int.Parse(ConfigurationManager.AppSettings["BackButtonW"]);
            int BackButtonFontSize = int.Parse(ConfigurationManager.AppSettings["BackButtonFontSize"]);

            int MenuBackGraundL3W = int.Parse(ConfigurationManager.AppSettings["MenuBackGraundL3W"]);
            int MenuBackGraundL3H = int.Parse(ConfigurationManager.AppSettings["MenuBackGraundL3H"]);

            string MenuBackgraundColor = ConfigurationManager.AppSettings["MenuBackgraundColor"];
            string ButtonBackgraundColor = ConfigurationManager.AppSettings["ButtonBackgraundColor"];

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

            // Adding secound layers buttons to their arr

            buttomMenuL2 = new Button[] { l2ButtonLVL1, l2ButtonLVL2, l2ButtonLVL3, l2ButtonLVL4, backButton };



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

        }


        //Buttons handlers, action by button click

        public void StartGame(object sender, EventArgs e)
        {
            if (levelGameModeEnabled)
            {
                currentMenuLayer = 1;
                LayerSwitcher(1);
            }
            else if (infinityGameModeEnabled)
            {
                // Start infinty mode 
                MessageBox.Show("Infinity mode started");
                currentMenuLayer = 3;
                LayerSwitcher(3);
            }
            else { MessageBox.Show("You mast to choose game mode"); }
            
            
        }

        public void InfinityMode(object sender, EventArgs e)
        {

            string ButtonClicked = ConfigurationManager.AppSettings["MenuBackgraundColor"];
            string ButtonBackgraundColor = ConfigurationManager.AppSettings["ButtonBackgraundColor"];

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

        public void LevelMode(object sender, EventArgs e)
        {

            string ButtonClicked = ConfigurationManager.AppSettings["MenuBackgraundColor"];
            string ButtonBackgraundColor = ConfigurationManager.AppSettings["ButtonBackgraundColor"];
            

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

        public void BackButton(object sender, EventArgs e)
        {
            currentMenuLayer = 0;
            LayerSwitcher(0);
        }

        public void L2ButtonLVL1(object sender, EventArgs e)
        {
        }

        public void L2ButtonLVL2(object sender, EventArgs e)
        {
        }

        public  void L2ButtonLVL3(object sender, EventArgs e)
        {
        }

        public  void L2ButtonLVL4(object sender, EventArgs e)
        {
        }

        public  void FadeButton(object sender, EventArgs e)
        {
        }

        public  void ResumeGame(object sender, EventArgs e)
        {
        }

        public  void QuitButton(object sender, EventArgs e)
        {
        }


        // Layer swither function 

        public void LayerSwitcher(int layer)
        {
            switch (layer)
            {
                case 0:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = true;
                    }
                    foreach (Button button in buttomMenuL2)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = false;
                    }
                    break;


                case 1:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttomMenuL2)
                    {
                        button.Visible = true;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = false;
                    }
                    break;

                case 2:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttomMenuL2)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = true;
                    }
                    break;
                case 3:
                    foreach (Button button in buttonMenuL1)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttomMenuL2)
                    {
                        button.Visible = false;
                    }
                    foreach (Button button in buttonMenuL3)
                    {
                        button.Visible = false;
                    }
                    break;
                default:
                    break;
            }
        }

        


    }
}

