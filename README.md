# Author 
[![MIT License1](https://img.shields.io/pypi/l/ansicolortags.svg)](https://github.com/atulmish/Author/blob/main/LICENSE)

# About
The **Author** is a tool to generate multiple documents from a source data file.  

# Installation Guide

 - Clone or download the source code
 - Navigate to the dist directory
 - The dist directory contains the Author.exe
 - Setup the config.yml file as per your need
 - Run Author.exe

# Sample config.yml
```
Source:
  
  # The source data file full path. The currently suported formats are csv and excel.
  DataFile: .\SampleData\data.csv
  
  # The source template file. The currently supported formats are word file .doc and .docs
  TemplateFile: .\SampleData\template.docx

Output:
  
  # The output directory. If the directory is not provided a directory will be created.
  Directory: 
  
  # The file name of the output. 
  Filename: "[id] {First Name} {Last Name}"
  
  # The output file format.
  Format: DOCX
```