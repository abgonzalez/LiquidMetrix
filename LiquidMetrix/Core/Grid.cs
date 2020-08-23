using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidMetrix
{
    public class Grid : IGrid
    {
        #region public properties
        public int _width { get; set; }
        public int _height { get; set; }
        #endregion

        #region Constructor
        public Grid()
        {
            _width = 40;
            _height = 30;
        }
        public Grid(int width, int height)
        {
            _width = width;
            _height = height;
        }
        #endregion
    }
}
