namespace MatrixMultiplicationConsoleApp;

public class BaseResponse
{
    public required string Cause { get; set; }
    public bool Success { get; set; }
}

public class InitResponse: BaseResponse
{
    public required int Value { get; set; }
}

public class MatrixResponse:BaseResponse
{
    public required int[] Value { get; set; }
}

public class ValidationResponse: BaseResponse
{
    public required string Value { get; set; }
}