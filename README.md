# About this project
Z85 encoding extended dotnet standard 1.0, and 2.0

This project implements the Z85 encoding standard as described in this [rfc](https://rfc.zeromq.org/spec:32/Z85/) together with an extended version where you can encode bytes with no restriction on the length of the bytes (ie. it is not required to have a length of a multiple of 4).

## Branching model
We use [GitFlow](http://nvie.com/posts/a-successful-git-branching-model/) as branching model.

## Nuget
The alpha releases can be found using this [MyGet feed](https://www.myget.org/F/coenm/api/v3/index.json). 
Beta and final releases will be located at NuGet.

| Type | Package |
| :--- | :--- | 
| Alpha | [![MyGet Pre Release](https://img.shields.io/myget/coenm/vpre/CoenM.Encoding.Z85e.svg?label=myget)](https://www.myget.org/feed/Packages/coenm/) |
| Beta | not yet | 
| Final | not yet |
| :--- | :--- | 


# Z85e
Originally, Z85 only encodes blocks of 4 bytes. To allow blocks of all lengths this library introduces Z85e (extended).

## Goals
- Z85e uses the same output characters as Z85;
- Z85e encodes an input byte array (with a length multiple of 4) exactly the same as Z85;
- Z85e decodes an input string (with a length multiple of 5) exactly the same as Z85;




# Example encoding Z85
Data and definitions are taken from the extended documentation of [Z85](https://rfc.zeromq.org/spec:32/Z85/).

Z85 uses this representation for each base-85 value from zero to 84:
```
 0 -  9:  0 1 2 3 4 5 6 7 8 9
10 - 19:  a b c d e f g h i j
20 - 29:  k l m n o p q r s t
30 - 39:  u v w x y z A B C D
40 - 49:  E F G H I J K L M N
50 - 59:  O P Q R S T U V W X
60 - 69:  Y Z . - : + = ^ ! /
70 - 79:  * ? & < > ( ) [ ] {
80 - 84:  } @ % $ #
```


The following 4 bytes:
```
+------+------+------+------+
| 0x86 | 0x4F | 0xD2 | 0x6F |
+------+------+------+------+
```

should encode as the following 5 characters:
```
+---+---+---+---+---+ 
| H | e | l | l | o | 
+---+---+---+---+---+ 
```

## Explanation of the calculations:

```
0x86 * 0xFF * 0xFF * 0xFF 
0x4F * 0xFF * 0xFF 
0xD2 * 0xFF
0x6F                        +
----------------------------
2253378159
```

For the first character:
```
2253378159 / (85 ^ 4) = 43 
2253378159 % (85 ^ 4) = 8751284
```
Looking at the table, you'll see that 43 maps to an 'H'.
The remainder 8751284 is used for the second character:

```
8751284 / (85 ^ 3) = 14 
8751284 % (85 ^ 3) = 153534
```
Now, the 14 maps to an 'e'. 

For the third character we use 153534:
```
153534 / (85 ^ 2) = 21 
153534 % (85 ^ 2) = 1809
```

The fourth character:
```
1809 / (85 ^ 1) = 21 
1809 % (85 ^ 1) = 24
```

And the last character:
```
24 / (85 ^ 0) = 24 
24 % (85 ^ 0) = 0
```


The following values are found:
```
+----+----+----+----+----+ 
| 43 | 14 | 21 | 21 | 24 | 
+----+----+----+----+----+ 
```
and these map to:
```
+---+---+---+---+---+ 
| H | e | l | l | o | 
+---+---+---+---+---+ 
```




# Continuous integration status

| Service | Status |
| :--- | :--- |
| Appveyor Windows build (last build): | [![Build status](https://ci.appveyor.com/api/projects/status/s24908kye3ipfosw/branch/develop?svg=true)](https://ci.appveyor.com/project/coenm/z85e/branch/develop) |
| Appveyor Windows build (develop branch last build): | [![Build status](https://ci.appveyor.com/api/projects/status/s24908kye3ipfosw/branch/develop?svg=true)](https://ci.appveyor.com/project/coenm/z85e/) |
| Coverage of develop branch: | [![codecov](https://codecov.io/gh/coenm/z85e/branch/develop/graph/badge.svg)](https://codecov.io/gh/coenm/z85e)

[![Build history](https://buildstats.info/appveyor/chart/coenm/z85e)](https://ci.appveyor.com/project/coenm/z85e/history)


### Coverage
Coverage trend of the develop branch.
 ![Coverage trend of develop](https://codecov.io/gh/coenm/z85e/branch/develop/graphs/commits.svg)