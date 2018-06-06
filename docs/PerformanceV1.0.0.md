#  Performance version 1.0.0

Measured using BenchmarkDotNet.

## Encode 

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4700HQ CPU 2.40GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2338344 Hz, Resolution=427.6531 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-PYHMSB : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-QJBENT : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3101.0
  Job-AVPNBI : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-DAGYTR : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3101.0

Platform=X64  LaunchCount=1  TargetCount=3  
WarmupCount=3  

```
|       Method |     Toolchain | Size |        Mean |         Error |      StdDev | Rank |  Gen 0 | Allocated |
|------------- |-------------- |----- |------------:|--------------:|------------:|-----:|-------:|----------:|
|    Z85Encode | .NET Core 2.0 | 6000 | 87,297.3 ns | 11,764.417 ns | 664.7114 ns |    9 | 9.5215 |   30056 B |
|    Z85Encode |  CsProjnet461 | 6000 | 59,695.0 ns | 14,881.960 ns | 840.8583 ns |    8 | 9.5215 |   30104 B |
|    Z85Encode | .NET Core 2.1 | 6000 | 59,357.8 ns |    907.637 ns |  51.2832 ns |    8 | 9.5215 |   30056 B |
|    Z85Encode |  CsProjnet471 | 6000 | 58,248.4 ns |  3,262.934 ns | 184.3618 ns |    7 | 9.5215 |   30104 B |
| Base64Encode |  CsProjnet461 | 6000 |  9,406.3 ns |  3,087.646 ns | 174.4577 ns |    6 | 5.0964 |   16056 B |
| Base64Encode |  CsProjnet471 | 6000 |  9,141.3 ns |    258.469 ns |  14.6040 ns |    6 | 5.0964 |   16056 B |
| Base64Encode | .NET Core 2.1 | 6000 |  9,066.3 ns |  1,749.807 ns |  98.8673 ns |    6 | 5.0964 |   16032 B |
| Base64Encode | .NET Core 2.0 | 6000 |  8,745.2 ns |    969.146 ns |  54.7586 ns |    5 | 5.0964 |   16032 B |
|    Z85Encode | .NET Core 2.0 |  120 |  1,763.9 ns |    183.025 ns |  10.3412 ns |    4 | 0.2079 |     656 B |
|    Z85Encode | .NET Core 2.1 |  120 |  1,247.4 ns |      6.370 ns |   0.3599 ns |    3 | 0.2079 |     656 B |
|    Z85Encode |  CsProjnet471 |  120 |  1,223.6 ns |    238.393 ns |  13.4697 ns |    3 | 0.2079 |     656 B |
|    Z85Encode |  CsProjnet461 |  120 |  1,191.1 ns |     64.600 ns |   3.6500 ns |    2 | 0.2079 |     656 B |
| Base64Encode | .NET Core 2.1 |  120 |    272.6 ns |  1,692.112 ns |  95.6075 ns |    1 | 0.1118 |     352 B |
| Base64Encode |  CsProjnet461 |  120 |    243.4 ns |    103.663 ns |   5.8572 ns |    1 | 0.1116 |     352 B |
| Base64Encode |  CsProjnet471 |  120 |    231.5 ns |      8.000 ns |   0.4520 ns |    1 | 0.1116 |     352 B |
| Base64Encode | .NET Core 2.0 |  120 |    217.4 ns |    180.720 ns |  10.2110 ns |    1 | 0.1118 |     352 B |


## Decode

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4700HQ CPU 2.40GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2338344 Hz, Resolution=427.6531 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-URBSGC : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-NBLVBR : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-SRXWRA : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3101.0
  Job-SPARAJ : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3101.0

Platform=X64  LaunchCount=1  TargetCount=3  
WarmupCount=3  

```
|       Method |     Toolchain | Size |        Mean |       Error |      StdDev | Rank |  Gen 0 | Allocated |
|------------- |-------------- |----- |------------:|------------:|------------:|-----:|-------:|----------:|
|    Z85Decode | .NET Core 2.0 | 6000 | 80,040.8 ns | 86,901.7 ns | 4,910.11 ns |    9 | 1.8311 |    6024 B |
| Base64Decode | .NET Core 2.0 | 6000 | 55,552.1 ns |  1,470.3 ns |    83.07 ns |    8 | 1.8921 |    6024 B |
|    Z85Decode | .NET Core 2.1 | 6000 | 45,069.5 ns |  1,467.1 ns |    82.89 ns |    7 | 1.8921 |    6024 B |
|    Z85Decode |  CsProjnet471 | 6000 | 44,658.5 ns |  1,763.4 ns |    99.64 ns |    6 | 1.8921 |    6026 B |
|    Z85Decode |  CsProjnet461 | 6000 | 44,650.4 ns |  4,931.6 ns |   278.64 ns |    6 | 1.8921 |    6026 B |
| Base64Decode |  CsProjnet461 | 6000 | 42,101.0 ns | 13,022.4 ns |   735.79 ns |    5 | 1.8921 |    6026 B |
| Base64Decode |  CsProjnet471 | 6000 | 41,579.7 ns |  1,436.7 ns |    81.18 ns |    5 | 1.8921 |    6026 B |
| Base64Decode | .NET Core 2.1 | 6000 | 16,137.0 ns |  1,801.1 ns |   101.76 ns |    4 | 1.8921 |    6024 B |
|    Z85Decode | .NET Core 2.0 |  120 |  1,650.9 ns |  3,430.5 ns |   193.83 ns |    3 | 0.0439 |     144 B |
|    Z85Decode |  CsProjnet461 |  120 |  1,154.2 ns |  1,935.9 ns |   109.38 ns |    2 | 0.0448 |     144 B |
|    Z85Decode |  CsProjnet471 |  120 |  1,137.7 ns |  4,202.4 ns |   237.45 ns |    2 | 0.0439 |     144 B |
|    Z85Decode | .NET Core 2.1 |  120 |    961.0 ns |    566.9 ns |    32.03 ns |    2 | 0.0448 |     144 B |
| Base64Decode | .NET Core 2.0 |  120 |    608.0 ns |    301.4 ns |    17.03 ns |    1 | 0.0448 |     144 B |
| Base64Decode |  CsProjnet471 |  120 |    588.7 ns |  1,782.0 ns |   100.69 ns |    1 | 0.0448 |     144 B |
| Base64Decode |  CsProjnet461 |  120 |    480.7 ns |    782.3 ns |    44.20 ns |    1 | 0.0448 |     144 B |
| Base64Decode | .NET Core 2.1 |  120 |    400.5 ns |    483.7 ns |    27.33 ns |    1 | 0.0448 |     144 B |
