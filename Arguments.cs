using System;
using System.Collections.Generic;

namespace FlatDatabase{
    public class Arguments
    {
        private List<string> _OwnArguments = new List<string>();
        private List<string> _KestrelArguments = new List<string>();

        protected bool isOwnOption(ref string s, ref int c)
        {
            if (c > 0)
            {
                //  We still have arguments from the previous option, keep gathering
                //  them as our own.
                c--;
                return true;
            }
            if (s == "--")
            {
                //  The -- separator indicates that everything that follows is
                //  an own option. Actually we only support 2 billion more arguments.
                c = Int32.MaxValue;
                s = null;
                return true;
            }
            else if (s.StartsWith("--with."))
            {
                // Anything prefixed with --with: is one of our own arguments.
                // There is no reason to use this instead of -- except for compatibility
                // reasons.
                s = s.Substring(7);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Arguments(string[] args)
        { 
            int owncount = 0;

            foreach (string ss in args)
            {
                string s = ss;
                if (isOwnOption(ref s, ref owncount))
                {
                    if (s != null)
                    {
                        _OwnArguments.Add(s);
                    }
                }
                else
                {
                    _KestrelArguments.Add(s);
                }
            }
        }

        public string[] OwnerArguments{ get { return _KestrelArguments.ToArray(); } }

        /// <summary>
        ///     Enumerates the argument list in search for one option with the given name.
        /// </summary>
        /// <param name="aName">
        ///     The name of the option. This is either the short name (example "-f") or
        ///     the long name (example "--file") if the option has no short name.
        /// </param>
        /// <param name="aLongName">
        ///     The long name of the option, for example "--file". If the option has onlu
        /// </param>
        /// <param name="aWithValue">
        ///     Whether we expect a value or not. When true, the argument following the
        ///     option is the value (for example "--file filename"). When false, only
        ///     the presence of the option is returned.
        /// </param>
        /// <param name="aPresent">
        ///      An action that will be called when the option is found. The first parameter
        ///      is the name of the option (either short or long), and the second parameter
        ///      is the value (or null if aWithValue is false).
        /// </param>
        /// <param name="aAbsent">
        ///     An action that will be called when the option was not found. The first
        ///     argument is the short name of the option.
        /// </param>

        public void Parse (string aName, string aLongName, bool aWithValue, Action<string,string> aPresent, Action<string> aAbsent)
        {
            string foundname = null;

            foreach (string s in _OwnArguments)
            {
                if (foundname != null)
                {
                    aPresent(foundname, s);
                    return;
                }
                else if (s == aName || s == aLongName)
                {
                    if (aWithValue)
                    {
                        foundname = s;
                    }
                    else
                    {
                        aPresent(s, null);
                        return;
                    }
                }
            }
            aAbsent(aName);
        }

        public void Parse (string aName, string aLongName, Action<string> aPresent)
        {
            Parse(aName, aLongName, true, (n, v) => aPresent(v), n => { });
        }

        public void Parse (string aName, Action<string> aPresent)
        {
            Parse(aName, null, true, (n, v) => aPresent(v), n => { });
        }

        public void Parse (string aName, string aLongName, Action<int> aPresent)
        {
            Parse(aName, aLongName, true, (n, v) => aPresent(Int32.Parse(v)), n => { });
        }

        public void Parse (string aName, Action<int> aPresent)
        {
            Parse(aName, null, true, (n, v) => aPresent(Int32.Parse(v)), n => { });
        }

        public void Parse (string aName, string aLongName, Action<bool> aPresent)
        {
            Parse(aName, aLongName, false, (n, v) => aPresent(true), n => aPresent(false));
        }

        public void Parse (string aName, Action<bool> aPresent)
        {
            Parse(aName, null, false, (n, v) => aPresent(true), n => aPresent(false));
        }
    }
}