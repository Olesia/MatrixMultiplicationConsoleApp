using MatrixMultiplicationConsoleApp;
using System.Diagnostics;
using System.Collections.Concurrent;
using Newtonsoft.Json;

const int MATRIX_SIZE = 1000;

#region Initializing variables

HttpClient httpClient = new();
var numbersService = new NumbersServiceConsumer(httpClient);

var result = new int[MATRIX_SIZE, MATRIX_SIZE];
ConcurrentDictionary<int, int[]> rows = new();
ConcurrentDictionary<int, int[]> cols = new();
Stopwatch stopwatch = new();

#endregion

stopwatch.Start();

#region Initializing datasets

var initResponse = await numbersService.InitDatasetsAsync(MATRIX_SIZE);
Console.WriteLine($"Datasets initialized: {JsonConvert.DeserializeObject<InitResponse>(initResponse)?.Value} \n");

#endregion

#region Fetching Rows from A and columns from B

Console.WriteLine("Fetching columns from B and rows from A \n ");

var tasks = new List<Task>();
for (int i = 0; i < MATRIX_SIZE; i++)
{
    tasks.Add(Helpers.FetchRowAsync(numbersService, rows, i));
    tasks.Add(Helpers.FetchColAsync(numbersService, cols, i));
}
await Task.WhenAll(tasks);

#endregion

#region Multiplication

Console.WriteLine("Multiplication \n ");

Parallel.For(0, MATRIX_SIZE, i =>
{
    for (int j = 0; j < MATRIX_SIZE; j++)
    {
        for (int k = 0; k < MATRIX_SIZE; k++)
        {
            result[i, j] += rows[i][k] * cols[j][k];
        }
    }
});

#endregion

#region Concatenation 

Console.WriteLine("Concatenation \n");
var concatenatedMatrix = Helpers.ConcatenateMatrixByColumnAndRow(result, MATRIX_SIZE);

#endregion

#region Getting MD5 string

Console.WriteLine("Getting MD5 string \n");
var md5String = Helpers.GetMD5String(concatenatedMatrix);

#endregion

#region Validation

var validateResponse = await numbersService.ValidateCalculationAsync(md5String);
Console.WriteLine($"Done! {JsonConvert.DeserializeObject<ValidationResponse>(validateResponse)?.Value} \n");

#endregion

stopwatch.Stop();
Console.WriteLine($"Elapsed Time: {stopwatch.Elapsed:mm\\:ss\\.ff}");

