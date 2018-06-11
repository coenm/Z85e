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


``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4700HQ CPU 2.40GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2338344 Hz, Resolution=427.6531 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-PYUMDI : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-GKHCEF : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-WKBMYH : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3101.0


```
|       Method |     Toolchain | Categories | Size |        Mean |  Gen 0 | Allocated |
|------------- |-------------- |----------- |----- |------------:|-------:|----------:|
|    **Z85Decode** | **.NET Core 2.0** |        **Z85** |  **120** |    **266.9 ns** | **0.0911** |     **288 B** |
|    Z85Decode | .NET Core 2.1 |        Z85 |  120 |    197.6 ns | 0.0913 |     288 B |
|    Z85Decode |  CsProjnet471 |        Z85 |  120 |    263.4 ns | 0.0911 |     288 B |
|    **Z85Decode** | **.NET Core 2.0** |        **Z85** | **6000** | **10,882.0 ns** | **3.8147** |   **12048 B** |
|    Z85Decode | .NET Core 2.1 |        Z85 | 6000 |  9,212.9 ns | 3.8147 |   12048 B |
|    Z85Decode |  CsProjnet471 |        Z85 | 6000 | 10,940.4 ns | 3.8147 |   12053 B |
|              |               |            |      |             |        |           |
| **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **120** |    **547.0 ns** | **0.0448** |     **144 B** |
| Base64Decode | .NET Core 2.1 |     Base64 |  120 |    360.5 ns | 0.0453 |     144 B |
| Base64Decode |  CsProjnet471 |     Base64 |  120 |    402.5 ns | 0.0453 |     144 B |
| **Base64Decode** | **.NET Core 2.0** |     **Base64** | **6000** | **54,487.2 ns** | **1.8921** |    **6024 B** |
| Base64Decode | .NET Core 2.1 |     Base64 | 6000 | 15,710.2 ns | 1.8921 |    6024 B |
| Base64Decode |  CsProjnet471 |     Base64 | 6000 | 40,881.3 ns | 1.8921 |    6026 B |
