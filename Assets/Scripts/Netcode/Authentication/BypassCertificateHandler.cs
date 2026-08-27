using UnityEngine;
using UnityEngine.Networking;

namespace Netcode.Authentication
{
    public class BypassCertificateHandler: CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Always return true because unity rejects self-signed certificates
            Debug.Log("Certificate validation bypassed");
            return true;
        }
    }
}