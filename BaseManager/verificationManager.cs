using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Taskrouter.V1.Workspace.TaskQueue;
using Twilio.Types;
using Twilio.Rest.Verify.V2.Service;


namespace BaseManager
{
    
    public class VerificationManager
    {
        public string AccountSid;
        public string AuthToken;
        public string VerifyServiceSid;

        public VerificationManager(string accountSid, string authToken, string verifyServiceSid)
        {
            AccountSid = accountSid;
            AuthToken = authToken;
            VerifyServiceSid = verifyServiceSid;

            TwilioClient.Init(AccountSid, AuthToken);
        }


        /// verificacion de numeros telefonicos ANTES de subirlos a la base de datos
        public class PhoneVerification
        {
            private readonly string _verifyServiceSid;

            public PhoneVerification(string verifyServiceSid)
            {
                _verifyServiceSid = verifyServiceSid;
            }

            public void SendCode(string phoneNumber)
            {
                var verification = VerificationResource.Create(
                    to: phoneNumber,
                    channel: "sms",
                    pathServiceSid: _verifyServiceSid
                );

                Console.WriteLine($"Estado del envío SMS: {verification.Status}");
            }

            public bool VerifyCode(string phoneNumber, string code)
            {
                var verificationCheck = VerificationCheckResource.Create(
                    to: phoneNumber,
                    code: code,
                    pathServiceSid: _verifyServiceSid
                );

                return verificationCheck.Status == "approved";
            }
        }


        /// verificacion de email ANTES de subirlos a la base de datos

        public class EmailVerification
        {
            private readonly string _verifyServiceSid;

            public EmailVerification(string verifyServiceSid)
            {
                _verifyServiceSid = verifyServiceSid;
            }

            public void SendCode(string email)
            {
                var verification = VerificationResource.Create(
                    to: email,
                    channel: "email",
                    pathServiceSid: _verifyServiceSid
                );

                Console.WriteLine($"Estado del envío Email: {verification.Status}");
            }

            public bool VerifyCode(string email, string code)
            {
                var verificationCheck = VerificationCheckResource.Create(
                    to: email,
                    code: code,
                    pathServiceSid: _verifyServiceSid
                );

                return verificationCheck.Status == "approved";
            }
        }
    }
}
