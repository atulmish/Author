# About
The **Author** is a tool to generate multiple documents from a source data file.  

# Sample config
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