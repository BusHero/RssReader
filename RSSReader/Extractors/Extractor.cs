using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSReader.Extractors
{
    public class Extractor
    {
        private string Html { get; }

        public Extractor(string html) => Html = html;

        public string ImageUrl => Html;

        public string Description => Html;

    }
}
