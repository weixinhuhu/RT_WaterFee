using System;
using System.Collections.Generic;
using System.Text;

namespace TestAsposeWordForWeb
{
    public class TestInfo
    {
        private string m_title;
        private string m_content;
        private DateTime m_datetime;

        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public string Content
        {
            get { return m_content; }
            set { m_content = value; }
        }

        public DateTime Datetime
        {
            get { return m_datetime; }
            set { m_datetime = value; }
        }
    }
}
