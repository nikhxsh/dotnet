-- Aggregate Examples

-- Get All Accounts having loan
SELECT A.Id, A.Name, L.Amount, CASE WHEN L.Defaulter = 1 THEN 'Defaulter' ELSE 'Normal' END As IsDefaulter
FROM ACCOUNT A
INNER JOIN LOAN L
ON A.Id = L.AccountNumber
ORDER BY L.Amount DESC

-- Get Loan Amount by Country
SELECT A.Country, SUM(L.Amount) TotalLoanAmount
FROM ACCOUNT A
INNER JOIN LOAN L
ON A.Id = L.AccountNumber
GROUP BY A.Country
ORDER BY TotalLoanAmount DESC

-- Get Loan Amount by Age less than 40
SELECT A.Age, SUM(L.Amount) TotalLoanAmount
FROM ACCOUNT A
INNER JOIN LOAN L
ON A.Id = L.AccountNumber
GROUP BY A.Age
HAVING A.Age < 40
ORDER BY TotalLoanAmount DESC

/* 
- You can use the PIVOT and UNPIVOT relational operators to change a table-valued expression into another table. 
- PIVOT rotates a table-valued expression by turning the unique values from one column in the expression into multiple columns in the output. 
  and PIVOT runs aggregations where they're required on any remaining column values that are wanted in the final output. 
- UNPIVOT carries out the opposite operation to PIVOT by rotating columns of a table-valued expression into column values.
Eg: SalesData
+---------+---------+-------+
| Country | Quarter | Sales |
+---------+---------+-------+
| USA     | Q1      | 1000  |
| USA     | Q2      | 1100  |
| UK      | Q1      | 900   |
| UK      | Q2      | 950   |
| IN      | Q1      | 800   |
| IN      | Q2      | 850   |
+---------+---------+-------+
*/
	
SELECT Country, [Q1], [Q2]
FROM (
    SELECT Country, Quarter, Sales
    FROM SalesData
) AS SourceTable
PIVOT (
    SUM(Sales)
    FOR Quarter IN ([Q1], [Q2])
) AS PivotedTable;

/* 
Output:
+---------+------+------+
| Country | Q1   | Q2   |
+---------+------+------+
| USA     | 1000 | 1100 |
| UK      | 900  | 950  |
| IN      | 800  | 850  |
+---------+------+------+
*/

SELECT Country, Quarter, Sales
FROM (
    SELECT Country, [Q1], [Q2]
    FROM (
        SELECT Country, Quarter, Sales
        FROM SalesData
    ) AS SourceTable
    PIVOT (
        SUM(Sales)
        FOR Quarter IN ([Q1], [Q2])
    ) AS PivotedTable
) AS PivotResult
UNPIVOT (
    Sales FOR Quarter IN ([Q1], [Q2])
) AS UnpivotedTable;

/* 
+---------+---------+-------+
| Country | Quarter | Sales |
+---------+---------+-------+
| USA     | Q1      | 1000  |
| USA     | Q2      | 1100  |
| UK      | Q1      | 900   |
| UK      | Q2      | 950   |
| IN      | Q1      | 800   |
| IN      | Q2      | 850   |
+---------+---------+-------+
*/
