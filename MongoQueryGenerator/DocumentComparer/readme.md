## Document Comparer

This application was designed to compare the results when running a query with different mappings.

In order to compare the results, the query was executed with a pre-defined set of keys (randomly generated [min: 1, max: dbSet.count - 1]) and them the resulting data was saved with the following folder structure: 

+ query-A
+ query-A/mapping-1
+ query-A/mapping-2
+ query-A/mapping-n

The application then will load the files and compare all ocurrences of the same index. The comparision itself is done using the package Newtonsoft.JSON.