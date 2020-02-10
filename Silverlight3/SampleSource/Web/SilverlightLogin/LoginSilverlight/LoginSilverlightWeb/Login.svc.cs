using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace LoginSilverlightWeb
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Login
    {
        [OperationContract]
        public LoginInfo DoLogin(string login, string password)
        {
            // Add your operation implementation here
            //Do User validation
            LoginInfo u = new LoginInfo();
            if (login == "login" && password == "password")
            {
                u.UserID = 1;
                u.LoginMessage = "Login successful";
            }
            else
            {

                // if validation fails, set u.Login
                u.LoginMessage = "Loing Failed. Invalid login";
            }
            return u;
        }

        // Add more operations here and mark them with [OperationContract]
    }

    [DataContract]
    public class LoginInfo
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string LoginMessage { get; set; }
    }    
}
