using System;
using System.Runtime.Serialization;

namespace ChangeFileExtension
{
    [Serializable]
    internal class NothingInTheTextBoxException : Exception
    {
        public NothingInTheTextBoxException()
        {

        }
    }
}