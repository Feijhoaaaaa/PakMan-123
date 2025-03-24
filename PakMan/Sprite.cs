using System.Diagnostics;

namespace PakMan
{

    public class Sprite
    {
        public int posX, posY, speed;
    }
    public class Player : Sprite
    {
        public int score;

        private readonly Form _form;
        private readonly EventArgs _event;
        private readonly Map _map;
        private readonly Menu _menu;
        private readonly MainForm _mainForm;

        public Player(Form form)
        {
            _form = form;
            _map = new Map(form);
            _mainForm = (MainForm)form; // Cast form to MainForm
            _menu = new Menu(form); // Initialize the Menu instance
            _event = new EventArgs(); // Initialize the EventArgs instance
            speed = 45; // Initialize speed in the constructor
        }

        public bool CanMove(int posX, int posY)
        {
            int[,,]? mapMatrix = Map.ReadMapMatrixFromJson(Map.jsonPath);
            if (mapMatrix == null)
            {
                Debug.WriteLine("Map matrix is null.");
                return false;
            }

            int[,] map = _map.GetMapFromMatrix(Menu.currentLevel, mapMatrix);

            Debug.WriteLine($"Checking if player can move to position: {posX}, {posY}");
            if (posX < 0 || posX >= map.GetLength(1) * 45 || posY < 0 || posY >= map.GetLength(0) * speed)
            {
                return false;
            }
            foreach (Control c in _form.Controls)
            {
                if (c is PictureBox && c.Name == "Wall")
                {
                    if (c.Left == posX && c.Top == posY)
                    {
                        return false;
                    }
                }
                else if (c is PictureBox && c.Name == "Coin")
                {
                    if (c.Left == posX && c.Top == posY)
                    {
                        c.Visible = false;
                        score++;
                        _form.Controls.Remove(c);
                        _menu.UpdateScore(score); // Update the score using the Menu instance
                        return true;
                    }
                }
                else if (c is PictureBox && c.Name == "Ghost")
                {
                    if (c.Left == posX && c.Top == posY)
                    {
                        _mainForm.GameOver();
                        score = 0;
                        return true;
                    }
                }
                else if (c is PictureBox && c.Name == "Fruit")
                {
                    if (c.Left == posX && c.Top == posY)
                    {
                        c.Visible = false;
                        score += 10;
                        _form.Controls.Remove(c);
                        _menu.UpdateScore(score); // Update the score using the Menu instance
                        return true;
                    }
                }
            }
            return true;
        }

        public void PlayerMove(int speedX, int speedY)
        {
            if (Map._player == null)
            {
                Debug.WriteLine("Player object is not initialized.");
                return;
            }

            posX = Map._player.Left;
            posY = Map._player.Top;

            int newPosX = posX + speedX;
            int newPosY = posY + speedY;

            if (CanMove(newPosX, newPosY))
            {
                Map._player.Left = newPosX;
                Map._player.Top = newPosY;
                Debug.WriteLine($"Player position: {posX}, {posY}");
            }
            else
            {
                Debug.WriteLine("Player cannot move to this position.");
            }
        }
    }
    public class Ghost : Sprite
    {

        private readonly Form _form;
        private readonly Map _map;
        private readonly Menu _menu;
        private readonly MainForm _mainForm;


        int priviousPosXPink, priviousPosYPink;
        int priviousPosXRed, priviousPosYRed;
        int priviousPosXBlue, priviousPosYBlue;




        public Ghost(Form form)
        {
            _form = form;
            _map = new Map(form);
            _menu = new Menu(form); // Initialize the Menu instance
            _mainForm = (MainForm)form;

            speed = 45; // Initialize speed in the constructor
        }
        public bool CanMoveGhost(int posX, int posY)
        {
            int[,,]? mapMatrix = Map.ReadMapMatrixFromJson(Map.jsonPath);
            if (mapMatrix == null)
            {
                Debug.WriteLine("Map matrix is null.");
                return false;
            }

            int[,] map = _map.GetMapFromMatrix(Menu.currentLevel, mapMatrix);

            if (posX < 0 || posX >= map.GetLength(1) * speed || posY < 0 || posY >= map.GetLength(0) * speed)
            {
                return false;
            }
            foreach (Control c in _form.Controls)
            {
                if (c is PictureBox && c.Name == "Wall")
                {
                    if (c.Left == posX && c.Top == posY)
                    {
                        return false;
                    }
                }
            }
            foreach (Control c in _form.Controls)
            {
                if (c is PictureBox && c.Name == "Player")
                {
                    if (c.Left == posX && c.Top == posY)
                    {
                        _mainForm.GameOver();
                        return false;
                    }
                }
            }
            // Additional logic for checking movement can be added here
            return true;
        }
        public List<List<int>> GhostPossibleMoveWithOutMap(PictureBox ghost)
        {
            int posXNow = ghost.Left;
            int posYNow = ghost.Top;

            List<List<int>> ghostPossibleMoveWithOutMap = new List<List<int>>();

            if (ghost.Name == "Ghost")
            {
                ghostPossibleMoveWithOutMap = new List<List<int>>()
                {
                    new List<int>{posXNow, posYNow - speed},
                    new List<int>{posXNow, posYNow + speed},
                    new List<int>{posXNow - speed, posYNow},
                    new List<int>{posXNow + speed, posYNow}
                };
            }

            return ghostPossibleMoveWithOutMap;
        }

        public void GhostMovePink(PictureBox ghost)
        {
            int posXNow = ghost.Left;
            int posYNow = ghost.Top;

            List<List<int>> ghostPossibleMoveWithOutMap = GhostPossibleMoveWithOutMap(ghost);

            ghostPossibleMoveWithOutMap.RemoveAll(subList => subList.SequenceEqual(new List<int> { priviousPosXPink, priviousPosYPink }));

            List<List<int>> itemsToRemove = new List<List<int>>();

            foreach (List<int> item in ghostPossibleMoveWithOutMap)
            {
                if (!CanMoveGhost(item[0], item[1]))
                {
                    itemsToRemove.Add(item);
                }
            }

            foreach (var item in itemsToRemove)
            {
                ghostPossibleMoveWithOutMap.RemoveAll(subList => subList.SequenceEqual(new List<int> { item[0], item[1] }));
            }

            if (ghostPossibleMoveWithOutMap.Count == 0)
            {
                Debug.WriteLine("No valid moves available for the ghost.");
                return;
            }

            Random random = new Random();
            int randomMove = random.Next(0, ghostPossibleMoveWithOutMap.Count);
            int newPosX = ghostPossibleMoveWithOutMap[randomMove][0];
            int newPosY = ghostPossibleMoveWithOutMap[randomMove][1];
            if (CanMoveGhost(newPosX, newPosY))
            {
                ghost.Left = newPosX;
                ghost.Top = newPosY;
                Debug.WriteLine($"Ghost position: {newPosX}, {newPosY}");
            }
            else
            {
                Debug.WriteLine("Ghost cannot move to this position.");
            }
            priviousPosXPink = posXNow;
            priviousPosYPink = posYNow;
        }

        public void GhostMoveRed(PictureBox ghost)
        {
            int posXNow = ghost.Left;
            int posYNow = ghost.Top;
            List<List<int>> ghostPossibleMoveWithOutMap = GhostPossibleMoveWithOutMap(ghost);
            ghostPossibleMoveWithOutMap.RemoveAll(subList => subList.SequenceEqual(new List<int> { priviousPosXRed, priviousPosYRed }));
            List<List<int>> itemsToRemove = new List<List<int>>();
            foreach (List<int> item in ghostPossibleMoveWithOutMap)
            {
                if (!CanMoveGhost(item[0], item[1]))
                {
                    itemsToRemove.Add(item);
                }
            }
            foreach (var item in itemsToRemove)
            {
                ghostPossibleMoveWithOutMap.RemoveAll(subList => subList.SequenceEqual(new List<int> { item[0], item[1] }));
            }
            if (ghostPossibleMoveWithOutMap.Count == 0)
            {
                Debug.WriteLine("No valid moves available for the ghost.");
                return;
            }
            Random random = new Random();
            int randomMove = random.Next(0, ghostPossibleMoveWithOutMap.Count);
            int newPosX = ghostPossibleMoveWithOutMap[randomMove][0];
            int newPosY = ghostPossibleMoveWithOutMap[randomMove][1];
            if (CanMoveGhost(newPosX, newPosY))
            {
                ghost.Left = newPosX;
                ghost.Top = newPosY;
                Debug.WriteLine($"Ghost position: {newPosX}, {newPosY}");
            }
            else
            {
                Debug.WriteLine("Ghost cannot move to this position.");
            }
            priviousPosXRed = posXNow;
            priviousPosYRed = posYNow;
        }
        public void GhostMoveBlue(PictureBox ghost)
        {
            int posXNow = ghost.Left;
            int posYNow = ghost.Top;
            List<List<int>> ghostPossibleMoveWithOutMap = GhostPossibleMoveWithOutMap(ghost);
            ghostPossibleMoveWithOutMap.RemoveAll(subList => subList.SequenceEqual(new List<int> { priviousPosXBlue, priviousPosYBlue }));
            List<List<int>> itemsToRemove = new List<List<int>>();
            foreach (List<int> item in ghostPossibleMoveWithOutMap)
            {
                if (!CanMoveGhost(item[0], item[1]))
                {
                    itemsToRemove.Add(item);
                }
            }
            foreach (var item in itemsToRemove)
            {
                ghostPossibleMoveWithOutMap.RemoveAll(subList => subList.SequenceEqual(new List<int> { item[0], item[1] }));
            }
            if (ghostPossibleMoveWithOutMap.Count == 0)
            {
                Debug.WriteLine("No valid moves available for the ghost.");
                return;
            }
            Random random = new Random();
            int randomMove = random.Next(0, ghostPossibleMoveWithOutMap.Count);
            int newPosX = ghostPossibleMoveWithOutMap[randomMove][0];
            int newPosY = ghostPossibleMoveWithOutMap[randomMove][1];
            if (CanMoveGhost(newPosX, newPosY))
            {
                ghost.Left = newPosX;
                ghost.Top = newPosY;
                Debug.WriteLine($"Ghost position: {newPosX}, {newPosY}");
            }
            else
            {
                Debug.WriteLine("Ghost cannot move to this position.");
            }
            priviousPosXBlue = posXNow;
            priviousPosYBlue = posYNow;
        }
    }
}
