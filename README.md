# ExportCircuit
Converting old (python) IEEE excel format to new (C#) excel format

## Requirements
<hr>

1. Visual Studio (C#, .NET)
2. Python (version 3+)
3. .NET 6.0 (If it is not installed with Visual Studio)

## How to install
<hr>

### Install using code
1. Clone the repo
2. Open the .sln file in Visual Studio
3. Right Click the name of the project and choose 'Publish'
4. Select the folder to publish to and modify options (as shown in video) to generate a single executable
<br>

### Install from Releases
1. Go to the 'Releases' tab and download the latest release

## How to run
<hr>

### Running exportcircuit
1. The command looks like: `exportcircuit -in <input file/folder> -out <output folder>`
2. To generate DERs you can add extra flags to the command above:
    1. `-addallder <scale> [<loadClass> <quartile>]`
    2. `-addevchargingstation <scale> [<loadClass> <quartile>]`
    3. `-addheatpump <scale> [<loadClass> <quartile>]`
    4. `-addgenerator <scale> [<loadClass> <quartile>]`
    5. `-addsolar <scale> [<loadClass> <quartile>]`
    6. `-addstorage <scale> [<loadClass> <quartile>]`
3. *NOTE* If you choose the `-addallder` flag the rest of the flags will not be used.
<br>

### Running using a factors.xlsx file
1. Install python requirements from the provided requirements.txt.
2. Run generate_commands.py with the following format: `python generate_commands.py <factors xlsx file> <input folder> <output folder> >commands.bat`
3. Copy paste the commands.bat file in the same location as the exportcircuit executable.
4. Open a terminal here and run commands.bat

## Video Tutorial
<hr>

[![Tutorial Video](https://img.youtube.com/vi/lwiY5JxVft4/0.jpg)](https://youtu.be/lwiY5JxVft4)

link: https://youtu.be/lwiY5JxVft4
