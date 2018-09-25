DECLARE @year INT SET @year = 2018
DECLARE @month INT SET @month = 07
DECLARE @day INT SET @day = 01

SELECT 
    DATEFROMPARTS (@year, @month, @day) AS 'Date Period of Report',
    COALESCE(SUM(IsVegetarian), 0) AS 'Number of Vegetarian Pizzas Sold',
    COALESCE(COUNT(DistinctDate), 0) - COALESCE(SUM(IsVegetarian), 0) AS 'Number of Non-vegetarian Pizzas Sold'
FROM 
(
    SELECT 
        OrderDate AS 'FilteredDate', 
        COUNT(DISTINCT(OrderDate)) AS 'IsVegetarian'
    FROM Orders
    GROUP BY OrderDate
    HAVING 
        SUM(CASE WHEN IsToppingVegetarian='False' THEN 1 ELSE 0 END) = 0 AND
        CAST(OrderDate AS DATE) = DATEFROMPARTS (@year, @month, @day)
) AS vegetarianList
FULL OUTER JOIN 
(
    SELECT 
        DISTINCT(OrderDate) AS 'DistinctDate' 
    FROM Orders 
    WHERE CAST(OrderDate AS DATE) = DATEFROMPARTS (@year, @month, @day)
) AS totalList 
ON vegetarianList.FilteredDate = totalList.DistinctDate
