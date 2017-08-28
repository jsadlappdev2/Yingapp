using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.DataService
{
    public class EventArgsVoiceRecognition : EventArgs
    {
        public EventArgsVoiceRecognition(string text, bool isFinal)
        {
            this.Text = text;
            this.IsFinal = isFinal;
        }
        public string Text { get; set; }
        public bool IsFinal { get; set; }

    }

}
