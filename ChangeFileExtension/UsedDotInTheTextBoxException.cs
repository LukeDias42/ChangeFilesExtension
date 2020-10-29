using System;
using System.Runtime.Serialization;

namespace ChangeFileExtension
{
    [Serializable]
    internal class UsedDotInTheTextBoxException : Exception
    {
        public UsedDotInTheTextBoxException()
        {
        }
    }
}