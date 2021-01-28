using System;

namespace HovelHouse.GameController
{
    public class GameControllerException : Exception
    {
        public NSException NSException { get; }
        public GameControllerException(NSException nativeException, string message) : base(message)
        {
            NSException = nativeException;
        }
    }
}