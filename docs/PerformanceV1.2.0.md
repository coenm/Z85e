#  Performance version 1.2.0

## Configuration 

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

|       Method |    Job | Categories | Size |        Mean |  Gen 0 | Allocated |
|------------- |------- |----------- |----- |------------:|-------:|----------:|
|    **Z85Encode** | **Core20** |        **Z85** |  **120** |    **933.1 ns** | **0.1040** |     **328 B** |
|    Z85Encode | Core21 |        Z85 |  120 |    357.9 ns | 0.1040 |     328 B |
|    Z85Encode | Net471 |        Z85 |  120 |    348.9 ns | 0.1040 |     328 B |
|    **Z85Encode** | **Core20** |        **Z85** | **6000** | **46,123.9 ns** | **4.7607** |   **15032 B** |
|    Z85Encode | Core21 |        Z85 | 6000 | 17,290.4 ns | 4.7607 |   15032 B |
|    Z85Encode | Net471 |        Z85 | 6000 | 17,171.0 ns | 4.7607 |   15056 B |
|              |        |            |      |             |        |           |
| **Base64Encode** | **Core20** |     **Base64** |  **120** |    **198.3 ns** | **0.1118** |     **352 B** |
| Base64Encode | Core21 |     Base64 |  120 |    197.7 ns | 0.1118 |     352 B |
| Base64Encode | Net471 |     Base64 |  120 |    221.4 ns | 0.1118 |     352 B |
| **Base64Encode** | **Core20** |     **Base64** | **6000** |  **8,353.2 ns** | **5.0964** |   **16032 B** |
| Base64Encode | Core21 |     Base64 | 6000 |  8,368.0 ns | 5.0964 |   16032 B |
| Base64Encode | Net471 |     Base64 | 6000 |  8,845.6 ns | 5.0964 |   16056 B |

### Decoding

|       Method |    Job | Categories | Size |        Mean |  Gen 0 | Allocated |
|------------- |------- |----------- |----- |------------:|-------:|----------:|
|    **Z85Decode** | **Core20** |        **Z85** |  **120** |    **189.3 ns** | **0.0455** |     **144 B** |
|    Z85Decode | Core21 |        Z85 |  120 |    190.3 ns | 0.0455 |     144 B |
|    Z85Decode | Net471 |        Z85 |  120 |    192.9 ns | 0.0455 |     144 B |
|    **Z85Decode** | **Core20** |        **Z85** | **6000** |  **9,257.0 ns** | **1.9073** |    **6024 B** |
|    Z85Decode | Core21 |        Z85 | 6000 |  9,251.0 ns | 1.9073 |    6024 B |
|    Z85Decode | Net471 |        Z85 | 6000 |  9,355.3 ns | 1.9073 |    6026 B |
|              |        |            |      |             |        |           |
| **Base64Decode** | **Core20** |     **Base64** |  **120** |    **558.3 ns** | **0.0448** |     **144 B** |
| Base64Decode | Core21 |     Base64 |  120 |    353.1 ns | 0.0453 |     144 B |
| Base64Decode | Net471 |     Base64 |  120 |    392.7 ns | 0.0453 |     144 B |
| **Base64Decode** | **Core20** |     **Base64** | **6000** | **46,063.6 ns** | **1.8921** |    **6024 B** |
| Base64Decode | Core21 |     Base64 | 6000 | 15,170.3 ns | 1.9073 |    6024 B |
| Base64Decode | Net471 |     Base64 | 6000 | 41,319.4 ns | 1.8921 |    6026 B |


## Z85 Extended vs. Base64

### Encoding

|            Method |    Job | Categories | Size |       Mean |  Gen 0 | Allocated |
|------------------ |------- |----------- |----- |-----------:|-------:|----------:|
| **Z85ExtendedEncode** | **Core20** |        **Z85** |  **122** | **1,047.4 ns** | **0.1049** |     **336 B** |
| Z85ExtendedEncode | Core21 |        Z85 |  122 |   437.5 ns | 0.1063 |     336 B |
| Z85ExtendedEncode | Net471 |        Z85 |  122 |   435.1 ns | 0.1063 |     336 B |
| **Z85ExtendedEncode** | **Core20** |        **Z85** |  **123** | **1,059.2 ns** | **0.1049** |     **336 B** |
| Z85ExtendedEncode | Core21 |        Z85 |  123 |   442.1 ns | 0.1063 |     336 B |
| Z85ExtendedEncode | Net471 |        Z85 |  123 |   441.1 ns | 0.1063 |     336 B |
| **Z85ExtendedEncode** | **Core20** |        **Z85** |  **124** |   **956.3 ns** | **0.1049** |     **336 B** |
| Z85ExtendedEncode | Core21 |        Z85 |  124 |   377.9 ns | 0.1063 |     336 B |
| Z85ExtendedEncode | Net471 |        Z85 |  124 |   384.5 ns | 0.1063 |     336 B |
| **Z85ExtendedEncode** | **Core20** |        **Z85** |  **125** | **1,019.0 ns** | **0.1087** |     **344 B** |
| Z85ExtendedEncode | Core21 |        Z85 |  125 |   399.9 ns | 0.1092 |     344 B |
| Z85ExtendedEncode | Net471 |        Z85 |  125 |   393.5 ns | 0.1092 |     344 B |
|                   |        |            |      |            |        |           |
|      **Base64Encode** | **Core20** |     **Base64** |  **122** |   **199.5 ns** | **0.1142** |     **360 B** |
|      Base64Encode | Core21 |     Base64 |  122 |   199.7 ns | 0.1142 |     360 B |
|      Base64Encode | Net471 |     Base64 |  122 |   224.2 ns | 0.1142 |     360 B |
|      **Base64Encode** | **Core20** |     **Base64** |  **123** |   **200.9 ns** | **0.1142** |     **360 B** |
|      Base64Encode | Core21 |     Base64 |  123 |   203.7 ns | 0.1142 |     360 B |
|      Base64Encode | Net471 |     Base64 |  123 |   227.4 ns | 0.1142 |     360 B |
|      **Base64Encode** | **Core20** |     **Base64** |  **124** |   **203.7 ns** | **0.1168** |     **368 B** |
|      Base64Encode | Core21 |     Base64 |  124 |   202.9 ns | 0.1168 |     368 B |
|      Base64Encode | Net471 |     Base64 |  124 |   233.0 ns | 0.1168 |     368 B |
|      **Base64Encode** | **Core20** |     **Base64** |  **125** |   **203.3 ns** | **0.1168** |     **368 B** |
|      Base64Encode | Core21 |     Base64 |  125 |   205.8 ns | 0.1168 |     368 B |
|      Base64Encode | Net471 |     Base64 |  125 |   228.4 ns | 0.1168 |     368 B |


### Decoding

|            Method |    Job | Categories | Size |     Mean |  Gen 0 | Allocated |
|------------------ |------- |----------- |----- |---------:|-------:|----------:|
| **Z85ExtendedDecode** | **Core20** |        **Z85** |  **122** | **216.7 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | Core21 |        Z85 |  122 | 205.6 ns | 0.0482 |     152 B |
| Z85ExtendedDecode | Net471 |        Z85 |  122 | 211.1 ns | 0.0482 |     152 B |
| **Z85ExtendedDecode** | **Core20** |        **Z85** |  **123** | **262.6 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | Core21 |        Z85 |  123 | 259.3 ns | 0.0482 |     152 B |
| Z85ExtendedDecode | Net471 |        Z85 |  123 | 261.7 ns | 0.0482 |     152 B |
| **Z85ExtendedDecode** | **Core20** |        **Z85** |  **124** | **206.1 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | Core21 |        Z85 |  124 | 201.3 ns | 0.0482 |     152 B |
| Z85ExtendedDecode | Net471 |        Z85 |  124 | 199.0 ns | 0.0482 |     152 B |
| **Z85ExtendedDecode** | **Core20** |        **Z85** |  **125** | **212.5 ns** | **0.0482** |     **152 B** |
| Z85ExtendedDecode | Core21 |        Z85 |  125 | 203.9 ns | 0.0482 |     152 B |
| Z85ExtendedDecode | Net471 |        Z85 |  125 | 211.9 ns | 0.0482 |     152 B |
|                   |        |            |      |          |        |           |
|      **Base64Decode** | **Core20** |     **Base64** |  **122** | **572.8 ns** | **0.0477** |     **152 B** |
|      Base64Decode | Core21 |     Base64 |  122 | 361.5 ns | 0.0482 |     152 B |
|      Base64Decode | Net471 |     Base64 |  122 | 408.1 ns | 0.0482 |     152 B |
|      **Base64Decode** | **Core20** |     **Base64** |  **123** | **574.5 ns** | **0.0477** |     **152 B** |
|      Base64Decode | Core21 |     Base64 |  123 | 361.5 ns | 0.0482 |     152 B |
|      Base64Decode | Net471 |     Base64 |  123 | 404.6 ns | 0.0482 |     152 B |
|      **Base64Decode** | **Core20** |     **Base64** |  **124** | **584.5 ns** | **0.0477** |     **152 B** |
|      Base64Decode | Core21 |     Base64 |  124 | 370.0 ns | 0.0482 |     152 B |
|      Base64Decode | Net471 |     Base64 |  124 | 417.2 ns | 0.0482 |     152 B |
|      **Base64Decode** | **Core20** |     **Base64** |  **125** | **587.0 ns** | **0.0477** |     **152 B** |
|      Base64Decode | Core21 |     Base64 |  125 | 364.4 ns | 0.0482 |     152 B |
|      Base64Decode | Net471 |     Base64 |  125 | 412.7 ns | 0.0482 |     152 B |




## Span API
### Encoding

|       Method |    Job | Categories | Size |        Mean | Allocated |
|------------- |------- |----------- |----- |------------:|----------:|
|    **Z85Encode** | **Core20** |        **Z85** |  **120** |    **927.9 ns** |       **0 B** |
|    Z85Encode | Core21 |        Z85 |  120 |    332.8 ns |       0 B |
|    Z85Encode | Net471 |        Z85 |  120 |    366.3 ns |       0 B |
|    **Z85Encode** | **Core20** |        **Z85** | **6000** | **42,982.2 ns** |       **0 B** |
|    Z85Encode | Core21 |        Z85 | 6000 | 14,893.4 ns |       0 B |
|    Z85Encode | Net471 |        Z85 | 6000 | 15,087.2 ns |       0 B |
|              |        |            |      |             |           |
| **Base64Encode** | **Core20** |     **Base64** |  **120** |    **205.8 ns** |       **0 B** |
| Base64Encode | Core21 |     Base64 |  120 |    170.5 ns |       0 B |
| Base64Encode | Net471 |     Base64 |  120 |    195.5 ns |       0 B |
| **Base64Encode** | **Core20** |     **Base64** | **6000** |  **6,952.7 ns** |       **0 B** |
| Base64Encode | Core21 |     Base64 | 6000 |  6,813.6 ns |       0 B |
| Base64Encode | Net471 |     Base64 | 6000 |  6,992.7 ns |       0 B |



### Decoding

|       Method |    Job | Categories | Size |       Mean | Allocated |
|------------- |------- |----------- |----- |-----------:|----------:|
|    **Z85Decode** | **Core20** |        **Z85** |  **120** |   **176.6 ns** |       **0 B** |
|    Z85Decode | Core21 |        Z85 |  120 |   138.4 ns |       0 B |
|    Z85Decode | Net471 |        Z85 |  120 |   174.4 ns |       0 B |
|    **Z85Decode** | **Core20** |        **Z85** | **6000** | **5,937.6 ns** |       **0 B** |
|    Z85Decode | Core21 |        Z85 | 6000 | 5,878.0 ns |       0 B |
|    Z85Decode | Net471 |        Z85 | 6000 | 5,909.7 ns |       0 B |
|              |        |            |      |            |           |
| **Base64Decode** | **Core20** |     **Base64** |  **120** |   **195.4 ns** |       **0 B** |
| Base64Decode | Core21 |     Base64 |  120 |   167.3 ns |       0 B |
| Base64Decode | Net471 |     Base64 |  120 |   179.0 ns |       0 B |
| **Base64Decode** | **Core20** |     **Base64** | **6000** | **6,461.2 ns** |       **0 B** |
| Base64Decode | Core21 |     Base64 | 6000 | 6,326.6 ns |       0 B |
| Base64Decode | Net471 |     Base64 | 6000 | 5,969.4 ns |       0 B |

