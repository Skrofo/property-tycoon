using Godot;
using System;
using static Godot.HttpRequest;

public partial class CommandLine : LineEdit
{
    private GameLoop gameLoop;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNode<LineEdit>(".").Visible = false;
        gameLoop = FindParent("Game").GetNode<GameLoop>(".");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //Opens the command prompt on keypress
        if (Input.IsActionJustPressed("OpenCommandPrompt"))
        {
            GetNode<LineEdit>(".").Visible = !GetNode<LineEdit>(".").Visible;
            if (GetNode<LineEdit>(".").Visible) GetNode<LineEdit>(".").GrabFocus();
        }
    }

    //Called when a command is entered
    public void CommandEntered(string command)
    {
        string[] words = command.ToLower().Split(' ');

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
            default:
                break;
        }
    }

    private void Roll(int num1, int num2 = 0)
    {
        gameLoop.diceResult[0] = num1;
        gameLoop.diceResult[1] = num2;
        gameLoop.diceRolled = true;
    }
}
