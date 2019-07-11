using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSReader.services
{
    public interface IService
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}
