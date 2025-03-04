using Godot;
using PropertyTycoon.Scripts;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

public partial class GameLoop : Node
{
	public const int MaxPlayerCount = 6;//Maximum  number  of players including bots
    public const char moneySymbol = (char)163; //If you type the pound symbol in vs it can crash godot whenever it tries to load the file so just use this instead

    private const string playerPieceFileType = "svg"; //Filename extension for the filetype of the player icons without the: "."

    [Export] public int playerMoveSpeed;//How fast the player's piece moves to it's next space.
    [Export] public int playerRotateSpeed;//How fast the player's piece rotates to align with it's side of the board

    public struct TmpPlayer //Temp struct until David has done his
    {
		public TmpPlayer(bool isCpu, string pieceName)
		{
			cpu = isCpu;
			piece = pieceName;
		}
		public bool cpu;
		public string piece;
    }

	public Player[] players;
    public Dictionary<Player, Vector2> movingPlayers;//Players currently being moved and to where
    public int currentPlayer; //Num of the player who's turn it is currently
	public bool diceRolled; //Whether the diehave been rolled since the last frame
	public int[] diceResult; //The result of both die
    private int parkingMoney; //How much money is on free parking
    public bool justMoved; //Tells the game whether the current player has triggered the event relevent for their current space after rolling for it

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		//Setting startup variable values
		diceRolled = false;
		diceResult = new int[2];
        movingPlayers = new Dictionary<Player, Vector2>();
        justMoved = false;
        parkingMoney = 0;

        TestGame(1, 0);//First num is human players, second is ai players (Ai currently not implemented)
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //Moving any players that need to be moved
        foreach (var movingPlayer in movingPlayers)
        {
            Move(delta, movingPlayer.Key, movingPlayer.Value);
            if (movingPlayer.Key.node.GlobalPosition == movingPlayer.Value)
            {
                movingPlayers.Remove(movingPlayer.Key);
            }
        }

        //Waiting for player to roll dice
        if (diceRolled)
        {
            //Moving the player
			diceRolled = false;
			int total = diceResult[0] + diceResult[1];
            Marker currentLocation = players[currentPlayer].location;
            justMoved = true;
            players[currentPlayer].MoveForward(total);

            //Checking is player passed go
            if (players[currentPlayer].passedGo) players[currentPlayer].AddMoney(200);

            NextTurn();
        }

        //Doing whatever action is associated with the current space
        if (justMoved && movingPlayers.Count == 0)
        {
            justMoved = false;
            switch (players[currentPlayer].location.type)
            {
                case Marker.SpaceType.Go:
                    break;
                case Marker.SpaceType.Property:
                    break;
                case Marker.SpaceType.PotLuck:
                    break;
                case Marker.SpaceType.OpportunityKnocks:
                    break;
                case Marker.SpaceType.FreeParking:
                    break;
                case Marker.SpaceType.VisitingJail:
                    break;
                case Marker.SpaceType.Jail:
                    GetNode<Control>("UI/JailBail").Visible = true;
                    break;
                case Marker.SpaceType.GoToJail:
                    players[currentPlayer].GoToJail();
                    break;
                default:
                    break;
            }
        }
    }

    //Runs when the roll die button is pressed
    public void RollDiceButtonPressed()
    {
        if (players[currentPlayer].cpu == false) //Roll die button only works if current player is human
        {
            RollDie();
        }
    }

    //TODO:Make the actual dice roll. This is temperory
    public  void RollDie()
	{
        if (movingPlayers.Count == 0)//Making sure all player pieces are done moving before queing up more
        {
            //Generatin random numbers
            Random rnd = new Random();
            int result1 = rnd.Next(1, 7);
            int result2 = rnd.Next(1, 7);

            //Printing results  to UI
            GD.Print("Dice Result: " + result1 + ", " + result2);
            GetNode<Label>("UI/TMPDiceResult1").Text = "" + result1;
            GetNode<Label>("UI/TMPDiceResult2").Text = "" + result2;

            //Saving result
            diceResult[0] = result1;
            diceResult[1] = result2;
            diceRolled = true;
        }
		
	}

    public void StartGame(TmpPlayer[] playerList)
	{
		players = new Player[playerList.Length];

		//Creating player pieces
		PackedScene player = GD.Load<PackedScene>("res://Objects/Player.tscn");
        for (int i = 0; i < players.Length; i++)
        {
			//Creating player piece object
			var instance = (Node2D)player.Instantiate();
			AddChild(instance);
			instance.Name += i + 1;//Making match the naming convention: player1, player2...

			Texture2D texture = GD.Load<Texture2D>("res://Assets/PlayerIcons/" + playerList[i].piece + '.' + playerPieceFileType);
			instance.GetChild<Sprite2D>(0).Texture = texture;

			players[i] = new Player(instance, playerList[i].cpu);

			//Moving player piece to start pos
			var go = GetNode<Marker>("Board/Places/Go");
			go.MoveTo(players[i], true);
            players[i].location = go;
        }

        SetTurn(0);
    }

    public int GetParkingMoney() => parkingMoney;
    public void AddParkingMoney(int amount)
    {
        parkingMoney += amount;
    }

    //Used to test the game by generating a predefined number of human and ai players
    public void TestGame(int humans, int bots)
    {
        TmpPlayer[] strtPlayers = new TmpPlayer[humans + bots];
        for (int i = 0; i < humans; i++)
        {
            strtPlayers[i] = new TmpPlayer(false, "Player" + (i + 1));
        }
        for (int i = humans; i < humans + bots; i++)
        {
            strtPlayers[i] = new TmpPlayer(true, "Player" + (i + 1));
        }
        StartGame(strtPlayers);
    }

    //Changes whos turn it is to the next player
    private void NextTurn()
    {
        //updating current player value
        if (currentPlayer == players.Length - 1)
        {
            SetTurn(0);
        }
        else
        {
            SetTurn(currentPlayer + 1);
        }
    }

    //changes turn to specified value
    private void SetTurn(int turn)
    {
        currentPlayer = turn;
        //changing money display
        GetNode<Label>("UI/Money").Text = moneySymbol + players[currentPlayer].GetMoney().ToString();
    }

    //Moves the given player towards the given location. Need to be called every frame
    private void Move(double delta, Player player, Vector2 pos)
    {
        player.node.GlobalPosition = player.node.GlobalPosition.MoveToward(pos, (float)(delta * playerMoveSpeed));
    }
}
