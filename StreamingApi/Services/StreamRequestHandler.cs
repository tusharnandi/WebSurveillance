using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace VideoStreamApi.Services;

public enum HashAlgorithmType
{
    None = 0,
    sha256 = 256,
    sha384 = 384,
    Sha512 = 512
}

public class StreamOperationResponse
{
    public string requestId { get; set; }
    public int seq { get; set; }
    public string hashValue { get; set; }
}
public static class StreamRequestHandler
{
    //private readonly IConfiguration configuration;
    //private readonly ILogger<StreamRequestHandler> logger;
    public static async Task<string> WriteToFile(int seq, Stream input, bool isFirst, bool isLast, string outputfile)
    {


        Stream output;
        if (isFirst)
        {
            output = CreateFile(outputfile);
        }
        else
        {
            output = OpenForAppendFile(outputfile);
        }

        string hashValue = await CopyandHash(input, output);

        Close(output);
        return hashValue;
    }

    public static async Task<bool> SaveToFile(string inputfile, string outputfile)
    {

        Stream input;
        Stream output;
        input = OpenForReadFile(inputfile);

        output = CreateFile(outputfile);

        await Copy(input, output);

        Close(input);
        Close(output);
        return true;
    }

    private static Stream CreateFile(string filepath)
    {
        //Console.WriteLine("CreateFile...");
        return new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
    }
    private static Stream OpenForAppendFile(string filepath)
    {
        //Console.WriteLine("OpenFile...");
        return new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.Read);
    }
    private static Stream OpenForReadFile(string filepath)
    {
        // Console.WriteLine("OpenForReadFile...");
        return new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
    }
    private static async Task Copy(Stream input, Stream output)
    {
        byte[] buffer = new byte[40 * 1024];
        int len;
        while ((len = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, len);
            Console.WriteLine($"Written {len} bytes.");
        }
    }

    private static async Task<string> CopyandHash(Stream input, Stream output)
    {
        string hashValue = string.Empty;
        HashAlgorithmType type = HashAlgorithmType.Sha512;
        byte[] buffer = new byte[40 * 1024];
        int len;
        while ((len = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            //hashValue = ComputeHash(buffer, type);
            output.Write(buffer, 0, len);
            Console.WriteLine($"Written {len} bytes and hashValue {hashValue}.");
        }
        return hashValue;

    }
    private static void Close(Stream stream)
    {
        //Console.WriteLine("Close.");
        stream.Close();
    }

    private static string ComputeHash(byte[] data, HashAlgorithmType type)
    {
        string hashValue = string.Empty;

        switch (type)
        {
            case HashAlgorithmType.sha256:

                using (SHA256 shaM = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = shaM.ComputeHash(data);
                    //Convert to based64
                    hashValue = Convert.ToBase64String(bytes);
                }
                break;

            case HashAlgorithmType.sha384:

                using (SHA384 shaM = SHA384.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = shaM.ComputeHash(data);
                    //Convert to based64
                    hashValue = Convert.ToBase64String(bytes);
                }
                break;

            case HashAlgorithmType.Sha512:

                using (SHA512 shaM = SHA512.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = shaM.ComputeHash(data);
                    //Convert to based64
                    hashValue = Convert.ToBase64String(bytes);
                }
                break;
            default:
                throw new ArgumentException("invalid hash algorithm type. there is no implementation available");

        }

        return hashValue;

    }

}