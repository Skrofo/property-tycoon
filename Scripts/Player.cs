using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyTycoon.Scripts
{
    public class Player
    {
        public bool cpu { get; }
        public bool hasGetOutOfJail = false;
        public int money = 0;
        public string[] properties = Array.Empty<string>();
        public Node2D node { get; }
        public Marker location = null; //Where the player currently is
        public bool passedGo = false;
        public int jailTurns = 0;

        private GameLoop gameLoop; //The gameloop script

        public Player(Node2D playerNode, bool isCpu)
        {
            cpu = isCpu;
            node = playerNode;
            gameLoop = node.FindParent("Game").GetNode<GameLoop>(".");
        }

        public void MoveForward(int numberOfPlaces)
        {
            location?.RemovePlayer(this);
            location = location.GetNthNextPos(numberOfPlaces, this);
            location.MoveTo(this);
        }

        public void MoveBackward(int numberOfPlaces)
        {
            location?.RemovePlayer(this);
            location = location.GetNthPreviousPos(numberOfPlaces, this);
            location.MoveTo(this);
        }

        public void GoToJail()
        {
            jailTurns = 2;
            MoveTo("Board/Places/Jail");
            gameLoop.justMoved = true;
        }

        //Runs when the player tries to pay their bail. Returns true if the bail was succesfully paid
        public bool PayBail()
        {
            if (GetMoney() >= 50)
            {
                AddMoney(-50);
                gameLoop.AddParkingMoney(50);
                getOutOfJail();
                return true;
            }
            else return false;
        }

        public void getOutOfJail()
        {
            jailTurns = 0;
            MoveTo("Board/Places/VisitingJail");
        }

        public void PassGo()
        {
            GD.Print("Passed Go!");
            AddMoney(200);
            passedGo = false;
        }

        public int GetMoney() => money;

        public void AddMoney(int amount)
        {
            money += amount;
            //TODO: Add visual effect when changing player's money value
            node.FindParent("Game").GetNode<Label>("UI/Money").Text = GameLoop.moneySymbol + GetMoney().ToString();
        }

        public void MoveTo(string path)
        {
            location?.RemovePlayer(this);
            location = node.FindParent("Game").GetNode<Marker>(path);
            location.MoveTo(this);
        }
    }
}
