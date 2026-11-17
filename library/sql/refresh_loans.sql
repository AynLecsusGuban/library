-- refresh_loans.sql : move the sample loans to recent dates, for Laboratory Activity 11
-- Run it ONCE in the Supabase SQL Editor. It changes only the dates of lending.loan:
-- every member, book, note and loan stays, and the order of the dates is kept.
 
WITH ref AS (
    SELECT GREATEST(MAX(loaned_on), MAX(returned_on)) AS last_date FROM lending.loan
)
UPDATE lending.loan l
SET    loaned_on   = CURRENT_DATE - ROUND((ref.last_date - l.loaned_on)   / 4.0)::int - 1,
       returned_on = CURRENT_DATE - ROUND((ref.last_date - l.returned_on) / 4.0)::int - 1
FROM   ref;
 
SELECT CASE WHEN returned_on IS NOT NULL THEN 'Returned'
            WHEN loaned_on + days_allowed < CURRENT_DATE THEN 'Overdue'
            ELSE 'Out' END AS status,
       COUNT(*) AS loans
FROM   lending.loan
GROUP  BY 1
ORDER  BY 1;
