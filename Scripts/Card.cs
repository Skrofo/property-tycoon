using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                    case "Opportunity Knocks Card - 2.png"://Bank pays player £100
                        gameLoop.currentPlayer.AddMoney(100);
                        break;
                    case "Opportunity Knocks Card - 3.png"://Player moves to Turing Heights
                        gameLoop.currentPlayer.MoveTo("Board/Places/Place36");
                        break;
                    case "Opportunity Knocks Card - 4.png": //player moves to Han Xin Gardens
                        gameLoop.currentPlayer.MoveTo("Board/Places/Place19");
                        break;
                    case "Opportunity Knocks Card - 5.png"://Player puts £15 on Free Parking
                        gameLoop.currentPlayer.AddMoney(-15);
                        gameLoop.AddParkingMoney(15);
                        break;
                    case "Opportunity Knocks Card - 6.png"://Player pays £150 to bank
                        gameLoop.currentPlayer.AddMoney(-150);
                        break;
                    case "Opportunity Knocks Card - 7.png"://Player moves to Hove Station
                        gameLoop.currentPlayer.MoveTo("Board/Places/Place14");
                        break;
                    case "Opportunity Knocks Card - 8.png"://Bank pays player £150
                        gameLoop.currentPlayer.AddMoney(150);
                        break;
                    /*
                    case "Opportunity Knocks Card - 9.png"://Player pays £40 to bank for house and £115 for hotel
                        gameLoop.currentPlayer.AddMoney(-150); //TODO: rent functionality
                        break;
                    */
                    case "Opportunity Knocks Card - 10.png"://Advance to GO
                        gameLoop.currentPlayer.MoveTo("Board/Places/Go");
                        break;
                    /*
                    case "Opportunity Knocks Card - 11.png": //Player pays £25 to bank for house and £100 for hotel
                                                    //TODO: rent functionality
                        break;
                    */
                    /*
                    case "Opportunity Knocks Card - 12.png": // player moves back 3 spaces
                        gameLoop.currentPlayer.MoveTo(); //TODO: functionality for moving backwards
                        break;
                    */
                    case "Opportunity Knocks Card - 13.png": // player moves to Skywalker drive
                        gameLoop.currentPlayer.MoveTo("Board/Places/Place12");
                        break;
                    case "Opportunity Knocks Card - 14.png": //player goes to jail
                        gameLoop.currentPlayer.GoToJail();
                        break;
                    case "Opportunity Knocks Card - 15.png": //player puts £30 on free parking
                        gameLoop.currentPlayer.AddMoney(-30);
                        gameLoop.AddParkingMoney(30);
                        break;
                    /*
                    case "Opportunity Knocks Card - 16.png": // get out of jail
                        break; //TODO: get out of jail functionality
                        */
                    default:
                        GD.PrintErr($"Couldn't find action associated with Opportunity Knocks card: {name}");
                        return false;
                }
            }
            else if (type == CardType.PotLuck)
            {
                switch (name)
                {
                    case "Pot Luck Card - 1.png": //Bank gives £200 to player
                        gameLoop.currentPlayer.AddMoney(200);
                        break;
                    case "Pot Luck Card - 2.png": //Bank gives £50 to player
                        gameLoop.currentPlayer.AddMoney(50);
                        break;
                    case "Pot Luck Card - 3.png": //player moves backward to old creek
                        gameLoop.currentPlayer.MoveTo("Board/Places/Place3");
                        break;
                    case "Pot Luck Card - 4.png": //Bank pays player £20
                        gameLoop.currentPlayer.AddMoney(20);
                        break;
                    case "Pot Luck Card - 5.png": //Bank pays player £200
                        gameLoop.currentPlayer.AddMoney(200);
                        break;
                    case "Pot Luck Card - 6.png": //player pays £100 to bank
                        gameLoop.currentPlayer.AddMoney(-100);
                        break;
                    case "Pot Luck Card - 7.png": //player pays £50 to bank
                        gameLoop.currentPlayer.AddMoney(-50);
                        break;
                    case "Pot Luck Card - 8.png": //player advances to GO
                        gameLoop.currentPlayer.MoveTo("Board/Places/Go");
                        break;
                    case "Pot Luck Card - 9.png": //Bank pays player £50
                        gameLoop.currentPlayer.AddMoney(50);
                        break;
                    case "Pot Luck Card - 10.png": //player pays £50 to bank
                        gameLoop.currentPlayer.AddMoney(-50);
                        break;
                    /*
                    case "Pot Luck Card - 11.png": //player pays £10 fine to free parking or takes opportunity knocks card
                        gameLoop.currentPlayer.AddMoney(200);
                        break;
                    */
                    case "Pot Luck Card - 12.png": //player puts £50 on free parking
                        gameLoop.currentPlayer.AddMoney(-50);
                        gameLoop.AddParkingMoney(50);
                        break;
                    case "Pot Luck Card - 13.png": //bank pays £100 to player
                        gameLoop.currentPlayer.AddMoney(100);
                        break;
                    case "Pot Luck Card - 14.png": //go to jail
                        gameLoop.currentPlayer.GoToJail();
                        break;
                    case "Pot Luck Card - 15.png": //bank pays player £25
                        gameLoop.currentPlayer.AddMoney(25);
                        break;
                    /*
                    case "Pot Luck Card - 16.png": //player receives £10 from each player
                        gameLoop.currentPlayer.AddMoney(200); //TODO: functionality for getting £10 from each player
                        break;
                    */
                    /*
                    case "Pot Luck Card - 17.png": //Get out of jail
                        gameLoop.currentPlayer.AddMoney(200); //TODO: get out of jail functionality
                        break;
                    */
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
