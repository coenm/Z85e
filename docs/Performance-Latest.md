``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4700HQ CPU 2.40GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2338344 Hz, Resolution=427.6531 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-RQJPJD : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-GESNPT : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-FPDFEK : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3101.0


```
|       Method |     Toolchain | Categories | Size |        Mean |   Gen 0 | Allocated |
|------------- |-------------- |----------- |----- |------------:|--------:|----------:|
|    **Z85Encode** | **.NET Core 2.0** |        **Z85** |  **120** |  **1,226.8 ns** |  **0.3109** |     **984 B** |
|    Z85Encode | .NET Core 2.1 |        Z85 |  120 |    380.1 ns |  0.3123 |     984 B |
|    Z85Encode |  CsProjnet471 |        Z85 |  120 |    501.5 ns |  0.3119 |     984 B |
|    **Z85Encode** | **.NET Core 2.0** |        **Z85** | **6000** | **57,208.2 ns** | **14.2822** |   **45080 B** |
|    Z85Encode | .NET Core 2.1 |        Z85 | 6000 | 17,660.9 ns | 14.2822 |   45080 B |
|    Z85Encode |  CsProjnet471 |        Z85 | 6000 | 22,017.6 ns | 14.2822 |   45152 B |
|              |               |            |      |             |         |           |
| **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **120** |    **202.2 ns** |  **0.1118** |     **352 B** |
| Base64Encode | .NET Core 2.1 |     Base64 |  120 |    202.5 ns |  0.1118 |     352 B |
| Base64Encode |  CsProjnet471 |     Base64 |  120 |    227.6 ns |  0.1118 |     352 B |
| **Base64Encode** | **.NET Core 2.0** |     **Base64** | **6000** |  **8,415.0 ns** |  **5.0964** |   **16032 B** |
| Base64Encode | .NET Core 2.1 |     Base64 | 6000 |  8,447.1 ns |  5.0964 |   16032 B |
| Base64Encode |  CsProjnet471 |     Base64 | 6000 |  9,160.5 ns |  5.0964 |   16056 B |
