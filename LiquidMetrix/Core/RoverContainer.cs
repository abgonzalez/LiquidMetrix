using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace LiquidMetrix
{
    public class RoverContainer : IRoverContainer
    {
        #region public property
        public Rover SelectedRover => Rovers[_selectedRoverId];
        public int SelectedRoverId
        {
            get => _selectedRoverId;
            set
            {
                if (Rovers.ContainsKey(value))
                    _selectedRoverId = value;
                else
                    _logger.LogWarning($"Rover does not exist with this id: {value}");

            }
        }
        public Dictionary<int, Rover> Rovers { get => _rovers; }
        #endregion
        #region private properties
        ILogger<RoverContainer> _logger;
        IRover _roverService;
        
        private int _Id;
        private int _selectedRoverId { get; set; }
        private Dictionary<int, Rover> _rovers = new Dictionary<int, Rover>();
        #endregion

        #region Constructor
        public RoverContainer( ILogger<RoverContainer> logger,
                              IRover rover)
        {
            _Id = 0;
            _selectedRoverId = 0;
            _logger = logger;
            _roverService = rover;
        }
        #endregion

        #region Operations
        public int Add()
        {
            Rovers.Add(++_Id, _roverService.createNew());
            SelectedRoverId = _Id;
            return SelectedRoverId;
        }
        public Rover Find(int key)
        {
            return Rovers.Where(c => c.Key == key).FirstOrDefault().Value;
        }
        public bool Contains(int key)
        {
            return Find(key) != null;
        }

        #endregion
    }
}
