[![DOI](https://zenodo.org/badge/165153072.svg)](https://zenodo.org/badge/latestdoi/165153072)

## Project list

* **DataFaker**: Used at earlier stages to seed relational databases for testing purposes. (executable)

* **QueryAnalyzer**: The first version aimed to run queries against MongoDB and record the performance. Now used to prepare workloads and queries to be executed on YCSB (executable)

* **QueryBuilder**: Query generator algorithm. (class library)

* **QueryBuilder-Parser**: Parser project for both mapping files and query language. The output is used on the guery generator algorithm. (class library)

* **QueryBuilder-TestParser**: Test app for the parser project. (executable)

* **TestApp**: App for algorithm tests. Used to type a query and prints the resulting MongoDB query. (executable)

* **UnitTests**: Automated Unit tests for algorithm concepts and for test cases for two systems called MKCMS (custom) and ProgradWeb (from UFSCAR)

All projects use either .NET Core ou .NET Standard and will probably work on other OSs other than Windows.

## Test data

The folder ``data/`` contains all data obtained from tests and benchmarks.