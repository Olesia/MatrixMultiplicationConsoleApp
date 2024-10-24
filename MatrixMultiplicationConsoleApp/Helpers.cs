using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace MatrixMultiplicationConsoleApp;

static class Helpers
{
    public static async Task FetchRowAsync(NumbersServiceConsumer numbersService, ConcurrentDictionary<int, int[]> rows, int i)
    {
        var row = await numbersService.GetDataAsync("A", "row", i);
        rows[i] = JsonConvert.DeserializeObject<MatrixResponse>(row)!.Value;
    }

    public static async Task FetchColAsync(NumbersServiceConsumer numbersService, ConcurrentDictionary<int, int[]> cols, int i)
    {
        var col = await numbersService.GetDataAsync("B", "col", i);
        cols[i] = JsonConvert.DeserializeObject<MatrixResponse>(col)!.Value;
    }

    public static string ConcatenateMatrixByColumnAndRow(int[,] matrix, int size)
    {
        StringBuilder sb = new();

        for (int col = 0; col < size; col++)
        {
            for (int row = 0; row < size; row++)
            {
                sb.Append(matrix[col, row]);
            }
        }
        return sb.ToString();
    }

    public static string GetMD5String(string concatenatedMatrix)
    {
        using MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(concatenatedMatrix);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return string.Join("", hashBytes);
    }

}
