/*
ACCOUNT
+----+---------+---------+-----+--------+
| Id | Name    | Amount  | Age | Bank   |
+----+---------+---------+-----+--------+
|  1 | Alice   | 1200.50 | 30  | HDFC   |
|  2 | Bob     | 950.00  | 25  | ICICI  |
|  3 | Charlie | 1100.75 | 40  | HDFC   |
|  4 | Alice   | 1200.50 | 30  | HDFC   |
|  5 | Diana   | 800.00  | 30  | ICICI  |
|  6 | Ethan   | 1600.25 | 35  | HDFC   |
|  7 | Fiona   | 700.00  | 25  | AXIS   |
|  8 | George  | 1230.00 | 40  | ICICI  |
|  9 | Hannah  | 980.50  | 35  | HDFC   |
| 10 | Ian     | 760.00  | 30  | AXIS   |
| 11 | Julia   | 1125.00 | 25  | ICICI  |
| 12 | Bob     | 950.00  | 25  | ICICI  |
+----+---------+---------+-----+--------+

LOAN
+---------------+----------+-----------+
| AccountNumber | LoanAmt  | LoanType  |
+---------------+----------+-----------+
| 1             | 50000.00 | Home      |
| 2             | 20000.00 | Personal  |
| 3             | 25000.00 | Car       |
| 4             | 30000.00 | Education |
| 5             | 15000.00 | Personal  |
| 6             | 10000.00 | Gold      |
| 7             | 40000.00 | Home      |
| 8             | 22000.00 | Car       |
| 9             | 18000.00 | Education |
| 10            | 26000.00 | Personal  |
+---------------+----------+-----------+

+----+----------+-----------+--------+
| Id | Name     | ManagerId | DeptId |
+----+----------+-----------+--------+
| 1  | Alice    | NULL      | 1      |  -- HR
| 2  | Bob      | 1         | 2      |  -- Engineering
| 3  | Charlie  | 1         | 2      |  -- Engineering
| 4  | Diana    | 2         | 3      |  -- Marketing
| 5  | Ethan    | 2         | 3      |  -- Marketing
+----+----------+-----------+--------+

DEPARTMENT
+----+----------------+
| Id | Name           |
+----+----------------+
| 1  | HR             |
| 2  | Engineering    |
| 3  | Marketing      |
| 4  | Legal          | -- Has no employees
+----+----------------+
*/

-- INNER JOIN
SELECT *
FROM ACCOUNT A
INNER JOIN LOAN L
ON A.Id = L.AccountNumber
/*
Output:
+----+---------+---------+-----+--------+---------------+----------+-----------+
| Id | Name    | Amount  | Age | Bank   | AccountNumber | LoanAmt  | LoanType  |
+----+---------+---------+-----+--------+---------------+----------+-----------+
|  1 | Alice   | 1200.50 | 30  | HDFC   | 1             | 50000.00 | Home      |
|  2 | Bob     | 950.00  | 25  | ICICI  | 2             | 20000.00 | Personal  |
|  3 | Charlie |1100.75  | 40  | HDFC   | 3             | 25000.00 | Car       |
|  4 | Diana   | 800.00  | 30  | ICICI  | 4             | 30000.00 | Education |
|  5 | Ethan   |1600.25  | 35  | HDFC   | 5             | 15000.00 | Personal  |
|  6 | Fiona   | 700.00  | 25  | AXIS   | 6             | 10000.00 | Gold      |
|  7 | George  |1230.00  | 40  | ICICI  | 7             | 40000.00 | Home      |
|  8 | Hannah  | 980.50  | 35  | HDFC   | 8             | 22000.00 | Car       |
|  9 | Ian     | 760.00  | 30  | AXIS   | 9             | 18000.00 | Education |
| 10 | Julia   |1125.00  | 25  | ICICI  |10             | 26000.00 | Personal  |
+----+---------+---------+-----+--------+---------------+----------+-----------+
*/

/*
- INNER MERGE JOIN
  A merge join is selected by SQL Server when:
	•	Both inputs are sorted on the join key
	•	It determines merge is faster than nested loops or hash joins
	•	You’re joining large datasets efficiently on indexed keys
*/
  
SELECT *
FROM ACCOUNT A
INNER JOIN LOAN L
    ON A.Id = L.AccountNumber
OPTION (MERGE JOIN);

/*
- INNER HASH JOIN
The SQL Server optimizer automatically chooses a hash join when:
	•	You’re joining large tables
	•	Join keys are not sorted (no useful indexes)
	•	It’s cheaper than nested loops or merge joins
*/

SELECT *
FROM ACCOUNT A
INNER JOIN LOAN L
    ON A.Id = L.AccountNumber
OPTION (HASH JOIN);

/*
- LEFT JOIN
	•	Returns all records from ACCOUNT
	•	Includes matching data from LOAN if it exists
	•	If there’s no matching record in LOAN, columns from LOAN will be NULL
- Use cases
	•	See which accounts do not have loans
	•	Audit or cleanup tasks: find orphan records
	•	Build inclusive reports showing both active and inactive loan customers
*/

SELECT *
FROM ACCOUNT A
LEFT JOIN LOAN L
ON A.Id = L.AccountNumber
/*
Output:
+----+---------+---------+-----+--------+---------------+----------+-----------+
| Id | Name    | Amount  | Age | Bank   | AccountNumber | LoanAmt  | LoanType  |
+----+---------+---------+-----+--------+---------------+----------+-----------+
| 1  | Alice   | 1200.50 | 30  | HDFC   | 1             | 50000.00 | Home      |
| 2  | Bob     | 950.00  | 25  | ICICI  | 2             | 20000.00 | Personal  |
| 3  | Charlie |1100.75  | 40  | HDFC   | 3             | 25000.00 | Car       |
| 4  | Diana   | 800.00  | 30  | ICICI  | 4             | 30000.00 | Education |
| 5  | Ethan   |1600.25  | 35  | HDFC   | 5             | 15000.00 | Personal  |
| 6  | Fiona   | 700.00  | 25  | AXIS   | 6             | 10000.00 | Gold      |
| 7  | George  |1230.00  | 40  | ICICI  | 7             | 40000.00 | Home      |
| 8  | Hannah  | 980.50  | 35  | HDFC   | 8             | 22000.00 | Car       |
| 9  | Ian     | 760.00  | 30  | AXIS   | NULL          | NULL     | NULL      |
|10  | Julia   |1125.00  | 25  | ICICI  | NULL          | NULL     | NULL      |
+----+---------+---------+-----+--------+---------------+----------+-----------+
*/

/*
- RIGHT JOIN
  •	Returns all records from LOAN
	•	Includes matching data from ACCOUNT if it exists
	•	If an account doesn’t exist for a given loan’s AccountNumber, ACCOUNT columns will be NULL
- Use Cases for RIGHT JOIN
	•	Loan audit: Find loans that don’t have a valid linked account
	•	Data consistency: Ensure no orphaned records in LOAN
	•	Generate full loan reports, even when customer details are missing
*/
SELECT *
FROM ACCOUNT A
RIGHT JOIN LOAN L
ON A.Id = L.AccountNumber
/*
Output:
+------+---------+--------+-----+--------+---------------+----------+-----------+
| Id   | Name    | Amount | Age | Bank   | AccountNumber | LoanAmt  | LoanType  |
+------+---------+--------+-----+--------+---------------+----------+-----------+
| 1    | Alice   | 1200.5 | 30  | HDFC   | 1             | 50000.0  | Home      |
| 2    | Bob     | 950.0  | 25  | ICICI  | 2             | 20000.0  | Personal  |
| 3    | Charlie |1100.75 | 40  | HDFC   | 3             | 25000.0  | Car       |
| 4    | Diana   | 800.0  | 30  | ICICI  | 4             | 30000.0  | Education |
| 5    | Ethan   |1600.25 | 35  | HDFC   | 5             | 15000.0  | Personal  |
| 6    | Fiona   | 700.0  | 25  | AXIS   | 6             | 10000.0  | Gold      |
| 7    | George  |1230.0  | 40  | ICICI  | 7             | 40000.0  | Home      |
| 8    | Hannah  | 980.5  | 35  | HDFC   | 8             | 22000.0  | Car       |
| 9    | Ian     | 760.0  | 30  | AXIS   | 9             | 18000.0  | Education |
| NULL | NULL    | NULL   | NULL| NULL   | 11            | 27000.0  | Business  |
+------+---------+--------+-----+--------+---------------+----------+-----------+
*/

/*
- FULL OUTER JOIN
  •	Rows with matches between ACCOUNT and LOAN
	•	Rows in ACCOUNT without a loan (LOAN columns will be NULL)
	•	Rows in LOAN without a matching account (ACCOUNT columns will be NULL)
- Use Cases for FULL OUTER JOIN
	•	Data audit: Detect mismatches between accounts and loans
	•	Reporting: Show all customers and all loans, even unmatched
	•	ETL validations: Check for orphaned or missing relationships
*/
SELECT *
FROM ACCOUNT A
FULL OUTER JOIN LOAN L
ON A.Id = L.AccountNumber
/*
Output:
+------+---------+--------+-----+--------+---------------+----------+-----------+
| Id   | Name    | Amount | Age | Bank   | AccountNumber | LoanAmt  | LoanType  |
+------+---------+--------+-----+--------+---------------+----------+-----------+
| 1    | Alice   | 1200.5 | 30  | HDFC   | 1             | 50000.0  | Home      |
| 2    | Bob     | 950.0  | 25  | ICICI  | 2             | 20000.0  | Personal  |
| 3    | Charlie |1100.75 | 40  | HDFC   | 3             | 25000.0  | Car       |
| 4    | Diana   | 800.0  | 30  | ICICI  | 4             | 30000.0  | Education |
| 5    | Ethan   |1600.25 | 35  | HDFC   | 5             | 15000.0  | Personal  |
| 6    | Fiona   | 700.0  | 25  | AXIS   | 6             | 10000.0  | Gold      |
| 7    | George  |1230.0  | 40  | ICICI  | 7             | 40000.0  | Home      |
| 8    | Hannah  | 980.5  | 35  | HDFC   | 8             | 22000.0  | Car       |
| 9    | Ian     | 760.0  | 30  | AXIS   | NULL          | NULL     | NULL      |
|10    | Julia   |1125.0  | 25  | ICICI  | NULL          | NULL     | NULL      |
|NULL | NULL    | NULL   | NULL| NULL   | 11            | 27000.0  | Business  |
+------+---------+--------+-----+--------+---------------+----------+-----------+
*/

-- This query returns: •	Accounts without corresponding loans •	Loans without corresponding accounts
SELECT *
FROM ACCOUNT A
FULL OUTER JOIN LOAN L
ON A.Id = L.AccountNumber
WHERE A.Id IS NULL OR L.AccountNumber IS NULL;
/*
Output:
+------+-------+--------+-----+--------+---------------+----------+-----------+
| Id   | Name  | Amount | Age | Bank   | AccountNumber | LoanAmt  | LoanType  |
+------+-------+--------+-----+--------+---------------+----------+-----------+
| 9    | Ian   | 760.00 | 30  | AXIS   | NULL          | NULL     | NULL      |
| 10   | Julia |1125.00 | 25  | ICICI  | NULL          | NULL     | NULL      |
| NULL | NULL  | NULL   | NULL| NULL   | 11            | 27000.00 | Business  |
+------+-------+--------+-----+--------+---------------+----------+-----------+
*/

-- CROSS JOIN
SELECT *
FROM ACCOUNT A
CROSS JOIN LOAN L
ORDER BY A.Name

/*
-- SELF JOIN
 1. Find Duplicate Records in Account
 2. Employee → Manager Hierarchy (Self Join on Employee)
 3. Pair of Accounts in the Same Bank
 4. Next Transaction Example (Time Series Self Join)
*/
SELECT A.Id AS A_Id, B.Id AS B_Id, A.Name, A.Age, A.Bank
FROM Account A
JOIN Account B
  ON A.Name = B.Name AND A.Age = B.Age AND A.Bank = B.Bank
WHERE A.Id < B.Id;
/*
Duplicates:
+------+-------+--------+-----+--------+
| A_Id | B_Id  | Name   | Age | Bank   |
+------+-------+--------+-----+--------+
| 1    | 4     | Alice  | 30  | HDFC   |
| 2    | 7     | Bob    | 25  | ICICI  |
+------+-------+--------+-----+--------+
*/
SELECT E.Name AS Employee, M.Name AS Manager
FROM Employee E
LEFT JOIN Employee M ON E.ManagerId = M.Id;
/*
Employee → Manager Hierarchy:
+----------+---------+
| Employee | Manager |
+----------+---------+
| Alice    | NULL    |
| Bob      | Alice   |
| Charlie  | Alice   |
| Diana    | Bob     |
| Ethan    | Bob     |
+----------+---------+
*/
SELECT A.Name AS Account1, B.Name AS Account2, A.Bank
FROM Account A
JOIN Account B
  ON A.Bank = B.Bank AND A.Id < B.Id;
/*
Pair of Accounts in the Same Bank:
+----------+----------+--------+
| Account1 | Account2 | Bank   |
+----------+----------+--------+
| Alice    | Charlie  | HDFC   |
| Alice    | Alice    | HDFC   |
| Alice    | Ethan    | HDFC   |
| Charlie  | Alice    | HDFC   |
| Charlie  | Ethan    | HDFC   |
| Alice    | Ethan    | HDFC   |
| Bob      | Diana    | ICICI  |
| Bob      | Bob      | ICICI  |
| Diana    | Bob      | ICICI  |
+----------+----------+--------+
*/

/*
	•	DEPARTMENT is a table with department data.
	•	GetEmployeeReports(D.Id) is a table-valued function (TVF) that takes a department ID and returns a table of employees for that department.
	•	CROSS APPLY works like a correlated subquery — it:
	  •	Passes each D.Id from DEPARTMENT into the function
	  •	Joins the result to the department row
	  •	Only includes departments that return one or more rows
*/
SELECT D.Name, EmployeeName, Salary
FROM DEPARTMENT D
CROSS APPLY GetEmployeeReports(D.Id);
/*
Output:
+--------------+--------------+--------+
| DeptName     | EmployeeName | Salary |
+--------------+--------------+--------+
| HR           | Alice        | 80000  |
| Engineering  | Bob          | 90000  |
| Engineering  | Charlie      | 95000  |
| Marketing    | Diana        | 85000  |
+--------------+--------------+--------+
*/

/*
	•	For each department (D.Id), SQL Server:
	  •	Calls the TVF GetEmployeeReports(D.Id)
	  •	Joins the returned rows to the department
	•	If the function returns no employees for a department:
	  •	The department row is still included
	  •	The columns from E (employee name, salary) will be NULL
*/
SELECT D.Name, E.EmployeeName, E.Salary
FROM DEPARTMENT D
OUTER APPLY GetEmployeeReports(D.Id) E;
/*
Output:
+--------------+--------------+--------+
| DeptName     | EmployeeName | Salary |
+--------------+--------------+--------+
| HR           | Alice        | 80000  |
| Engineering  | Bob          | 90000  |
| Engineering  | Charlie      | 95000  |
| Marketing    | Diana        | 85000  |
| Legal        | NULL         | NULL   |  <-- no employees
+--------------+--------------+--------+
*/
