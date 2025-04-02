using Godot;
using PropertyTycoon.Scripts;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class GameLoop : Node
{
    public const int MaxPlayerCount = 6;//Maximum  number  of players including bots
    public const char moneySymbol = (char)163; //If you type the pound symbol in vs it can crash godot whenever it tries to load the file so just use this instead
    public const int StartingMoney = 1500; //How much money each player starts with

    private const string playerPieceFileType = "svg"; //Filename extension for the filetype of the player icons without the: "."

    [Export] public int playerMoveSpeed;//How fast the player's piece moves to it's next space.
    [Export] public int playerRotateSpeed;//How fast the player's piece rotates to align with it's side of the board
    [Export] public int humanPlayers = 1;//When running a test game, how many human players are there?
    [Export] public int aiPlayers = 0;//When running a test game, how many ai players are there?

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
    public Player currentPlayer; //The player who's turn it is
    public bool diceRolled; //Whether the diehave been rolled since the last frame
    public int[] diceResult; //The result of both die
    private int _doublesCount = 0;
    private int parkingMoney; //How much money is on free parking
    public bool justMoved; //Tells the game whether the current player has triggered the event relevent for their current space after rolling for it
    public int currentPlayerIndex; //Num of the player who's turn it is currently

    private int parkingMoney; //How much money is on free parking


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Setting startup variable values
        diceRolled = false;
        diceResult = new int[2];
        movingPlayers = new Dictionary<Player, Vector2>();
        justMoved = false;
        parkingMoney = 0;
        _LoadDiceTextures();

        TestGame(humanPlayers, aiPlayers);//First num is human players, second is ai players (Ai currently not implemented)
        var data = GetNode<GameData>("/root/GameData");
        if (data != null) GD.Print("Player data found");
        else GD.PrintErr("Can't find player data");
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

        //Reducing jail time if current player is in jail
        if (currentPlayer.jailTurns > 0)
        {
            currentPlayer.jailTurns--;
            NextTurn();
        }

        //Waiting for player to roll dice
        if (diceRolled)
        {
            //Moving the player
            diceRolled = false;
            int total = diceResult[0] + diceResult[1];
            Marker currentLocation = currentPlayer.location;
            justMoved = true;
            currentPlayer.MoveForward(total);

            //Checking is player passed go
            if (currentPlayer.passedGo)
            {
                currentPlayer.PassGo();
            }
        }

        //Doing whatever action is associated with the current space
        if (justMoved && movingPlayers.Count == 0)
        {
            justMoved = false;
            GD.Print($"Player {currentPlayerIndex} landed on a {currentPlayer.location.type} space");
            switch (currentPlayer.location.type)
            {
                case Marker.SpaceType.Go:
                    //This should be empty as nothing happpens specifically on go, only when you pass it
                    break;
                case Marker.SpaceType.Property:
                    break;
                case Marker.SpaceType.OpportunityKnocks:
                    GetNode<CardDisplay>("UI/CardDisplay").Show(Card.CardType.OpportunityKnocks);
                    break;
                case Marker.SpaceType.PotLuck:
                    GetNode<CardDisplay>("UI/CardDisplay").Show(Card.CardType.PotLuck);
                    break;
                case Marker.SpaceType.FreeParking:
                    ClaimParkingMoney(currentPlayer);
                    break;
                case Marker.SpaceType.VisitingJail:
                    //This should be empty as "Just visiting" space does nothing by itself
                    break;
                case Marker.SpaceType.Jail:
                    if (currentPlayer.hasGetOutOfJail)
                    {
                        GetNode<Control>("UI/GetOutOfJailFree").Visible = true;
                    }
                    else
                    {
                        GetNode<Control>("UI/JailBail").Visible = true;
                    }
                    break;
                case Marker.SpaceType.GoToJail:
                    currentPlayer.GoToJail();
                    break;
                default:
                    break;
            }
            if (diceResult[0] == diceResult[1])
            {
                _doublesCount++;
                Console.WriteLine($"Doubles! You've rolled {_doublesCount} doubles");

                if (_doublesCount >= 3)
                {
                    //go to jail
                    currentPlayer.GoToJail();
                    _doublesCount = 0;
                }
                else
                {
                    //TODO give current player another turn
                }

            }
            else
            {
                _doublesCount = 0;
            }
            NextTurn();//Once the action for the current space has been (mostly) completed the turn is changed
        }
    }

    //Runs when the roll die button is pressed
    public void RollDiceButtonPressed()
    {
        if (currentPlayer.cpu == false && currentPlayer.jailTurns == 0) //Roll die button only works if current player is human and not in jail
        {
            RollDie();
        }
    }
    private Texture2D[] _diceFaces = new Texture2D[6];

    //Load textures in _LoadDiceTextures()
    public void _LoadDiceTextures()
    {
        _diceFaces[0] = GD.Load<Texture2D>("res://Assets/Dice/Dice_Side_1.png");
        _diceFaces[1] = GD.Load<Texture2D>("res://Assets/Dice/Dice_Side_2.png");
        _diceFaces[2] = GD.Load<Texture2D>("res://Assets/Dice/Dice_Side_3.png");
        _diceFaces[3] = GD.Load<Texture2D>("res://Assets/Dice/Dice_Side_4.png");
        _diceFaces[4] = GD.Load<Texture2D>("res://Assets/Dice/Dice_Side_5.png");
        _diceFaces[5] = GD.Load<Texture2D>("res://Assets/Dice/Dice_Side_6.png");
    }


        //TODO:Make the actual dice roll. This is temperory
        public void RollDie()
        {
            if (movingPlayers.Count == 0)//Making sure all player pieces are done moving before queing up more
            {


                //Generatin random numbers
                Random rnd = new Random();
                int result1 = rnd.Next(1, 7);
                int result2 = rnd.Next(1, 7);

                GD.Print($"Dice Faces: {result1}, {result2}");
                GetNode<TextureRect>("UI/DiceFace1").Texture = _diceFaces[result1 - 1];
                GetNode<TextureRect>("UI/DiceFace2").Texture = _diceFaces[result2 - 1];

                //Printing results  to UI
                //GD.Print("Dice Result: " + result1 + ", " + result2);
                //GetNode<Label>("UI/TMPDiceResult1").Text = "" + result1;
                //GetNode<Label>("UI/TMPDiceResult2").Text = "" + result2;




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
            players[i].AddMoney(StartingMoney);
        }

        SetTurn(0);
    }

    public int GetParkingMoney() => parkingMoney;
    public void AddParkingMoney(int amount)
    {
        parkingMoney += amount;
    }
    public void ClaimParkingMoney(Player player)
    {
        //TODO: Add some kind of popup for claiming parking money
        player.AddMoney(parkingMoney);
        parkingMoney = 0;
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
        if (currentPlayerIndex == players.Length - 1)
        {
            SetTurn(0);
        }
        else
        {
            SetTurn(currentPlayerIndex + 1);
        }
    }

    //changes turn to specified value
    private void SetTurn(int turn)
    {
        currentPlayerIndex = turn;
        currentPlayer = players[currentPlayerIndex];
        //changing money display
        GetNode<Label>("UI/Money").Text = moneySymbol + currentPlayer.GetMoney().ToString();
    }

    //Moves the given player towards the given location. Need to be called every frame
    private void Move(double delta, Player player, Vector2 pos)
    {
        player.node.GlobalPosition = player.node.GlobalPosition.MoveToward(pos, (float)(delta * playerMoveSpeed));
    }
}
