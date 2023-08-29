SELECT EXISTS
(
    SELECT name
    FROM sqlite_schema
    
    WHERE type = 'table'
    AND name = $name
);