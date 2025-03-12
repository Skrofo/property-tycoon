using Godot;
using System;

public partial class CommandLine : LineEdit
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
        string[] words = command.ToLower().Split(' ');

        bool correctCommand = true;
        switch (words[0])
        {
            case "roll"://Rolls specified number on die
                if (words.Length == 3)
                {
                    Roll(words[1].ToInt(), words[2].ToInt());
                }
                else
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
            default:
                correctCommand = false;
                break;
        }
        if (correctCommand)// If a correct command is entered the command line is cleared
        {
            cmdLine.Clear();
        }
    }

    private void Roll(int num1, int num2 = 0)
    {
        gameLoop.diceResult[0] = num1;
        gameLoop.diceResult[1] = num2;
        gameLoop.diceRolled = true;
    }
}
