using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OctoPawn.Components
{
    public class BoardState
    {
        public float Weight { get => Outcomes.Item1 / (Outcomes.Item2 == 0 ? 1 : Outcomes.Item2); }
        public float GetWeight()
        {
            return (float)Outcomes.Item1 / (float)Outcomes.Item2;
        }
        public Tuple<int, int> Outcomes { get; set; }
        public List<List<int>> Board { get; set; }

        public BoardState(Tuple<int,int> outcomes, List<List<int>> board)
        {
            Outcomes = outcomes;
            Board = board;
        }
    }

    public class CPU
    {
        public List<List<int>> CopyTable(List<List<int>> input)
        {
            return input;
        }
        public bool IsEqual(List<List<int>> input, List<List<int>> input2)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(input[i][j] != input2[i][j])
                        return false;
                }
            }
            return true;
        }
        public Tuple<bool,List<List<int>>> LookUpTable(List<List<int>> input)
        {
            Random Random = new Random(Guid.NewGuid().GetHashCode());
            var output = CopyTable(input);
            var check = new List<List<int>>()
                {
                    new List<int>() { 2, 2, 2, 2 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 1, 1, 1, 1 }
                };
            if (IsEqual(input,check))
            {
                var x = Random.Next(0, 3);
                switch (x)
                {
                    case 0:
                    output = new List<List<int>>()
                    {
                        new List<int>() { 0, 2, 2, 2 },
                        new List<int>() { 2, 0, 0, 0 },
                        new List<int>() { 0, 0, 0, 0 },
                        new List<int>() { 1, 1, 1, 1 }
                    };
                    return new Tuple<bool, List<List<int>>>(true, output);
                    case 1:
                    output = new List<List<int>>()
                    {
                        new List<int>() { 2, 0, 2, 2 },
                        new List<int>() { 0, 2, 0, 0 },
                        new List<int>() { 0, 0, 0, 0 },
                        new List<int>() { 1, 1, 1, 1 }
                    };
                    return new Tuple<bool, List<List<int>>>(true, output);
                    case 2:
                    output = new List<List<int>>()
                    {
                        new List<int>() { 2, 2, 0, 2 },
                        new List<int>() { 0, 0, 2, 0 },
                        new List<int>() { 0, 0, 0, 0 },
                        new List<int>() { 1, 1, 1, 1 }
                    };
                    return new Tuple<bool, List<List<int>>>(true, output);
                    case 3:
                    output = new List<List<int>>()
                    {
                        new List<int>() { 2, 2, 2, 0 },
                        new List<int>() { 0, 0, 0, 2 },
                        new List<int>() { 0, 0, 0, 0 },
                        new List<int>() { 1, 1, 1, 1 }
                    };
                    return new Tuple<bool, List<List<int>>>(true, output);
                }
               
            }
            else
            {
                check = new List<List<int>>()
                {
                    new List<int>() { 2, 2, 2, 2 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 1, 1, 1, 1 }
                };
            }

            if (IsEqual(input, check))
            {
                output = new List<List<int>>()
                {
                    new List<int>() { 2, 2, 2, 2 },
                    new List<int>() { 0, 0, 2, 0 },
                    new List<int>() { 1, 0, 0, 0 },
                    new List<int>() { 0, 1, 1, 1 }
                };
                return new Tuple<bool, List<List<int>>>(true, output);
            }
            else
            {
                check = new List<List<int>>()
                {
                    new List<int>() { 2, 2, 2, 2 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 1, 1, 1, 1 }
                };
            }

            if (IsEqual(input, check))
            {
                output = new List<List<int>>()
                {
                    new List<int>() { 2, 2, 2, 2 },
                    new List<int>() { 0, 0, 2, 0 },
                    new List<int>() { 0, 1, 0, 0 },
                    new List<int>() { 1, 0, 1, 1 }
                };
                return new Tuple<bool, List<List<int>>>(true, output);
            }
            else
            {
                check = new List<List<int>>()
                {
                    new List<int>() { 2, 2, 2, 2 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 1, 1, 1, 1 }
                };
            }

            if (IsEqual(input, check))
            {
                output = new List<List<int>>()
                {
                    new List<int>() { 2, 0, 2, 2 },
                    new List<int>() { 0, 2, 0, 0 },
                    new List<int>() { 0, 0, 1, 0 },
                    new List<int>() { 1, 1, 0, 1 }
                };
                return new Tuple<bool, List<List<int>>>(true, output);
            }
            else
            {
                check = new List<List<int>>()
                {
                    new List<int>() { 2, 2, 2, 2 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 0, 0, 0, 0 },
                    new List<int>() { 1, 1, 1, 1 }
                };
            }

            if (IsEqual(input, check))
            {
                output = new List<List<int>>()
                {
                    new List<int>() { 2, 0, 2, 2 },
                    new List<int>() { 0, 2, 0, 0 },
                    new List<int>() { 0, 0, 0, 1 },
                    new List<int>() { 1, 1, 1, 0 }
                };
                return new Tuple<bool, List<List<int>>>(true, output);
            }

            return new Tuple<bool, List<List<int>>>(false, output);
        }

        public List<List<int>> BlankBoard()
        {
            return new List<List<int>>()
            {
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0, 0 }
            };
        }
        public List<List<int>> DetermineBestMove(List<List<int>> currentBoard)
        {
            var checkBeforeCalulate = LookUpTable(currentBoard);
            if (checkBeforeCalulate.Item1)
                return checkBeforeCalulate.Item2;

            List<BoardState> allBoardCombos = new List<BoardState>();

            //Find all possible current moves
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(currentBoard[i][j] == 2)
                    {
                        var copy = BlankBoard();
                        for(int k = 0; k < 4; k++)
                        {
                            for(int l = 0; l < 4; l++)
                            {
                                copy[k][l] = currentBoard[k][l];
                            }
                        }
                        //can piece move straight forward
                        try
                        {
                            if (currentBoard[i + 1][j] == 0)
                            {
                                var straightMove = copy;
                                straightMove[i][j] = 0;
                                straightMove[i + 1][j] = 2;
                                allBoardCombos.Add(new BoardState(new Tuple<int, int>(0,0), straightMove));
                            }
                        }
                        catch { /*continue on*/ }

                        var copy2 = BlankBoard();
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                copy2[k][l] = currentBoard[k][l];
                            }
                        }
                        //can piece capture to left side
                        try
                        {
                            if (currentBoard[i + 1][j - 1] == 1)
                            {
                                var leftMove = copy2;
                                leftMove[i][j] = 0;
                                leftMove[i + 1][j - 1] = 2;
                                allBoardCombos.Add(new BoardState(new Tuple<int, int>(0, 0), leftMove));
                            }
                        }
                        catch { /*continue on*/ }

                        var copy3 = BlankBoard();
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                copy3[k][l] = currentBoard[k][l];
                            }
                        }
                        //can piece capture to right side
                        try
                        {
                            if (currentBoard[i + 1][j + 1] == 1)
                            {
                                var rightMove = copy3;
                                rightMove[i][j] = 0;
                                rightMove[i + 1][j + 1] = 2;
                                allBoardCombos.Add(new BoardState(new Tuple<int, int>(0, 0), rightMove));
                            }
                        }
                        catch { /*continue on*/ }
                    }
                }
            }

            //If no moves can legally be made, then lost occurs
            if (allBoardCombos.Count == 0)
                return currentBoard;

            //Check if any possible moves cause a win
            foreach(var board in allBoardCombos)
            {
                //Check if moved piece to other side for win
                for(int i = 0; i < 4; i++)
                {
                    if (board.Board[3][i] == 2)
                        return board.Board;
                }

                //If close to victory, go for it!
                for (int i = 0; i < 4; i++)
                {
                    if (board.Board[1].Contains(1))
                        break;
                    var x = 0;
                    try
                    {
                        if (board.Board[2][i] == 2)
                            x++;
                    }
                    catch 
                    {
                        x++;
                    }
                    try
                    {
                        if (board.Board[3][i - 1] == 0)
                            x++;
                    }
                    catch
                    {
                        x++;
                    }
                    try
                    {
                        if (board.Board[3][i + 1] == 0)
                            x++;
                    }
                    catch
                    {
                        x++;
                    }
                    if(x == 3)
                        return board.Board;
                }

                var cantMove = true;
                //Check if opponent cannot make any moves
                for (int i = 0; i < 4; i++)
                {
                    for(var j = 0; j < 4; j++)
                    {
                        if(board.Board[i][j] == 1)
                        {
                            try
                            {
                                if (board.Board[i - 1][j] == 0)
                                    cantMove = false;
                            }
                            catch { /*continue on*/ }

                            try
                            {
                                if (board.Board[i - 1][j + 1] == 2)
                                    cantMove = false;
                            }
                            catch { /*continue on*/ }

                            try
                            {
                                if (board.Board[i - 1][j - 1] == 2)
                                    cantMove = false;
                            }
                            catch { /*continue on*/ }
                        }
                    }
                }
                if(cantMove)
                    return board.Board;
            }

            //Check if any possible moves cause a loss and discard them
            foreach(var board in allBoardCombos)
            {
                //Check if moved piece to other side for win
                for (int i = 0; i < 4; i++)
                {
                    if (board.Board[1][i] == 1 && board.Board[1][i] == 0)
                    {
                        allBoardCombos.Remove(board);
                        break;
                    }
                }
            }

            foreach(var board in allBoardCombos)
            {
                board.Outcomes = PermutateBoards(board.Board, true);
            }

            var bestBoardOutcome = allBoardCombos.OrderByDescending(x => x.GetWeight()).ToList();
            string stuff = "";
            foreach(var board in allBoardCombos)
            {
                float x = board.Outcomes.Item1 / board.Outcomes.Item2;
                stuff += allBoardCombos.IndexOf(board).ToString() + "\n";
                stuff += board.GetWeight().ToString() + "\n";
                stuff += board.Outcomes.Item1.ToString() + "\n";
                stuff += board.Outcomes.Item2.ToString() + "\n";
                for (int i = 0; i < 4; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        stuff += board.Board[i][j].ToString() + ",";
                    }
                    stuff += "\n";
                }

                stuff += "\n\n";
            }
            System.IO.File.WriteAllText(@"C:\Users\Matthew\source\repos\OctoPawn\OctoPawn\bin\Debug\netcoreapp3.1\state1.txt", stuff);
            return bestBoardOutcome[0].Board;
        }

        public Tuple<int,int> PermutateBoards(List<List<int>> currentBoard, bool isOpponentsTurn)
        {
            var total = 0;
            var wins = 0;
            var forwardMovement = isOpponentsTurn ? -1 : 1;
            var enemyPawnValue = isOpponentsTurn ? 2 : 1;
            var allyPawnValue = isOpponentsTurn ? 1 : 2;
            var canCurrentPlayerCanMove = false;

            //check if winning/losing position
            for(int i = 0; i < 4; i++)
            {
                if (isOpponentsTurn && currentBoard[0][i] == 1)
                {
                    return new Tuple<int,int>(0, 1);
                }
                else if(!isOpponentsTurn && currentBoard[3][i] == 2)
                {
                    return new Tuple<int, int>(1, 1);
                }
            }

            //Find all possible current moves
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (currentBoard[i][j] == allyPawnValue)
                    {
                        var copy = BlankBoard();
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                copy[k][l] = currentBoard[k][l];
                            }
                        }
                        //can piece move straight forward
                        try
                        {
                            if (currentBoard[i + forwardMovement][j] == 0)
                            {
                                var straightMove = copy;
                                straightMove[i][j] = 0;
                                straightMove[i + forwardMovement][j] = allyPawnValue;
                                canCurrentPlayerCanMove = true;
                                var permutation = PermutateBoards(straightMove, !isOpponentsTurn);
                                wins += permutation.Item1;
                                total += permutation.Item2;
                            }
                        }
                        catch { /*continue on*/ }

                        var copy2 = BlankBoard();
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                copy2[k][l] = currentBoard[k][l];
                            }
                        }
                        //can piece capture to left side
                        try
                        {
                            if (currentBoard[i + forwardMovement][j - 1] == enemyPawnValue)
                            {
                                var leftMove = copy2;
                                leftMove[i][j] = 0;
                                leftMove[i + forwardMovement][j - 1] = allyPawnValue;
                                canCurrentPlayerCanMove = true;
                                var permutation = PermutateBoards(leftMove, !isOpponentsTurn);
                                wins += permutation.Item1;
                                total += permutation.Item2;
                            }
                        }
                        catch { /*continue on*/ }

                        var copy3 = BlankBoard();
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                copy3[k][l] = currentBoard[k][l];
                            }
                        }
                        //can piece capture to right side
                        try
                        {
                            if (currentBoard[i + forwardMovement][j + 1] == enemyPawnValue)
                            {
                                var rightMove = copy3;
                                rightMove[i][j] = 0;
                                rightMove[i + forwardMovement][j + 1] = allyPawnValue;
                                canCurrentPlayerCanMove = true;
                                var permutation = PermutateBoards(rightMove, !isOpponentsTurn);
                                wins += permutation.Item1;
                                total += permutation.Item2;
                            }
                        }
                        catch { /*continue on*/ }
                    }
                }
            }

            if (!canCurrentPlayerCanMove)
            {
                if (isOpponentsTurn)
                    return new Tuple<int, int>(1, 1);
                else
                    return new Tuple<int, int>(0, 1);
            }

            return new Tuple<int, int>(wins, total);
        }

        public List<List<int>> MakeMove(List<List<int>> board)
        {
            var updateBoard = DetermineBestMove(board);

            //TODO: Play move sound

            return updateBoard;
        }
    }
}
