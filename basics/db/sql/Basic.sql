/*
Eg: Account
+---------+---------+---------+--------+
| Name    | Amount  | Age     | Bank   |
+---------+---------+---------+--------+
| Alice   | 1200.50 | 30      | HDFC   |
| Bob     | 950.00  | 25      | ICICI  |
| Charlie | 1100.75 | 40      | HDFC   |
| Diana   | 800.00  | 30      | ICICI  |
| Ethan   | 1600.25 | 35      | HDFC   |
| Fiona   | 700.00  | 25      | AXIS   |
| George  | 1230.00 | 40      | ICICI  |
| Hannah  | 980.50  | 35      | HDFC   |
| Ian     | 760.00  | 30      | AXIS   |
| Julia   | 1125.00 | 25      | ICICI  |
+---------+---------+---------+--------+
*/
-- Get Max Amount
SELECT MAX(Amount) FROM ACCOUNT

-- Get Second Max Amount
SELECT MAX(Amount) FROM ACCOUNT WHERE Amount < ( SELECT MAX(Amount) FROM ACCOUNT)

-- Get Third Max Amount
SELECT MAX(Amount) FROM ACCOUNT WHERE Amount < (SELECT MAX(Amount) FROM ACCOUNT WHERE Amount < ( SELECT MAX(Amount) FROM ACCOUNT))

-- Get Max Amount With Name
SELECT A.Name, A.Amount
FROM ACCOUNT A
WHERE A.Amount = (SELECT MAX(Amount) FROM ACCOUNT)

/*
- Get top 5 balace using ROW_NUMBER()
- The ROW_NUMBER() is a MSSQL function that assigns a sequential integer to each row within the partition of a result set. 
*/
SELECT Row#, A.Name, A.Amount
FROM (
	SELECT ROW_NUMBER() OVER (ORDER BY Amount DESC) AS Row#, Name, Amount, Age, Bank	
	FROM ACCOUNT
) A
WHERE Row# IN (1,2,3,4,5) 
ORDER BY A.Amount DESC
/*
+------+---------+---------+
| Row# | Name    | Amount  |
+------+---------+---------+
| 1    | Ethan   | 1600.25 |
| 2    | George  | 1230.00 |
| 3    | Alice   | 1200.50 |
| 4    | Julia   | 1125.00 |
| 5    | Charlie | 1100.75 |
+------+---------+---------+
*/

-- Adding a PARTITION BY clause on the Id column, will restart the numbering when the Id value changes.
-- PARTITION BY to divide the result set into partitions and perform computation on each subset of partitioned data.
SELECT Row#, A.*
FROM (
	SELECT ROW_NUMBER() OVER (PARTITION BY Bank ORDER BY Amount DESC) AS Row#,
	       Name,
	       Amount,
	       Bank
	FROM Account
) A
/*
+------+---------+---------+--------+
| Row# | Name    | Amount  | Bank   |
+------+---------+---------+--------+
| 1    | Ethan   | 1600.25 | HDFC   |
| 2    | Alice   | 1200.50 | HDFC   |
| 3    | Charlie | 1100.75 | HDFC   |
| 4    | Hannah  | 980.50  | HDFC   |
| 1    | George  | 1230.00 | ICICI  |
| 2    | Julia   | 1125.00 | ICICI  |
| 3    | Bob     | 950.00  | ICICI  |
| 4    | Diana   | 800.00  | ICICI  |
| 1    | Ian     | 760.00  | AXIS   |
| 2    | Fiona   | 700.00  | AXIS   |
+------+---------+---------+--------+
*/

-- Get max Amount by Bank
SELECT A.Bank, MAX(A.Amount) As TotalAmount
FROM ACCOUNT A
GROUP BY A.Bank
ORDER BY TotalAmount DESC

-- Get top 5 Age using RANK()
SELECT Rank#, A.Name, A.Age, A.Amount
FROM (
    -- ROW_NUMBER and RANK are similar. ROW_NUMBER numbers all rows sequentially (1, 2, 3...).
    -- RANK provides the same numeric value for ties (e.g., 1, 2, 2, 4, ...).
	SELECT RANK() OVER (ORDER BY Age DESC, Amount DESC) AS Rank#,
	       Id,
	       Name,
	       Age,
	       Amount	
	FROM ACCOUNT
) A
WHERE Rank# IN (1,2,3,4,5);
/*
+--------+---------+-----+---------+
| Rank#  | Name    | Age | Amount  |
+--------+---------+-----+---------+
| 1      | George  | 40  | 1230.00 |
| 2      | Charlie | 40  | 1100.75 |
| 3      | Ethan   | 35  | 1600.25 |
| 4      | Hannah  | 35  | 980.50  |
| 5      | Alice   | 30  | 1200.50 |
+--------+---------+-----+---------+


-- Get top 5 Age using RANK()
SELECT Rank#, A.Name, A.Age, A.Amount
FROM (
    -- If two or more rows have the same rank value, DENSE_RANK assigns the same number and doesn't skip the next one.
    SELECT DENSE_RANK() OVER (ORDER BY Age DESC) AS Rank#,
           Id,
           Name,
           Age,
           Amount
    FROM Account
) A;
/*
+--------+---------+-----+---------+
| Rank#  | Name    | Age | Amount  |
+--------+---------+-----+---------+
| 1      | Charlie | 40  | 1100.75 |
| 1      | George  | 40  | 1230.00 |
| 2      | Ethan   | 35  | 1600.25 |
| 2      | Hannah  | 35  | 980.50  |
| 3      | Alice   | 30  | 1200.50 |
| 3      | Diana   | 30  | 800.00  |
| 3      | Ian     | 30  | 760.00  |
| 4      | Bob     | 25  | 950.00  |
| 4      | Fiona   | 25  | 700.00  |
| 4      | Julia   | 25  | 1125.00 |
+--------+---------+-----+---------+
*/

-- NTILE(n) is a window function that divides rows into n approximately equal groups (called tiles or buckets) based on an ORDER BY clause. 
-- It assigns each group a bucket number starting from one. For each row in a group, the NTILE() function assigns a bucket number representing the group to which the row belongs.
SELECT 
    NTILE(4) OVER (PARTITION BY Bank ORDER BY Age DESC) AS Quartile,
    Name,
    Age,
    Amount,
    Bank
FROM Account;
/*
+----------+---------+-----+---------+--------+
| Quartile | Name    | Age | Amount  | Bank   |
+----------+---------+-----+---------+--------+
| 1        | Charlie | 40  | 1100.75 | HDFC   |
| 1        | George  | 40  | 1230.00 | ICICI  |
| 1        | Ethan   | 35  | 1600.25 | HDFC   |
| 2        | Hannah  | 35  | 980.50  | HDFC   |
| 2        | Alice   | 30  | 1200.50 | HDFC   |
| 2        | Diana   | 30  | 800.00  | ICICI  |
| 3        | Ian     | 30  | 760.00  | AXIS   |
| 3        | Bob     | 25  | 950.00  | ICICI  |
| 4        | Fiona   | 25  | 700.00  | AXIS   |
| 4        | Julia   | 25  | 1125.00 | ICICI  |
+----------+---------+-----+---------+--------+
*/

SELECT 
    Bank,
    SUM(Amount) AS Total_Deposits,
    NTILE(3) OVER (ORDER BY SUM(Amount) DESC) AS Tier
FROM Account
GROUP BY Bank
ORDER BY Tier;
/*
+--------+----------------+-------+
| Bank   | Total_Deposits | Tier  |
+--------+----------------+-------+
| HDFC   | 4881.00        | 1     |
| ICICI  | 4105.00        | 2     |
| AXIS   | 1460.00        | 3     |
+--------+----------------+-------+
*/

-- Employee salary between 10000 to 40000
SELECT *
FROM [dbo].[EMPLOYEE]
WHERE Salary BETWEEN 10000 AND 40000

-- The ANY operator returns TRUE if any of the subquery values meet the condition
SELECT *
FROM [dbo].[DEPARTMENT]
WHERE ManagerId = ANY (SELECT Id FROM [dbo].[EMPLOYEE])

-- The ALL operator returns TRUE if all of the subquery values meet the condition.
SELECT *
FROM [dbo].[DEPARTMENT]
WHERE ManagerId = ALL (SELECT Id FROM [dbo].[EMPLOYEE])

-- Account Number without loan (EXISTS used to test for the existence of any record in a subquery.)
SELECT A.Id, A.Name, A.Amount
FROM ACCOUNT A
WHERE NOT EXISTS (SELECT E.AccountNumber FROM [dbo].[LOAN] E WHERE E.AccountNumber = A.Id)

-- Account Number with loan
SELECT A.Id, A.Name, A.Amount
FROM ACCOUNT A
WHERE EXISTS (SELECT E.AccountNumber FROM [dbo].[LOAN] E WHERE E.AccountNumber = A.Id)

-- Account Number with loan
SELECT A.Id, A.Name, A.Amount
FROM ACCOUNT A
WHERE EXISTS (SELECT E.AccountNumber FROM [dbo].[LOAN] E WHERE E.AccountNumber = A.Id)

-- The ALL operator returns TRUE if all of the subquery values meet the condition.
SELECT *
FROM [dbo].[EMPLOYEE] E1
WHERE E1.Salary > SOME (SELECT Salary FROM [dbo].[EMPLOYEE] WHERE E1.DeptId = 104)

