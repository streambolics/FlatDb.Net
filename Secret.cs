using System;

namespace FlatDatabase
{
    public class Secret
    {
        private string _Password;
        private static readonly TimeSpan _Short = TimeSpan.FromSeconds(1);
        private DateTime _LockDown = DateTime.Now;
        private TimeSpan _LockDownTime = _Short;

        public bool Check(string aPassword)
        {
            if (String.IsNullOrEmpty(aPassword))
            {
                GlobalLog.LogString("MSG-xxxxx Empty password");
                return false;
            }
            else if (_Password != aPassword)
            {
                GlobalLog.LogString("MSG-xxxxx Password Mismatch");
                _LockDown = DateTime.Now + _LockDownTime;
                _LockDownTime += _Short;
                return false;
            }
            else if (DateTime.Now < _LockDown)
            {
                GlobalLog.LogString("MSG-xxxxx Lockdown");
                return false;
            }
            else
            {
                GlobalLog.LogString("MSG-xxxxx Password accepted");
                _LockDownTime = TimeSpan.FromSeconds(1);
                return true;

            }
        }

        public void SetPassword(string aPassword) => _Password = aPassword;
    }
}