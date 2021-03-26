using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Internal
{
    public class AnswerTemplate
    {
        private string link;
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Link
        {
            get { return link; }
            set { link = value; }
        }
    }
}
