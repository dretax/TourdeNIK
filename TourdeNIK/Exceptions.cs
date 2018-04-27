using System;

namespace TourdeNIK
{
    internal class BTSException : Exception
    {
        private string _err;
        
        public BTSException(string err)
        {
            _err = err;
        }

        public override string Message
        {
            get
            {
                return _err;
            }
        }
    }
    
    internal class InvalidRacerDatas : Exception
    {
        private string _err;
        
        public InvalidRacerDatas(string err)
        {
            _err = err;
        }

        public override string Message
        {
            get
            {
                return _err;
            }
        }
    }
}