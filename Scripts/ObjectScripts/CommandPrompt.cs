using Godot;
using PropertyTycoon.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public partial class CommandPrompt : LineEdit
{
    private GameLoop gameLoop;
    private LineEdit cmdLine;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNode<LineEdit>(".").Visible = false;
        gameLoop = FindParent("Game").GetNode<GameLoop>(".");
        cmdLine = GetNode<LineEdit>(".");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //Opens the command prompt on keypress
        if (Input.IsActionJustPressed("OpenCommandPrompt"))
        {
            cmdLine.Visible = !cmdLine.Visible;
            if (cmdLine.Visible)
            {
                cmdLine.GrabFocus();
                cmdLine.Clear();
            }
        }
    }

    //Called when a command is entered
    public void CommandEntered(string command)
    {
        string[] words = OrganiseCommand(command);

        bool correctCommand = true;
        switch (words[0])
        {
            case "roll"://Rolls specified number on die
                if (words.Length == 3)//If two numbers are entered both rolled number are set
                {
                    Roll(words[1].ToInt(), words[2].ToInt());
                }
                else//if only 1 number is entered only the first rolled number is set
                {
                    Roll(words[1].ToInt());
                }
                break;
            case "money"://Adds specified number to money
                gameLoop.currentPlayer.AddMoney(words[1].ToInt());
                break;
            case "parkingmoney"://Add specified number to free parking money
                gameLoop.AddParkingMoney(words[1].ToInt());
                break;
            case "card"://Shows a card, if the second word is a card type it shows a random card of that type. If the second word is a card name it shows that card.(if the card name has spaces enclose it in speech marks)
                if (words[1] == "opportunity")
                {
                    gameLoop.GetNode<CardDisplay>("UI/CardDisplay").Show(Card.CardType.OpportunityKnocks);
                }
                else if (words[1] == "potluck")
                {
                    gameLoop.GetNode<CardDisplay>("UI/CardDisplay").Show(Card.CardType.PotLuck);
                }
                else//if the command is specifying which card to show
                {
                    gameLoop.GetNode<CardDisplay>("UI/CardDisplay").Show(words[1]);
                }
                break;
            default:
                correctCommand = false;
                break;
        }
        if (correctCommand)// If a correct command is entered the command line is cleared
        {
            cmdLine.Clear();
        }
    }

    private static string[] OrganiseCommand(string command)//organises the given command into word or text in speech marks
    {
        string pattern = @"[^\s""]+|""([^""]*)""";  // Match words OR text in speech marks

        var matches = Regex.Matches(command, pattern);
        string[] words = new string[matches.Count];

        for (int i = 0; i < matches.Count; i++)
        {
            words[i] = matches[i].Groups[1].Success ? matches[i].Groups[1].Value : matches[i].Value;
        }

        return words;
    }

    private void Roll(int num1, int num2 = 0)
    {
        gameLoop.diceResult[0] = num1;
        gameLoop.diceResult[1] = num2;
        gameLoop.diceRolled = true;
    }
}
