using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace LibCommonUtil.WebBrowser
{
    public class CustomCertificatePolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest req, int problem)
        {
            //* Return "true" to force the certificate to be accepted.
            return true;
        }

        public bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors ==
               SslPolicyErrors.RemoteCertificateChainErrors)
            {
                return false;
            }
            else if (sslPolicyErrors ==
               SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                System.Security.Policy.Zone z =
                   System.Security.Policy.Zone.CreateFromUrl
                   (((HttpWebRequest)sender).RequestUri.ToString());
                if (z.SecurityZone ==
                   System.Security.SecurityZone.Intranet ||
                   z.SecurityZone ==
                   System.Security.SecurityZone.MyComputer)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
