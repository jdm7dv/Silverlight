using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;

namespace samp_LiveMessenger
{


    //EVENT WIREUPS & DELEGATES
    public class CallbackEventArgs : EventArgs
    {
        private string _action;
        public string Action
        {
            get
            {
                return _action;
            }
        }
        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
        }

        public CallbackEventArgs(string Action, string Value)
        {
            _action = Action;
            _value = Value;
        }
    }


    public delegate void CallbackDelegate(CallbackEventArgs args);


    
    //DATA OBJECTS


    //DUMMY DATA
}

