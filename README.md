# DBDocTemplateGenerator
## Brief Intro
- It generates a document templates about your database structure(table, stored procedures and function) for you, including folder structure, *.md and *.sql.

## Details Intro
- Assume you have a Database call `MyDB`, and there are some tables, stored procedures and functions inside. maybe like:
    - tables: `Table`, `Tbble`
    - stored procedures: `SPA`, `SPB`
    - functions: `FNA`, `FNB`
- It will generate the markdown templates, the folder structure like:

```
\MyDB\
     \SPA
         \SPA.md
         \SPA.sql
     \SPB
         \SPB.md
         \SPB.sql
     \FNA
         \FNA.md
         \FNA.sql
     \FNB
         \FNB.md
         \FNB.sql
     \Table
           \Table.md
     \Tbble
           \Tbble.md
     \Tcble
           \Tcble.md
```

- the content of template (.md) will generate for you, example:
```
# [SPA](a link to the SPA.sql)
## Parameters
|Name|Type (Size, Precision, Scale)|Notes|
|:---|:---|:---|
|@Param1|uniqueidentifier (16, 0, 0)||
|@Param2|varchar (20, 0, 0)||
|@Param3|int (4, 10, 0)||

## Description
- 

## Examples
`-- SPA examples`

## References
- [Table](a link to Table.md)
- [Tbble](a link to Tbble.md)
- [FNA](a link to FNA.md)

## Notes
- 
```

## Make a Try