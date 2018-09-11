#  Performance current version.

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4700HQ CPU 2.40GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2338344 Hz, Resolution=427.6531 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host] : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Core20 : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Core21 : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Net471 : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3101.0

```

## Z85 vs. Base64

### Encoding

|       Method |     Toolchain | Categories | Size |        Mean |  Gen 0 | Allocated |
|------------- |-------------- |----------- |----- |------------:|-------:|----------:|
|    **Z85Encode** | **.NET Core 2.0** |        **Z85** |  **120** |  **1,054.2 ns** | **0.1030** |     **328 B** |
|    Z85Encode | .NET Core 2.1 |        Z85 |  120 |    361.5 ns | 0.1040 |     328 B |
|    Z85Encode |  CsProjnet471 |        Z85 |  120 |    417.8 ns | 0.1040 |     328 B |
|    **Z85Encode** | **.NET Core 2.0** |        **Z85** | **6000** | **50,825.3 ns** | **4.7607** |   **15032 B** |
|    Z85Encode | .NET Core 2.1 |        Z85 | 6000 | 17,290.2 ns | 4.7607 |   15032 B |
|    Z85Encode |  CsProjnet471 |        Z85 | 6000 | 19,581.6 ns | 4.7607 |   15056 B |
|              |               |            |      |             |        |           |
| **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **120** |    **197.3 ns** | **0.1118** |     **352 B** |
| Base64Encode | .NET Core 2.1 |     Base64 |  120 |    200.7 ns | 0.1118 |     352 B |
| Base64Encode |  CsProjnet471 |     Base64 |  120 |    224.5 ns | 0.1118 |     352 B |
| **Base64Encode** | **.NET Core 2.0** |     **Base64** | **6000** |  **8,393.7 ns** | **5.0964** |   **16032 B** |
| Base64Encode | .NET Core 2.1 |     Base64 | 6000 |  8,283.7 ns | 5.0964 |   16032 B |
| Base64Encode |  CsProjnet471 |     Base64 | 6000 |  8,898.3 ns | 5.0964 |   16056 B |

### Decoding

|       Method |     Toolchain | Categories | Size |        Mean |  Gen 0 | Allocated |
|------------- |-------------- |----------- |----- |------------:|-------:|----------:|
|    **Z85Decode** | **.NET Core 2.0** |        **Z85** |  **120** |    **198.6 ns** | **0.0455** |     **144 B** |
|    Z85Decode | .NET Core 2.1 |        Z85 |  120 |    191.9 ns | 0.0455 |     144 B |
|    Z85Decode |  CsProjnet471 |        Z85 |  120 |    188.9 ns | 0.0455 |     144 B |
|    **Z85Decode** | **.NET Core 2.0** |        **Z85** | **6000** |  **9,179.1 ns** | **1.9073** |    **6024 B** |
|    Z85Decode | .NET Core 2.1 |        Z85 | 6000 |  9,274.0 ns | 1.9073 |    6024 B |
|    Z85Decode |  CsProjnet471 |        Z85 | 6000 |  9,222.6 ns | 1.9073 |    6026 B |
|              |               |            |      |             |        |           |
| **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **120** |    **532.5 ns** | **0.0448** |     **144 B** |
| Base64Decode | .NET Core 2.1 |     Base64 |  120 |    354.1 ns | 0.0453 |     144 B |
| Base64Decode |  CsProjnet471 |     Base64 |  120 |    392.6 ns | 0.0453 |     144 B |
| **Base64Decode** | **.NET Core 2.0** |     **Base64** | **6000** | **52,365.7 ns** | **1.8921** |    **6024 B** |
| Base64Decode | .NET Core 2.1 |     Base64 | 6000 | 15,086.4 ns | 1.9073 |    6024 B |
| Base64Decode |  CsProjnet471 |     Base64 | 6000 | 39,602.1 ns | 1.8921 |    6026 B |


## Z85 Extended vs. Base64

### Encoding

|            Method |     Toolchain | Categories | Size |       Mean |  Gen 0 | Allocated |
|------------------ |-------------- |----------- |----- |-----------:|-------:|----------:|
| **Z85ExtendedEncode** | **.NET Core 2.0** |        **Z85** |  **122** | **1,086.6 ns** | **0.1049** |     **336 B** |
| Z85ExtendedEncode | .NET Core 2.1 |        Z85 |  122 |   545.5 ns | 0.1063 |     336 B |
| Z85ExtendedEncode |  CsProjnet471 |        Z85 |  122 |   467.6 ns | 0.1063 |     336 B |
| **Z85ExtendedEncode** | **.NET Core 2.0** |        **Z85** |  **123** | **1,097.8 ns** | **0.1049** |     **336 B** |
| Z85ExtendedEncode | .NET Core 2.1 |        Z85 |  123 |   473.3 ns | 0.1059 |     336 B |
| Z85ExtendedEncode |  CsProjnet471 |        Z85 |  123 |   476.2 ns | 0.1059 |     336 B |
| **Z85ExtendedEncode** | **.NET Core 2.0** |        **Z85** |  **124** | **1,128.8 ns** | **0.1049** |     **336 B** |
| Z85ExtendedEncode | .NET Core 2.1 |        Z85 |  124 |   397.5 ns | 0.1063 |     336 B |
| Z85ExtendedEncode |  CsProjnet471 |        Z85 |  124 |   451.8 ns | 0.1063 |     336 B |
| **Z85ExtendedEncode** | **.NET Core 2.0** |        **Z85** |  **125** | **1,048.0 ns** | **0.1087** |     **344 B** |
| Z85ExtendedEncode | .NET Core 2.1 |        Z85 |  125 |   424.2 ns | 0.1092 |     344 B |
| Z85ExtendedEncode |  CsProjnet471 |        Z85 |  125 |   417.4 ns | 0.1092 |     344 B |
|                   |               |            |      |            |        |           |
|      **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **122** |   **206.4 ns** | **0.1142** |     **360 B** |
|      Base64Encode | .NET Core 2.1 |     Base64 |  122 |   209.6 ns | 0.1142 |     360 B |
|      Base64Encode |  CsProjnet471 |     Base64 |  122 |   233.6 ns | 0.1140 |     360 B |
|      **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **123** |   **209.4 ns** | **0.1142** |     **360 B** |
|      Base64Encode | .NET Core 2.1 |     Base64 |  123 |   211.1 ns | 0.1142 |     360 B |
|      Base64Encode |  CsProjnet471 |     Base64 |  123 |   234.0 ns | 0.1140 |     360 B |
|      **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **124** |   **211.0 ns** | **0.1168** |     **368 B** |
|      Base64Encode | .NET Core 2.1 |     Base64 |  124 |   211.9 ns | 0.1168 |     368 B |
|      Base64Encode |  CsProjnet471 |     Base64 |  124 |   241.1 ns | 0.1168 |     368 B |
|      **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **125** |   **212.3 ns** | **0.1168** |     **368 B** |
|      Base64Encode | .NET Core 2.1 |     Base64 |  125 |   212.5 ns | 0.1168 |     368 B |
|      Base64Encode |  CsProjnet471 |     Base64 |  125 |   238.0 ns | 0.1168 |     368 B |


### Decoding

|            Method |     Toolchain | Categories | Size |     Mean |  Gen 0 | Allocated |
|------------------ |-------------- |----------- |----- |---------:|-------:|----------:|
| **Z85ExtendedDecode** | **.NET Core 2.0** |        **Z85** |  **122** | **225.3 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | .NET Core 2.1 |        Z85 |  122 | 213.8 ns | 0.0482 |     152 B |
| Z85ExtendedDecode |  CsProjnet471 |        Z85 |  122 | 219.6 ns | 0.0482 |     152 B |
| **Z85ExtendedDecode** | **.NET Core 2.0** |        **Z85** |  **123** | **282.7 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | .NET Core 2.1 |        Z85 |  123 | 270.7 ns | 0.0482 |     152 B |
| Z85ExtendedDecode |  CsProjnet471 |        Z85 |  123 | 271.9 ns | 0.0482 |     152 B |
| **Z85ExtendedDecode** | **.NET Core 2.0** |        **Z85** |  **124** | **215.7 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | .NET Core 2.1 |        Z85 |  124 | 213.7 ns | 0.0482 |     152 B |
| Z85ExtendedDecode |  CsProjnet471 |        Z85 |  124 | 212.7 ns | 0.0482 |     152 B |
| **Z85ExtendedDecode** | **.NET Core 2.0** |        **Z85** |  **125** | **224.9 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | .NET Core 2.1 |        Z85 |  125 | 224.7 ns | 0.0482 |     152 B |
| Z85ExtendedDecode |  CsProjnet471 |        Z85 |  125 | 224.6 ns | 0.0482 |     152 B |
|                   |               |            |      |          |        |           |
|      **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **122** | **568.4 ns** | **0.0477** |     **152 B** |
|      Base64Decode | .NET Core 2.1 |     Base64 |  122 | 481.3 ns | 0.0482 |     152 B |
|      Base64Decode |  CsProjnet471 |     Base64 |  122 | 438.4 ns | 0.0482 |     152 B |
|      **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **123** | **580.6 ns** | **0.0477** |     **152 B** |
|      Base64Decode | .NET Core 2.1 |     Base64 |  123 | 377.5 ns | 0.0482 |     152 B |
|      Base64Decode |  CsProjnet471 |     Base64 |  123 | 417.7 ns | 0.0482 |     152 B |
|      **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **124** | **593.3 ns** | **0.0477** |     **152 B** |
|      Base64Decode | .NET Core 2.1 |     Base64 |  124 | 400.5 ns | 0.0482 |     152 B |
|      Base64Decode |  CsProjnet471 |     Base64 |  124 | 440.0 ns | 0.0482 |     152 B |
|      **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **125** | **580.4 ns** | **0.0477** |     **152 B** |
|      Base64Decode | .NET Core 2.1 |     Base64 |  125 | 390.3 ns | 0.0482 |     152 B |
|      Base64Decode |  CsProjnet471 |     Base64 |  125 | 429.7 ns | 0.0482 |     152 B |




## Span API
### Encoding

|       Method |    Job |     Toolchain | Size |         Mean |        Error |       StdDev |   Gen 0 | Allocated |
|------------- |------- |-------------- |----- |-------------:|-------------:|-------------:|--------:|----------:|
|    **Z85Encode** | **Core20** | **.NET Core 2.0** |  **120** |   **2,916.1 ns** |    **688.01 ns** |    **38.874 ns** |  **0.6828** |    **2160 B** |
|    Z85Encode | Core21 | .NET Core 2.1 |  120 |   1,455.7 ns |    191.48 ns |    10.819 ns |  0.6847 |    2160 B |
|    Z85Encode | Net471 |  CsProjnet471 |  120 |   2,247.2 ns |    501.08 ns |    28.312 ns |  0.6828 |    2160 B |
|    **Z85Encode** | **Core20** | **.NET Core 2.0** | **6000** | **141,654.4 ns** | **25,465.10 ns** | **1,438.825 ns** | **34.1797** |  **108000 B** |
|    Z85Encode | Core21 | .NET Core 2.1 | 6000 |  83,969.2 ns |  5,060.57 ns |   285.932 ns | 34.3018 |  108000 B |
|    Z85Encode | Net471 |  CsProjnet471 | 6000 | 120,485.9 ns | 29,212.47 ns | 1,650.559 ns | 34.1797 |  108002 B |
|              |        |               |      |              |              |              |         |           |
| **Base64Encode** | **Core20** | **.NET Core 2.0** |  **120** |     **216.5 ns** |     **33.84 ns** |     **1.912 ns** |       **-** |       **0 B** |
| Base64Encode | Core21 | .NET Core 2.1 |  120 |     184.1 ns |     46.09 ns |     2.604 ns |       - |       0 B |
| Base64Encode | Net471 |  CsProjnet471 |  120 |     207.9 ns |     20.91 ns |     1.181 ns |       - |       0 B |
| **Base64Encode** | **Core20** | **.NET Core 2.0** | **6000** |   **8,028.6 ns** | **11,833.50 ns** |   **668.615 ns** |       **-** |       **0 B** |
| Base64Encode | Core21 | .NET Core 2.1 | 6000 |   7,591.9 ns |  3,217.43 ns |   181.791 ns |       - |       0 B |
| Base64Encode | Net471 |  CsProjnet471 | 6000 |   7,766.8 ns |  2,140.96 ns |   120.968 ns |       - |       0 B |



### Decoding

|       Method |    Job |     Toolchain | Size |        Mean |        Error |       StdDev |  Gen 0 | Allocated |
|------------- |------- |-------------- |----- |------------:|-------------:|-------------:|-------:|----------:|
|    **Z85Decode** | **Core20** | **.NET Core 2.0** |  **120** |    **355.2 ns** |    **171.09 ns** |     **9.667 ns** | **0.0453** |     **144 B** |
|    Z85Decode | Core21 | .NET Core 2.1 |  120 |    364.7 ns |    119.07 ns |     6.728 ns | 0.1497 |     472 B |
|    Z85Decode | Net471 |  CsProjnet471 |  120 |    343.6 ns |    178.28 ns |    10.073 ns | 0.0453 |     144 B |
|    **Z85Decode** | **Core20** | **.NET Core 2.0** | **6000** | **10,618.3 ns** |  **3,660.38 ns** |   **206.818 ns** | **1.9073** |    **6024 B** |
|    Z85Decode | Core21 | .NET Core 2.1 | 6000 | 13,473.8 ns | 13,583.76 ns |   767.508 ns | 6.6528 |   21056 B |
|    Z85Decode | Net471 |  CsProjnet471 | 6000 | 14,315.7 ns | 44,333.82 ns | 2,504.943 ns | 1.9073 |    6026 B |
|              |        |               |      |             |              |              |        |           |
| **Base64Decode** | **Core20** | **.NET Core 2.0** |  **120** |    **211.7 ns** |     **48.83 ns** |     **2.759 ns** |      **-** |       **0 B** |
| Base64Decode | Core21 | .NET Core 2.1 |  120 |    188.4 ns |    144.93 ns |     8.189 ns |      - |       0 B |
| Base64Decode | Net471 |  CsProjnet471 |  120 |    203.3 ns |    115.01 ns |     6.498 ns |      - |       0 B |
| **Base64Decode** | **Core20** | **.NET Core 2.0** | **6000** |  **7,321.0 ns** |  **2,114.20 ns** |   **119.456 ns** |      **-** |       **0 B** |
| Base64Decode | Core21 | .NET Core 2.1 | 6000 |  7,233.0 ns |  3,017.77 ns |   170.509 ns |      - |       0 B |
| Base64Decode | Net471 |  CsProjnet471 | 6000 |  6,588.9 ns |  2,724.76 ns |   153.954 ns |      - |       0 B |
