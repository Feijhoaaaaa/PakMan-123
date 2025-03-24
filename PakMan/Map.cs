using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.Drawing;
using Timer = System.Windows.Forms.Timer;


namespace PakMan
{
    class Map
    {
        public int currentMap { get; set; }
        public int score;
        public static string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Map_Matrix_3D.json");
        public int[,,]? mapMatrix = ReadMapMatrixFromJson(jsonPath);

        private Form _form;
        public static PictureBox? _player;
        public static PictureBox[]? _coins;
        public static List<PictureBox>? _ghosts;

        //private Image[]? pakmanImg;
        public bool isOpen = false;
        public bool ghostEU = false;
        public Timer animationTimer;
        public string currentDirection = "right";



        public Map(Form form)
        {
            _form = form;

            // Initialize and start the animation timer
            
        }

        

        

        public static int[,,]? ReadMapMatrixFromJson(string filePath)
        {
            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read the JSON content from the file
                    string json = File.ReadAllText(filePath);

                    // Deserialize the JSON content into a 3D array
                    return JsonConvert.DeserializeObject<int[,,]>(json);
                }
                else
                {
                    Debug.WriteLine("File not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., JSON format errors, file reading errors)
                Debug.WriteLine($"Error reading or deserializing the JSON file: {ex.Message}");
                return null;
            }
        }

        public void MapLoad(int currentMap)
        {
            var mapMatrix = ReadMapMatrixFromJson(jsonPath);
            if (mapMatrix == null) return;

            for (int y = 0; y < mapMatrix.GetLength(1); y++)
            {
                for (int x = 0; x < mapMatrix.GetLength(2); x++)
                {
                    ArrangeSprites(mapMatrix[currentMap, y, x], x, y);
                }
            }
        }

        public void ArrangeSprites(int value, int posXFromMatrix, int posYFromMatrix)
        {
            switch (value)
            {
                case 0:
                    break;
                case 1:
                    CreateWall(posXFromMatrix, posYFromMatrix);
                    break;
                case 2:
                    CreateCoin(posXFromMatrix, posYFromMatrix);
                    break;
                case 3:
                    CreateGhost(posXFromMatrix, posYFromMatrix);
                    break;
                case 4:
                    CreateFruits(posXFromMatrix, posYFromMatrix);
                    break;
                case 5:
                    CreatePlayer(posXFromMatrix, posYFromMatrix);
                    break;
                default:
                    break;
            }
        }

        public int[,] GetMapFromMatrix(int currentMap, int[,,] matrix)
        {
            int[,] map = new int[matrix.GetLength(1), matrix.GetLength(2)];
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                for (int x = 0; x < matrix.GetLength(2); x++)
                {
                    map[y, x] = matrix[currentMap, y, x];
                }
            }
            return map;
        }

        public void PrintToOutput2DArr(int[,] matrix)
        {
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    Debug.Write($"{matrix[y, x]} ");
                }
                Debug.WriteLine(" ");
            }
        }

        public void CreateCoin(int x, int y)
        {
            PictureBox coin = new PictureBox();
            coin.Name = "Coin";
            coin.BackColor = Color.Gold;
            coin.Location = new Point(x * 45, y * 45);
            coin.Size = new Size(45, 45);
            coin.Visible = true;
            _form.Controls.Add(coin);
        }

        public void CreateWall(int x, int y)
        {
            PictureBox wall = new PictureBox();
            wall.Name = "Wall";
            using (var ms = new MemoryStream(Properties.Resources.Wall))
            {
                wall.Image = Image.FromStream(ms);
            }
            wall.SizeMode = PictureBoxSizeMode.StretchImage;
            wall.Location = new Point(x * 45, y * 45);
            wall.Size = new Size(45, 45);
            wall.Visible = true;
            _form.Controls.Add(wall);
        }

        public void CreateGhost(int x, int y)
        {
            PictureBox ghost = new PictureBox();
            ghost.Name = "Ghost";
            ghost.Image = Image.FromFile(@"C:\Users\bohda\source\repos\PakMan\PakMan\GhostRedRight.png");
            ghost.SizeMode = PictureBoxSizeMode.StretchImage;
            ghost.Location = new Point(x * 45, y * 45);
            ghost.Size = new Size(45, 45);
            ghost.Visible = true;
            if (_ghosts == null)
            {
                _ghosts = new List<PictureBox>();
            }
            _ghosts.Add(ghost);
            _form.Controls.Add(ghost);
        }

        public void CreatePlayer(int x, int y)
        {
            PictureBox player = new PictureBox();
            player.Name = "Player";
            using (var ms = new MemoryStream(Properties.Resources.PakManO))
            {
                player.Image = Image.FromStream(ms);
            }
            player.Location = new Point(x * 45, y * 45);
            player.Size = new Size(45, 45);
            player.Visible = true;
            _player = player;
            _form.Controls.Add(player);
        }

        public void CreateFruits(int x, int y)
        {
            PictureBox fruit = new PictureBox();
            fruit.Name = "Fruit";
            fruit.BackColor = Color.Pink;
            fruit.Location = new Point(x * 45, y * 45);
            fruit.Size = new Size(45, 45);
            fruit.Visible = true;
            _form.Controls.Add(fruit);
        }
    }


   
}