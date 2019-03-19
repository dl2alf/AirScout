using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LibADSB
{
    public class BadFormatException : Exception
    {
	    public BadFormatException(string message) 
            : base (message)
        {
	    }
    }

    public class MissingInformationException : Exception 
    {
        public MissingInformationException(string message)
            : base(message)
        {
        }
    }

    public class PositionStraddleException : Exception
    {
        public PositionStraddleException(string message)
            : base(message)
        {
        }
    }

    public class UnspecifiedFormatException : Exception
    {
        public UnspecifiedFormatException(string message)
            : base(message)
        {
        }
    }

    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException(string message)
            : base(message)
        {
        }
    }


}
