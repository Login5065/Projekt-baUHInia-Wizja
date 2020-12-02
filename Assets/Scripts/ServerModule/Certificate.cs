using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Networking;

public class Certificate : CertificateHandler {
    private const string pubKey =
        "3082010A0282010100BA57CD7A083B1B941F92C724BCE7B87858C0B632B6F66323D66C2F19803B95FE" +
        "DA9BB830DD12F5D5BAA58DDC9BEBB8055F5BF2E35F0DAADB904F14F3AB8D0A3EC0B5CD3D864A25A5AF" +
        "CE73E1CAA9098B53B868F270FBC04DC02C2881801155FF4DF5D2211214EF6D77E6422EB83F09D9B9EB" +
        "35C557CFBB0A5862F617B0F67F9FB1DEA11928A57EDFBE63F801221AC0D097B130FC5C927301827E45" +
        "92983AFD47476315FD5F3D6D3AB98DC541703221A2F87E2FE355F17B641CBB85F72856CB9C313283DB" +
        "5FE1D76334FFAA25362334C399EF2649F7F89288D2B96E9BDE60D5FE2556BB0B6A93412612FBCF5E8C" +
        "B7006003692300316B11C026DB99FC6144BD3F0203010001";

    protected override bool ValidateCertificate(byte[] certificateData) {
        if (new X509Certificate2(certificateData).GetPublicKeyString().Equals(pubKey)) {
            if (Config.ModuleVerbose) Debug.Log(Messages.preCT + Messages.CertificateOk);
            return true;
        }

        // Bad dog
        Debug.LogError(Messages.preCT + Messages.CertificateNotOk);
        return false;
    }
}
