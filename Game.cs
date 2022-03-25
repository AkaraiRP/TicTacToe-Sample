using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _03LMS1_Briones
{
    [Activity(Label = "Game")]
    public class Game : Activity
    {
        public Button TopLeft;
        public Button TopMiddle;
        public Button TopRight;
        public Button Left;
        public Button Middle;
        public Button Right;
        public Button BottomLeft;
        public Button BottomMiddle;
        public Button BottomRight;
        public TextView Turn;

        public bool IsPlayer;

        enum Difficulty {
            Easy,
            Medium,
            Hard
        }
        public int flip()
        {
            Random coinflip = new Random();
            int First = coinflip.Next(2);
            return First;
        }

        public int intDiff;

        public string[,] Board = new string[3, 3] {
            { "", "", "" },
            { "", "", "" },
            { "", "", "" }
        };

        public int[] BoardCoords(int value)
        {
            int[] coord = new int[2];

            switch (value) {
                case 1:
                    coord.SetValue(0, 0);
                    coord.SetValue(0, 1);
                    break;

                case 2:
                    coord.SetValue(0, 0);
                    coord.SetValue(1, 1);
                    break;

                case 3:
                    coord.SetValue(0, 0);
                    coord.SetValue(2, 1);
                    break;

                case 4:
                    coord.SetValue(1, 0);
                    coord.SetValue(0, 1);
                    break;

                case 5:
                    coord.SetValue(1, 0);
                    coord.SetValue(1, 1);
                    break;

                case 6:
                    coord.SetValue(1, 0);
                    coord.SetValue(2, 1);
                    break;

                case 7:
                    coord.SetValue(2, 0);
                    coord.SetValue(0, 1);
                    break;

                case 8:
                    coord.SetValue(2, 0);
                    coord.SetValue(1, 1);
                    break;

                case 9:
                    coord.SetValue(2, 0);
                    coord.SetValue(2, 1);
                    break;

            }

            return coord;
        }

        public Button ButtonCoords(int[] spot) {
            Button coords = TopLeft;
            if (spot[0] == 0 && spot[1] == 0) { coords = TopLeft; }
            if (spot[0] == 0 && spot[1] == 1) { coords = TopMiddle; }
            if (spot[0] == 0 && spot[1] == 2) { coords = TopRight; }
            if (spot[0] == 1 && spot[1] == 0) { coords = Left; }
            if (spot[0] == 1 && spot[1] == 1) { coords = Middle; }
            if (spot[0] == 1 && spot[1] == 2) { coords = Right; }
            if (spot[0] == 2 && spot[1] == 0) { coords = BottomLeft; }
            if (spot[0] == 2 && spot[1] == 1) { coords = BottomMiddle; }
            if (spot[0] == 2 && spot[1] == 2) { coords = BottomRight; }

            return coords;
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            string Diff = Intent.GetStringExtra("Difficulty");

            TopLeft = FindViewById<Button>(Resource.Id.TopLeft);
            TopMiddle = FindViewById<Button>(Resource.Id.TopMiddle);
            TopRight = FindViewById<Button>(Resource.Id.TopRight);
            Left = FindViewById<Button>(Resource.Id.Left);
            Middle = FindViewById<Button>(Resource.Id.Middle);
            Right = FindViewById<Button>(Resource.Id.Right);
            BottomLeft = FindViewById<Button>(Resource.Id.BottomLeft);
            BottomMiddle = FindViewById<Button>(Resource.Id.BottomMiddle);
            BottomRight = FindViewById<Button>(Resource.Id.BottomRight);
            Turn = FindViewById<TextView>(Resource.Id.Turn);

            TopLeft.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 1, TopLeft); };
            TopMiddle.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 2, TopMiddle); };
            TopRight.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 3, TopRight); };
            Left.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 4, Left); };
            Middle.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 5, Middle); };
            Right.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 6, Right); };
            BottomLeft.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 7, BottomLeft); };
            BottomMiddle.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 8, BottomMiddle); };
            BottomRight.Click += (sender, EventArgs) => { PickSpot(sender, EventArgs, 9, BottomRight); };



            switch (Diff) {
                case "easy":
                    intDiff = (int)Difficulty.Easy;
                    break;

                case "medium":
                    intDiff = (int)Difficulty.Medium;
                    break;

                case "hard":
                    intDiff = (int)Difficulty.Hard;
                    break;
            }


            TextView prompt = FindViewById<TextView>(Resource.Id.Turn);

            int coinflip = flip();

            if (coinflip == 1) {
                ChangeAnnouncer(0, Turn);
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Player goes first!");
                alert.SetMessage("Player is X");
                alert.SetButton("OK", (c, ev) =>
                {
                    IsPlayer = true;
                });
                alert.Show();
            }
            else {
                ChangeAnnouncer(1, Turn);
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("AI goes first!");
                alert.SetMessage("Player is X");
                alert.SetButton("OK", (c, ev) =>
                {
                    IsPlayer = false;
                    nextMove();
                });
                alert.Show();
            }

        }

        private void ChangeAnnouncer(int player, TextView announcer) {
            if (player == 1 && IsPlayer) {
                announcer.Text = "AI's Turn.";
            }

            else if (player == 0 && IsPlayer == false)
            {
                announcer.Text = "Player's Turn.";
            }
            else
            {
                //ToDO
            }
        }

        private void ChangeText(int player, Button spot, int[] innerSpot) {
            string playing;
            if (player == 1 && IsPlayer)
            {
                playing = "X";
                spot.Text = playing;
                WriteInner(innerSpot, playing);
                ChangeAnnouncer(player, Turn);
                if (CheckWinner() == "win") { IsWinner(CheckWinner()); }
                else if (CheckWinner() == "tie") { IsWinner(CheckWinner()); }

                if (IsPlayer)
                {
                    IsPlayer = false;
                }

                else
                {
                    IsPlayer = true;
                }
            }

            else if (player == 0 && IsPlayer == false) {
                playing = "O";
                spot.Text = playing;
                WriteInner(innerSpot, playing);
                ChangeAnnouncer(player, Turn);
                if (CheckWinner() == "lose") { IsWinner(CheckWinner()); }
                else if (CheckWinner() == "tie") { IsWinner(CheckWinner()); }


                if (IsPlayer)
                {
                    IsPlayer = false;
                }

                else
                {
                    IsPlayer = true;
                }
            }
            else
            {
                //ToDO
            }
        }

        private bool CheckSpot(int[] spot)
        {
            if (Board[spot[0], spot[1]].Equals("")) {
                return true;
            }

            else {
                return false;
            }

        }

        private void WriteInner(int[] Spot, string player)
        {
            Board[Spot[0], Spot[1]] = player;
        }

        private void IsWinner(string player)
        {
            if (player == "win")
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("No more moves!");
                alert.SetMessage("Player X is the winner!");
                alert.SetButton("OK", (c, ev) =>
                {
                    this.Finish();
                    Intent intent = new Intent(this, typeof(EndGame));
                    intent.PutExtra("Winner", player);
                    StartActivity(intent);
                });
                alert.Show();
            }

            else if (player == "lose")
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("No more moves!");
                alert.SetMessage("Player O is the winner!");
                alert.SetButton("OK", (c, ev) =>
                {
                    this.Finish();
                    Intent intent = new Intent(this, typeof(EndGame));
                    intent.PutExtra("Winner", player);
                    StartActivity(intent);
                });
                alert.Show();
            }

            else if (player == "tie")
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("No more moves!");
                alert.SetMessage("Its a Tie!");
                alert.SetButton("OK", (c, ev) =>
                {
                    this.Finish();
                    Intent intent = new Intent(this, typeof(EndGame));
                    intent.PutExtra("Winner", player);
                    StartActivity(intent);
                });
                alert.Show();
            }

            else { 
                //ToDO
            }
        }

        private string CheckWinner()
        {
            string player = "X";
            string ai = "O";

            string win = "win";
            string lose = "lose";
            string tie = "tie";


            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j].Equals(""))
                    {
                        count++;
                    }
                }
            }


            // check rows - Player
            if (Board[0, 0] == player && Board[0, 1] == player && Board[0, 2] == player) { return win; }
            if (Board[1, 0] == player && Board[1, 1] == player && Board[1, 2] == player) { return win; }
            if (Board[2, 0] == player && Board[2, 1] == player && Board[2, 2] == player) { return win; }

            // check columns - Player
            if (Board[0, 0] == player && Board[1, 0] == player && Board[2, 0] == player) { return win; }
            if (Board[0, 1] == player && Board[1, 1] == player && Board[2, 1] == player) { return win; }
            if (Board[0, 2] == player && Board[1, 2] == player && Board[2, 2] == player) { return win; }

            // check diags - Player
            if (Board[0, 0] == player && Board[1, 1] == player && Board[2, 2] == player) { return win; }
            if (Board[0, 2] == player && Board[1, 1] == player && Board[2, 0] == player) { return win; }

            // check rows
            if (Board[0, 0] == ai && Board[0, 1] == ai && Board[0, 2] == ai) { return lose; }
            if (Board[1, 0] == ai && Board[1, 1] == ai && Board[1, 2] == ai) { return lose; }
            if (Board[2, 0] == ai && Board[2, 1] == ai && Board[2, 2] == ai) { return lose; }

            // check columns
            if (Board[0, 0] == ai && Board[1, 0] == ai && Board[2, 0] == ai) { return lose; }
            if (Board[0, 1] == ai && Board[1, 1] == ai && Board[2, 1] == ai) { return lose; }
            if (Board[0, 2] == ai && Board[1, 2] == ai && Board[2, 2] == ai) { return lose; }

            // check diags
            if (Board[0, 0] == ai && Board[1, 1] == ai && Board[2, 2] == ai) { return lose; }
            if (Board[0, 2] == ai && Board[1, 1] == ai && Board[2, 0] == ai) { return lose; }

            else if (count == 0) { return tie; }
            else { return null; }
        }

       

        private async void PickSpot(object sender, EventArgs e, int value, Button button)
        {
            int[] Place = BoardCoords(value);
            if (CheckSpot(Place))
            {
                ChangeText(1, button, Place);
                Random delay = new Random();
                int sleep = delay.Next(800, 2500);
                await Task.Delay(sleep);
                nextMove();

            }

            else
            {
                //ToDO
            }
        }

        private void PickSpot_ai(Button button, int[] Place)
        {
            if (CheckSpot(Place))
            {
                ChangeText(0, button, Place);

            }

            else
            {
                //ToDO
            }
        }

        private void nextMove() {
            if (CheckWinner() == "lose") { IsWinner(CheckWinner()); }
            else if (CheckWinner() == "tie") { IsWinner(CheckWinner()); }
            else if (CheckWinner() == "win") { IsWinner(CheckWinner()); }

            int difficulty = intDiff + 1;
            if (difficulty == 2)
            {
                Random dice_roll = new Random();
                int chance = dice_roll.Next(0, 100);

                if (chance >= 40) { difficulty = 3; }
                else { difficulty = 1;  }
            }

            double BestScore = double.NegativeInfinity;
            int[] move = new int[2];


            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (Board[i, j] == "") {
                        Board[i, j] = "O";
                        double score = Prediction(Board, difficulty, false, BestScore);
                        Board[i, j] = "";
                        if (score > BestScore) {
                            BestScore = score;
                            move.SetValue(i, 0);
                            move.SetValue(j, 1);
                        }
                    }
                }
            }

            Button moveSpot = ButtonCoords(move);
            PickSpot_ai(moveSpot, move);

        }

        private double Prediction(string[,] board, int depth, bool inTurn, double currentScore) {
            string result = CheckWinner();
            double BestScore;

            if (depth == 0)
            {
                return currentScore;
            }

            if (result == "win")
            {
                return -10.0;
            }

            else if (result == "lose")
            {
                return 10.0;
            }

            else if (result == "tie")
            {
                return 0;
            }

            if (inTurn)
            {
                BestScore = double.NegativeInfinity;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[i, j] == "")
                        {
                            board[i, j] = "O";
                            double score = Prediction(board, depth - 1, false, BestScore);
                            board[i, j] = "";
                            BestScore = Math.Max(score, BestScore);
                        }
                    }
                }
                return BestScore;
            }
            else {
                BestScore = double.PositiveInfinity;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[i, j] == "")
                        {
                            board[i, j] = "X";
                            double score = Prediction(board, depth - 1, true, BestScore);
                            board[i, j] = "";
                            BestScore = Math.Min(score, BestScore);
                        }
                    }
                }
                return BestScore;
            }

            
        }


    }

}