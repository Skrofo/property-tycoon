using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyTycoon.Scripts
{
    public class Player
    {
        public bool cpu { get; }
        public int money = 0;
        public string[] properties = Array.Empty<string>();
        public Node2D node { get; }
        public Marker location = null; //Where the player currently is

        public Player(Node2D playerNode, bool isCpu)
        {
            node = playerNode;
            cpu = isCpu;
        }

        public void MoveForward(int numberOfPlaces)
        {
            location?.RemovePlayer(this);
            location = location.GetNthNextPos(numberOfPlaces);
            location.MoveTo(this);
        }
    }
}
