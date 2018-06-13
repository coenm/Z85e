#  Performance version 1.0.0

Measured using BenchmarkDotNet at 2018/06/06.


``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4700HQ CPU 2.40GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2338344 Hz, Resolution=427.6531 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-ONXXHA : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-BGJLHX : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-QVCBYA : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3101.0

```

## Z85 vs. Base64

### Encoding

|           Method |         Toolchain | Categories |     Size |            Mean |      Gen 0 |   Allocated |
|----------------- |------------------ |----------- |--------- |----------------:|-----------:|------------:|
|    **Z85Encode** | **.NET Core 2.0** |    **Z85** |  **120** |  **1,674.6 ns** | **0.2079** |   **656 B** |
|        Z85Encode |     .NET Core 2.1 |        Z85 |      120 |      1,164.6 ns |     0.2079 |       656 B |
|        Z85Encode |      CsProjnet471 |        Z85 |      120 |      1,117.0 ns |     0.2079 |       656 B |
|    **Z85Encode** | **.NET Core 2.0** |    **Z85** | **6000** | **82,533.6 ns** | **9.5215** | **30056 B** |
|        Z85Encode |     .NET Core 2.1 |        Z85 |     6000 |     55,888.5 ns |     9.5215 |     30056 B |
|        Z85Encode |      CsProjnet471 |        Z85 |     6000 |     54,729.1 ns |     9.5215 |     30104 B |
|                  |                   |            |          |                 |            |             |
| **Base64Encode** | **.NET Core 2.0** | **Base64** |  **120** |    **191.5 ns** | **0.1118** |   **352 B** |
|     Base64Encode |     .NET Core 2.1 |     Base64 |      120 |        194.5 ns |     0.1118 |       352 B |
|     Base64Encode |      CsProjnet471 |     Base64 |      120 |        216.9 ns |     0.1118 |       352 B |
| **Base64Encode** | **.NET Core 2.0** | **Base64** | **6000** |  **8,093.4 ns** | **5.0964** | **16032 B** |
|     Base64Encode |     .NET Core 2.1 |     Base64 |     6000 |      8,110.0 ns |     5.0964 |     16032 B |
|     Base64Encode |      CsProjnet471 |     Base64 |     6000 |      8,622.3 ns |     5.0964 |     16056 B |


### Decoding

|           Method |         Toolchain | Categories |     Size |            Mean |      Gen 0 |  Allocated |
|----------------- |------------------ |----------- |--------- |----------------:|-----------:|-----------:|
|    **Z85Decode** | **.NET Core 2.0** |    **Z85** |  **120** |  **1,172.3 ns** | **0.0439** |  **144 B** |
|        Z85Decode |     .NET Core 2.1 |        Z85 |      120 |        834.6 ns |     0.0448 |      144 B |
|        Z85Decode |      CsProjnet471 |        Z85 |      120 |        994.6 ns |     0.0448 |      144 B |
|    **Z85Decode** | **.NET Core 2.0** |    **Z85** | **6000** | **57,593.3 ns** | **1.8921** | **6024 B** |
|        Z85Decode |     .NET Core 2.1 |        Z85 |     6000 |     41,303.4 ns |     1.8921 |     6024 B |
|        Z85Decode |      CsProjnet471 |        Z85 |     6000 |     41,103.4 ns |     1.8921 |     6026 B |
|                  |                   |            |          |                 |            |            |
| **Base64Decode** | **.NET Core 2.0** | **Base64** |  **120** |    **518.2 ns** | **0.0448** |  **144 B** |
|     Base64Decode |     .NET Core 2.1 |     Base64 |      120 |        341.8 ns |     0.0453 |      144 B |
|     Base64Decode |      CsProjnet471 |     Base64 |      120 |        380.1 ns |     0.0453 |      144 B |
| **Base64Decode** | **.NET Core 2.0** | **Base64** | **6000** | **51,380.2 ns** | **1.8921** | **6024 B** |
|     Base64Decode |     .NET Core 2.1 |     Base64 |     6000 |     14,718.2 ns |     1.9073 |     6024 B |
|     Base64Decode |      CsProjnet471 |     Base64 |     6000 |     38,429.6 ns |     1.8921 |     6026 B |


## Z85 Extended vs. Base64

### Encoding

|            Method |     Toolchain | Categories | Size |       Mean |  Gen 0 | Allocated |
|------------------ |-------------- |----------- |----- |-----------:|-------:|----------:|
| **Z85ExtendedEncode** | **.NET Core 2.0** |        **Z85** |  **122** | **1,852.4 ns** | **0.2117** |     **672 B** |
| Z85ExtendedEncode | .NET Core 2.1 |        Z85 |  122 | 1,312.1 ns | 0.2117 |     672 B |
| Z85ExtendedEncode |  CsProjnet471 |        Z85 |  122 | 1,298.9 ns | 0.2117 |     672 B |
| **Z85ExtendedEncode** | **.NET Core 2.0** |        **Z85** |  **123** | **1,839.6 ns** | **0.2117** |     **672 B** |
| Z85ExtendedEncode | .NET Core 2.1 |        Z85 |  123 | 1,319.4 ns | 0.2117 |     672 B |
| Z85ExtendedEncode |  CsProjnet471 |        Z85 |  123 | 1,304.0 ns | 0.2117 |     672 B |
| **Z85ExtendedEncode** | **.NET Core 2.0** |        **Z85** |  **124** | **1,110.9 ns** | **0.1049** |     **336 B** |
| Z85ExtendedEncode | .NET Core 2.1 |        Z85 |  124 |   384.7 ns | 0.1063 |     336 B |
| Z85ExtendedEncode |  CsProjnet471 |        Z85 |  124 |   448.8 ns | 0.1063 |     336 B |
| **Z85ExtendedEncode** | **.NET Core 2.0** |        **Z85** |  **125** | **1,825.3 ns** | **0.2174** |     **688 B** |
| Z85ExtendedEncode | .NET Core 2.1 |        Z85 |  125 | 1,301.9 ns | 0.2174 |     688 B |
| Z85ExtendedEncode |  CsProjnet471 |        Z85 |  125 | 1,281.1 ns | 0.2174 |     688 B |
|                   |               |            |      |            |        |           |
|      **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **122** |   **206.8 ns** | **0.1142** |     **360 B** |
|      Base64Encode | .NET Core 2.1 |     Base64 |  122 |   270.7 ns | 0.1142 |     360 B |
|      Base64Encode |  CsProjnet471 |     Base64 |  122 |   234.6 ns | 0.1140 |     360 B |
|      **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **123** |   **208.0 ns** | **0.1142** |     **360 B** |
|      Base64Encode | .NET Core 2.1 |     Base64 |  123 |   209.2 ns | 0.1142 |     360 B |
|      Base64Encode |  CsProjnet471 |     Base64 |  123 |   235.3 ns | 0.1140 |     360 B |
|      **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **124** |   **209.1 ns** | **0.1168** |     **368 B** |
|      Base64Encode | .NET Core 2.1 |     Base64 |  124 |   211.1 ns | 0.1168 |     368 B |
|      Base64Encode |  CsProjnet471 |     Base64 |  124 |   239.7 ns | 0.1168 |     368 B |
|      **Base64Encode** | **.NET Core 2.0** |     **Base64** |  **125** |   **211.3 ns** | **0.1168** |     **368 B** |
|      Base64Encode | .NET Core 2.1 |     Base64 |  125 |   211.5 ns | 0.1168 |     368 B |
|      Base64Encode |  CsProjnet471 |     Base64 |  125 |   239.9 ns | 0.1168 |     368 B |


### Decoding

|            Method |     Toolchain | Categories | Size |       Mean |  Gen 0 | Allocated |
|------------------ |-------------- |----------- |----- |-----------:|-------:|----------:|
| **Z85ExtendedDecode** | **.NET Core 2.0** |        **Z85** |  **122** | **1,259.2 ns** | **0.0477** |     **152 B** |
| Z85ExtendedDecode | .NET Core 2.1 |        Z85 |  122 |   912.0 ns | 0.0477 |     152 B |
| Z85ExtendedDecode |  CsProjnet471 |        Z85 |  122 |   916.9 ns | 0.0477 |     152 B |
| **Z85ExtendedDecode** | **.NET Core 2.0** |        **Z85** |  **123** | **1,314.9 ns** | **0.0477** |     **152 B** |
| Z85ExtendedDecode | .NET Core 2.1 |        Z85 |  123 |   982.1 ns | 0.0477 |     152 B |
| Z85ExtendedDecode |  CsProjnet471 |        Z85 |  123 |   963.5 ns | 0.0477 |     152 B |
| **Z85ExtendedDecode** | **.NET Core 2.0** |        **Z85** |  **124** |   **214.9 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | .NET Core 2.1 |        Z85 |  124 |   214.2 ns | 0.0482 |     152 B |
| Z85ExtendedDecode |  CsProjnet471 |        Z85 |  124 |   214.0 ns | 0.0482 |     152 B |
| **Z85ExtendedDecode** | **.NET Core 2.0** |        **Z85** |  **125** | **1,312.5 ns** | **0.0477** |     **152 B** |
| Z85ExtendedDecode | .NET Core 2.1 |        Z85 |  125 |   932.0 ns | 0.0477 |     152 B |
| Z85ExtendedDecode |  CsProjnet471 |        Z85 |  125 |   942.4 ns | 0.0477 |     152 B |
|                   |               |            |      |            |        |           |
|      **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **122** |   **565.6 ns** | **0.0477** |     **152 B** |
|      Base64Decode | .NET Core 2.1 |     Base64 |  122 |   376.1 ns | 0.0482 |     152 B |
|      Base64Decode |  CsProjnet471 |     Base64 |  122 |   427.7 ns | 0.0482 |     152 B |
|      **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **123** |   **563.6 ns** | **0.0477** |     **152 B** |
|      Base64Decode | .NET Core 2.1 |     Base64 |  123 |   380.7 ns | 0.0482 |     152 B |
|      Base64Decode |  CsProjnet471 |     Base64 |  123 |   427.1 ns | 0.0482 |     152 B |
|      **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **124** |   **584.2 ns** | **0.0477** |     **152 B** |
|      Base64Decode | .NET Core 2.1 |     Base64 |  124 |   388.1 ns | 0.0482 |     152 B |
|      Base64Decode |  CsProjnet471 |     Base64 |  124 |   428.8 ns | 0.0482 |     152 B |
|      **Base64Decode** | **.NET Core 2.0** |     **Base64** |  **125** |   **584.5 ns** | **0.0477** |     **152 B** |
|      Base64Decode | .NET Core 2.1 |     Base64 |  125 |   385.8 ns | 0.0482 |     152 B |
|      Base64Decode |  CsProjnet471 |     Base64 |  125 |   430.9 ns | 0.0482 |     152 B |
