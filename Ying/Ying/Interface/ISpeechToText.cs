using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ying.DataService;

namespace Ying.Interface
{
    public interface ISpeechToText
    {
        void Start();
        void Stop();
        event EventHandler<EventArgsVoiceRecognition> textChanged;
    }

}
