using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] filesPaths = {"example1.txt",
            "example2.txt",
            "example3.txt"  };
        int count = filesPaths.Length;
        FileStream[] readarray = new FileStream[count];
        FileStream[] copyarray = new FileStream[count];
        byte[][] bufferarray = new byte[count][];
        for (int i = 0; i < count; i++)
        {
            bufferarray[i] = new byte[1024];
        }
        for (int i = 0; i < count; i++)
        {
            int index = i;
            readarray[index] = new FileStream(filesPaths[index], FileMode.Open,
                FileAccess.Read, FileShare.Read, 1024, FileOptions.Asynchronous);
            copyarray[index] = new FileStream($"copyfile{index + 1}.txt",
                FileMode.CreateNew,
                FileAccess.Write, FileShare.ReadWrite, 1024, FileOptions.Asynchronous);
            Console.WriteLine($"Copying file {index + 1}...");
            readarray[index].BeginRead(bufferarray[index], 0, bufferarray[index].Length,
                new AsyncCallback(ReadCallback), readarray[index]);
            copyarray[index].BeginWrite(bufferarray[index], 0, bufferarray[index].Length,
                new AsyncCallback(WriteCallback), copyarray[index]);
            Console.WriteLine($"file {index + 1} coped.");
        }

    }
    static void ReadCallback(IAsyncResult ar)
    {
        FileStream fs = (FileStream)ar.AsyncState;
        fs.EndRead(ar);
        fs.Close();
    }
    static void WriteCallback(IAsyncResult ar) 
    {
        FileStream fs = (FileStream)ar.AsyncState;
        fs.EndWrite(ar);
        fs.Close();
    }
}