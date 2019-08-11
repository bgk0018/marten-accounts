﻿using System;

namespace Accounts.API.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Type type) : base(nameof(type) + " can't be found.")
        {

        }

        public NotFoundException() : base()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}