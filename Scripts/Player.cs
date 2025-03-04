using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyTycoon.Scripts
{
    public class Player(Node2D playerNode, bool isCpu)
    {
        public bool cpu { get; } = isCpu;
        private int money = 0;
        public string[] properties = Array.Empty<string>();
        public Node2D node { get; } = playerNode;
        public Marker location = null; //Where the player currently is
        public bool passedGo = false;
        public int jailTurns = 0;

        public void MoveForward(int numberOfPlaces)
        {
            location?.RemovePlayer(this);
            location = location.GetNthNextPos(numberOfPlaces, this);
            location.MoveTo(this);
        }

        public void GoToJail()
        {
            jailTurns = 2;
            MoveTo("Board/Places/Jail");
            node.FindParent("Game").GetNode<GameLoop>(".").justMoved = true;
        }

        //TODO:Finish bail method
        public void PayBail()
        {
            AddMoney(-50);
            node.FindParent("Game").GetNode<GameLoop>(".").AddParkingMoney(50);
            jailTurns = 0;
            MoveTo("Board/Places/VisitingJail");
        }

        public int GetMoney() => money;

        public void AddMoney(int amount)
        {
            money += amount;
            node.FindParent("Game").GetNode<Label>("UI/Money").Text = GameLoop.moneySymbol + GetMoney().ToString();
        }

        private void MoveTo(string path)
        {
            location?.RemovePlayer(this);
            location = node.FindParent("Game").GetNode<Marker>(path);
            location.MoveTo(this);
        }
    }
}
