using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OctoPawn.Components
{
    public class BoardState
    {
        public float Weight { get => Outcomes.Item1 / (Outcomes.Item2 == 0 ? 1 : Outcomes.Item2); }
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
        public List<List<int>> DetermineBestMove(List<List<int>> currentBoard)
        {
            List<BoardState> allBoardCombos = new List<BoardState>();

            //Find all possible current moves
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(currentBoard[i][j] == 2)
                    {
                        //can piece move straight forward
                        try
                        {
                            if (currentBoard[i + 1][j] == 0)
                            {
                                var straightMove = new List<List<int>>(currentBoard);
                                straightMove[i][j] = 0;
                                straightMove[i + 1][j] = 2;
                                allBoardCombos.Add(new BoardState(new Tuple<int, int>(0,0), straightMove));
                            }
                        }
                        catch { /*continue on*/ }

                        //can piece capture to left side
                        try
                        {
                            if (currentBoard[i + 1][j - 1] == 1)
                            {
                                var leftMove = new List<List<int>>(currentBoard);
                                leftMove[i][j] = 0;
                                leftMove[i + 1][j - 1] = 2;
                                allBoardCombos.Add(new BoardState(new Tuple<int, int>(0, 0), leftMove));
                            }
                        }
                        catch { /*continue on*/ }

                        //can piece capture to right side
                        try
                        {
                            if (currentBoard[i + 1][j + 1] == 1)
                            {
                                var rightMove = new List<List<int>>(currentBoard);
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

            var bestBoardOutcome = allBoardCombos.OrderByDescending(x => x.Weight).ToList();
            return bestBoardOutcome[0].Board;
        }

        public Tuple<int,int> PermutateBoards(List<List<int>> currentBoard, bool isOpponentsTurn)
        {
            var total = 0;
            var wins = 0;
            var forwardMovement = isOpponentsTurn ? -1 : 1;
            var enemyPawnValue = isOpponentsTurn ? 2 : 1;
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
                    if (currentBoard[i][j] == 2)
                    {
                        //can piece move straight forward
                        try
                        {
                            if (currentBoard[i + forwardMovement][j] == 0)
                            {
                                var straightMove = new List<List<int>>(currentBoard);
                                straightMove[i][j] = 0;
                                straightMove[i + 1][j] = 2;
                                canCurrentPlayerCanMove = true;
                                wins += PermutateBoards(straightMove, !isOpponentsTurn).Item1;
                                total += PermutateBoards(straightMove, !isOpponentsTurn).Item2;
                            }
                        }
                        catch { /*continue on*/ }

                        //can piece capture to left side
                        try
                        {
                            if (currentBoard[i + forwardMovement][j - 1] == enemyPawnValue)
                            {
                                var leftMove = new List<List<int>>(currentBoard);
                                leftMove[i][j] = 0;
                                leftMove[i + 1][j - 1] = 2;
                                canCurrentPlayerCanMove = true;
                                wins += PermutateBoards(leftMove, !isOpponentsTurn).Item1;
                                total += PermutateBoards(leftMove, !isOpponentsTurn).Item2;
                            }
                        }
                        catch { /*continue on*/ }

                        //can piece capture to right side
                        try
                        {
                            if (currentBoard[i + forwardMovement][j + 1] == enemyPawnValue)
                            {
                                var rightMove = new List<List<int>>(currentBoard);
                                rightMove[i][j] = 0;
                                rightMove[i + 1][j + 1] = 2;
                                canCurrentPlayerCanMove = true;
                                wins += PermutateBoards(rightMove, !isOpponentsTurn).Item1;
                                total += PermutateBoards(rightMove, !isOpponentsTurn).Item2;
                            }
                        }
                        catch { /*continue on*/ }
                    }
                }
            }

            if (!canCurrentPlayerCanMove)
            {
                if (isOpponentsTurn)
                    return new Tuple<int, int>(0, 1);
                else
                    return new Tuple<int, int>(1, 1);
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
