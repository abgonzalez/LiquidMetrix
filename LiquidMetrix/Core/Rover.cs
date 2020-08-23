using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace LiquidMetrix
{
    public class Rover : IComparable<Rover>, IRover, IGrid
    {
        #region public property
        public int _X { get; set; }
        public int _Y { get; set; }
        public char _direction { get; set; } = 'N';
        public int _width { get => _grid._width; }
        public int _height { get => _grid._height; }
        #endregion

        #region private property
        private readonly ILogger<Rover> _logger;
        private readonly IGrid _grid;
        #endregion 
        #region Constructors
        public Rover(ILogger<Rover> logger,
                    IGrid grid)
        {
            _logger = logger;
            _grid = grid;
            _X = _Y = 0;
            _direction = 'N';
        }
        public Rover createNew()
        {
            return new Rover(_logger, _grid);
        }
         #endregion

        #region InitialValues
        public StatusCode SetPosition(int X = 0, int Y = 0, char direction = 'N')
        {
            if (X >= 0 && X <= _width)
            {
                _X = X;
            }
            else
            {
                _logger.LogWarning($"Set X position out of X borders.");
                return StatusCode.InvalidInput;
            }
            if (Y >= 0 && Y <= _height)
            {
                _Y = Y;
            }
            else
            {
                _logger.LogWarning($"Set X position out of Y borders.");
                return StatusCode.InvalidInput;
            }
            _direction = direction;
            return StatusCode.Successful;

        }


        #endregion

        #region Admin
        //TODO: Change the type of return of this function.
        public string GetCurrentPosition()
        {
            return this.ToString();
        }
        public override string ToString()
        {
            return $"[X: {_X}, Y: {_Y}, direction {_direction}]";
        }
        #endregion

        #region Move Functions
        /// <summary>
        /// Move: Function to move the rover according to inputOrders value.
        /// </summary>
        /// <param name="inputOrders"></param>
        /// <returns></returns>
        public StatusCode Move(string inputOrders)
        {
            StatusCode result= StatusCode.Successful;
            try
            {
                if (String.IsNullOrEmpty(inputOrders))
                {
                    _logger.LogError($"Move Order is empty: {inputOrders}");
                    return StatusCode.InvalidInput;
                }
                    
                Queue<string> orders = new Queue<string>();
                for (int i = 0; i < inputOrders.Length; i += 2)
                {
                    if (Regex.IsMatch(inputOrders.Substring(i, 2), @"[rlRL][0-9]"))
                    { 
                        orders.Enqueue(inputOrders.Substring(i, 2));
                    }
                    else
                    {
                        _logger.LogError($"Move Order is invalid: {inputOrders}");
                        return StatusCode.InvalidInput;
                    }
                }
                while (orders.Count > 0 && result ==StatusCode.Successful)
                {
                    result = MoveOrder(orders.Dequeue());
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Wrong instruction.{e.Message}");
                return StatusCode.Error;
            }
            return result;
        }
        private StatusCode MoveOrder(string s)
        {
            try
            {
                ChangeDirection(s[0]);
                return MovePositions(Int16.Parse(s[1].ToString()));
            }
            catch (Exception e)
            {
                _logger.LogError($"Wrong instruction.{e.Message}");
                return StatusCode.Error;
            }
        }
        private void ChangeDirection(char direction)
        {
            switch (direction)
            {
                case 'L':
                    if (_direction == 'N')
                        _direction = 'W';
                    else if (_direction == 'W')
                        _direction = 'S';
                    else if (_direction == 'S')
                        _direction = 'E';
                    else if (_direction == 'E')
                        _direction = 'N';
                    break;
                case 'R':
                    if (_direction == 'N')
                        _direction = 'E';
                    else if (_direction == 'E')
                        _direction = 'S';
                    else if (_direction == 'S')
                        _direction = 'W';
                    else if (_direction == 'W')
                        _direction = 'N';
                    break;
            }
        }

        private StatusCode MovePositions(int positions)
        {
            try
            {
                switch (_direction)
                {
                    case 'N':
                        if ((_Y + positions) >= 0 && (_Y + positions) <= _grid._height)
                        {
                            _Y += positions;
                        }
                        else
                        {
                            //        _logger.LogError($"Move Order out of Y value.");
                            _logger.LogWarning($"Move coordenate Y out of borders.");
                            return StatusCode.OutOfBounds;
                        }
                        break;
                    case 'W':
                        if ((_X - positions) >= 0 && (_X - positions) <= _grid._width)
                        {
                            _X -= positions;
                        }
                        else
                        {
                            _logger.LogWarning($"Move coordenate X out of borders.");
                            return StatusCode.OutOfBounds;
                        }
                        break;
                    case 'S':
                        if ((_Y - positions) >= 0 && (_Y - positions) <= _grid._height)
                        {
                            _Y -= positions;

                        }
                        else
                        {
                            _logger.LogWarning($"Move coordenate Y out of borders.");
                            return StatusCode.OutOfBounds;
                        }
                        break;
                    case 'E':
                        if ((_X + positions) >= 0 && (_X + positions) <= _grid._width)
                        {
                            _X += positions;
                        }
                        else
                        {
                            _logger.LogWarning($"Move coordenate X out of borders.");
                            return StatusCode.OutOfBounds;
                        }
                        break;
                }
            }
            catch(Exception e)
            {
                _logger.LogWarning($"Wrong instruction.{e.Message}");
                return StatusCode.Error;
            }
            return StatusCode.Successful;
        }
        #endregion

        #region Comparable
        public int CompareTo([AllowNull] Rover other)
        {
            if (_X == other._X && _Y == other._Y && _direction == other._direction)
                return 0;
            return -1;
        }
        #endregion
    }
}
