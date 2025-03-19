using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace PropertyTycoon.Scripts
{
    public class Card
    {
        public const string opportunityDirPath = "res://Assets/Card/OpportunityKnocks/TextCard/";//The path for the directory containing all the opportunity knocks card textures
        public const string potDirPath = "res://Assets/Card/PotLuck/TextCard/";//The path for the directory containing all the pot luck card textures

        public enum CardType { OpportunityKnocks, PotLuck }
        public CardType type;
        public string name;

        private readonly GameLoop gameLoop;

        public Card(CardType cardType)
        {
            type = cardType;
            string[] cards = GetCards(type);
            Random rnd = new();
            name = cards[rnd.Next(0,cards.Length)];
        }
        public Card(string cardName)
        {
            name = cardName;
            bool cardFound = false;
            foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
            {
                string[] cards = GetCards(cardType);
                if (cards.Contains<string>(name))
                {
                    type = cardType;
                    cardFound = true;
                }
            }
            if (cardFound == false)
            {
                GD.PrintErr("Couldn't find specified card");
            }
        }

        //Causes the current card to execute the action associated with it
        public bool ExecuteAction( GameLoop gameLoop)
        {
            if (type == CardType.OpportunityKnocks)
            {
                switch (name)
                {
                    case "Opportunity Knocks Card - 1.png"://Bank pays player £50
                        gameLoop.currentPlayer.AddMoney(50);
                        break;
                    default:
                        GD.PrintErr($"Couldn't find action associated with Opportunity Knocks card: {name}");
                        return false;
                }
            }
            else if (type == CardType.PotLuck)
            {
                switch (name)
                {
                    default:
                        GD.PrintErr($"Couldn't find action associated with Pot Luck card: {name}");
                        return false;
                }
            }
            return true;
        }

        public static String[] GetCards(CardType type)
        {
            //creating the global path for the card directory
            string folderPath = null;
            if (type == CardType.OpportunityKnocks)
            {
                folderPath = ProjectSettings.GlobalizePath(opportunityDirPath);
            }
            else if (type == CardType.PotLuck)
            {
                folderPath = ProjectSettings.GlobalizePath(potDirPath);
            }

            //Getting all the filenames in the given asset folder
            using DirAccess dir = DirAccess.Open(folderPath);
            if (dir == null)
            {
                GD.PrintErr($"Failed to find card asset dir.\tType:{type}\tPath:{folderPath}");
                return null;
            }
            GD.Print($"Loaded card asset dir: {folderPath}");

                dir.ListDirBegin();
            string fileName;
            List<string> fileNames = new();

            while ((fileName = dir.GetNext()) != "")
            {
                if (!dir.CurrentIsDir())//Making sure current item is a file and not a folder
                {
                    string ending = fileName.Length >= 7 ? fileName[^7..] : fileName;//Gets the last 7 chars of fileName
                    if (ending != ".import")//Making sure the .import files Godot generates when importing a resource aren't included in the list
                    {
                        fileNames.Add(fileName);
                    }
                }
            }
            return fileNames.ToArray();
        }
    }
}
