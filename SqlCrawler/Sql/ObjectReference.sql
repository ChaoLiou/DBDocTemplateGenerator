select object_name(referencing_id) as Name
, referenced_entity_name as Reference
, isnull(referenced_database_name, '') as ReferenceDatabase
from sys.sql_expression_dependencies