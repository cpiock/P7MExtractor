using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length > 0)
            //string fullFileName = Path.GetFullPath(args[0]);
            foreach (string fileName in Directory.GetFiles("p7m"))
            {
                
                FileStream file = new FileStream(fileName, FileMode.Open);
                bool isValid = true;
                Console.WriteLine("File to decrypt: " + fileName);
                try
                {
                    CmsSignedData signedFile = new CmsSignedData(file);
                    IX509Store certStore = signedFile.GetCertificates("Collection");
                    ICollection certs = certStore.GetMatches(new X509CertStoreSelector());
                    SignerInformationStore signerStore = signedFile.GetSignerInfos();
                    ICollection signers = signerStore.GetSigners();
                                       
                    foreach (object tempCertification in certs)
                    {
                        X509Certificate certification = tempCertification as X509Certificate;

                        foreach (object tempSigner in signers)
                        {
                            SignerInformation signer = tempSigner as SignerInformation;
                            if (!signer.Verify(certification.GetPublicKey()))
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }
                    string newFileName = Path.Combine(Directory.CreateDirectory("p7m-extracted").Name, Path.GetFileNameWithoutExtension(fileName));
                    using (var fileStream = new FileStream(newFileName, FileMode.Create, FileAccess.Write))
                    {
                        signedFile.SignedContent.Write(fileStream);
                        Console.WriteLine("File decrypted: " + newFileName);
                    }

                }
                catch (Exception ex)
                {
                    isValid = false;
                }

                Console.WriteLine("File valid: " + isValid);
                              
;            }
            Console.ReadLine();
        }
    }
}
