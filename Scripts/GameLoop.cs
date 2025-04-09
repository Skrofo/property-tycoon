using Godot;
using PropertyTycoon.Scripts;
using System;
using System.Collections.Generic;

public partial class GameLoop : Node
{
    public const int MaxPlayerCount = 6;//Maximum  number  of players including bots
    public const char moneySymbol = (char)163; //If you type the pound symbol in vs it can crash godot whenever it tries to load the file so just use this instead
    public const int StartingMoney = 1500; //How much money each player starts with

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


        var playerData = GetNode<GameData>("/root/GameData");
        if (playerData != null)
        {
            GD.Print($"Player data found for {playerData.SelectedPlayers.Length} players");
     
        }
        else 
        { 
            GD.PrintErr("Can't find player data");
            
        }

        if (playerData.SelectedPlayers.Length > 1)
        {
            StartGame(playerData.SelectedPlayers);
        }

        else
        {
            TestGame(humanPlayers, aiPlayers);//First num is human players, second is ai players (Ai currently not implemented)
        }
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
            NextTurn(true);
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
                    NextTurn();
                    //This should be empty as nothing happpens specifically on go, only when you pass it
                    break;
                case Marker.SpaceType.Property:
                    NextTurn();
                    break;
                case Marker.SpaceType.OpportunityKnocks:
                    GetNode<CardDisplay>("UI/CardDisplay").Show(Card.CardType.OpportunityKnocks);
                    break;
                case Marker.SpaceType.PotLuck:
                    GetNode<CardDisplay>("UI/CardDisplay").Show(Card.CardType.PotLuck);
                    break;
                case Marker.SpaceType.FreeParking:
                    ClaimParkingMoney(currentPlayer);
                    NextTurn();
                    break;
                case Marker.SpaceType.VisitingJail:
                    NextTurn();
                    //This should be empty as "Just visiting" space does nothing by itself
                    break;
                case Marker.SpaceType.Jail:
                    if (currentPlayer.hasGetOutOfJail)
                    {
                        GetNode<GetOutOfJailFreeDisplay>("UI/GetOutOfJailFree").ShowMenu(currentPlayer);
                    }
                    else
                    {
                        GetNode<JailBail>("UI/JailBail").ShowMenu(currentPlayer);
                    }
                    break;
                case Marker.SpaceType.GoToJail:
                    currentPlayer.GoToJail();
                    break;
                default:
                    break;
            }
            //NextTurn();
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

                //Printing results  to UI
                GD.Print($"Dice Faces: {result1}, {result2}");
                GetNode<TextureRect>("UI/DiceFace1").Texture = _diceFaces[result1 - 1];
                GetNode<TextureRect>("UI/DiceFace2").Texture = _diceFaces[result2 - 1];

                //Saving result
                diceResult[0] = result1;
                diceResult[1] = result2;
                diceRolled = true;
            }
        }

    public void StartGame(PlayerSelection.PlayerData[] playerList)
	{
        //Vector2 textureSize = Texture.GetSize();
        //Scale = TargetSize / textureSize;
        players = new Player[playerList.Length];

		//Creating player pieces
		PackedScene player = GD.Load<PackedScene>("res://Objects/Player.tscn");
        List<Texture2D> textures = PlayerSlot.GetTextures();

        for (int i = 0; i < players.Length; i++)
        {
			//Creating player piece object
			var instance = (Node2D)player.Instantiate();
			AddChild(instance);
			instance.Name += i + 1;//Making match the naming convention: player1, player2...
            GD.Print(instance.Name);

			instance.GetChild<Sprite2D>(0).Texture = textures[playerList[i].AvatarIndex];
            bool cpu = false;
            if (playerList[i].PlayerType == "AI") cpu = true;

            players[i] = new Player(instance, cpu);

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
        PlayerSelection.PlayerData[] strtPlayers = new PlayerSelection.PlayerData[humans + bots];
        for (int i = 0; i < humans; i++)
        {
            strtPlayers[i] = new PlayerSelection.PlayerData(i, "Human");
        }
        for (int i = humans; i < humans + bots; i++)
        {
            strtPlayers[i] = new PlayerSelection.PlayerData(i, "Human");
        }
        StartGame(strtPlayers);
    }

    //Changes whos turn it is to the next player
    public void NextTurn(bool forceNext = false)
    {
        if (diceResult[0] != diceResult[1] || forceNext)
        {
            _doublesCount = 0;
            if (currentPlayerIndex == players.Length - 1)
            {
                SetTurn(0);
            }
            else
            {
                SetTurn(currentPlayerIndex + 1);
            }
        }
        else
        {
            _doublesCount++;
            if (_doublesCount == 3)
            {
                currentPlayer.GoToJail();
            }
        }
    }

    //changes which player's turn it is to a specified value
    private void SetTurn(int turn)
    {
        currentPlayerIndex = turn;
        currentPlayer = players[currentPlayerIndex];
        //changing the current player
        GetNode<Label>("UI/CurrentPlayer").Text = ($"Player {currentPlayerIndex + 1}'s turn");
        GetNode<Label>("UI/CurrentPlayerProp").Text = ($"Player {currentPlayerIndex + 1}'s \nproperties");
        //changing money display
        GetNode<Label>("UI/Money").Text = moneySymbol + currentPlayer.GetMoney().ToString();

        //changing player avatar
        GetNode<TextureRect>("UI/Avatar").Texture = currentPlayer.node.GetChild<Sprite2D>(0).Texture;

        //TODO: add players owned properties

        GetNode<Label>("UI/Properties").Text = ("");
    }

    //Moves the given player towards the given location. Need to be called every frame
    private void Move(double delta, Player player, Vector2 pos)
    {
        player.node.GlobalPosition = player.node.GlobalPosition.MoveToward(pos, (float)(delta * playerMoveSpeed));
    }

}
