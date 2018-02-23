using System;
using System.Collections.Generic;
using System.Text;

namespace TLSharp.Tests
{
    public class MyClientConfigData
    {
        public string ApiHash { get; set; }
        public string ApiId { get; set; }
        public string NumberToAuthenticate { get; set; }
        public string CodeToAuthenticate { get; set; }
        public string PasswordToAuthenticate { get; set; }
        public string NotRegisteredNumberToSignUp { get; set; }
        public string NumberToSendMessage { get; set; }
        public string UserNameToSendMessage { get; set; }
        public string NumberToGetUserFull { get; set; }
        public string NumberToAddToChat { get; set; }
    }
}
